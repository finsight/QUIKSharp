// Copyright (C) 2015 Victor Baybekov

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;

namespace QuikSharp {
    /// <summary>
    /// 
    /// </summary>
    public class QuikService {
        private static Dictionary<int, QuikService> _services =
            new Dictionary<int, QuikService>();
        private static readonly object StaticSync = new object();
        /// <summary>
        /// For each port only one instance of QuikService
        /// </summary>
        public static QuikService Create(int port) {
            lock (StaticSync) {
                QuikService service;
                if (_services.ContainsKey(port)) {
                    service = _services[port];
                    service.Start();
                } else {
                    service = new QuikService(port);
                    _services.Add(port, service);
                }
                return service;
            }
        }

        private QuikService(int responsePort) {
            _responsePort = responsePort;
            _callbackPort = _responsePort + 1;
            Start();
            Events = new QuikEvents(this);
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStarted { get; private set; }



        internal QuikEvents Events { get; set; }
        internal IPersistentStorage Storage { get; set; }
        internal CandleFunctions Candles { get; set; }
        internal StopOrderFunctions StopOrders { get; set; }


        internal readonly string SessionId = DateTime.Now.ToString("yyMMddHHmmss");

        private readonly IPAddress _host = IPAddress.Parse("127.0.0.1");
        private readonly int _responsePort;
        private readonly int _callbackPort;
        private TcpClient _responseClient;
        private TcpClient _callbackClient;

        private readonly Object _syncRoot = new object();

        private CancellationTokenSource _cts;

        /// <summary>
        /// Current correlation id. Use Interlocked.Increment to get a new id.
        /// </summary>
        private static int _correlationId;
        /// <summary>
        /// IQuickCalls functions enqueue a message and return a task from TCS
        /// </summary>
        internal readonly BlockingCollection<IMessage> EnvelopeQueue
            = new BlockingCollection<IMessage>();

        /// <summary>
        /// If received message has a correlation id then use its Data to SetResult on TCS and remove the TCS from the dic
        /// </summary>
        internal readonly ConcurrentDictionary<long, KeyValuePair<TaskCompletionSource<IMessage>, Type>>
            Responses = new ConcurrentDictionary<long, KeyValuePair<TaskCompletionSource<IMessage>, Type>>();

        /// <summary>
        /// 
        /// </summary>
        public void Stop() {
            if (!IsStarted) return;
            IsStarted = false;
            _cts.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ApplicationException">Response message id does not exists in results dictionary</exception>
        public void Start() {
            if (IsStarted) return;
            IsStarted = true;
            _cts = new CancellationTokenSource();

            // Request Task
            Task.Factory.StartNew(() => {
                try {
                    // Enter the listening loop. 
                    while (IsStarted) {
                        Trace.WriteLine("Connecting on request channel... ");
                        EnsureConnectedClient();
                        // here we have a connected TCP client
                        Trace.WriteLine("Request channel connected");
                        try {
                            var stream = new NetworkStream(_responseClient.Client);
                            var writer = new StreamWriter(stream);
                            while (IsStarted) {
                                IMessage message = null;
                                try {
                                    // BLOCKING
                                    message = EnvelopeQueue.Take(_cts.Token);
                                    var request = message.ToJson();
                                    //Trace.WriteLine("Request: " + request);
                                    // scenario: Quik is restarted or script is stopped
                                    // then writer must throw and we will add a message back
                                    // then we will iterate over messages and cancel expired ones
                                    if (!message.ValidUntil.HasValue || message.ValidUntil >= DateTime.UtcNow) {
                                        writer.WriteLine(request);
                                        writer.Flush();
                                    } else {
                                        Trace.Assert(message.Id.HasValue, "All requests must have correlation id");
                                        Responses[message.Id.Value].Key.SetException(
                                            new TimeoutException("ValidUntilUTC is less than current time"));
                                        KeyValuePair<TaskCompletionSource<IMessage>, Type> tcs; // <IMessage>
                                        Responses.TryRemove(message.Id.Value, out tcs);
                                    }
                                } catch (IOException) {
                                    // this catch is for unexpected and unchecked connection termination
                                    // add back, there was an error while writing
                                    if (message != null) { EnvelopeQueue.Add(message); }
                                    break;
                                }
                            }
                        } catch (IOException e) {
                            Trace.WriteLine(e.Message);
                        }
                    }
                } catch (Exception e) {
                    Trace.WriteLine(e);
                } finally {
                    try {
                        Monitor.Enter(_syncRoot);
                        if (_responseClient != null) {
                            _responseClient.Client.Shutdown(SocketShutdown.Both);
                            _responseClient.Close();
                        }
                    } finally {
                        Monitor.Exit(_syncRoot);
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            // Response Task
            Task.Factory.StartNew(async () => {
                try {
                    while (IsStarted) {
                        Trace.WriteLine("Connecting on response channel... ");
                        EnsureConnectedClient();
                        // here we have a connected TCP client
                        Trace.WriteLine("Response channel connected");
                        try {
                            var stream = new NetworkStream(_responseClient.Client);
                            var reader = new StreamReader(stream, Encoding.GetEncoding(1251)); //true
                            while (IsStarted) {
                                var response = await reader.ReadLineAsync();
                                if (response == null) {
                                    throw new IOException("Lua returned an empty response or closed the connection");
                                }

                                // No IO exceptions possible for response, move its processing
                                // to the threadpool and wait for the next mesaage
                                // A new task here gives c.30% boost for full TransactionSpec echo

                                // ReSharper disable once UnusedVariable
                                var doNotAwaitMe = Task.Factory.StartNew(r => {
                                    //var r = response;
                                    //Trace.WriteLine("Response:" + response);
                                    try {

                                        var message = (r as string).FromJson(this);
                                        Trace.Assert(message.Id.HasValue && message.Id > 0);
                                        // it is a response message
                                        if (!Responses.ContainsKey(message.Id.Value)) throw new ApplicationException("Unexpected correlation ID");
                                        KeyValuePair<TaskCompletionSource<IMessage>, Type> tcs;
                                        Responses.TryRemove(message.Id.Value, out tcs);
                                        if (!message.ValidUntil.HasValue || message.ValidUntil >= DateTime.UtcNow) {
                                            tcs.Key.SetResult(message);
                                        } else {
                                            tcs.Key.SetException(
                                                new TimeoutException("ValidUntilUTC is less than current time"));
                                        }

                                    } catch (LuaException) {
                                        //Trace.WriteLine("Caught Lua exception");
                                    }
                                }, response, TaskCreationOptions.PreferFairness);

                            }
                        } catch (IOException e) {
                            Trace.WriteLine(e.Message);
                        }
                    }
                } catch (Exception e) {
                    Trace.WriteLine(e);
                } finally {
                    try {
                        Monitor.Enter(_syncRoot);
                        if (_responseClient != null) {
                            _responseClient.Client.Shutdown(SocketShutdown.Both);
                            _responseClient.Close();
                        }
                    } finally {
                        Monitor.Exit(_syncRoot);
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);


            // Callback Task
            Task.Factory.StartNew(async () => {
                try {
                    while (IsStarted) {
                        Trace.WriteLine("Connecting on callback channel... ");
                        EnsureConnectedClient();
                        // here we have a connected TCP client
                        Trace.WriteLine("Callback channel connected");
                        try {
                            var stream = new NetworkStream(_callbackClient.Client);
                            var reader = new StreamReader(stream, Encoding.GetEncoding(1251)); //true
                            while (IsStarted) {
                                var callback = await reader.ReadLineAsync();
                                if (callback == null) {
                                    throw new IOException("Lua returned an empty response or closed the connection");
                                }

                                // No IO exceptions possible for response, move its processing
                                // to the threadpool and wait for the next mesaage
                                // A new task here gives c.30% boost for full TransactionSpec echo

                                // ReSharper disable once UnusedVariable
                                var doNotAwaitMe = Task.Factory.StartNew(r => {
                                    //var r = response;
                                    //Trace.WriteLine("Response:" + response);
                                    try {

                                        var message = (r as string).FromJson(this);
                                        Trace.Assert(!(message.Id.HasValue && message.Id > 0));
                                        // it is a callback message
                                        ProcessCallbackMessage(message);

                                    } catch (LuaException) {
                                        //Trace.WriteLine("Caught Lua exception");
                                    }
                                }, callback, TaskCreationOptions.PreferFairness);

                            }
                        } catch (IOException e) {
                            Trace.WriteLine(e.Message);
                        }
                    }
                } catch (Exception e) {
                    Trace.WriteLine(e);
                } finally {
                    try {
                        Monitor.Enter(_syncRoot);
                        if (_callbackClient != null) {
                            _callbackClient.Client.Shutdown(SocketShutdown.Both);
                            _callbackClient.Close();
                        }
                    } finally {
                        Monitor.Exit(_syncRoot);
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);


        }

        private void EnsureConnectedClient() {
            try {
                Monitor.Enter(_syncRoot);
                if (_responseClient != null && _responseClient.Connected && _responseClient.Client.IsConnectedNow()) {
                    // reuse alive client
                } else {
                    if (!(_responseClient != null && _responseClient.Connected && _responseClient.Client.IsConnectedNow())) {
                        var connected = false;
                        while (!connected) {
                            try {
                                _responseClient = new TcpClient {
                                    ExclusiveAddressUse = true,
                                    NoDelay = true
                                };
                                _responseClient.Connect(_host, _responsePort);
                                connected = true;
                            } catch {
                                //Trace.WriteLine("Trying to connect...");
                            }
                        }
                    }
                }

                if (_callbackClient != null && _callbackClient.Connected && _callbackClient.Client.IsConnectedNow()) {
                    // reuse alive client
                } else {
                    if (!(_callbackClient != null && _callbackClient.Connected && _callbackClient.Client.IsConnectedNow())) {
                        var connected = false;
                        while (!connected) {
                            try {
                                _callbackClient = new TcpClient {
                                    ExclusiveAddressUse = true,
                                    NoDelay = true
                                };
                                _callbackClient.Connect(_host, _callbackPort);
                                connected = true;
                            } catch {
                                //Trace.WriteLine("Trying to connect...");
                            }
                        }
                    }
                }
            } finally { Monitor.Exit(_syncRoot); }
        }

        private void ProcessCallbackMessage(IMessage message) {
            if (message == null) throw new ArgumentNullException("message");
            EventNames eventName;
            var parsed = Enum.TryParse(message.Command, true, out eventName);
            if (parsed) {
                switch (eventName) {
                    case EventNames.OnAccountBalance:
                        break;
                    case EventNames.OnAccountPosition:
                        break;
                    case EventNames.OnAllTrade:
                        Trace.Assert(message is Message<AllTrade>);
                        var allTrade = ((Message<AllTrade>)message).Data;
                        allTrade.LuaTimeStamp = message.CreatedTime;
                        Events.OnAllTradeCall(allTrade);
                        break;
                    case EventNames.OnCleanUp:
                        Trace.Assert(message is Message<string>);
                        Events.OnCleanUpCall();
                        break;
                    case EventNames.OnClose:
                        Trace.Assert(message is Message<string>);
                        Events.OnCloseCall();
                        break;
                    case EventNames.OnConnected:
                        Trace.Assert(message is Message<string>);
                        Events.OnConnectedCall();
                        break;
                    case EventNames.OnDepoLimit:
                        break;
                    case EventNames.OnDepoLimitDelete:
                        break;
                    case EventNames.OnDisconnected:
                        Trace.Assert(message is Message<string>);
                        Events.OnDisconnectedCall();
                        break;

                    case EventNames.OnFirm:
                        break;
                    case EventNames.OnFuturesClientHolding:
                        break;
                    case EventNames.OnFuturesLimitChange:
                        break;
                    case EventNames.OnFuturesLimitDelete:
                        break;
                    case EventNames.OnInit:
                        Trace.Assert(message is Message<string>);
                        Events.OnInitCall(((Message<string>)message).Data, _callbackPort);
                        break;
                    case EventNames.OnMoneyLimit:
                        break;
                    case EventNames.OnMoneyLimitDelete:
                        break;
                    case EventNames.OnNegDeal:
                        break;
                    case EventNames.OnNegTrade:
                        break;

                    case EventNames.OnOrder:
                        Trace.Assert(message is Message<Order>);
                        var ord = ((Message<Order>)message).Data;
                        ord.LuaTimeStamp = message.CreatedTime;
                        Events.OnOrderCall(ord);
                        break;

                    case EventNames.OnParam:
                        break;

                    case EventNames.OnQuote:
                        Trace.Assert(message is Message<OrderBook>);
                        var ob = ((Message<OrderBook>)message).Data;
                        ob.LuaTimeStamp = message.CreatedTime;
                        Events.OnQuoteCall(ob);
                        break;

                    case EventNames.OnStop:
                        Trace.Assert(message is Message<string>);
                        Events.OnStopCall(int.Parse(((Message<string>)message).Data));
                        break;

                    case EventNames.OnStopOrder:
                        StopOrder stopOrder = (message as Message<StopOrder>).Data;
                        StopOrders.RaiseNewStopOrderEvent(stopOrder);
                        break;

                    case EventNames.OnTrade:
                        Trace.Assert(message is Message<Trade>);
                        var trade = ((Message<Trade>)message).Data;
                        trade.LuaTimeStamp = message.CreatedTime;
                        Events.OnTradeCall(trade);
                        break;

                    case EventNames.OnTransReply:
                        Trace.Assert(message is Message<TransactionReply>);
                        var trReply = ((Message<TransactionReply>)message).Data;
                        trReply.LuaTimeStamp = message.CreatedTime;
                        Events.OnTransReplyCall(trReply);
                        break;

                    case EventNames.NewCandle:
                        Candle candle = (message as Message<Candle>).Data;
                        Candles.RaiseNewCandleEvent(candle);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } else {
                switch (message.Command) {
                    // an error from an event not request (from req is caught is response loop)
                    case "lua_error":
                        Trace.Assert(message is Message<string>);
                        Trace.TraceError(((Message<string>)message).Data);
                        break;
                    default:
                        throw new InvalidOperationException("Unknown command in a message: " + message.Command);
                }
            }
        }

        /// <summary>
        /// Generate a new unique ID for current session
        /// </summary>
        internal int GetNewUniqueId() {
            lock (_syncRoot) {
                var newId = Interlocked.Increment(ref _correlationId);
                // 2^31 = 2147483648
                // with 1 000 000 messages per second it will take more than
                // 35 hours to overflow => safe for use as TRANS_ID in SendTransaction
                // very weird stuff: Уникальный идентификационный номер заявки, значение от 1 до 2 294 967 294 
                if (newId > 0) {
                    return newId;
                }
                _correlationId = 1;
                return 1;
            }
        }

        /// <summary>
        /// Устанавливает стартовое значение для CorrelactionId.
        /// </summary>
        /// <param name="startCorrelationId">Стартовое значение.</param>
        internal void InitializeCorrelationId(int startCorrelationId)
        {
            _correlationId = startCorrelationId;
        }

        internal string PrependWithSessionId(long id) {
            return SessionId + "." + id;
        }

        internal async Task<TResponse> Send<TResponse>(IMessage request, int timeout = 0)
            where TResponse : class, IMessage, new() {
            var tcs = new TaskCompletionSource<IMessage>();
            if (timeout > 0) {
                var ct = new CancellationTokenSource(timeout);
                ct.Token.Register(() => tcs.TrySetCanceled(), false);
            }
            var kvp = new KeyValuePair<TaskCompletionSource<IMessage>, Type>(tcs, typeof(TResponse));
            if (request.Id == null) {
                request.Id = GetNewUniqueId();
            }
            Responses[request.Id.Value] = kvp;
            // add to queue after responses dictionary
            EnvelopeQueue.Add(request);
            var response = await tcs.Task;
            return (response as TResponse);
        }

    }
}
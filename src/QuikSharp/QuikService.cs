// Copyright (C) 2014 Victor Baybekov

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

// TODOs
// http://stackoverflow.com/questions/1119841/net-console-application-exit-event
// A message queue could be easily persisted with Spreads's Stream type - actually a good
// use case for Rx instead of a blocking queue
//


namespace QuikSharp {
    /// <summary>
    /// 
    /// </summary>
    public class QuikService {
        private static Dictionary<int,QuikService> _services = 
            new Dictionary<int, QuikService>();

        public static QuikService Create(int port) {
            lock (_services) {
                QuikService service;
                if (_services.ContainsKey(port)) {
                    service = _services[port];
                    service.Start();
                } else {
                    service = new QuikService(port);
                    _services[port] = service;
                }
                return service;
            }
        }

        private QuikService(int port) {
            _port = port;
            Start();
            Events = new QuikEvents();
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsStarted { get; private set; }

        internal QuikEvents Events { get; set; }

        private readonly IPAddress _host = IPAddress.Parse("127.0.0.1");
        private readonly int _port;
        private TcpClient _client;
        private readonly Object _syncRoot = new object();
        //private static readonly List<Socket> Sockets = new List<Socket>();
        private CancellationTokenSource _cts;

        /// <summary>
        /// Current correlation id. Use Interlocked.Increment to get a new id.
        /// </summary>
        internal static long CorrelationId;
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
                            var stream = new NetworkStream(_client.Client);
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
                                        // TODO benchmark async
                                        writer.WriteLine(request);
                                        writer.Flush(); // TODO check with async
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
                        if (_client != null) {
                            _client.Client.Shutdown(SocketShutdown.Both);
                            _client.Close();
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
                            var stream = new NetworkStream(_client.Client);
                            var reader = new StreamReader(stream, Encoding.GetEncoding(1251)); //true
                            while (IsStarted) {
                                // TODO benchmark async
                                var response = await reader.ReadLineAsync();
                                //Trace.WriteLine("Response:" + response);
                                try {
                                    if (response == null) {
                                        throw new IOException("Lua returned an empty response or closed the connection");
                                    }
                                    var message = response.FromJson(this);
                                        if (message.Id.HasValue && message.Id > 0) {
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
                                        } else {
                                            // it is a callback message
                                            ProcessCallbackMessage(message);
                                        }
                                    
                                } catch (LuaException) {
                                    //Trace.WriteLine("Caught Lua exception");
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
                        if (_client != null) {
                            _client.Client.Shutdown(SocketShutdown.Both);
                            _client.Close();
                        }
                    } finally {
                        Monitor.Exit(_syncRoot);
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void EnsureConnectedClient() {
            var entered = false;
            try {
                if (_client != null && _client.Connected && _client.Client.IsConnectedNow()) {
                    // reuse alive client
                } else {
                    Monitor.Enter(_syncRoot);
                    entered = true;
                    if (!(_client != null && _client.Connected && _client.Client.IsConnectedNow())) {
                        var connected = false;
                        while (!connected) {
                            try {
                                _client = new TcpClient {
                                    ExclusiveAddressUse = true, 
                                    NoDelay = true
                                };
                                _client.Connect(_host, _port);
                                connected = true;
                            } catch {
                                //Trace.WriteLine("Trying to connect...");
                            }
                        }
                    }
                }
            } finally { if (entered) { Monitor.Exit(_syncRoot); } }
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
                        Events.OnInitCall(((Message<string>)message).Data, _port);
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
                        break;
                    case EventNames.OnParam:
                        break;
                    case EventNames.OnQuote:
                        break;
                    case EventNames.OnStop:
                        Trace.Assert(message is Message<string>);
                        Events.OnStopCall(int.Parse(((Message<string>)message).Data));
                        break;
                    case EventNames.OnStopOrder:
                        break;
                    case EventNames.OnTrade:
                        break;
                    case EventNames.OnTransReply:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } else {
                throw new NotImplementedException("Special processing for custom callbacks");
            }
        }


        internal async Task<TResponse> Send<TResponse>(IMessage request, int timeout = 0)
            where TResponse : class, IMessage, new() {
            var tcs = new TaskCompletionSource<IMessage>();
            if (timeout > 0) {
                var ct = new CancellationTokenSource(timeout);
                ct.Token.Register(() => tcs.TrySetCanceled(), false);
            }
            var kvp = new KeyValuePair<TaskCompletionSource<IMessage>, Type>(tcs, typeof(TResponse));
            if (request.Id == null) { request.Id = Interlocked.Increment(ref CorrelationId); }
            Responses[request.Id.Value] = kvp;
            // add to queue after responses dictionary
            EnvelopeQueue.Add(request);
            var response = await tcs.Task;
            return (response as TResponse);
        }

    }
}
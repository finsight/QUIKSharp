// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.IO.MemoryMappedFiles;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Diagnostics; // Для Process
using System.Runtime.InteropServices; // Для RuntimeInformation

namespace QuikSharp
{
    /// <summary>
    ///
    /// </summary>
    public sealed class QuikService
    {
        private static readonly Dictionary<int, QuikService> Services =
            new Dictionary<int, QuikService>();

        private static readonly object StaticSync = new object();
        private readonly AsyncManualResetEvent _connectedMre = new AsyncManualResetEvent();

        internal JsonSerializer Serializer;

        static QuikService()
        {
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.SustainedLowLatency;
        }

        /// <summary>
        /// For each port only one instance of QuikService
        /// </summary>
        public static QuikService Create(int port, string host)
        {
            lock (StaticSync)
            {
                QuikService service;
                if (Services.ContainsKey(port))
                {
                    service = Services[port];
                    service.Start();
                }
                else
                {
                    service = new QuikService(port, host);
                    Services.Add(port, service);
                }

                service.Serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.None,
                    NullValueHandling = NullValueHandling.Ignore
                };
                service.Serializer.Converters.Add(new MessageConverter(service));
                return service;
            }
        }

        private QuikService(int responsePort, string host)
        {
            _responsePort = responsePort;
            _callbackPort = _responsePort + 1;
            _host = IPAddress.Parse(host);
            Start();
            Events = new QuikEvents(this);
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsStarted { get; private set; }

        /// <summary>
        /// info.exe file path
        /// </summary>
        public string WorkingFolder { get; set; }

        internal QuikEvents Events { get; set; }
        internal IPersistentStorage Storage { get; set; }
        internal CandleFunctions Candles { get; set; }
        internal StopOrderFunctions StopOrders { get; set; }

        internal const int UniqueIdOffset = 0;
        internal readonly string SessionId = DateTime.Now.ToString("yyMMddHHmmss");
        internal MemoryMappedFile mmf;
        internal MemoryMappedViewAccessor accessor;

        //private readonly IPAddress _host = IPAddress.Parse("127.0.0.1");
        private readonly IPAddress _host;
        private readonly int _responsePort;
        private readonly int _callbackPort;
        private TcpClient _responseClient;
        private TcpClient _callbackClient;

        private readonly object _syncRoot = new object();

        private Task _requestTask;
        private Task _responseTask;
        private Task _callbackReceiverTask;
        private Task _callbackInvokerTask;

        private Channel<IMessage> _receivedCallbacksChannel =
            Channel.CreateUnbounded<IMessage>(new UnboundedChannelOptions()
            {
                SingleReader = true,
                SingleWriter = true
            });

        private CancellationTokenSource _cts;
        private TaskCompletionSource<bool> _cancelledTcs;
        private CancellationTokenRegistration _cancelRegistration;

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
        public void Stop()
        {
            if (!IsStarted) return;
            IsStarted = false;
            _cts.Cancel();
            _cancelRegistration.Dispose();

            try
            {
                // here all tasks must exit gracefully
                var isCleanExit = Task.WaitAll(new[] {_requestTask, _responseTask, _callbackReceiverTask, _callbackInvokerTask}, 5000);
                Trace.Assert(isCleanExit, "All tasks must finish gracefully after cancellation token is cancelled!");
            }
            finally
            {
                // cancel responses to release waiters
                foreach (var responseKey in Responses.Keys.ToList())
                {
                    if (Responses.TryRemove(responseKey, out var responseInfo))
                        responseInfo.Key.TrySetCanceled();
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <exception cref="ApplicationException">Response message id does not exists in results dictionary</exception>
        public void Start()
        {
            if (IsStarted) return;
            IsStarted = true;
            _cts?.Dispose();
            _cancelRegistration.Dispose();
            _cts = new CancellationTokenSource();
            _cancelledTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            _cancelRegistration = _cts.Token.Register(() => _cancelledTcs.TrySetResult(true));

            // Request Task
            _requestTask = Task.Factory.StartNew(() =>
                {
                    try
                    {
                        // Enter the listening loop.
                        while (!_cts.IsCancellationRequested)
                        {
                            Trace.WriteLine("Connecting on request/response channel... ");
                            EnsureConnectedClient(_cts.Token);
                            // here we have a connected TCP client
                            Trace.WriteLine("Request/response channel connected");
                            try
                            {
                                using (var stream = new NetworkStream(_responseClient.Client))
                                using (var writer = new StreamWriter(stream))
                                {
                                    while (!_cts.IsCancellationRequested)
                                    {
                                        IMessage message = null;
                                        try
                                        {
                                            // BLOCKING
                                            message = EnvelopeQueue.Take(_cts.Token);
                                            var request = message.ToJson();
                                            //Trace.WriteLine("Request: " + request);
                                            // scenario: Quik is restarted or script is stopped
                                            // then writer must throw and we will add a message back
                                            // then we will iterate over messages and cancel expired ones
                                            if (!message.ValidUntil.HasValue || message.ValidUntil >= DateTime.UtcNow)
                                            {
                                                writer.WriteLine(request);
                                                writer.Flush();
                                            }
                                            else
                                            {
                                                Trace.Assert(message.Id.HasValue, "All requests must have correlation id");
                                                Responses[message.Id.Value]
                                                    .Key.SetException(
                                                        new TimeoutException("ValidUntilUTC is less than current time"));
                                                KeyValuePair<TaskCompletionSource<IMessage>, Type> tcs; // <IMessage>
                                                Responses.TryRemove(message.Id.Value, out tcs);
                                            }
                                        }
                                        catch (OperationCanceledException)
                                        {
                                            // EnvelopeQueue.Take(_cts.Token) was cancelled via the token
                                        }
                                        catch (IOException)
                                        {
                                            // this catch is for unexpected and unchecked connection termination
                                            // add back, there was an error while writing
                                            if (message != null)
                                            {
                                                EnvelopeQueue.Add(message);
                                            }
    
                                            break;
                                        }
                                    }
                                }
                                
                            }
                            catch (IOException e)
                            {
                                Trace.TraceError(e.ToString());
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Trace.TraceInformation("Request task is cancelling");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                        _cts.Cancel();
                        throw new AggregateException("Unhandled exception in background task", e);
                    }
                    finally
                    {
                        lock (_syncRoot)
                        {
                            if (_responseClient != null)
                            {
                                _responseClient.Client.Shutdown(SocketShutdown.Both);
                                _responseClient.Close();
                                _responseClient.Dispose();
                                _responseClient =
                                    null; // У нас два потока работают с одним сокетом, но только один из них должен его закрыть !
                                Trace.WriteLine("Response channel disconnected");
                            }
                        }
                    }
                },
                CancellationToken.None, // NB we use the token for signalling, could use a simple TCS
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            // Response Task
            _responseTask = Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        while (!_cts.IsCancellationRequested)
                        {
                            // Поток Response использует тот же сокет, что и поток request
                            EnsureConnectedClient(_cts.Token);
                            // here we have a connected TCP client

                            try
                            {
                                using (var stream = new NetworkStream(_responseClient.Client))
                                using (var reader = new StreamReader(stream, Encoding.GetEncoding(1251))) //true
                                {
                                    while (!_cts.IsCancellationRequested)
                                    {
                                        var readLineTask = reader.ReadLineAsync();
                                        var completedTask = await Task.WhenAny(readLineTask, _cancelledTcs.Task).ConfigureAwait(false);
                                        if (completedTask == _cancelledTcs.Task || _cts.IsCancellationRequested)
                                        {
                                            break;
                                        }
    
                                        Trace.Assert(readLineTask.Status == TaskStatus.RanToCompletion);
                                        var response = readLineTask.Result;
                                        if (response == null)
                                        {
                                            throw new IOException("Lua returned an empty response or closed the connection");
                                        }
    
                                        // No IO exceptions possible for response, move its processing
                                        // to the threadpool and wait for the next mesaage
                                        // A new task here gives c.30% boost for full TransactionSpec echo
    
                                        // ReSharper disable once UnusedVariable
                                        var doNotAwaitMe = Task.Factory.StartNew(r =>
                                        {
                                            //var r = response;
                                            //Trace.WriteLine("Response:" + response);
                                            try
                                            {
                                                var message = (r as string).FromJson(this);
                                                Trace.Assert(message.Id.HasValue && message.Id > 0);
                                                // it is a response message
                                                if (!Responses.ContainsKey(message.Id.Value))
                                                    throw new ApplicationException("Unexpected correlation ID");
                                                KeyValuePair<TaskCompletionSource<IMessage>, Type> tcs;
                                                Responses.TryRemove(message.Id.Value, out tcs);
                                                if (!message.ValidUntil.HasValue || message.ValidUntil >= DateTime.UtcNow)
                                                {
                                                    tcs.Key.SetResult(message);
                                                }
                                                else
                                                {
                                                    tcs.Key.SetException(
                                                        new TimeoutException("ValidUntilUTC is less than current time"));
                                                }
                                            }
                                            catch (LuaException e)
                                            {
                                                Trace.TraceError(e.ToString());
                                            }
                                        }, response, TaskCreationOptions.PreferFairness);
                                    }
                                }
                                
                            }
                            catch (TaskCanceledException)
                            {
                            } // Это исключение возникнет при отмене ReadLineAsync через Cancellation Token
                            catch (IOException e)
                            {
                                Trace.TraceError(e.ToString());
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Trace.TraceInformation("Response task is cancelling");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                        _cts.Cancel();
                        throw new AggregateException("Unhandled exception in background task", e);
                    }
                    finally
                    {
                        lock (_syncRoot)
                        {
                            if (_responseClient != null)
                            {
                                _responseClient.Client.Shutdown(SocketShutdown.Both);
                                _responseClient.Close();
                                _responseClient.Dispose();
                                _responseClient = null; // У нас два потока работают с одним сокетом, но только один из них должен его закрыть !
                                Trace.WriteLine("Response channel disconnected");
                            }
                        }
                    }
                },
                CancellationToken.None, // NB we use the token for signalling, could use a simple TCS
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            // Callback Task
            _callbackReceiverTask = Task.Factory.StartNew(async () =>
                {
                    try
                    {
                        // reconnection loop
                        while (!_cts.IsCancellationRequested)
                        {
                            Trace.WriteLine("Connecting on callback channel... ");
                            EnsureConnectedClient(_cts.Token);
                            // now we are connected
                            this.Events.OnConnectedToQuikCall(_responsePort); // Оповещаем клиента что произошло подключение к Quik'у
                            _connectedMre.Set();

                            // here we have a connected TCP client
                            Trace.WriteLine("Callback channel connected");
                            try
                            {
                                using (var stream = new NetworkStream(_callbackClient.Client))
                                using (var reader = new StreamReader(stream, Encoding.GetEncoding(1251))) //true
                                {
                                    while (!_cts.IsCancellationRequested)
                                    {
                                        var readLineTask = reader.ReadLineAsync();
                                        var completedTask = await Task.WhenAny(readLineTask, _cancelledTcs.Task).ConfigureAwait(false);
    
                                        if (completedTask == _cancelledTcs.Task || _cts.IsCancellationRequested)
                                        {
                                            break;
                                        }
    
                                        Trace.Assert(readLineTask.Status == TaskStatus.RanToCompletion);
                                        var callback = readLineTask.Result;
                                        if (callback == null)
                                        {
                                            throw new IOException("Lua returned an empty response or closed the connection");
                                        }
    
                                        try
                                        {
                                            var message = callback.FromJson(this);
                                            Trace.Assert(!(message.Id.HasValue && message.Id > 0));
                                            // it is a callback message
                                            await _receivedCallbacksChannel.Writer.WriteAsync(message);
                                        }
                                        catch (Exception e) // deserialization exception is possible
                                        {
                                            Trace.TraceError(e.ToString());
                                        }
                                    }
                                }
                            }
                            catch (IOException e)
                            {
                                Trace.TraceError(e.ToString());
                                // handled exception will cause reconnect in the outer loop
                                _connectedMre.Reset();
                                this.Events.OnDisconnectedFromQuikCall();
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Trace.TraceInformation("Callback task is cancelling");
                    }
                    catch (Exception e)
                    {
                        Trace.TraceError(e.ToString());
                        _cts.Cancel();
                        throw new AggregateException("Unhandled exception in background task", e);
                    }
                    finally
                    {
                        lock (_syncRoot)
                        {
                            if (_callbackClient != null)
                            {
                                _callbackClient.Client.Shutdown(SocketShutdown.Both);
                                _callbackClient.Close();
                                _callbackClient.Dispose();
                                _callbackClient = null;
                                Trace.WriteLine("Callback channel disconnected");
                            }
                        }
                    }
                },
                CancellationToken.None, // NB we use the token for signalling, could use a simple TCS
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);

            _callbackInvokerTask = Task.Factory.StartNew(async () =>
                {
                    while (!_cts.IsCancellationRequested)
                    {
                        var cts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token);
                        try
                        {
                            var message = await _receivedCallbacksChannel.Reader.ReadAsync(cts.Token);
                            try
                            {
                                ProcessCallbackMessage(message);
                            }
                            catch (Exception e) // 
                            {
                                Trace.TraceError($"Error in callback event handler for {message.Command}:\n" + e);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                        }
                        finally
                        {
                            cts.Dispose();
                        }
                    }
                },
                CancellationToken.None, // NB we use the token for signalling, could use a simple TCS
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }

        public bool IsServiceConnected()
        {
            return (_responseClient != null && _responseClient.Connected && _responseClient.Client.IsConnectedNow())
                   && (_callbackClient != null && _callbackClient.Connected && _callbackClient.Client.IsConnectedNow());
        }

        private void EnsureConnectedClient(CancellationToken ct)
        {
            lock (_syncRoot)
            {
                var attempt = 0;
                if (!(_responseClient != null && _responseClient.Connected && _responseClient.Client.IsConnectedNow()))
                {
                    var connected = false;
                    while (!connected)
                    {
                        ct.ThrowIfCancellationRequested();
                        try
                        {
                            _responseClient?.Dispose();
                            _responseClient = new TcpClient
                            {
                                ExclusiveAddressUse = true,
                                NoDelay = true
                            };
                            _responseClient.Connect(_host, _responsePort);
                            connected = true;
                        }
                        catch
                        {
                            attempt++;
                            Thread.Sleep(100);
                            if (attempt % 10 == 0) Trace.WriteLine($"Trying to connect... {attempt}");
                        }
                    }
                }

                if (!(_callbackClient != null && _callbackClient.Connected && _callbackClient.Client.IsConnectedNow()))
                {
                    var connected = false;
                    while (!connected)
                    {
                        ct.ThrowIfCancellationRequested();
                        try
                        {
                            _callbackClient?.Dispose();
                            _callbackClient = new TcpClient
                            {
                                ExclusiveAddressUse = true,
                                NoDelay = true
                            };
                            _callbackClient.Connect(_host, _callbackPort);
                            connected = true;
                        }
                        catch
                        {
                            attempt++;
                            Thread.Sleep(100);
                            if (attempt % 10 == 0) Trace.WriteLine($"Trying to connect... {attempt}");
                        }
                    }
                }
            }
        }

        private void ProcessCallbackMessage(IMessage message)
        {
            if (message == null)
            {
                Trace.WriteLine("Trace: ProcessCallbackMessage(). message = NULL");
                throw new ArgumentNullException(nameof(message));
            }

            var parsed = Enum.TryParse(message.Command, ignoreCase: true, out EventNames eventName);
            if (parsed)
            {
                // TODO use as instead of assert+is+cast
                switch (eventName)
                {
                    case EventNames.OnAccountBalance:
                        Trace.Assert(message is Message<AccountBalance>);
                        var accBal = ((Message<AccountBalance>) message).Data;
                        Events.OnAccountBalanceCall(accBal);
                        break;

                    case EventNames.OnAccountPosition:
                        Trace.Assert(message is Message<AccountPosition>);
                        var accPos = ((Message<AccountPosition>) message).Data;
                        Events.OnAccountPositionCall(accPos);
                        break;

                    case EventNames.OnAllTrade:
                        Trace.Assert(message is Message<AllTrade>);
                        var allTrade = ((Message<AllTrade>) message).Data;
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
                        Trace.Assert(message is Message<DepoLimitEx>);
                        var dLimit = ((Message<DepoLimitEx>) message).Data;
                        Events.OnDepoLimitCall(dLimit);
                        break;

                    case EventNames.OnDepoLimitDelete:
                        Trace.Assert(message is Message<DepoLimitDelete>);
                        var dLimitDel = ((Message<DepoLimitDelete>) message).Data;
                        Events.OnDepoLimitDeleteCall(dLimitDel);
                        break;

                    case EventNames.OnDisconnected:
                        Trace.Assert(message is Message<string>);
                        Events.OnDisconnectedCall();
                        break;

                    case EventNames.OnFirm:
                        Trace.Assert(message is Message<Firm>);
                        var frm = ((Message<Firm>) message).Data;
                        Events.OnFirmCall(frm);
                        break;

                    case EventNames.OnFuturesClientHolding:
                        Trace.Assert(message is Message<FuturesClientHolding>);
                        var futPos = ((Message<FuturesClientHolding>) message).Data;
                        Events.OnFuturesClientHoldingCall(futPos);
                        break;

                    case EventNames.OnFuturesLimitChange:
                        Trace.Assert(message is Message<FuturesLimits>);
                        var futLimit = ((Message<FuturesLimits>) message).Data;
                        Events.OnFuturesLimitChangeCall(futLimit);
                        break;

                    case EventNames.OnFuturesLimitDelete:
                        Trace.Assert(message is Message<FuturesLimitDelete>);
                        var limDel = ((Message<FuturesLimitDelete>) message).Data;
                        Events.OnFuturesLimitDeleteCall(limDel);
                        break;

                    case EventNames.OnInit:
                        // Этот callback никогда не будет вызван так как на момент получения вызова OnInit в lua скрипте
                        // соединение с библиотекой QuikSharp не будет еще установлено. То есть этот callback не имеет смысла.
                        break;

                    case EventNames.OnMoneyLimit:
                        Trace.Assert(message is Message<MoneyLimitEx>);
                        var mLimit = ((Message<MoneyLimitEx>) message).Data;
                        Events.OnMoneyLimitCall(mLimit);
                        break;

                    case EventNames.OnMoneyLimitDelete:
                        Trace.Assert(message is Message<MoneyLimitDelete>);
                        var mLimitDel = ((Message<MoneyLimitDelete>) message).Data;
                        Events.OnMoneyLimitDeleteCall(mLimitDel);
                        break;

                    case EventNames.OnNegDeal:
                        break;

                    case EventNames.OnNegTrade:
                        break;

                    case EventNames.OnOrder:
                        Trace.Assert(message is Message<Order>);
                        var ord = ((Message<Order>) message).Data;
                        ord.LuaTimeStamp = message.CreatedTime;
                        Events.OnOrderCall(ord);
                        break;

                    case EventNames.OnParam:
                        Trace.Assert(message is Message<Param>);
                        var data = ((Message<Param>) message).Data;
                        Events.OnParamCall(data);
                        break;

                    case EventNames.OnQuote:
                        Trace.Assert(message is Message<OrderBook>);
                        var ob = ((Message<OrderBook>) message).Data;
                        ob.LuaTimeStamp = message.CreatedTime;
                        Events.OnQuoteCall(ob);
                        break;

                    case EventNames.OnStop:
                        Trace.Assert(message is Message<string>);
                        Events.OnStopCall(int.Parse(((Message<string>) message).Data));
                        break;

                    case EventNames.OnStopOrder:
                        Trace.Assert(message is Message<StopOrder>);
                        StopOrder stopOrder = ((Message<StopOrder>) message).Data;
                        //StopOrders.RaiseNewStopOrderEvent(stopOrder);
                        Events.OnStopOrderCall(stopOrder);
                        break;

                    case EventNames.OnTrade:
                        Trace.Assert(message is Message<Trade>);
                        var trade = ((Message<Trade>) message).Data;
                        trade.LuaTimeStamp = message.CreatedTime;
                        Events.OnTradeCall(trade);
                        break;

                    case EventNames.OnTransReply:
                        Trace.Assert(message is Message<TransactionReply>);
                        var trReply = ((Message<TransactionReply>) message).Data;
                        trReply.LuaTimeStamp = message.CreatedTime;
                        Events.OnTransReplyCall(trReply);
                        break;

                    case EventNames.NewCandle:
                        Trace.Assert(message is Message<Candle>);
                        var candle = ((Message<Candle>) message).Data;
                        Candles.RaiseNewCandleEvent(candle);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (message.Command)
                {
                    // an error from an event not request (from req is caught is response loop)
                    case "lua_error":
                        Trace.Assert(message is Message<string>);
                        Trace.TraceError(((Message<string>) message).Data);
                        break;

                    default:
                        throw new InvalidOperationException("Unknown command in a message: " + message.Command);
                }
            }
        }

        /// <summary>
        /// Generate a new unique ID for current session
        /// </summary>
        internal int GetNewUniqueId()
        {
            lock (_syncRoot)
            {
                var newId = Interlocked.Increment(ref _correlationId);
                // 2^31 = 2147483648
                // with 1 000 000 messages per second it will take more than
                // 35 hours to overflow => safe for use as TRANS_ID in SendTransaction
                // very weird stuff: Уникальный идентификационный номер заявки, значение от 1 до 2 294 967 294
                if (newId > 0)
                {
                    return newId;
                }

                _correlationId = 1;
                return 1;
            }
        }

        /// <summary>
        /// Get or Generate unique transaction ID for function SendTransaction()
        /// </summary>
        internal int GetUniqueTransactionId()
        {
            if (mmf == null || accessor == null)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    // Оригинальная логика для Windows
                    if (String.IsNullOrEmpty(WorkingFolder)) //WorkingFolder = Не определено. Создаем MMF в памяти
                    {
                        mmf = MemoryMappedFile.CreateOrOpen("UniqueID", 4096);
                    }
                    else //WorkingFolder определен. Открываем MMF с диска
                    {
                        string diskFileName = WorkingFolder + "\\" + "QUIKSharp.Settings";
                        try
                        {
                            mmf = MemoryMappedFile.CreateFromFile(diskFileName, FileMode.OpenOrCreate, "UniqueID", 4096);
                        }
                        catch
                        {
                            mmf = MemoryMappedFile.CreateOrOpen("UniqueID", 4096);
                        }
                    }
                }
                else
                {
                    // Логика для macOS/Linux: file-backed MMF без mapName
                    string filePath = !String.IsNullOrEmpty(WorkingFolder)
                        ? Path.Combine(WorkingFolder, "QUIKSharp.Settings")
                        : Path.Combine(Path.GetTempPath(), $"QUIKSharp.Settings_{Process.GetCurrentProcess().Id}.dat");

                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            if (fileStream.Length < 4096)
                            {
                                fileStream.SetLength(4096);
                            }
                            mmf = MemoryMappedFile.CreateFromFile(fileStream, null, 4096, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
                        }
                    }
                    catch
                    {
                        // Fallback на временный файл
                        filePath = Path.GetTempFileName();
                        using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            fileStream.SetLength(4096);
                            mmf = MemoryMappedFile.CreateFromFile(fileStream, null, 4096, MemoryMappedFileAccess.ReadWrite, HandleInheritability.None, true);
                        }
                    }
                }

                accessor = mmf.CreateViewAccessor();
            }

            int newId = accessor.ReadInt32(UniqueIdOffset);
            if (newId == 0)
            {
                newId = Convert.ToInt32(DateTime.Now.ToString("ddHHmmss"));
            }
            else
            {
                if (newId >= 2147483638) newId = 100;
                newId++;
            }

            try
            {
                accessor.Write(UniqueIdOffset, newId);
            }
            catch (Exception er)
            {
                Console.WriteLine("Неудачная попытка записини нового ID в файл MMF: " + er.Message);
            }

            return newId;
        }

        /// <summary>
        /// Устанавливает стартовое значение для CorrelactionId.
        /// </summary>
        /// <param name="startCorrelationId">Стартовое значение.</param>
        internal void InitializeCorrelationId(int startCorrelationId)
        {
            _correlationId = startCorrelationId;
        }

        internal string PrependWithSessionId(long id)
        {
            return SessionId + "." + id;
        }

        /// <summary>
        /// Default timeout to use for send operations if no specific timeout supplied.
        /// </summary>
        public TimeSpan DefaultSendTimeout { get; set; } = Timeout.InfiniteTimeSpan;

        internal async Task<TResponse> Send<TResponse>(IMessage request, int timeout = 0)
            where TResponse : class, IMessage, new()
        {
            // use DefaultSendTimeout for default calls
            if (timeout == 0)
                timeout = (int) DefaultSendTimeout.TotalMilliseconds;

            var task = _connectedMre.WaitAsync();
            if (timeout > 0)
            {
                if (await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false) == task)
                {
                    // task completed within timeout, do nothing
                }
                else
                {
                    // timeout
                    throw new TimeoutException("Send operation timed out");
                }
            }
            else
            {
                await task.ConfigureAwait(false);
            }

            var tcs = new TaskCompletionSource<IMessage>(TaskCreationOptions.RunContinuationsAsynchronously);
            var ctRegistration = default(CancellationTokenRegistration);

            var kvp = new KeyValuePair<TaskCompletionSource<IMessage>, Type>(tcs, typeof(TResponse));
            if (request.Id == null)
            {
                request.Id = GetNewUniqueId();
            }

            CancellationTokenSource ct = null;
            if (timeout > 0)
            {
                ct = new CancellationTokenSource();
                ctRegistration = ct.Token.Register(() =>
                {
                    tcs.TrySetException(new TimeoutException("Send operation timed out"));
                    KeyValuePair<TaskCompletionSource<IMessage>, Type> temp;
                    Responses.TryRemove(request.Id.Value, out temp);
                }, false);

                ct.CancelAfter(timeout);
            }

            Responses[request.Id.Value] = kvp;
            // add to queue after responses dictionary
            EnvelopeQueue.Add(request);
            IMessage response;

            try
            {
                response = await tcs.Task.ConfigureAwait(false);
            }
            finally
            {
	            if (timeout > 0)
	            {
		            ct?.Dispose();
		            ctRegistration.Dispose();
	            }
            }

            return (response as TResponse);
        }
    }
}
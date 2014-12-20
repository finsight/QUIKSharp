using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

// TODOs
// http://stackoverflow.com/questions/1119841/net-console-application-exit-event
// A message queue could be easily persisted with Spreads's Stream type - actually a good
// use case for Rx instead of a blocking queue
//


namespace QuikSharp {
    /// <summary>
    /// 
    /// </summary>
    public static class QuikService {
        // auto start when referenced for the first time
        //static QuikService() {
        //    Start();
        //}


        /// <summary>
        /// 
        /// </summary>
        public static bool Started { get; private set; }

        private static readonly IPAddress Host = IPAddress.Parse("127.0.0.1");
        private const int _PORT = 34130;
        private static TcpClient _client;
        private static readonly Object SyncRoot = new object();
        //private static readonly List<Socket> Sockets = new List<Socket>();
        private static CancellationTokenSource _cts;

        /// <summary>
        /// Current correlation id. Use Interlocked.Increment to get a new id.
        /// </summary>
        internal static long CorrelationId = 0;
        /// <summary>
        /// IQuickCalls functions enqueue a message and return a task from TCS
        /// </summary>
        internal static readonly BlockingCollection<IMessage> EnvelopeQueue
            = new BlockingCollection<IMessage>();
        /// <summary>
        /// If received message has a correlation id then use its Data to SetResult on TCS and remove the TCS from the dic
        /// </summary>
        internal static readonly ConcurrentDictionary<long, KeyValuePair<TaskCompletionSource<IMessage>, Type>>
            Responses = new ConcurrentDictionary<long, KeyValuePair<TaskCompletionSource<IMessage>, Type>>();

        /// <summary>
        /// 
        /// </summary>
        public static void Stop() {
            if (!Started) return;
            Started = false;
            _cts.Cancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ApplicationException">Response message id does not exists in results dictionary</exception>
        public static void Start() {
            if (Started) return;
            Started = true;
            _cts = new CancellationTokenSource();

            // Request Task
            Task.Factory.StartNew(async () => {
                try {
                    // Enter the listening loop. 
                    while (Started) {
                        Trace.WriteLine("Connecting on request channel... ");
                        EnsureConnectedClient();
                        // here we have a connected TCP client
                        Trace.WriteLine("Request channel connected");
                        try {
                            var stream = new NetworkStream(_client.Client);
                            var writer = new StreamWriter(stream);
                            while (Started) {
                                // BLOCKING
                                var message = EnvelopeQueue.Take(_cts.Token);
                                try {
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
                                    EnvelopeQueue.Add(message);
                                    break;
                                }
                            }
                        } catch (IOException) {
                            Trace.WriteLine("Request channel IOException in unexpected place");
                        }
                    }
                } catch (Exception e) {
                    //Trace.WriteLine(e);
                } finally {
                    try {
                        Monitor.Enter(SyncRoot);
                        if (_client != null) {
                            _client.Client.Shutdown(SocketShutdown.Both);
                            _client.Close();
                        }
                    } finally {
                        Monitor.Exit(SyncRoot);
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);


            // Response Task
            Task.Factory.StartNew(async () => {
                try {

                    while (Started) {
                        Trace.WriteLine("Connecting on response channel... ");
                        EnsureConnectedClient();
                        // here we have a connected TCP client
                        Trace.WriteLine("Response channel connected");
                        try {
                            var stream = new NetworkStream(_client.Client);
                            var reader = new StreamReader(stream, true);
                            while (Started) {
                                // TODO benchmark async
                                var response = await reader.ReadLineAsync();
                                //Trace.WriteLine("Response:" + response);
                                try {
                                    var message = response.FromJson();
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
                        } catch (IOException) {
                            Trace.WriteLine("Response channel IOException in unexpected place");
                        }
                    }
                } catch (Exception e) {
                    Trace.WriteLine(e);
                } finally {
                    try {
                        Monitor.Enter(SyncRoot);
                        if (_client != null) {
                            _client.Client.Shutdown(SocketShutdown.Both);
                            _client.Close();
                        }
                    } finally {
                        Monitor.Exit(SyncRoot);
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);


            //Task.Factory.StartNew(() => {
            //    Task.WaitAll(requestTask, responseTask);
            //});

        }

        private static void EnsureConnectedClient() {
            try {

                if (_client != null && _client.Connected && _client.Client.IsConnectedNow()) {
                    // reuse connected client
                } else {
                    Monitor.Enter(SyncRoot);
                    if (!(_client != null && _client.Connected && _client.Client.IsConnectedNow())) {
                        // create a new client
                        _client = new TcpClient();
                        // TODO profile with different options
                        //_client.ExclusiveAddressUse = true;
                        //_client.NoDelay = true;

                        _client.Connect(Host, _PORT);
                        
                    }
                }
            } finally {
                Monitor.Exit(SyncRoot);
            }
        }

        private static void ProcessCallbackMessage(IMessage message) {
            if (message == null) throw new ArgumentNullException("message");
            throw new NotImplementedException("Special processing for callbacks");
        }
    }
}
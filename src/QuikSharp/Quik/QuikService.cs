using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

// TODOs
// http://stackoverflow.com/questions/1119841/net-console-application-exit-event
// A message queue could be easily persisted with Spreads's Stream type - actually a good
// use case for Rx instead of a blocking queue
//


namespace QuikSharp.Quik {
    /// <summary>
    /// 
    /// </summary>
    public static class QuikService {

        /// <summary>
        /// 
        /// </summary>
        public static bool Started { get; private set; }

        const int _REQUEST_PORT = 34130;
        const int _RESPONSE_PORT = 34131;

        private static TcpListener _requestChannel;
        private static TcpListener _responseChannel;
        private static readonly List<Socket> Sockets = new List<Socket>();
        private static CancellationTokenSource _cts;

        /// <summary>
        /// Current correlation id. Use Interlocked.Increment to get a new id.
        /// </summary>
        internal static long CorrelationId = 0;
        /// <summary>
        /// IQuickCalls functions enqueue a message and return a task from TCS
        /// </summary>
        internal static readonly BlockingCollection<Envelope> EnvelopeQueue //<IMessage>
            = new BlockingCollection<Envelope>(); //<IMessage>
        /// <summary>
        /// If received message has a correlation id then use its Data to SetResult on TCS and remove the TCS from the dic
        /// </summary>
        internal static readonly ConcurrentDictionary<long, TaskCompletionSource<Envelope>> //<IMessage>
            Responses = new ConcurrentDictionary<long, TaskCompletionSource<Envelope>>(); //<IMessage>

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
                    var localAddr = IPAddress.Parse("127.0.0.1");
                    _requestChannel = new TcpListener(localAddr, _REQUEST_PORT);

                    _requestChannel.Server.SetSocketOption(
                        SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                    // Start listening for client requests.
                    _requestChannel.Start(100);

                    Console.WriteLine("Quik request service started");

                    // Enter the listening loop. 
                    while (Started) {
                        Console.WriteLine("Waiting for a Quik connection on request channel... ");

                        var socket = await _requestChannel.AcceptSocketAsync();
                        Sockets.Add(socket);

                        // ReSharper disable once UnusedVariable
                        var messageTaskDoNotAwait = Task.Factory.StartNew(async s => {
                            Trace.WriteLine("Request channel connected");
                            try {
                                var stream = new NetworkStream((Socket)s);
                                var writer = new StreamWriter(stream);
                                while (Started) {
                                    // BLOCKING
                                    var message = EnvelopeQueue.Take(_cts.Token);
                                    try {
                                        var request = JsonConvert.SerializeObject(message, Formatting.None, new JsonSerializerSettings {
                                            TypeNameHandling = TypeNameHandling.None, // Objects
                                            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                                        });
                                        //Trace.WriteLine("Request: " + request);
                                        // scenario: Quik is restarted or script is stopped
                                        // then writer must throw and we will add a message back
                                        // then we will iterate over messages and cancel expired ones
                                        if (!message.ValidUntil.HasValue || message.ValidUntil >= DateTime.UtcNow) {
                                            // TODO benchmark async
                                            await writer.WriteLineAsync(request);
                                            await writer.FlushAsync(); // TODO check that it is not needed
                                        } else {
                                            Trace.Assert(message.Id.HasValue, "All requests must have correlation id");
                                            Responses[message.Id.Value].SetException(new TimeoutException("ValidUntilUTC is less than current time"));
                                            TaskCompletionSource<Envelope> tcs; // <IMessage>
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
                                Trace.WriteLine("Connection closed");
                            } finally {
                                ((Socket)s).Close();
                                Sockets.Remove((Socket)s);
                                Trace.WriteLine("Connection closed");
                            }
                        }, socket, TaskCreationOptions.LongRunning);
                    }

                } finally {
                    _requestChannel.Stop();
                    foreach (var socket in Sockets) {
                        try {
                            socket.Close();
                        } catch {
                            // ReSharper disable once EmptyStatement
                            ;
                        }
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);


            // Response Task
            Task.Factory.StartNew(async () => {
                try {
                    var localAddr = IPAddress.Parse("127.0.0.1");
                    _responseChannel = new TcpListener(localAddr, _RESPONSE_PORT);

                    _responseChannel.Server.SetSocketOption(
                        SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                    // Start listening for client requests.
                    _responseChannel.Start(100);

                    Console.WriteLine("Quik response service started");

                    // Enter the listening loop. 
                    while (Started) {
                        Console.WriteLine("Waiting for a Quik connection on response channel... ");

                        var socket = await _responseChannel.AcceptSocketAsync();
                        Sockets.Add(socket);

                        // ReSharper disable once UnusedVariable
                        var messageTaskDoNotAwait = Task.Factory.StartNew(async s => {
                            Trace.WriteLine("Response channel connected");
                            try {
                                var stream = new NetworkStream((Socket)s);
                                var reader = new StreamReader(stream, true);
                                while (Started) {
                                    // TODO benchmark async
                                    var response = await reader.ReadLineAsync();
                                    //Trace.WriteLine("Response:" + response);
                                    var message =
                                      JsonConvert.DeserializeObject<Envelope>(response, new JsonSerializerSettings { //<IMessage>
                                          TypeNameHandling = TypeNameHandling.None // .Objects
                                      });
                                    if (message.Id.HasValue && message.Id > 0) {
                                        // it is a response message
                                        if (!Responses.ContainsKey(message.Id.Value)) throw new ApplicationException("Unexpected correlation ID");
                                        TaskCompletionSource<Envelope> tcs; //<IMessage>
                                        Responses.TryRemove(message.Id.Value, out tcs);
                                        if (!message.ValidUntil.HasValue || message.ValidUntil >= DateTime.UtcNow) {
                                            tcs.SetResult(message);
                                        } else {
                                            tcs.SetException(new TimeoutException("ValidUntilUTC is less than current time"));
                                        }
                                    } else {
                                        // it is a callback message
                                        ProcessCallbackMessage(message);
                                    }
                                }
                            } catch (IOException) {
                                Trace.WriteLine("Connection closed");
                            } finally {
                                ((Socket)s).Close();
                                Sockets.Remove((Socket)s);
                                Trace.WriteLine("Connection closed");
                            }
                        }, socket, TaskCreationOptions.LongRunning);
                    }

                } finally {
                    _responseChannel.Stop();
                    foreach (var socket in Sockets) {
                        try {
                            socket.Close();
                        } catch {
                            // ReSharper disable once EmptyStatement
                            ;
                        }
                    }
                }
            }, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private static void ProcessCallbackMessage(Envelope message) { //<IMessage>
            throw new NotImplementedException("Special processing for callbacks");
        }

    }
}
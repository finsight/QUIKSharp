using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace QuikSharp.Quik {
    public class QuikCallbackProxy : IClientsCallbackProxy<IQuikCallback> {
        static QuikCallbackProxy() {
            StartCallbackServer();
        }

        private static TcpListener _server;
        private static readonly List<Socket> Sockets = new List<Socket>();
        //private static Task _sendCommandTask;
        private static CancellationTokenSource _cts;

        private static void StartCallbackServer() {

            Task.Factory.StartNew(() => {

                try {
                    const int port = 34131;
                    var localAddr = IPAddress.Parse("127.0.0.1");
                    _server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    _server.Start(100);

                    Console.WriteLine("Quik callback server started");

                    //_sendCommandTask = new Task(SendCommands, _cts.Token);

                    // Enter the listening loop. 
                    while (QuikService.DoWork) {
                        Console.WriteLine("Waiting for a Quik callback client connection... ");

                        // Perform a blocking call to accept requests. 
                        var socket = _server.AcceptSocket();
                        Sockets.Add(socket);

                        // garbage collect sockets

                        foreach (var socket1 in Sockets.ToList().Where(socket1 => !socket1.IsReallyConnected())) {
                            socket1.Close();
                            Sockets.Remove(socket1);
                        }

                        if (Sockets.Count == 1) {
                            if (_cts != null && _cts.Token.IsCancellationRequested == false) _cts.Cancel();
                            _cts = new CancellationTokenSource();
                            Task.Factory.StartNew(SendCommands, _cts.Token);
                        }

                        //if (_sendCommandTask.Status == TaskStatus.RanToCompletion)
                        //    _sendCommandTask = new Task(SendCommands, _cts.Token);

                        //if (_sendCommandTask.Status != TaskStatus.Running)
                        //    _sendCommandTask.Start();

                    }

                } catch (Exception e) {
                    Console.WriteLine("Unexpected error: {0}", e);
                } finally {
                    if (_cts != null) _cts.Cancel();
                    _server.Stop();
                    foreach (var socket in Sockets) {
                        try {
                            socket.Close();
                        } catch {
                            // ReSharper disable once EmptyStatement
                            ;
                        }
                    }
                }

            });


        }


        public static void SendCommands() {
            Thread.Sleep(1000);

            var i = 0;
            while (Sockets.Count > 0) {
                // ReSharper disable once UnusedVariable
                var res = CallClients("Server request #" + i);
                i++;
            }
            Console.WriteLine("No connected callback clients!");

        }

        private static List<string> CallClients(string line) {
            var res = new List<string>();
            foreach (var socket in Sockets) {
                try {
                    lock (socket) {
                        if (!socket.IsReallyConnected()) {
                            throw new IOException("Socket is disconnected");
                        }

                        var stream = new NetworkStream(socket); // .GetStream();

                        var reader = new StreamReader(stream, true);
                        var writer = new StreamWriter(stream);

                        Debug.Assert((socket).Available == 0); // should not interrupt clients when they have something to say

                        Console.WriteLine("Calling client");
                        writer.WriteLine(line);
                        writer.Flush();

                        Console.WriteLine("Reading client callback response");
                        var response = reader.ReadLine();
                        res.Add(response);
                        Console.WriteLine("Client response: " + response);
                    }

                } catch {
                    Console.WriteLine("Connection closed");
                    socket.Close();
                    Sockets.Remove(socket);
                    if (Sockets.Count == 0)
                        _cts.Cancel();
                }
            }

            return res;
        }



        public TRes Call<TRes>(Func<IQuikCallback, TRes> f) {
            throw new NotImplementedException();
        }

        public void Call(Action<IQuikCallback> f) {
            throw new NotImplementedException();
        }
    }
}
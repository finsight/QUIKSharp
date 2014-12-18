using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuikSharp.Quik {
    public class QuikService : IQuikService, IDisposable {
        static QuikService() {
            StartServiceServer();
            DoWork = true;
        }

        public static bool DoWork { get; set; }

        private static TcpListener _server;
        private static readonly List<Socket> Sockets = new List<Socket>();

        private static readonly IClientsCallbackProxy<IQuikCallback> Callback = new QuikCallbackProxy();

        private static void StartServiceServer() {

            Task.Factory.StartNew(() => {

                try {
                    const int port = 34130;
                    var localAddr = IPAddress.Parse("127.0.0.1");
                    _server = new TcpListener(localAddr, port);

                    //_server.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                    // Start listening for client requests.
                    _server.Start(100);

                    Console.WriteLine("Quik service started");

                    // Enter the listening loop. 
                    while (DoWork) {
                        Console.WriteLine("Waiting for a Quik connection... ");

                        // Perform a blocking call to accept requests. 
                        var socket = _server.AcceptSocket();
                        Sockets.Add(socket);

                        Task.Factory.StartNew(s => {

                            Console.WriteLine("Connected!");
                            try {
                                var stream = new NetworkStream((Socket)s); // .GetStream();

                                var reader = new StreamReader(stream, true);
                                var writer = new StreamWriter(stream);
                                //writer.AutoFlush = true;

                                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                                while (DoWork) {

                                    lock (socket) // if there is any incoming value then lock stream
                                    {
                                        //if (((Socket)s).Available == 0) continue;

                                        if (!((Socket)s).IsReallyConnected()) {
                                            throw new IOException("Socket is disconnected");
                                        }

                                        Console.WriteLine("Reading service call");
                                        var request = reader.ReadLine(); // blocking

                                        var response = string.IsNullOrWhiteSpace(request)
                                            ? string.Empty
                                            : ProcessIncomingLine(request);

                                        Console.WriteLine("Replying to service call");
                                        writer.WriteLine(response); //write line
                                        writer.Flush();
                                    }
                                }
                            } catch {
                                Console.WriteLine("Connection closed");
                            } finally {
                                ((Socket)s).Close();
                                Sockets.Remove((Socket)s);
                            }
                        }, socket);
                    }

                } finally {
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

            //ConnectToServer(); 
        }


        // test service
        /*
                private static void ConnectToServer()
                {
                    Task.Factory.StartNew(() =>
                    {
                        var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 34130);

                        var client = new TcpClient();
                        client.Connect(endPoint);

                        var stream = client.GetStream();

                        var reader = new StreamReader(stream);
                        var writer = new StreamWriter(stream);
                        //writer.AutoFlush = true;

                        int i = 0;

                        while (true)
                        {
                            i++;
                            writer.WriteLine("Request #" + i);
                            writer.Flush();
                            reader.ReadLine();

                            //Thread.Sleep(2000);
                        }

                        // ReSharper disable once FunctionNeverReturns
                    });

                }
        */



        private static string ProcessIncomingLine(string line) {
            Console.WriteLine("Received: " + line);
            return "Response: " + line;
        }

        public IClientsCallbackProxy<IQuikCallback> ClientsCallbackProxy {
            get {
                return Callback;
            }
        }


        public void OnInit(string scriptPath) {
            MessageBox.Show("Quik started");
        }

        public void OnStop(int stopFlag) {
            throw new NotImplementedException();
        }

        public void Dispose() {
            throw new NotImplementedException();
        }
    }
}

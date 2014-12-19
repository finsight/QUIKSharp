using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuikSharp.Quik {
    /// <summary>
    /// 
    /// </summary>
    public class QuikCallbackService : IQuikCallbacks, IDisposable {
        static QuikCallbackService() {
            StartServiceServer();
        }

        

        private static TcpListener _server;
        private static readonly List<Socket> Sockets = new List<Socket>();

        private static void StartServiceServer() {

           

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

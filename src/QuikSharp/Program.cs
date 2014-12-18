using System;

namespace QuikSharp
{
    static class Program
    {

        static void Main(string[] args)
        {

            //var listeningOn = args.Length == 0 ? "http://localhost:12345/" : args[0];
            //var appHost = new AppHost();
            //appHost.Init();
            //appHost.Start(listeningOn);

            //Console.WriteLine("AppHost Created at {0}, listening on {1}", DateTime.Now, listeningOn);


            ServiceManager.StartServices();
            Console.WriteLine("Services are available. " +
                  "Press <ENTER> to exit.");
            Console.ReadLine();
            ServiceManager.StopServices();

        }
    }



}
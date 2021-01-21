using System;
using System.Diagnostics;
using System.Threading.Tasks;
using QuikSharp;
using QuikSharp.DataStructures.Transaction;

namespace Profiler {
    public class Program {

        public static void Ping() {
            var _df = new DebugFunctions(Quik.DefaultPort, Quik.DefaultHost);

            var sw = new Stopwatch();
            Console.WriteLine("Started");
            for (int round = 0; round < 10; round++) {
                sw.Reset();
                sw.Start();

                var count = 10000;
                var array = new Task<string>[count];
                for (int i = 0; i < array.Length; i++) {
                    array[i] = _df.Ping();
                }
                for (int i = 0; i < array.Length; i++) {
                    var pong = array[i].Result;
                    array[i] = null;
                    Trace.Assert(pong == "Pong");
                }

                //for (var i = 0; i < count; i++) {
                //    var pong = qc.Ping().Result;
                //    Trace.Assert(pong == "Pong");
                //}
                sw.Stop();
                Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
            }
            Console.WriteLine("Finished");
            Console.ReadKey();
        }


        public static void EchoTransaction() {
            var _df = new DebugFunctions(Quik.DefaultPort, Quik.DefaultHost);

            var sw = new Stopwatch();
            Console.WriteLine("Started");
            for (int round = 0; round < 10; round++) {
                sw.Reset();
                sw.Start();

                var count = 10000;
                var t = new Transaction();

                var array = new Task<Transaction>[count];
                for (int i = 0; i < array.Length; i++) {
                    array[i] = _df.Echo(t);
                }
                for (int i = 0; i < array.Length; i++) {
                    var res = array[i].Result;
                    array[i] = null;
                }

                sw.Stop();
                Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
            }
            Console.WriteLine("Finished");
            Console.ReadKey();
        }

        public static void Main() {
            Ping();
            //EchoTransaction();
        }
    }
}

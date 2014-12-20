using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuikSharp.Quik;

namespace Profiler {
    public class Program {
        public static void Main() {
            var qc = new DebugFunctions();
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < 1000000; i++) {
                var pong = qc.Ping().Result;
            }
            sw.Stop();
            Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
            Console.ReadKey();
        }
    }
}

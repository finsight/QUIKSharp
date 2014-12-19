using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QuikSharp.Quik;

namespace QuikSharp.Tests
{
    [TestFixture]
    public class Ping
    {
        [Test]
        public void PingWorks() {
            var qc = new QuikCalls();
            var pong = qc.Ping().Result;
        }

        [Test]
        public void MultiPing() {
            var qc = new QuikCalls();
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 5000; i++) {
                var pong = qc.Ping().Result;
            }
            sw.Stop();
            Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
        }
    }
}

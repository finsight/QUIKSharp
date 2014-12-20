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
        readonly DebugFunctions _df = new DebugFunctions();

        [Test]
        public void PingWorks() {
            var pong = _df.Ping().Result;
        }

        /// <summary>
        /// Nice error messages and error location in lua
        /// </summary>
        [Test]
        public void DivideStringByZero() {
            //Assert.Throws<AggregateException>(() => {
                var x = _df.DivideStringByZero().Result;
            //});
        }

        [Test]
        public void MultiPing() {
            var sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 10000; i++) {
                var pong = _df.Ping().Result;
            }
            sw.Stop();
            Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
        }
    }
}

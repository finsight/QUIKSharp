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
            // Without multiplexing we wait for Lua and latency on each trip
            // Profiling shows that it is 73% of total time

            var sw = new Stopwatch();
            Console.WriteLine("Started");
            for (int round = 0; round < 10; round++) {
                sw.Reset();
                sw.Start();

                var count = 10000;

                //var array = new Task<string>[count];
                //for (int i = 0; i < array.Length; i++) {
                //    array[i] = _df.Ping();
                //}
                //for (int i = 0; i < array.Length; i++) {
                //    var pong = array[i].Result;
                //    array[i] = null;
                //    Trace.Assert(pong == "Pong");
                //}

                for (var i = 0; i < count; i++) {
                    var pong = _df.Ping().Result;
                    Trace.Assert(pong == "Pong");
                }
                sw.Stop();
                Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
            }
        }

        [Test]
        public void MultiPingFast() {

            // Multiplexing in action
            // We access results asyncronously and do not wait
            // for round trips to Lua and back on each ping
            // Task.Result takes 43% and stream reader takes 7% (of 2x reduced time!)
            // We are bound by Lua here
            // Average time 500 msec per 5000 calls or 10 calls per msec or 100 microseconds per call
            // Warmed up standalone profile takes on average 800 msec per 10000 calls, 12.5 calls per msec or 80 micros
            // Quik is c.2x times slower 
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
        }

    }
}

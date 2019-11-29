using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace QuikSharp.Tests
{
    [TestFixture]
    public class Ping
    {
        readonly DebugFunctions _df = new DebugFunctions(Quik.DefaultPort, Quik.DefaultHost);

        [Test]
        public void PingWorks() {
            var df = new DebugFunctions(Quik.DefaultPort, Quik.DefaultHost);
            var pong = df.Ping().Result;
        }

        [Test]
        public void EchoWorks() {
            var echo = _df.Echo("echo").Result;
            Assert.AreEqual("echo", echo);
        }

        [Test]
        public void IsQuik() {
            Console.WriteLine(_df.IsQuik().Result);
        }

        /// <summary>
        /// Nice error messages and error location in lua
        /// </summary>
        [Test]
        public void DivideStringByZero() {
            Assert.Throws<AggregateException>(() => {
                var x = _df.DivideStringByZero().Result;
            });
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

				/* for (var i = 0; i < count; i++) {
					 var pong = _df.Ping().Result;
					 Trace.Assert(pong == "Pong");
				 }*/

				for (var i = 0; i < count; i++)
					_df.Ping ().Wait ();

				sw.Stop();
                Console.WriteLine("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
            }
        }

		[Test]
		public async void MultiPingFast2 ()
		{
			var sw = new Stopwatch ();
			Console.WriteLine ("Started");
			for (int round = 0; round < 10; round++)
			{
				sw.Reset ();
				sw.Start ();

				var count = 10000;
				var array = new Task<string> [count];
				for (int i = 0; i < array.Length; i++)
					array [i] = _df.Ping ();

				// Чудесным образом данная конструкция работает чуточку быстрей (на 10% где то) чем _df.Ping ().Wait (); в функции MultiPing
				await Task.WhenAll (array);
				sw.Stop ();
				Console.WriteLine ("MultiPing takes msecs: " + sw.ElapsedMilliseconds);
			}
		}

		[Test]
        public void MultiPingFast() {

            // Multiplexing in action
            // We access results asyncronously and do not wait
            // for round trips to Lua and back on each ping
            // Task.Result takes 43% and stream reader takes 7% (of 2x reduced time!)
            // We are bound by Lua and pure sockets on locahost here

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

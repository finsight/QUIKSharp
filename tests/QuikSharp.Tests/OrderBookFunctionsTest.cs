using System;
using NUnit.Framework;

namespace QuikSharp.Tests {
    [TestFixture]
    public class OrderBookFunctionsTest {
        public OrderBookFunctionsTest() { _isQuik = _q.Debug.IsQuik().Result; }
        private Quik _q = new Quik();
        private bool _isQuik;

        [Test]
        public void Subscribe_Level_II_Quotes() {
            Console.WriteLine("Subscribe_Level_II_Quotes: "
                + String.Join(",", _q.OrderBook.Subscribe("SPBFUT", "RIH5").Result));
        }

        [Test]
        public void Unsubscribe_Level_II_Quotes() {
            Console.WriteLine("Unsubscribe_Level_II_Quotes: "
                + String.Join(",", _q.OrderBook.Unsubscribe("SPBFUT", "RIH5").Result));
        }

        [Test]
        public void IsSubscribed_Level_II_Quotes() {
            Console.WriteLine("IsSubscribed_Level_II_Quotes: "
                + String.Join(",", _q.OrderBook.IsSubscribed("SPBFUT", "RIH5").Result));
        }


    }
}

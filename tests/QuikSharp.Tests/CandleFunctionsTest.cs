using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using QuikSharp.DataStructures;

namespace QuikSharp.Tests
{
    [TestFixture]
    public class CandleFunctionsTest
    {
        [Test]
        public void GetCandlesTest()
        {
            Quik quik = new Quik();
            string graphicTag = "SBER2M";//В квике должен быть открыт график с этим (SBER2M) идентификатором.

            List<Candle> allCandles = quik.Candles.GetAllCandles(graphicTag).Result;
            Console.WriteLine("All candles count: " + allCandles.Count);
            
            List<Candle> partCandles = quik.Candles.GetCandles(graphicTag, 0, 100, 250).Result;
            Console.WriteLine("Part candles count:" + partCandles.Count);
        }

        [Test]
        public void CandlesSubscriptionTest()
        {
            Quik quik = new Quik();
            quik.Candles.NewCandle += OnNewCandle;

            bool isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            Assert.AreEqual(false, isSubscribed);

            quik.Candles.Subscribe("TQBR", "SBER", CandleInterval.M1);
            isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            Assert.AreEqual(true, isSubscribed);

            quik.Candles.Unsubscribe("TQBR", "SBER", CandleInterval.M1);
            isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            Assert.AreEqual(false, isSubscribed);

            Thread.Sleep(120000);//must get at leat one candle as use minute timeframe
        }

        private void OnNewCandle(Candle candle)
        {
            if (candle.Sec == "SBER" && candle.Class == "TQBR" && candle.Interval == CandleInterval.M1)
            {
                Console.WriteLine("Sec:{0}, Open:{1}, Close:{2}, Volume:{3}", candle.Sec, candle.Open, candle.Close, candle.Volume);
            }
        }
    }
}

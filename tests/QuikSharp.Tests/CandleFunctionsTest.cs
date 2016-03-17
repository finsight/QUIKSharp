using System;
using System.Collections.Generic;
<<<<<<< HEAD
=======
using System.Diagnostics;
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
using System.Threading;
using NUnit.Framework;
using QuikSharp.DataStructures;

namespace QuikSharp.Tests
{
    [TestFixture]
    public class CandleFunctionsTest
    {
<<<<<<< HEAD
        

=======
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
        [Test]
        public void GetCandlesTest()
        {
            CandleFunctions _cf = new CandleFunctions(Quik.DefaultPort);

            //Quik quik = new Quik();
            string graphicTag = "RIU5M1";//В квике должен быть открыт график с этим (SBER2M) идентификатором.

            List<Candle> allCandles = _cf.GetAllCandles(graphicTag).Result;
            Console.WriteLine("All candles count: " + allCandles.Count);
            
            List<Candle> partCandles = _cf.GetCandles(graphicTag, 0, 100, 250).Result;
            Console.WriteLine("Part candles count:" + partCandles.Count);
        }

        [Test]
<<<<<<< HEAD
=======
        public void GetAllCandlesTest()
        {
            Quik quik = new Quik();

            //Получаем месячные свечки по инструменту "Северсталь"
            List<Candle> candles = quik.Candles.GetAllCandles("TQBR", "CHMF", CandleInterval.MN).Result;
            Trace.WriteLine("Candles count: " + candles.Count);
        }

        [Test]
        public void GetLastCandlesTest()
        {
            Quik quik = new Quik();

            int Days = 7;
            List<Candle> candles = quik.Candles.GetLastCandles("TQBR", "SBER", CandleInterval.D1, Days).Result;
            Assert.AreEqual(Days, candles.Count);

            Days = 77;
            candles = quik.Candles.GetLastCandles("TQBR", "SBER", CandleInterval.D1, Days).Result;
            Assert.AreEqual(Days, candles.Count);

            Days = 1;
            candles = quik.Candles.GetLastCandles("TQBR", "SBER", CandleInterval.D1, Days).Result;
            Assert.AreEqual(Days, candles.Count);
        }

        [Test]
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
        public void CandlesSubscriptionTest()
        {
            Quik quik = new Quik();
            quik.Candles.NewCandle += OnNewCandle;

            bool isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            Assert.AreEqual(false, isSubscribed);

            quik.Candles.Subscribe("TQBR", "SBER", CandleInterval.M1);
            isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            Assert.AreEqual(true, isSubscribed);

            //quik.Candles.Unsubscribe("TQBR", "SBER", CandleInterval.M1);
            //isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            //Assert.AreEqual(false, isSubscribed);

            Thread.Sleep(120000);//must get at leat one candle as use minute timeframe
        }

        private void OnNewCandle(Candle candle)
        {
<<<<<<< HEAD
            if (candle.Sec == "SBER" && candle.Class == "TQBR" && candle.Interval == CandleInterval.M1)
            {
                Console.WriteLine("Sec:{0}, Open:{1}, Close:{2}, Volume:{3}", candle.Sec, candle.Open, candle.Close, candle.Volume);
=======
            if (candle.SecCode == "SBER" && candle.ClassCode == "TQBR" && candle.Interval == CandleInterval.M1)
            {
                Console.WriteLine("Sec:{0}, Open:{1}, Close:{2}, Volume:{3}", candle.SecCode, candle.Open, candle.Close, candle.Volume);
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
            }
        }
    }
}

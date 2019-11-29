using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            CandleFunctions _cf = new CandleFunctions(Quik.DefaultPort, Quik.DefaultHost);

            //Quik quik = new Quik();
            string graphicTag = "RIU5M1";//В квике должен быть открыт график с этим (SBER2M) идентификатором.

            List<Candle> allCandles = _cf.GetAllCandles(graphicTag).Result;
            Console.WriteLine("All candles count: " + allCandles.Count);
            
            List<Candle> partCandles = _cf.GetCandles(graphicTag, 0, 100, 250).Result;
            Console.WriteLine("Part candles count:" + partCandles.Count);
        }

        [Test]
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
        public void CandlesSubscriptionTest()
        {
            Quik quik = new Quik();
            quik.Candles.NewCandle += OnNewCandle;

			// На всякий случай вначале нужно отписатся (иначе может вылететь Assert)
			// TODO: Вообще у библиотеки огромная проблема - Lua скрипт не отписывается от того к чему он подписался при отключении клиента.
			// В результате при следующем подключении клиент начинает получать сразу кучу CallBack'ов, на которые он не подписывался в текущей сессии.
			// По большому счету сейчас клиент должен сам заботаться о том, что бы гарантированно отписываться от всего к чему подписался при выходе.
            bool isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
			if (isSubscribed)
				quik.Candles.Unsubscribe ("TQBR", "SBER", CandleInterval.M1).Wait ();

			// Проверяем что мы действительно отписались
			isSubscribed = quik.Candles.IsSubscribed ("TQBR", "SBER", CandleInterval.M1).Result;
			Assert.AreEqual(false, isSubscribed);

            quik.Candles.Subscribe("TQBR", "SBER", CandleInterval.M1).Wait ();
            isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
            Assert.AreEqual(true, isSubscribed);

			// Раскомментарить если необходимо получать данные в функции OnNewCandle 2 минуты. В течении этих двух минут должна прийти еще одна свечка
			//Thread.Sleep(120000);//must get at leat one candle as use minute timeframe

			quik.Candles.Unsubscribe("TQBR", "SBER", CandleInterval.M1).Wait ();
			isSubscribed = quik.Candles.IsSubscribed("TQBR", "SBER", CandleInterval.M1).Result;
			Assert.AreEqual(false, isSubscribed);


		}

		private void OnNewCandle(Candle candle)
        {
            if (candle.SecCode == "SBER" && candle.ClassCode == "TQBR" && candle.Interval == CandleInterval.M1)
            {
                Console.WriteLine("Sec:{0}, Open:{1}, Close:{2}, Volume:{3}", candle.SecCode, candle.Open, candle.Close, candle.Volume);
            }
        }
    }
}

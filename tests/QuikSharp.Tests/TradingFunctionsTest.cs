using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using QuikSharp.DataStructures;

namespace QuikSharp.Tests {
    [TestFixture]
    public class TradingFunctionsTest
    {

        private string _orderBookSample =
            @"
{

""bid_count"":""20.000000"",""offer_count"":""20.000000"",

""bid"":[{""price"":""59002.000000"",""quantity"":""3.000000""},{""price"":""59006.000000"",""quantity"":""1.000000""},{""price"":""59007.000000"",""quantity"":""1.000000""},{""price"":""59012.000000"",""quantity"":""4.000000""},{""price"":""59013.000000"",""quantity"":""60.000000""},{""price"":""59014.000000"",""quantity"":""3.000000""},{""price"":""59015.000000"",""quantity"":""1.000000""},{""price"":""59031.000000"",""quantity"":""2.000000""},{""price"":""59033.000000"",""quantity"":""1.000000""},{""price"":""59042.000000"",""quantity"":""1.000000""},{""price"":""59048.000000"",""quantity"":""1.000000""},{""price"":""59049.000000"",""quantity"":""1.000000""},{""price"":""59051.000000"",""quantity"":""3.000000""},{""price"":""59052.000000"",""quantity"":""3.000000""},{""price"":""59053.000000"",""quantity"":""4.000000""},{""price"":""59054.000000"",""quantity"":""5.000000""},{""price"":""59055.000000"",""quantity"":""3.000000""},{""price"":""59056.000000"",""quantity"":""1.000000""},{""price"":""59058.000000"",""quantity"":""1.000000""},{""price"":""59059.000000"",""quantity"":""2.000000""}],

""offer"":[{""price"":""59065.000000"",""quantity"":""1.000000""},{""price"":""59068.000000"",""quantity"":""3.000000""},{""price"":""59069.000000"",""quantity"":""3.000000""},{""price"":""59070.000000"",""quantity"":""3.000000""},{""price"":""59071.000000"",""quantity"":""5.000000""},{""price"":""59073.000000"",""quantity"":""6.000000""},{""price"":""59074.000000"",""quantity"":""5.000000""},{""price"":""59076.000000"",""quantity"":""1.000000""},{""price"":""59086.000000"",""quantity"":""1.000000""},{""price"":""59094.000000"",""quantity"":""2.000000""},{""price"":""59098.000000"",""quantity"":""8.000000""},{""price"":""59099.000000"",""quantity"":""5.000000""},{""price"":""59100.000000"",""quantity"":""37.000000""},{""price"":""59102.000000"",""quantity"":""6.000000""},{""price"":""59109.000000"",""quantity"":""7.000000""},{""price"":""59110.000000"",""quantity"":""102.000000""},{""price"":""59120.000000"",""quantity"":""5.000000""},{""price"":""59123.000000"",""quantity"":""5.000000""},{""price"":""59124.000000"",""quantity"":""5.000000""},{""price"":""59127.000000"",""quantity"":""5.000000""}]

}

";

        [Test]
        public void CouldDeserializeOrderBook()
        {
            var ob = _orderBookSample.FromJson<OrderBook>();
            Console.WriteLine("Order book: " + ob.ToJson());
        }

        [Test]
        public void GetDepoLimitsTest()
        {
            Quik quik = new Quik();

            // Получаем информацию по всем лимитам со всех доступных счетов.
            List<DepoLimitEx> depoLimits = quik.Trading.GetDepoLimits().Result;
            Console.WriteLine ($"Все лимиты со всех доступных счетов {depoLimits.Count}");

            // Получаем информацию по лимитам инструмента "Сбербанк"
            depoLimits = quik.Trading.GetDepoLimits("SBER").Result;
            Console.WriteLine($"Лимиты инструмента сбербанк {depoLimits.Count}");

            DepoLimitEx depoLimit = depoLimits.SingleOrDefault(_ => _.LimitKind == LimitKind.T2);
            if (depoLimit != null)
                Console.WriteLine("Открыта позиция по сбербанку.");

        }
    }
}

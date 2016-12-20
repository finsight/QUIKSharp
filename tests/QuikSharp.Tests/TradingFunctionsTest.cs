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
			if (depoLimits.Count > 0)
				PrintDepoLimits (depoLimits);

			// Получаем информацию по лимитам инструмента "Сбербанк"
			depoLimits = quik.Trading.GetDepoLimits("SBER").Result;
            Console.WriteLine($"Лимиты инструмента сбербанк {depoLimits.Count}");
			if (depoLimits.Count > 0)
				PrintDepoLimits (depoLimits);

			// Если информация по бумаге есть в таблице, это не значит что открыта позиция. Нужно проверять еще CurrentBalance
			DepoLimitEx depoLimit = depoLimits.SingleOrDefault(_ => _.LimitKind == LimitKind.T2 && _.CurrentBalance > 0);
            if (depoLimit != null)
                Console.WriteLine("Открыта позиция по сбербанку.");

        }

		private void PrintDepoLimits (List<DepoLimitEx> depoLimits)
		{
			Console.WriteLine ($"Количество стро: {depoLimits.Count}");
			foreach (var depo in depoLimits)
			{
				Console.WriteLine ($"Код бумаги: {depo.SecCode}");
				Console.WriteLine ($"Счет депо: {depo.TrdAccId}");
				Console.WriteLine ($"Идентификатор фирмы: {depo.FirmId}");
				Console.WriteLine ($"Код клиента: {depo.ClientCode}");
				Console.WriteLine ($"Входящий остаток по бумагам: {depo.OpenBalance}");
				Console.WriteLine ($"Входящий лимит по бумагам: {depo.OpenLimit}");
				Console.WriteLine ($"Текущий остаток по бумагам: {depo.CurrentBalance}");
				Console.WriteLine ($"Текущий лимит по бумагам: {depo.CurrentLimit}");
				Console.WriteLine ($"Заблокировано на продажу количества лотов: {depo.LockedSell}");
				Console.WriteLine ($"Заблокированного на покупку количества лотов: {depo.LockedBuy}");
				Console.WriteLine ($"Стоимость ценных бумаг, заблокированных под покупку: {depo.LockedBuyValue}");
				Console.WriteLine ($"Стоимость ценных бумаг, заблокированных под продажу: {depo.LockedSellValue}");
				Console.WriteLine ($"Цена приобретения: {depo.AweragePositionPrice}");
				Console.WriteLine ($"Тип лимита.  = «0» обычные лимиты, <> «0» – технологические лимиты: {depo.LimitKindInt}");
				Console.WriteLine ($"Тип лимита бумаги (Т0, Т1 или Т2): {depo.LimitKind}");
				Console.WriteLine ("------------------------------------------------------------------------");
			}
		}
	}
}

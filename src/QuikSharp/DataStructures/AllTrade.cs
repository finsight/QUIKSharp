// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Таблица с параметрами обезличенной сделки
    /// </summary>
    public class AllTrade : IWithLuaTimeStamp
    {
        /// <summary>
        /// Номер сделки в торговой системе
        /// </summary>
        [JsonProperty("trade_num")]
        public long TradeNum { get; set; }

        /// <summary>
        /// Набор битовых флагов:
        /// бит 0 (0x1)  Сделка на продажу
        /// бит 1 (0x2)  Сделка на покупку
        /// </summary>
        [JsonProperty("flags")]
        public AllTradeFlags Flags { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }

        /// <summary>
        /// Количество бумаг в последней сделке в лотах
        /// </summary>
        [JsonProperty("qty")]
        public long Qty { get; set; }

        /// <summary>
        /// Объем в денежных средствах
        /// </summary>
        [JsonProperty("value")]
        public double Value { get; set; }

        /// <summary>
        /// Накопленный купонный доход
        /// </summary>
        [JsonProperty("accruedint")]
        public double Accruedint { get; set; }

        /// <summary>
        /// Доходность
        /// </summary>
        [JsonProperty("yield")]
        public double Yield { get; set; }

        /// <summary>
        /// Код расчетов
        /// </summary>
        [JsonProperty("settlecode")]
        public string Settlecode { get; set; }

        /// <summary>
        /// Ставка РЕПО (%)
        /// </summary>
        [JsonProperty("reporate")]
        public double Reporate { get; set; }

        /// <summary>
        /// Сумма РЕПО
        /// </summary>
        [JsonProperty("repovalue")]
        public double Repovalue { get; set; }

        /// <summary>
        /// Объем выкупа РЕПО
        /// </summary>
        [JsonProperty("repo2value")]
        public double Repo2Value { get; set; }

        /// <summary>
        /// Срок РЕПО в днях
        /// </summary>
        [JsonProperty("repoterm")]
        public double Repoterm { get; set; }

        /// <summary>
        /// Код бумаги заявки
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Дата и время
        /// </summary>
        [JsonProperty("datetime")]
        public QuikDateTime Datetime { get; set; }

        /// <summary>
        /// Период торговой сессии. Возможные значения:
        /// «0» – Открытие;
        /// «1» – Нормальный;
        /// «2» – Закрытие
        /// </summary>
        [JsonProperty("period")]
        public int Period { get; set; }

        /// <summary>
        /// Открытый интерес
        /// </summary>
        [JsonProperty("open_interest")]
        public double OpenInterest { get; set; }

        /// <summary>
        /// Код биржи в торговой системе
        /// </summary>
        [JsonProperty("exchange_code")]
        public string ExchangeCode { get; set; }

        /// <summary>
        /// Площадка исполнения
        /// </summary>
        [JsonProperty("exec_market")]
        public string ExecMarket { get; set; }

        public long LuaTimeStamp { get; set; }
    }
}
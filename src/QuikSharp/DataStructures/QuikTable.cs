// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Таблицы, используемые в функциях «getItem», «getOrderByNumber», «getNumberOf» и «SearchItems»
    /// </summary>
    internal class QuikTable
    {
        /// <summary>
        /// Фирмы
        /// </summary>
        [JsonProperty("firms")]
        public string Firms { get; set; }

        /// <summary>
        /// Классы
        /// </summary>
        [JsonProperty("classes")]
        public string Classes { get; set; }

        /// <summary>
        /// Инструменты
        /// </summary>
        [JsonProperty("securities")]
        public string Securities { get; set; }

        /// <summary>
        /// Торговые счета
        /// </summary>
        [JsonProperty("trade_accounts")]
        public string TradeAccounts { get; set; }

        /// <summary>
        /// Коды клиентов
        ///  - функция getNumberOf("client_codes") возвращает количество доступных кодов клиента в терминале, а функция getItem("client_codes", i) - строку содержащую клиентский код с индексом i, где i может принимать значения от 0 до getNumberOf("client_codes") -1
        /// </summary>
        [JsonProperty("client_codes")]
        public string ClientCodes { get; set; }

        /// <summary>
        /// Обезличенные сделки
        /// </summary>
        [JsonProperty("all_trades")]
        public string AllTrades { get; set; }

        /// <summary>
        /// Денежные позиции
        /// </summary>
        [JsonProperty("account_positions")]
        public string AccountPositions { get; set; }

        /// <summary>
        /// Заявки
        /// </summary>
        [JsonProperty("orders")]
        public string Orders { get; set; }

        /// <summary>
        /// Позиции по клиентским счетам (фьючерсы)
        /// </summary>
        [JsonProperty("futures_client_holding")]
        public string FuturesClientHolding { get; set; }

        /// <summary>
        /// Лимиты по фьючерсам
        /// </summary>
        [JsonProperty("futures_client_limits")]
        public string FuturesClientLimits { get; set; }

        /// <summary>
        /// Лимиты по денежным средствам
        /// </summary>
        [JsonProperty("money_limits")]
        public string MoneyLimits { get; set; }

        /// <summary>
        /// Лимиты по бумагам
        /// </summary>
        [JsonProperty("depo_limits")]
        public string DepoLimits { get; set; }

        /// <summary>
        /// Сделки
        /// </summary>
        [JsonProperty("trades")]
        public string Trades { get; set; }

        /// <summary>
        /// Стоп-заявки
        /// </summary>
        [JsonProperty("stop_orders")]
        public string StopOrders { get; set; }

        /// <summary>
        /// Заявки на внебиржевые сделки
        /// </summary>
        [JsonProperty("neg_deals")]
        public string NegDeals { get; set; }

        /// <summary>
        /// Сделки для исполнения
        /// </summary>
        [JsonProperty("neg_trades")]
        public string NegTrades { get; set; }

        /// <summary>
        /// Отчеты по сделкам для исполнения
        /// </summary>
        [JsonProperty("neg_deal_reports")]
        public string NegDealReports { get; set; }

        /// <summary>
        /// Позиции участника по инструментам
        /// </summary>
        [JsonProperty("firm_holding")]
        public string FirmHolding { get; set; }

        /// <summary>
        /// Текущие позиции клиентским счетам
        /// </summary>
        [JsonProperty("account_balance")]
        public string AccountBalance { get; set; }

        /// <summary>
        /// Обязательства и требования по активам
        /// </summary>
        [JsonProperty("ccp_holdings")]
        public string CCPHoldings { get; set; }

        /// <summary>
        /// Валюта: обязательства и требования по активам
        /// </summary>
        [JsonProperty("rm_holdings")]
        public string RMHoldings { get; set; }

        /// <summary>
        /// Обязательства и требования по деньгам
        /// </summary>
        [JsonProperty("ccp_positions")]
        public string CCPPositions { get; set; }
    }
}
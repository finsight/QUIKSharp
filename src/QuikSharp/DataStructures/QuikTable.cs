using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Таблицы, используемые в функциях «getItem», «getOrderByNumber», «getNumberOf» и «SearchItems» 
    /// </summary>
    class QuikTable
    {
        /// <summary>
        /// Фирмы
        /// </summary>
        [JsonProperty("qtable_firms")]
        public string firms { get; set; }
        /// <summary>
        /// Классы
        /// </summary>
        [JsonProperty("qtable_classes")]
        public string classes { get; set; }
        /// <summary>
        /// Инструменты
        /// </summary>
        [JsonProperty("qtable_securities")]
        public string securities { get; set; }
        /// <summary>
        /// Торговые счета
        /// </summary>
        [JsonProperty("qtable_trade_accounts")]
        public string trade_accounts { get; set; }
        /// <summary>
        /// Коды клиентов
        ///  - функция getNumberOf("client_codes") возвращает количество доступных кодов клиента в терминале, а функция getItem("client_codes", i) - строку содержащую клиентский код с индексом i, где i может принимать значения от 0 до getNumberOf("client_codes") -1
        /// </summary>
        [JsonProperty("qtable_client_codes")]
        public string client_codes { get; set; }
        /// <summary>
        /// Обезличенные сделки
        /// </summary>
        [JsonProperty("qtable_all_trades")]
        public string all_trades { get; set; }
        /// <summary>
        /// Денежные позиции
        /// </summary>
        [JsonProperty("qtable_account_positions")]
        public string account_positions { get; set; }
        /// <summary>
        /// Заявки
        /// </summary>
        [JsonProperty("qtable_orders")]
        public string orders { get; set; }
        /// <summary>
        /// Позиции по клиентским счетам (фьючерсы)
        /// </summary>
        [JsonProperty("qtable_futures_client_holding")]
        public string futures_client_holding { get; set; }
        /// <summary>
        /// Лимиты по фьючерсам
        /// </summary>
        [JsonProperty("qtable_futures_client_limits")]
        public string futures_client_limits { get; set; }
        /// <summary>
        /// Лимиты по денежным средствам
        /// </summary>
        [JsonProperty("qtable_money_limits")]
        public string money_limits { get; set; }
        /// <summary>
        /// Лимиты по бумагам
        /// </summary>
        [JsonProperty("qtable_depo_limits")]
        public string depo_limits { get; set; }
        /// <summary>
        /// Сделки
        /// </summary>
        [JsonProperty("qtable_trades")]
        public string trades { get; set; }
        /// <summary>
        /// Стоп-заявки
        /// </summary>
        [JsonProperty("qtable_stop_orders")]
        public string stop_orders { get; set; }
        /// <summary>
        /// Заявки на внебиржевые сделки
        /// </summary>
        [JsonProperty("qtable_neg_deals")]
        public string neg_deals { get; set; }
        /// <summary>
        /// Сделки для исполнения
        /// </summary>
        [JsonProperty("qtable_neg_trades")]
        public string neg_trades { get; set; }
        /// <summary>
        /// Отчеты по сделкам для исполнения
        /// </summary>
        [JsonProperty("qtable_neg_deal_reports")]
        public string neg_deal_reports { get; set; }
        /// <summary>
        /// Текущие позиции по бумагам
        /// </summary>
        [JsonProperty("qtable_firm_holding")]
        public string firm_holding { get; set; }
        /// <summary>
        /// Текущие позиции клиентским счетам
        /// </summary>
        [JsonProperty("qtable_account_balance")]
        public string account_balance { get; set; }
        /// <summary>
        /// Обязательства и требования по деньгам
        /// </summary>
        [JsonProperty("qtable_ccp_positions")]
        public string ccp_positions { get; set; }
        /// <summary>
        /// Обязательства и требования по активам
        /// </summary>
        [JsonProperty("qtable_ccp_holdings")]
        public string ccp_holdings { get; set; }
    }
}

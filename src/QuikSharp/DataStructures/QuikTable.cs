
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
        public string firms { get; set; }
        /// <summary>
        /// Классы
        /// </summary>
        public string classes { get; set; }
        /// <summary>
        /// Инструменты
        /// </summary>
        public string securities { get; set; }
        /// <summary>
        /// Торговые счета
        /// </summary>
        public string trade_accounts { get; set; }
        /// <summary>
        /// Коды клиентов
        ///  - функция getNumberOf("client_codes") возвращает количество доступных кодов клиента в терминале, а функция getItem("client_codes", i) - строку содержащую клиентский код с индексом i, где i может принимать значения от 0 до getNumberOf("client_codes") -1
        /// </summary>
        public string client_codes { get; set; }
        /// <summary>
        /// Обезличенные сделки
        /// </summary>
        public string all_trades { get; set; }
        /// <summary>
        /// Денежные позиции
        /// </summary>
        public string account_positions { get; set; }
        /// <summary>
        /// Заявки
        /// </summary>
        public string orders { get; set; }
        /// <summary>
        /// Позиции по клиентским счетам (фьючерсы)
        /// </summary>
        public string futures_client_holding { get; set; }
        /// <summary>
        /// Лимиты по фьючерсам
        /// </summary>
        public string futures_client_limits { get; set; }
        /// <summary>
        /// Лимиты по денежным средствам
        /// </summary>
        public string money_limits { get; set; }
        /// <summary>
        /// Лимиты по бумагам
        /// </summary>
        public string depo_limits { get; set; }
        /// <summary>
        /// Сделки
        /// </summary>
        public string trades { get; set; }
        /// <summary>
        /// Стоп-заявки
        /// </summary>
        public string stop_orders { get; set; }
        /// <summary>
        /// Заявки на внебиржевые сделки
        /// </summary>
        public string neg_deals { get; set; }
        /// <summary>
        /// Сделки для исполнения
        /// </summary>
        public string neg_trades { get; set; }
        /// <summary>
        /// Отчеты по сделкам для исполнения
        /// </summary>
        public string neg_deal_reports { get; set; }
        /// <summary>
        /// Текущие позиции по бумагам
        /// </summary>
        public string firm_holding { get; set; }
        /// <summary>
        /// Текущие позиции клиентским счетам
        /// </summary>
        public string account_balance { get; set; }
        /// <summary>
        /// Обязательства и требования по деньгам
        /// </summary>
        public string ccp_positions { get; set; }
        /// <summary>
        /// Обязательства и требования по активам
        /// </summary>
        public string ccp_holdings { get; set; }
    }
}

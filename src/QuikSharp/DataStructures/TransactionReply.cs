namespace QuikSharp {
    /// <summary>
    /// Результат OnTransReply
    /// </summary>
    public class TransactionReply {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Пользовательский идентификатор транзакции
        /// </summary>
        public int trans_id { get; set; }
        /// <summary>
        /// Статус
        /// </summary>
        public double status { get; set; }
        /// <summary>
        /// Сообщение
        /// </summary>
        public string result_msg { get; set; }
        /// <summary>
        /// Время (в QLUA представлено как число)
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long uid { get; set; }
        /// <summary>
        /// Флаги транзакции (временно не используется)
        /// </summary>
        public long flags { get; set; }
        /// <summary>
        /// Идентификатор транзакции на сервере
        /// </summary>
        public long server_trans_id { get; set; }
        /// <summary>
        /// Номер заявки
        /// </summary>
        public long? order_num { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public double? price { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public double? quantity { get; set; }
        /// <summary>
        /// Остаток
        /// </summary>
        public double? balance { get; set; }
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        public string firm_id { get; set; }
        /// <summary>
        /// Торговый счет
        /// </summary>
        public string account { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        public string client_code { get; set; }
        /// <summary>
        /// Поручение
        /// </summary>
        public string brokerref { get; set; }
        /// <summary>
        /// Код класса
        /// </summary>
        public string class_code { get; set; }
        /// <summary>
        /// Код бумаги
        /// </summary>
        public string sec_code { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
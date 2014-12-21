namespace QuikSharp {
    /// <summary>
    /// Лимиты по бумагам
    /// </summary>
    public class DepoLimitEx {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код бумаги
        /// </summary>
        public string sec_code { get; set; }
        
        /// <summary>
        /// Счет депо
        /// </summary>
        public string trdaccid { get; set; }
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        public string firmid { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        public string client_code { get; set; }
        /// <summary>
        /// Входящий остаток по бумагам
        /// </summary>
        public double openbal { get; set; }
        /// <summary>
        /// Входящий лимит по бумагам
        /// </summary>
        public double openlimit { get; set; }
        /// <summary>
        /// Текущий остаток по бумагам
        /// </summary>
        public double currentbal { get; set; }
        /// <summary>
        /// Текущий лимит по бумагам
        /// </summary>
        public double currentlimit { get; set; }
        /// <summary>
        /// Заблокировано на продажу количества лотов
        /// </summary>
        public double locked_sell { get; set; }
        /// <summary>
        /// Заблокированного на покупку количества лотов
        /// </summary>
        public double locked_buy { get; set; }
        /// <summary>
        /// Стоимость ценных бумаг, заблокированных под покупку
        /// </summary>
        public double locked_buy_value { get; set; }
        /// <summary>
        /// Стоимость ценных бумаг, заблокированных под продажу
        /// </summary>
        public double locked_sell_value { get; set; }
        /// <summary>
        /// Цена приобретения
        /// </summary>
        public double awg_position_price { get; set; }
        /// <summary>
        /// Тип лимита. Возможные значения:
        /// «0» – обычные лимиты,
        /// значение не равное «0» – технологические лимиты
        /// </summary>
        public double limit_kind { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
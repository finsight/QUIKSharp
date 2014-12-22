namespace QuikSharp {
    /// <summary>
    /// Стакан
    /// </summary>
    public class OrderBook {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Строка стакана
        /// </summary>
        public class PriceQuantity {
            /// <summary>
            /// Цена покупки / продажи
            /// </summary>
            public double price { get; set; }
            /// <summary>
            /// Количество в лотах
            /// </summary>
            public double quantity { get; set; }
        }

        /// <summary>
        /// Код класса
        /// </summary>
        public string class_code { get; set; }
        /// <summary>
        /// Код бумаги
        /// </summary>
        public string sec_code { get; set; }

        /// <summary>
        /// Количество котировок покупки
        /// </summary>
        public double bid_count { get; set; }
        /// <summary>
        /// Количество котировок продажи
        /// </summary>
        public double offer_count { get; set; }
        

        /// <summary>
        /// Котировки спроса (покупки)
        /// </summary>
        public PriceQuantity[] bid { get; set; }

        /// <summary>
        /// Котировки предложений (продажи)
        /// </summary>
        public PriceQuantity[] offer { get; set; }
        // ReSharper restore InconsistentNaming
    }

    
}

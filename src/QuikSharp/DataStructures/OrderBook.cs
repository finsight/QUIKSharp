// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Стакан
    /// </summary>
    public class OrderBook : IWithLuaTimeStamp
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Строка стакана
        /// </summary>
        public class PriceQuantity
        {
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
        /// time in msec from lua epoch
        /// </summary>
        public long LuaTimeStamp { get; set; }

        /// <summary>
        /// Result of getInfoParam("SERVERTIME") right before getQuoteLevel2 call
        /// </summary>
        public string server_time { get; set; }

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
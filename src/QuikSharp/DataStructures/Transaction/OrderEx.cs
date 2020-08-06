// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Описание параметров Таблицы заявок. Данный класс содержит дополнительные поля, которые вычисляются библиотекой на стороне Lua кода
    /// </summary>
    public class OrderEx : Order
    {
        /// <summary>
        /// Средняя цена заявки. Будет только в том случае, если по заявке есть хоть одна сделка
        /// </summary>
        [JsonProperty("average_price_ex")]
        public decimal AveragePriceEx { get; set; }

        /// <summary>
        /// Время последней сделки в заявке. Будет только в том случае, если по заявке есть хоть одна сделка
        /// </summary>
        [JsonProperty("last_trade_datetime_ex")]
        public QuikDateTime LastTradeDatetimeEx { get; set; }
    }
}
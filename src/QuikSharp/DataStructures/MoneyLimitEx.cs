// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Лимиты по денежным средствам
    /// </summary>
    public class MoneyLimitEx
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код валюты
        /// </summary>
        [JsonProperty("currcode")]
        public string CurrCode { get; set; }

        /// <summary>
        /// Тэг расчетов
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

        /// <summary>
        /// Входящий остаток по деньгам
        /// </summary>
        [JsonProperty("openbal")]
        public double OpenBal { get; set; }

        /// <summary>
        /// Входящий лимит по деньгам
        /// </summary>
        [JsonProperty("openlimit")]
        public double OpenLimit { get; set; }

        /// <summary>
        /// Текущий остаток по деньгам
        /// </summary>
        [JsonProperty("currentbal")]
        public double CurrentBal { get; set; }

        /// <summary>
        /// Текущий лимит по деньгам
        /// </summary>
        [JsonProperty("currentlimit")]
        public double CurrentLimit { get; set; }

        /// <summary>
        /// Заблокированное количество
        /// </summary>
        [JsonProperty("locked")]
        public double Locked { get; set; }

        /// <summary>
        /// Стоимость активов в заявках на покупку немаржинальных бумаг
        /// </summary>
        [JsonProperty("locked_value_coef")]
        public double LockedValueCoef { get; set; }

        /// <summary>
        /// Стоимость активов в заявках на покупку маржинальных бумаг
        /// </summary>
        [JsonProperty("locked_margin_value")]
        public double LockedMarginValue { get; set; }

        /// <summary>
        /// Плечо
        /// </summary>
        [JsonProperty("leverage")]
        public double Leverage { get; set; }

        /// <summary>
        /// Тип лимита. Возможные значения:
        /// «0» – обычные лимиты,
        /// иначе – технологические лимиты
        /// </summary>
        [JsonProperty("limit_kind")]
        public int LimitKind { get; set; }

        /// <summary>
        /// Средневзвешенная цена приобретения позиции
        /// </summary>
        [JsonProperty("wa_position_price")]
        public double WaPositionPrice { get; set; }

        /// <summary>
        /// Гарантийное обеспечение заявок
        /// </summary>
        [JsonProperty("orders_collateral")]
        public double OrdersCollateral { get; set; }

        /// <summary>
        /// Гарантийное обеспечение позиций
        /// </summary>
        [JsonProperty("positions_collateral")]
        public double PositionsCollateral { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
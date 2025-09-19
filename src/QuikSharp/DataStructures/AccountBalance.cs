// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При изменении текущей позиции по счету функция возвращает таблицу Lua «Позиции участника по торговым счетам» с параметрами
    /// </summary>
    public class AccountBalance
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Код бумаги
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Торговый счет
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

        /// <summary>
        /// Счет депо
        /// </summary>
        [JsonProperty("depaccid")]
        public string DepAccId { get; set; }

        /// <summary>
        /// Входящий остаток
        /// </summary>
        [JsonProperty("openbal")]
        public double OpenBal { get; set; }

        /// <summary>
        /// Текущий остаток
        /// </summary>
        [JsonProperty("currentpos")]
        public double CurrentPos { get; set; }

        /// <summary>
        /// Плановая продажа
        /// </summary>
        [JsonProperty("plannedpossell")]
        public double PlannedPosSell { get; set; }

        /// <summary>
        /// Плановая покупка
        /// </summary>
        [JsonProperty("plannedposbuy")]
        public double PlannedPosBuy { get; set; }

        /// <summary>
        /// Контрольный остаток простого клиринга, равен входящему остатку минус плановая позиция на продажу, включенная в простой клиринг
        /// </summary>
        [JsonProperty("planbal")]
        public double PlanBal { get; set; }

        /// <summary>
        /// Куплено
        /// </summary>
        [JsonProperty("usqtyb")]
        public double UsQtyB { get; set; }

        /// <summary>
        /// Продано
        /// </summary>
        [JsonProperty("usqtys")]
        public double UsQtyS { get; set; }

        /// <summary>
        /// Плановый остаток, равен текущему остатку минус плановая позиция на продажу
        /// </summary>
        [JsonProperty("planned")]
        public double Planned { get; set; }

        /// <summary>
        /// Плановая позиция после проведения расчетов
        /// </summary>
        [JsonProperty("settlebal")]
        public double SettleBal { get; set; }

        /// <summary>
        /// Идентификатор расчетного счета/кода в клиринговой организации
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccId { get; set; }

        /// <summary>
        /// Признак счета обеспечения. Возможные значения:
        /// «0» – для обычных счетов,
        /// «1» – для счета обеспечения.
        /// </summary>
        [JsonProperty("firmuse")]
        public double FirmUse { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
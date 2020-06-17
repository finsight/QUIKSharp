// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При изменении денежной позиции по счету функция возвращает таблицу Lua с параметрами
    /// </summary>
    public class AccountPosition
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

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
        /// Описание
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

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
        /// Плановый остаток
        /// </summary>
        [JsonProperty("plannedpos")]
        public double PlannedPos { get; set; }

        /// <summary>
        /// Внешнее ограничение по деньгам
        /// </summary>
        [JsonProperty("limit1")]
        public double Limit1 { get; set; }

        /// <summary>
        /// Внутреннее (собственное) ограничение по деньгам
        /// </summary>
        [JsonProperty("limit2")]
        public double Limit2 { get; set; }

        /// <summary>
        /// В заявках на продажу // Странно. Не ошибка ли????
        /// </summary>
        [JsonProperty("orderbuy")]
        public double OrderBuy { get; set; }

        /// <summary>
        /// В заявках на покупку // Странно. Не ошибка ли????
        /// </summary>
        [JsonProperty("ordersell")]
        public double OrderSell { get; set; }

        /// <summary>
        /// Нетто-позиция
        /// </summary>
        [JsonProperty("netto")]
        public double Netto { get; set; }

        /// <summary>
        /// Плановая позиция
        /// </summary>
        [JsonProperty("plannedbal")]
        public double PlannedBal { get; set; }

        /// <summary>
        /// Дебит
        /// </summary>
        [JsonProperty("debit")]
        public double Debit { get; set; }

        /// <summary>
        /// Кредит
        /// </summary>
        [JsonProperty("credit")]
        public double Credit { get; set; }

        /// <summary>
        /// Идентификатор счета
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccId { get; set; }

        /// <summary>
        /// Маржинальное требование на начало торгов
        /// </summary>
        [JsonProperty("margincall")]
        public double MarginCall { get; set; }

        /// <summary>
        /// Плановая позиция после проведения расчетов
        /// </summary>
        [JsonProperty("settlebal")]
        public double SettleBal { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
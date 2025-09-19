// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание параметров таблицы Торговые счета
    /// </summary>
    public class TradesAccounts
    {
        /// <summary>
        /// Список кодов классов, разделенных символом «|»
        /// </summary>
        [JsonProperty("class_codes")]
        public string ClassCodes { get; set; }

        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string Firmid { get; set; }

        /// <summary>
        /// Код торгового счета
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdaccId { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Запрет необеспеченных продаж. Возможные значения:
        /// «0» – Нет;
        /// «1» – Да
        /// </summary>
        [JsonProperty("fullcoveredsell")]
        public int Fullcoveredsell { get; set; }

        /// <summary>
        /// Номер основного торгового счета
        /// </summary>
        [JsonProperty("main_trdaccid")]
        public string MainTrdaccid { get; set; }

        /// <summary>
        /// Расчетная организация по «Т0»
        /// </summary>
        [JsonProperty("bankid_t0")]
        public string BankIdT0 { get; set; }

        /// <summary>
        /// Расчетная организация по «Т+»
        /// </summary>
        [JsonProperty("bankid_tplus")]
        public string BankidTplus { get; set; }

        /// <summary>
        /// Тип депозитарного счета
        /// </summary>
        [JsonProperty("trdacc_type")]
        public int TrdaccType { get; set; }

        /// <summary>
        /// Раздел счета Депо
        /// </summary>
        [JsonProperty("depunitid")]
        public string DepunitId { get; set; }

        /// <summary>
        /// Статус торгового счета. Возможные значения:
        /// «0» – операции разрешены;
        /// «1» – операции запрещены
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }

        /// <summary>
        /// Тип раздела. Возможные значения:
        /// «0» – раздел обеспечения;
        /// иначе – для торговых разделов
        /// </summary>
        [JsonProperty("firmuse")]
        public int Firmuse { get; set; }

        /// <summary>
        /// Номер счета депо в депозитарии
        /// </summary>
        [JsonProperty("depaccid")]
        public string DepaccId { get; set; }

        /// <summary>
        /// Код дополнительной позиции по денежным средствам
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccId { get; set; }
    }
}
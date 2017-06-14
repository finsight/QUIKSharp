using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// trades accounts class
    /// </summary>
    public class TradesAccounts
    {
        /// <summary>
        /// Описание
        /// </summary>
        [JsonProperty("description")]
        public string description { get; set; }
        /// <summary>
        /// Список кодов классов, разделенных символом «|»
        /// </summary>
        [JsonProperty("class_codes")]
        public string class_codes { get; set; }
        /// <summary>
        /// Запрет необеспеченных продаж. Возможные значения:
        /// «0» – Нет;
        /// «1» – Да
        /// </summary>
        [JsonProperty("fullcoveredsell")]
        public int fullcoveredsell { get; set; }
        /// <summary>
        /// Номер основного торгового счета
        /// </summary>
        [JsonProperty("main_trdaccid")]
        public string main_trdaccid { get; set; }
        /// <summary>
        /// Расчетная организация по «Т+»
        /// </summary>
        [JsonProperty("bankid_tplus")]
        public string bankid_tplus { get; set; }
        /// <summary>
        /// Тип депозитарного счета
        /// </summary>
        [JsonProperty("trdacc_type")]
        public int trdacc_type { get; set; }
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string firmid { get; set; }
        /// <summary>
        /// Раздел счета Депо
        /// </summary>
        [JsonProperty("depunitid")]
        public string depunitid { get; set; }
        /// <summary>
        /// Расчетная организация по «Т0»
        /// </summary>
        [JsonProperty("bankid_t0")]
        public string bankid_t0 { get; set; }
        /// <summary>
        /// Тип раздела. Возможные значения:
        /// «0» – раздел обеспечения;
        /// иначе – для торговых разделов
        /// </summary>
        [JsonProperty("firmuse")]
        public int firmuse { get; set; }
        /// <summary>
        /// Статус торгового счета. Возможные значения:
        /// «0» – операции разрешены;
        /// «1» – операции запрещены
        /// </summary>
        [JsonProperty("status")]
        public int status { get; set; }
        /// <summary>
        /// Номер счета депо в депозитарии
        /// </summary>
        [JsonProperty("depaccid")]
        public string depaccid { get; set; }
        /// <summary>
        /// Код торгового счета
        /// </summary>
        [JsonProperty("trdaccid")]
        public string trdaccid { get; set; }
        /// <summary>
        /// Код дополнительной позиции по денежным средствам
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string bank_acc_id { get; set; }
    }
}

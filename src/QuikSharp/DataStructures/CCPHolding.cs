using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание параметров Таблицы обязательств и требований по активам
    /// </summary>
    public class CCPHolding
    {
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Номер счета депо в Депозитарии (НДЦ)
        /// </summary>
        [JsonProperty("depo_account")]
        public string DepoAccount { get; set; }

        /// <summary>
        /// Торговый счет
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Идентификатор расчетного счета/кода в клиринговой организации
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccId { get; set; }

        /// <summary>
        /// Дата расчетов
        /// </summary>
        [JsonProperty("settle_date")]
        public int SettleDate { get; set; }

        /// <summary>
        /// Количество инструментов в сделках
        /// </summary>
        [JsonProperty("qty")]
        public long Quantity { get; set; }

        /// <summary>
        /// Количество инструментов в заявках на покупку
        /// </summary>
        [JsonProperty("qty_buy")]
        public long QuantityBuy { get; set; }

        /// <summary>
        /// Количество инструментов в заявках на продажу
        /// </summary>
        [JsonProperty("qty_sell")]
        public long QuantitySell { get; set; }

        /// <summary>
        /// Нетто-позиция
        /// </summary>
        [JsonProperty("netto")]
        public long Netto { get; set; }

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
        /// Код инструмента
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Плановая позиция Т+
        /// </summary>
        [JsonProperty("planned_covered")]
        public long PlannedCovered { get; set; }

        /// <summary>
        /// Тип раздела. Возможные значения: 
        /// «0» – торговый раздел; 
        /// «1» – раздел обеспечения
        /// </summary>
        [JsonProperty("firm_use")]
        public int FirmUse { get; set; }
    }
}
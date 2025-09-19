using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание параметров Таблицы обязательств и требований по активам на валютном рынке
    /// </summary>
    public class RMHolding
    {
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
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

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
        /// Дата расчётов
        /// </summary>
        [JsonProperty("date")]
        public int Date { get; set; }

        /// <summary>
        /// Дебит (Размер денежных обязательств)
        /// </summary>
        [JsonProperty("debit")]
        public double Debit { get; set; }

        /// <summary>
        /// Кредит (Размер денежных требований)
        /// </summary>
        [JsonProperty("credit")]
        public double Credit { get; set; }

        /// <summary>
        /// Сумма денежных средств в заявках на покупку
        /// </summary>
        [JsonProperty("value_buy")]
        public double ValueBuy{ get; set; }

        /// <summary>
        /// Сумма денежных средств в заявках на продажу
        /// </summary>
        [JsonProperty("value_sell")]
        public double ValueSell { get; set; }

        /// <summary>
        /// Сумма возврата компенсационного перевода
        /// </summary>
        [JsonProperty("margin_call")]
        public double MarginCall { get; set; }

        /// <summary>
        /// Плановая позиция Т+
        /// </summary>
        [JsonProperty("planned_covered")]
        public long PlannedCovered { get; set; }

        /// <summary>
        /// Размер денежных обязательств на начало дня, с точностью до 2 знаков после десятичного разделителя
        /// </summary>
        [JsonProperty("debit_balance")]
        public double DebitBalance { get; set; }

        /// <summary>
        /// Размер денежных требований на начало дня, с точностью до 2 знаков после десятичного разделителя
        /// </summary>
        [JsonProperty("credit_balance")]
        public double CreditBalance { get; set; }
    }
}
using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание параметров таблицы «Позиции участника по инструментам»
    /// </summary>
    public class FirmHolding
    {
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Код инструмента
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

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
        /// Количество инструментов в активных заявках на покупку 
        /// </summary>
        [JsonProperty("plannedposbuy")]
        public double PlannedPosBuy { get; set; }

        /// <summary>
        /// Количество инструментов в активных заявках на продажу
        /// </summary>
        [JsonProperty("plannedpossell")]
        public double PlannedPosSell { get; set; }

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
    }
}
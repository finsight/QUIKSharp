using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    public class CalcBuySellResult
    {
        /// <summary>
        /// Максимально возможное количество бумаги
        /// </summary>
        [JsonProperty("qty")]
        public int Qty { get; set; }

        /// <summary>
        /// Сумма комиссии
        /// </summary>
        [JsonProperty("comission")]
        public double Comission { get; set; }
    }
}
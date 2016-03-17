using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Свеча
    /// </summary>
    public class Candle
    {
        /// <summary>
        /// Минимальная цена сделки
        /// </summary>
        [JsonProperty("low")]
<<<<<<< HEAD
        public float Low { get; set; }
=======
        public decimal Low { get; set; }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

        /// <summary>
        /// Цена закрытия
        /// </summary>
        [JsonProperty("close")]
<<<<<<< HEAD
        public float Close { get; set; }
=======
        public decimal Close { get; set; }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

        /// <summary>
        /// Максимальная цена сделки
        /// </summary>
        [JsonProperty("high")]
<<<<<<< HEAD
        public float High{get; set; }
=======
        public decimal High {get; set; }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

        /// <summary>
        /// Цена открытия
        /// </summary>
        [JsonProperty("open")]
<<<<<<< HEAD
        public float Open { get; set; }
=======
        public decimal Open { get; set; }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

        /// <summary>
        /// Объем последней сделки
        /// </summary>
        [JsonProperty("volume")]
        public int Volume{ get; set; }

        /// <summary>
        /// Дата и время
        /// </summary>
        [JsonProperty("datetime")]
        public QuikDateTime Datetime { get; set; }

        //todo: not wrapped following:
        //"doesExist": 1,

        #region Candles subscription info
        // Информация о принадлежности свечки к одной из подписок. Заполняется в тех свечках, которые приходят в событии NewCandle

        /// <summary>
        /// Код инструмента.
        /// </summary>
        [JsonProperty("sec")]
<<<<<<< HEAD
        public string Sec { get; set; }
=======
        public string SecCode { get; set; }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

        /// <summary>
        /// Код класса.
        /// </summary>
        [JsonProperty("class")]
<<<<<<< HEAD
        public string Class { get; set; }
=======
        public string ClassCode { get; set; }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

        /// <summary>
        /// Интервал подписки.
        /// </summary>
        [JsonProperty("interval")]
        public CandleInterval Interval { get; set; }

        #endregion
<<<<<<< HEAD
=======

        public override string ToString()
        {
            return $"Open: {Open}, Close: {Close}, High: {High}, Low: {Low}, Volume: {Volume}";
        }
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
    }
}

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
        public decimal Low { get; set; }

        /// <summary>
        /// Цена закрытия
        /// </summary>
        [JsonProperty("close")]
        public decimal Close { get; set; }

        /// <summary>
        /// Максимальная цена сделки
        /// </summary>
        [JsonProperty("high")]
        public decimal High {get; set; }

        /// <summary>
        /// Цена открытия
        /// </summary>
        [JsonProperty("open")]
        public decimal Open { get; set; }

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
        public string SecCode { get; set; }

        /// <summary>
        /// Код класса.
        /// </summary>
        [JsonProperty("class")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Интервал подписки.
        /// </summary>
        [JsonProperty("interval")]
        public CandleInterval Interval { get; set; }

        #endregion

        public override string ToString()
        {
            return @"Open: {Open}, Close: {Close}, High: {High}, Low: {Low}, Volume: {Volume}";
        }
    }
}

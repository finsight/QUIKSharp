﻿using Newtonsoft.Json;

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
        public float Low { get; set; }

        /// <summary>
        /// Цена закрытия
        /// </summary>
        [JsonProperty("close")]
        public float Close { get; set; }

        /// <summary>
        /// Максимальная цена сделки
        /// </summary>
        [JsonProperty("high")]
        public float High{get; set; }

        /// <summary>
        /// Цена открытия
        /// </summary>
        [JsonProperty("open")]
        public float Open { get; set; }

        /// <summary>
        /// Объем последней сделки
        /// </summary>
        [JsonProperty("volume")]
        public int Volume{get; set; }

        //todo: not wrapped following:
        //"doesExist": 1,
        //"datetime": {
        //  "ms": 0,
        //  "year": 2015,
        //  "day": 24,
        //  "week_day": 5,
        //  "month": 4,
        //  "sec": 0,
        //  "hour": 14,
        //  "min": 26
        //},
    }
}
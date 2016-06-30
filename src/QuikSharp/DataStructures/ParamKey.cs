// Copyright (C) 2015 Victor Baybekov, Anton Kytmanov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Таблица с описанием изменившегося инструмента для коллбека OnParam
    /// </summary>
    public class ParamKey
    {
        /// <summary>
        /// Код класса.
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Код инструмента.
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecurityCode { get; set; }
    }
}
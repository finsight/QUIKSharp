// Copyright (C) 2015 Victor Baybekov, Anton Kytmanov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures {
    /// <summary>
    /// Таблица Lua с описанием фирм
    /// </summary>
    public class Firm
    {
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }
        /// <summary>
        /// Название фирмы
        /// </summary>
        [JsonProperty("firm_name")]
        public string FirmName { get; set; }
        /// <summary>
        /// Статус
        /// </summary>
        [JsonProperty("status")]
        public int Status { get; set; }
        /// <summary>
        /// Торговая площадка
        /// </summary>
        [JsonProperty("exchange")]
        public string Exchange { get; set; }
    }
}
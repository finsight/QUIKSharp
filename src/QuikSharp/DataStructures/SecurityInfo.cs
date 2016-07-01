// Copyright (C) 2015 Victor Baybekov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures {
    /// <summary>
    /// Результат getSecurityInfo
    /// </summary>
    public class SecurityInfo {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код инструмента
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// </summary>
        
        /// <summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// Наименование инструмента
        /// </summary>
        /// <summary>
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        /// Краткое наименование
        /// </summary>
        /// <summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }
        /// Код класса
        /// </summary>
        /// <summary>
        [JsonProperty("class_name")]
        public string ClassName { get; set; }
        /// Наименование класса
        /// </summary>
        /// <summary>
        [JsonProperty("face_value")]
        public string FaceValue { get; set; }
        /// Номинал
        /// </summary>
        /// <summary>
        [JsonProperty("face_unit")]
        public string FaceUnit { get; set; }
        /// Код валюты номинала
        /// </summary>
        /// <summary>
        [JsonProperty("scale")]
        public int Scale { get; set; }
        /// Количество значащих цифр после запятой
        /// </summary>
        /// <summary>
        /// Дата погашения (в QLUA это число, но на самом деле дата записанная как YYYYMMDD),
        [JsonProperty("mat_date")]
        public string MatDate { get; set; }
        /// поэтому здесь сохраняем просто как строку
        /// </summary>
        /// <summary>
        [JsonProperty("lot_size")]
        public int LotSize { get; set; }

        /// Размер лота
        /// </summary>
        /// </summary>
        [JsonProperty("isin_code")]
        public string IsinCode { get; set; }
    }
}

// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Результат getSecurityInfo
    /// </summary>
    public class SecurityInfo
    {
        /// <summary>
        /// Код инструмента
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Наименование инструмента
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Наименование класса
        /// </summary>
        [JsonProperty("class_name")]
        public string ClassName { get; set; }

        /// <summary>
        /// Номинал
        /// </summary>
        [JsonProperty("face_value")]
        public string FaceValue { get; set; }

        /// <summary>
        /// Код валюты номинала
        /// </summary>
        [JsonProperty("face_unit")]
        public string FaceUnit { get; set; }

        /// <summary>
        /// Количество значащих цифр после запятой
        /// </summary>
        [JsonProperty("scale")]
        public int Scale { get; set; }

        /// <summary>
        /// Дата погашения (в QLUA это число, но на самом деле дата записанная как YYYYMMDD),
        /// поэтому здесь сохраняем просто как строку
        /// </summary>
        [JsonProperty("mat_date")]
        public string MatDate { get; set; }

        /// <summary>
        /// Размер лота
        /// </summary>
        [JsonProperty("lot_size")]
        public int LotSize { get; set; }

        /// <summary>
        /// ISIN-код
        /// </summary>
        [JsonProperty("isin_code")]
        public string IsinCode { get; set; }

        /// <summary>
        /// Минимальный шаг цены
        /// </summary>
        [JsonProperty("min_price_step")]
        public double MinPriceStep { get; set; }
    }
}
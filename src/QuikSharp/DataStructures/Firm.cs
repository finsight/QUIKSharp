// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При получении описания новой фирмы от сервера функция возвращает таблицу Lua с параметрами
    /// </summary>
    public class Firm
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Название класса
        /// </summary>
        [JsonProperty("firm_name")]
        public string FirmName { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        [JsonProperty("status")]
        public double Status { get; set; }

        /// <summary>
        /// Торговая площадка
        /// </summary>
        [JsonProperty("exchange")]
        public string Exchange { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание класса
    /// </summary>
    public class ClassInfo
    {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Код фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Наименование класса
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Количество параметров в классе
        /// </summary>
        [JsonProperty("npars")]
        public int NPars { get; set; }

        /// <summary>
        /// Количество бумаг в классе
        /// </summary>
        [JsonProperty("nsecs")]
        public int NSecs { get; set; }

        // ReSharper restore InconsistentNaming

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
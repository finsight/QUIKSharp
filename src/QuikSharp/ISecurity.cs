// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("class_code")]
        string ClassCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("sec_code")]
        string SecCode { get; set; }

        /// <summary>
        /// Свойство возвращает коммбинацию ClassCode и SecCode в выбраном пользователем формате,
        ///  например: $"{ClassCode}@{SecCode}" или $"{ClassCode}-{SecCode}"
        /// </summary>
        string FullCode { get; }
    }
}
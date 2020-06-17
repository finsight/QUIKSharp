// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    public class Param
    {
        /// <summary>
        /// sec_code  STRING  Код бумаги
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// class_code  STRING  Код бумаги
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }
    }
}
// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При удалении лимита по срочному рынку функция возвращает таблицу Lua "Удаление фьючерсного лимита" с параметрами
    /// </summary>
    public class FuturesLimitDelete
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код торгового счета
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

        /// <summary>
        /// Тип лимита
        /// </summary>
        [JsonProperty("limit_type")]
        public string LimitType { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
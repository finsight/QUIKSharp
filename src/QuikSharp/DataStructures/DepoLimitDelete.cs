// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При обработке удаления бумажного лимита функция возвращает таблицу Lua "Удаление позиции по инструментам" с параметрами
    /// </summary>
    public class DepoLimitDelete
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код инструмента
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Код торгового счета
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

        /// <summary>
        /// Тип лимита. Возможные значения:
        /// ///«0» – обычные лимиты,
        /// ///значение не равное «0» – технологические лимиты
        /// </summary>
        [JsonProperty("limit_kind")]
        public int LimitKindInt { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При обработке изменения денежного лимита функция getMoney возвращает таблицу Lua с параметрами:
    /// </summary>
    public class MoneyLimit
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Входящий лимит по денежным средствам
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyOpenLimit { get; set; }

        /// <summary>
        /// Стоимость немаржинальных бумаг в заявках на покупку
        /// </summary>
        [JsonProperty("money_limit_locked_nonmarginal_value")]
        public double MoneyLimitLockedNonmarginalValue { get; set; }

        /// <summary>
        /// Заблокированное в заявках на покупку количество денежных средств
        /// </summary>
        [JsonProperty("money_limit_locked")]
        public double MoneyLimitLocked { get; set; }

        /// <summary>
        /// Входящий остаток по денежным средствам
        /// </summary>
        [JsonProperty("money_open_balance")]
        public double MoneyOpenBalance { get; set; }

        /// <summary>
        /// Текущий лимит по денежным средствам
        /// </summary>
        [JsonProperty("money_current_limit")]
        public double MoneyCurrentLimit { get; set; }

        /// <summary>
        /// Текущий остаток по денежным средствам
        /// </summary>
        [JsonProperty("money_current_balance")]
        public double MoneyCurrentBalance { get; set; }

        /// <summary>
        /// Доступное количество денежных средств
        /// </summary>
        [JsonProperty("money_limit_available")]
        public double MoneyLimitAvailable { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
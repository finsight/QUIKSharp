// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Флаги для таблицы Сделки (Признак того, что транзакция совершена по правилам пре-трейда)
    /// </summary>
    [Flags]
    public enum TradeWaiverFlags
    {
        /// <summary>
        /// RFPT
        /// </summary>
        RFPT = 0x1,

        /// <summary>
        /// NLIQ
        /// </summary>
        NLIQ = 0x2,

        /// <summary>
        /// OILQ
        /// </summary>
        OILQ = 0x4,

        /// <summary>
        /// PRC
        /// </summary>
        PRC = 0x8,

        /// <summary>
        /// SIZE
        /// </summary>
        SIZE = 0x10,

        /// <summary>
        /// ILQD
        /// </summary>
        ILQD = 0x20
    }
}
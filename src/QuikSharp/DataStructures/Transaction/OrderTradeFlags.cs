// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum OrderTradeFlags
    {
        /// <summary>
        /// 
        /// </summary>
        Active = 0x1,

        /// <summary>
        /// 
        /// </summary>
        Canceled = 0x2,

        /// <summary>
        /// 
        /// </summary>
        IsSell = 0x4,

        /// <summary>
        /// 
        /// </summary>
        IsLimit = 0x8,

        /// <summary>
        /// 
        /// </summary>
        AllowDiffPrice = 0x10,

        /// <summary>
        ///
        /// </summary>
        FillOrKill = 0x20,

        /// <summary>
        /// 
        /// </summary>
        IsMarketMakerOrSent = 0x40,

        /// <summary>
        /// 
        /// </summary>
        IsReceived = 0x80,

        /// <summary>
        /// 
        /// </summary>
        IsKillBalance = 0x100,

        /// <summary>
        /// 
        /// </summary>
        Iceberg = 0x200
    }
}
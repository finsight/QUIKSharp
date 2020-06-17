// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Флаги для таблиц Сделки (OTC post-trade индикатор)
    /// </summary>
    [Flags]
    public enum TradeOTCPostTradeIndicatorFlags
    {
        /// <summary>
        /// Benchmark
        /// </summary>
        Benchmark = 0x1,

        /// <summary>
        /// Agency cross
        /// </summary>
        AgencyCross = 0x2,

        /// <summary>
        /// Large in scale
        /// </summary>
        LargeInScale = 0x4,

        /// <summary>
        /// Illiquid instrument
        /// </summary>
        IlliquidInstrument = 0x8,

        /// <summary>
        /// Above specified size
        /// </summary>
        AboveSpecifiedSize = 0x10,

        /// <summary>
        /// Cancellations
        /// </summary>
        Cancellations = 0x20,

        /// <summary>
        /// Amendments
        /// </summary>
        Amendments = 0x40,

        /// <summary>
        /// Special dividend
        /// </summary>
        SpecialDividend = 0x80,

        /// <summary>
        /// Price improvement
        /// </summary>
        PriceImprovement = 0x100,

        /// <summary>
        /// Duplicative
        /// </summary>
        Duplicative = 0x200,

        /// <summary>
        /// Not contributing to the price discovery process
        /// </summary>
        NotContributingToPriceDiscoveryProcess = 0x400,

        /// <summary>
        /// Package
        /// </summary>
        Package = 0x800,

        /// <summary>
        /// Exchange for Physical
        /// </summary>
        ExchangeForPhysical = 0x1000
    }
}
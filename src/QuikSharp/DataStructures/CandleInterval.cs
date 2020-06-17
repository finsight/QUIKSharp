// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Интервал запрашиваемого графика
    /// </summary>
    public enum CandleInterval
    {
        /// <summary>
        /// Тиковые данные
        /// </summary>
        TICK = 0,

        /// <summary>
        /// 1 минута
        /// </summary>
        M1 = 1,

        /// <summary>
        /// 2 минуты
        /// </summary>
        M2 = 2,

        /// <summary>
        /// 3 минуты
        /// </summary>
        M3 = 3,

        /// <summary>
        /// 4 минуты
        /// </summary>
        M4 = 4,

        /// <summary>
        /// 5 минут
        /// </summary>
        M5 = 5,

        /// <summary>
        /// 6 минут
        /// </summary>
        M6 = 6,

        /// <summary>
        /// 10 минут
        /// </summary>
        M10 = 10,

        /// <summary>
        /// 15 минут
        /// </summary>
        M15 = 15,

        /// <summary>
        /// 20 минут
        /// </summary>
        M20 = 20,

        /// <summary>
        /// 30 минут
        /// </summary>
        M30 = 30,

        /// <summary>
        /// 1 час
        /// </summary>
        H1 = 60,

        /// <summary>
        /// 2 часа
        /// </summary>
        H2 = 120,

        /// <summary>
        /// 4 часа
        /// </summary>
        H4 = 240,

        /// <summary>
        /// 1 день
        /// </summary>
        D1 = 1440,

        /// <summary>
        /// 1 неделя
        /// </summary>
        W1 = 10080,

        /// <summary>
        /// 1 месяц
        /// </summary>
        MN = 23200
    }
}
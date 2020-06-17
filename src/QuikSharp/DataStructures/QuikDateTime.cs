// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Формат даты и времени, используемый таблицах.
    /// Для корректного отображения даты и времени все параметры должны быть заданы.
    /// </summary>
    public class QuikDateTime
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Микросекунды игнорируются в текущей версии.
        /// </summary>
        public int mcs { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int ms { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int sec { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int min { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int hour { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int day { get; set; }

        /// <summary>
        /// Monday is 1
        /// </summary>
        public int week_day { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int month { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int year { get; set; }

        // ReSharper restore InconsistentNaming

        /// <summary>
        ///
        /// </summary>
        /// <param name="qdt"></param>
        /// <returns></returns>
        public static explicit operator DateTime(QuikDateTime qdt)
        {
            var dt = new DateTime(qdt.year, qdt.month, qdt.day, qdt.hour, qdt.min, qdt.sec, qdt.ms);
            return dt; //dt.AddTicks(qdt.mcs * 10);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static explicit operator QuikDateTime(DateTime dt)
        {
            return new QuikDateTime
            {
                year = dt.Year,
                month = dt.Month,
                day = dt.Day,
                hour = dt.Hour,
                min = dt.Minute,
                sec = dt.Second,
                ms = dt.Millisecond,
                mcs = 0, // ((int)(dt.TimeOfDay.Ticks) - ((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 * 10000) / 10,
                week_day = (int) dt.DayOfWeek
            };
        }
    }
}
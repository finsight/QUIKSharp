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
            var dt = new DateTime(qdt.year, qdt.month, qdt.day, qdt.hour, qdt.min, qdt.sec);
            // 1 микросекунда = 10 тиков (1000 наносекунд / 100 наносекунд на тик). // приоритет mcs
            long ticks = qdt.mcs > 0 ? qdt.mcs * 10L : qdt.ms * 1000L;
            return dt.AddTicks(ticks); // Добавляем вычисленные тики к базовому времени.
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static explicit operator QuikDateTime(DateTime dt)
        {
            // Вычисляем тики, которые составляют дробную часть текущей секунды. TimeSpan.TicksPerSecond = 10,000,000.
            // Оператор остатка (%) мгновенно дает нам тики внутри секунды (от 0 до 9,999,999).
            long fractionalTicks = dt.Ticks % TimeSpan.TicksPerSecond;
            int totalMicroseconds = (int)(fractionalTicks / 10);

            return new QuikDateTime
            {
                year = dt.Year,
                month = dt.Month,
                day = dt.Day,
                hour = dt.Hour,
                min = dt.Minute,
                sec = dt.Second,
                ms = dt.Millisecond,
                mcs = totalMicroseconds,
                //week_day = (int) dt.DayOfWeek
                week_day = (int)dt.DayOfWeek == 0 ? 7 : (int)dt.DayOfWeek // воскресенье  в QUIK — 7
            };
        }
    }
}
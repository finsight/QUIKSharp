// Copyright (C) 2014 Victor Baybekov

using System;

namespace QuikSharp.DataStructures {
    /// <summary>
    /// Формат даты и времени, используемый таблицах.
    /// Для корректного отображения даты и времени все параметры должны быть заданы. 
    /// </summary>
    public class QuikDateTime {
        // ReSharper disable InconsistentNaming
        public int mcs { get; set; }
        public int ms { get; set; }
        public int sec { get; set; }
        public int min { get; set; }
        public int hour { get; set; }
        public int day { get; set; }
        public int week_day { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        // ReSharper restore InconsistentNaming
        public static explicit operator DateTime(QuikDateTime qdt) {
            var dt = new DateTime(qdt.year, qdt.month, qdt.day, qdt.hour, qdt.min, qdt.sec, qdt.ms);
            return dt.AddTicks(qdt.mcs*10);
        }

        public static explicit operator QuikDateTime(DateTime dt) {
            // TODO test, especially mcs and day of week (monday 0 or 1)
            return new QuikDateTime {
                year = dt.Year,
                month = dt.Month,
                day = dt.Day,
                hour = dt.Hour,
                min = dt.Minute,
                sec = dt.Second,
                ms = dt.Millisecond,
                mcs = ((int) (dt.TimeOfDay.Ticks) -
                      ((dt.Hour * 60 + dt.Minute) * 60 + dt.Second) * 1000 * 10000)/10,
                      week_day = (int)dt.DayOfWeek
            };
        }
        // TODO operators for int msec/sec from os.time and timemsec() in qsutils.lua
        
    }
}

// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Результат getSecurityInfo
    /// </summary>
    public class SecurityInfo {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код инструмента
        /// </summary>
        public string sec_code { get; set; }
        
        /// <summary>
        /// Наименование инструмента
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Краткое наименование
        /// </summary>
        public string short_name { get; set; }
        /// <summary>
        /// Код класса
        /// </summary>
        public string class_code { get; set; }
        /// <summary>
        /// Наименование класса
        /// </summary>
        public string class_name { get; set; }
        /// <summary>
        /// Номинал
        /// </summary>
        public string face_value { get; set; }
        /// <summary>
        /// Код валюты номинала
        /// </summary>
        public string face_unit { get; set; }
        /// <summary>
        /// Количество значащих цифр после запятой
        /// </summary>
        public int scale { get; set; }
        /// <summary>
        /// Дата погашения (в QLUA это число, но на самом деле дата записанная как YYYYMMDD),
        /// поэтому здесь сохраняем просто как строку
        /// </summary>
        public string mat_date { get; set; }
        /// <summary>
        /// Размер лота
        /// </summary>
        public int lot_size { get; set; }
        // ReSharper restore InconsistentNaming
    }
}

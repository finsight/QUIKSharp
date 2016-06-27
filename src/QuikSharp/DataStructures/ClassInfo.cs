// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание класса
    /// </summary>
    public class ClassInfo {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Код фирмы
        /// </summary>
        public string firmid { get; set; }
        
        /// <summary>
        /// Наименование класса
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Код класса
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// Количество параметров в классе
        /// </summary>
        public int npars { get; set; }
        /// <summary>
        /// Количество бумаг в классе
        /// </summary>
        public int nsecs { get; set; }
        // ReSharper restore InconsistentNaming


        public override string ToString() {
            return this.ToJson();
        }
    }
}
// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При обработке изменения бумажного лимита функция возвращает таблицу Lua с параметрами
    /// </summary>
    public class DepoLimit {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Стоимость ценных бумаг, заблокированных на покупку
        /// </summary>
        public double depo_limit_locked_buy_value { get; set; }
        /// <summary>
        /// Текущий остаток по бумагам
        /// </summary>
        public double depo_current_balance { get; set; }
        /// <summary>
        /// Количество лотов ценных бумаг, заблокированных на покупку
        /// </summary>
        public double depo_limit_locked_buy { get; set; }
        /// <summary>
        /// Заблокированное Количество лотов ценных бумаг
        /// </summary>
        public double depo_limit_locked { get; set; }
        /// <summary>
        /// Доступное количество ценных бумаг
        /// </summary>
        public double depo_limit_available { get; set; }
        /// <summary>
        /// Текущий лимит по бумагам
        /// </summary>
        public double depo_current_limit { get; set; }
        /// <summary>
        /// Входящий остаток по бумагам
        /// </summary>
        public double depo_open_balance { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При обработке изменения денежного лимита функция getMoney возвращает таблицу Lua с параметрами: 
    /// </summary>
    public class MoneyLimit {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Входящий лимит по денежным средствам
        /// </summary>
        public double money_open_limit { get; set; }

        /// <summary>
        /// Стоимость немаржинальных бумаг в заявках на покупку
        /// </summary>
        public double money_limit_locked_nonmarginal_value { get; set; }
        /// <summary>
        /// Заблокированное в заявках на покупку количество денежных средств
        /// </summary>
        public double money_limit_locked { get; set; }
        /// <summary>
        /// Входящий остаток по денежным средствам
        /// </summary>
        public double money_open_balance { get; set; }
        /// <summary>
        /// Текущий лимит по денежным средствам
        /// </summary>
        public double money_current_limit { get; set; }
        /// <summary>
        /// Текущий остаток по денежным средствам
        /// </summary>
        public double money_current_balance { get; set; }
        /// <summary>
        /// Доступное количество денежных средств
        /// </summary>
        public double money_limit_available { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
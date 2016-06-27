// Copyright (C) 2015 Victor Baybekov

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Лимиты по денежным средствам
    /// </summary>
    public class MoneyLimitEx {
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

        /// <summary>
        /// Код валюты
        /// </summary>
        public string currcode { get; set; }
        /// <summary>
        /// Тэг расчетов
        /// </summary>
        public string tag { get; set; }
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        public string firmid { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        public string client_code { get; set; }
        /// <summary>
        /// Входящий остаток по деньгам
        /// </summary>
        public double openbal { get; set; }
        /// <summary>
        /// Входящий лимит по деньгам
        /// </summary>
        public double openlimit { get; set; }
        /// <summary>
        /// Текущий остаток по деньгам
        /// </summary>
        public double currentbal { get; set; }
        /// <summary>
        /// Текущий лимит по деньгам
        /// </summary>
        public double currentlimit { get; set; }
        /// <summary>
        /// Заблокированное количество
        /// </summary>
        public double locked { get; set; }
        /// <summary>
        /// Стоимость активов в заявках на покупку немаржинальных бумаг
        /// </summary>
        public double locked_value_coef { get; set; }
        /// <summary>
        /// Стоимость активов в заявках на покупку маржинальных бумаг
        /// </summary>
        public double locked_margin_value { get; set; }
        /// <summary>
        /// Плечо
        /// </summary>
        public double leverage { get; set; }
        /// <summary>
        /// Тип лимита. Возможные значения:
        /// «0» – обычные лимиты,
        /// иначе – технологические лимиты
        /// </summary>
        public double limit_kind { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
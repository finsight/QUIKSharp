// Copyright (C) 2015 Victor Baybekov
using Newtonsoft.Json;

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
        [JsonProperty("money_open_limit")]
        public double MoneyOpenLimit { get; set; }

        /// <summary>
        /// Стоимость немаржинальных бумаг в заявках на покупку
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyLimitLockedNonmarginalValue { get; set; }
        /// <summary>
        /// Заблокированное в заявках на покупку количество денежных средств
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyLimitLocked { get; set; }
        /// <summary>
        /// Входящий остаток по денежным средствам
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyOpenBalance { get; set; }
        /// <summary>
        /// Текущий лимит по денежным средствам
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyCurrentLimit { get; set; }
        /// <summary>
        /// Текущий остаток по денежным средствам
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyCurrentBalance { get; set; }
        /// <summary>
        /// Доступное количество денежных средств
        /// </summary>
        [JsonProperty("money_open_limit")]
        public double MoneyLimitAvailable { get; set; }

        /// <summary>
        /// Код валюты
        /// </summary>
        [JsonProperty("currcode")]
        public string CurrCode { get; set; }
        /// <summary>
        /// Тэг расчетов
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }
        /// <summary>
        /// Входящий остаток по деньгам
        /// </summary>
        [JsonProperty("openbal")]
        public double OpenBal { get; set; }
        /// <summary>
        /// Входящий лимит по деньгам
        /// </summary>
        [JsonProperty("openlimit")]
        public double OpenLimit { get; set; }
        /// <summary>
        /// Текущий остаток по деньгам
        /// </summary>
        [JsonProperty("currentbal")]
        public double CurrentBal { get; set; }
        /// <summary>
        /// Текущий лимит по деньгам
        /// </summary>
        [JsonProperty("currentlimit")]
        public double CurrentLimit { get; set; }
        /// <summary>
        /// Заблокированное количество
        /// </summary>
        [JsonProperty("locked")]
        public double Locked { get; set; }
        /// <summary>
        /// Стоимость активов в заявках на покупку немаржинальных бумаг
        /// </summary>
        [JsonProperty("locked_value_coef")]
        public double LockedValueCoef { get; set; }
        /// <summary>
        /// Стоимость активов в заявках на покупку маржинальных бумаг
        /// </summary>
        [JsonProperty("locked_margin_value")]
        public double LockedMarginValue { get; set; }
        /// <summary>
        /// Плечо
        /// </summary>
        [JsonProperty("leverage")]
        public double Leverage { get; set; }
        /// <summary>
        /// Тип лимита. Возможные значения:
        /// «0» – обычные лимиты,
        /// иначе – технологические лимиты
        /// </summary>
        [JsonProperty("limit_kind")]
        public double LimitKind { get; set; }
        // ReSharper restore InconsistentNaming
    }
}
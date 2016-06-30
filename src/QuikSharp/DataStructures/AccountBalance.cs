// Copyright (C) 2015 Victor Baybekov, Anton Kytmanov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Таблица с параметрами спот-счета для акций
    /// </summary>
    public class AccountBalance : IWithLuaTimeStamp
    {
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }
        /// <summary>
        /// Код бумаги
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }
        /// <summary>
        /// Торговый счет
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TradeAccId { get; set; }
        /// <summary>
        /// Счет депо
        /// </summary>
        [JsonProperty("depaccid")]
        public string DepoAccId { get; set; }
        /// <summary>
        /// Входящий остаток
        /// </summary>
        [JsonProperty("openbal")]
        public double OpenBalance { get; set; }
        /// <summary>
        /// Текущий остаток
        /// </summary>
        [JsonProperty("currentpos")]
        public double CurrentPos { get; set; }
        /// <summary>
        /// Плановая продажа
        /// </summary>
        [JsonProperty("plannedpossell")]
        public double PlannedPosSell { get; set; }
        /// <summary>
        /// Плановая покупка
        /// </summary>
        [JsonProperty("plannedposbuy")]
        public double PlannedPosBuy { get; set; }
        /// <summary>
        /// Контрольный остаток простого клиринга, равен входящему остатку 
        /// минус плановая позиция на продажу, включенная в простой клиринг
        /// </summary>
        [JsonProperty("planbal")]
        public double PlanBalance { get; set; }
        /// <summary>
        /// Куплено
        /// </summary>
        [JsonProperty("usqtyb")]
        public double Bought { get; set; }
        /// <summary>
        /// Продано
        /// </summary>
        [JsonProperty("usqtys")]
        public double Sold { get; set; }
        /// <summary>
        /// Плановый остаток, равен текущему остатку минус плановая позиция на продажу
        /// </summary>
        [JsonProperty("planned")]
        public double Planned { get; set; }
        /// <summary>
        /// Плановая позиция после проведения расчетов
        /// </summary>
        [JsonProperty("settlebal")]
        public double SettleBalance { get; set; }
        /// <summary>
        /// Идентификатор расчетного счета/кода в клиринговой организации
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccId { get; set; }
        /// <summary>
        /// Признак счета обеспечения. Возможные значения:
        ///«0» – для обычных счетов,
        ///«1» – для счета обеспечения.
        /// </summary>
        [JsonProperty("firmuse")]
        public int FirmUse { get; set; }
        
        public long LuaTimeStamp { get; set; }
    }
}

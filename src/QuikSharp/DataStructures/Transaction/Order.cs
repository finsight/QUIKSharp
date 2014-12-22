// Copyright (C) 2014 Victor Baybekov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures.Transaction {
    /// <summary>
    /// Описание параметров Таблицы заявок
    /// </summary>
    public class Order : IWithLuaTimeStamp {

        [JsonProperty("lua_timestamp")]
        public long LuaTimeStamp { get; internal set; }

        /// <summary>
        /// Номер заявки в торговой системе
        /// </summary>
        [JsonProperty("order_num")]
        public long OrderNum { get; set; }
        /// <summary>
        /// Набор битовых флагов
        /// </summary>
        [JsonProperty("flags")]
        public OrderTradeFlags Flags { get; set; }
        /// <summary>
        /// Поручение/комментарий, обычно: код клиента/номер поручения
        /// </summary>
        [JsonProperty("brokerref")]
        public string Comment { get; set; }
        /// <summary>
        /// Идентификатор трейдера
        /// </summary>
        [JsonProperty("userid")]
        public string UserId { get; set; }
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }
        /// <summary>
        /// Торговый счет
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("price")]
        public double Price { get; set; }
        /// <summary>
        /// Количество в лотах
        /// </summary>
        [JsonProperty("qty")]
        public int Quantity { get; set; }
        /// <summary>
        /// Остаток
        /// </summary>
        [JsonProperty("balance")]
        public int Balance { get; set; }
        /// <summary>
        /// Объем в денежных средствах
        /// </summary>
        [JsonProperty("value")]
        public double Value { get; set; }
        /// <summary>
        /// Накопленный купонный доход
        /// </summary>
        [JsonProperty("accruedint")]
        public double AccruedInterest { get; set; }
        /// <summary>
        /// Доходность
        /// </summary>
        [JsonProperty("yield")]
        public double Yield { get; set; }
        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        [JsonProperty("trans_id")]
        public long TransID { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }
        /// <summary>
        /// Цена выкупа
        /// </summary>
        [JsonProperty("price2")]
        public double Price2 { get; set; }
        /// <summary>
        /// Код расчетов
        /// </summary>
        [JsonProperty("settlecode")]
        public string Settlecode { get; set; }
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }
        /// <summary>
        /// Код биржи в торговой системе
        /// </summary>
        [JsonProperty("exchange_code")]
        public string ExchangeCode { get; set; }
        /// <summary>
        /// Время активации
        /// </summary>
        [JsonProperty("activation_time")]
        public double ActivationTime { get; set; }
        /// <summary>
        /// Номер заявки в торговой системе
        /// </summary>
        [JsonProperty("linkedorder")]
        public long Linkedorder { get; set; }
        /// <summary>
        /// Дата окончания срока действия заявки
        /// </summary>
        [JsonProperty("expiry")]
        public double Expiry { get; set; }
        /// <summary>
        /// Код бумаги заявки
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }
        /// <summary>
        /// Код класса заявки
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }
        /// <summary>
        /// Дата и время
        /// </summary>
        [JsonProperty("datetime")]
        public QuikDateTime Datetime { get; set; }
        /// <summary>
        /// Дата и время снятия заявки
        /// </summary>
        [JsonProperty("withdraw_datetime")]
        public QuikDateTime WithdrawDatetime { get; set; }
        /// <summary>
        /// Идентификатор расчетного счета/кода в клиринговой организации
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccID { get; set; }
        /// <summary>
        /// Способ указания объема заявки. Возможные значения: «0» – по количеству, «1» – по объему
        /// </summary>
        [JsonProperty("value_entry_type")]
        public int ValueEntryType { get; set; }
        /// <summary>
        /// Срок РЕПО, в календарных днях
        /// </summary>
        [JsonProperty("repoterm")]
        public double Repoterm { get; set; }
        /// <summary>
        /// Сумма РЕПО на текущую дату. Отображается с точностью 2 знака
        /// </summary>
        [JsonProperty("repovalue")]
        public double Repovalue { get; set; }
        /// <summary>
        /// Объём сделки выкупа РЕПО. Отображается с точностью 2 знака
        /// </summary>
        [JsonProperty("repo2value")]
        public double Repo2Value { get; set; }
        /// <summary>
        /// Остаток суммы РЕПО за вычетом суммы привлеченных или предоставленных по сделке РЕПО денежных средств в неисполненной части заявки, по состоянию на текущую дату. Отображается с точностью 2 знака
        /// </summary>
        [JsonProperty("repo_value_balance")]
        public double RepoValueBalance { get; set; }
        /// <summary>
        /// Начальный дисконт, в %
        /// </summary>
        [JsonProperty("start_discount")]
        public double StartDiscount { get; set; }
        /// <summary>
        /// Причина отклонения заявки брокером
        /// </summary>
        [JsonProperty("reject_reason")]
        public string RejectReason { get; set; }
        /// <summary>
        /// Битовое поле для получения специфических параметров с западных площадок
        /// </summary>
        [JsonProperty("ext_order_flags")]
        public int ExtOrderFlags { get; set; }
        /// <summary>
        /// Минимально допустимое количество, которое можно указать в заявке по данному инструменту. Если имеет значение «0», значит ограничение по количеству не задано
        /// </summary>
        [JsonProperty("min_qty")]
        public int MinQty { get; set; }
        /// <summary>
        /// Тип исполнения заявки. Если имеет значение «0», значит значение не задано
        /// </summary>
        [JsonProperty("exec_type")]
        public int ExecType { get; set; }
        /// <summary>
        /// Поле для получения параметров по западным площадкам. Если имеет значение «0», значит значение не задано
        /// </summary>
        [JsonProperty("side_qualifier")]
        public int SideQualifier { get; set; }
        /// <summary>
        /// Поле для получения параметров по западным площадкам. Если имеет значение «0», значит значение не задано
        /// </summary>
        [JsonProperty("acnt_type")]
        public int AcntType { get; set; }
        /// <summary>
        /// Поле для получения параметров по западным площадкам. Если имеет значение «0», значит значение не задано
        /// </summary>
        [JsonProperty("capacity")]
        public int Capacity { get; set; }
        /// <summary>
        /// Поле для получения параметров по западным площадкам. Если имеет значение «0», значит значение не задано
        /// </summary>
        [JsonProperty("passive_only_order")]
        public int PassiveOnlyOrder { get; set; }

    }
}

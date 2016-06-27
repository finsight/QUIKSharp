// Copyright (C) 2015 Victor Baybekov

using Newtonsoft.Json;
using QuikSharp.DataStructures;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Описание параметров Таблицы сделок 
    /// </summary>
    public class Trade : IWithLuaTimeStamp {

        [JsonProperty("lua_timestamp")]
        public long LuaTimeStamp { get; internal set; }

        /// <summary>
        /// Номер сделки в торговой системе
        /// </summary>
        [JsonProperty("trade_num")]
        public long TradeNum { get; set; }
        /// <summary>
        /// Номер заявки в торговой системе
        /// </summary>
        [JsonProperty("order_num")]
        public long OrderNum { get; set; }
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
        /// Идентификатор дилера
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
        /// Количество бумаг в последней сделке в лотах
        /// </summary>
        [JsonProperty("qty")]
        public int Quantity { get; set; }
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
        /// Код расчетов
        /// </summary>
        [JsonProperty("settlecode")]
        public string SettleCode { get; set; }
        /// <summary>
        /// Код фирмы партнера
        /// </summary>
        [JsonProperty("cpfirmid")]
        public string CpFirmId { get; set; }
        /// <summary>
        /// Набор битовых флагов
        /// </summary>
        [JsonProperty("flags")]
        public OrderTradeFlags Flags { get; set; }
        /// <summary>
        /// Цена выкупа
        /// </summary>
        [JsonProperty("price2")]
        public double Price2 { get; set; }
        /// <summary>
        /// Ставка РЕПО (%)
        /// </summary>
        [JsonProperty("reporate")]
        public double RepoRate { get; set; }
        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }
        /// <summary>
        /// Доход (%) на дату выкупа
        /// </summary>
        [JsonProperty("accrued2")]
        public double Accrued2 { get; set; }
        /// <summary>
        /// Сумма РЕПО
        /// </summary>
        [JsonProperty("repovalue")]
        public double RepoValue { get; set; }
        /// <summary>
        /// Объем выкупа РЕПО
        /// </summary>
        [JsonProperty("repo2value")]
        public double Repo2Value { get; set; }
        /// <summary>
        /// Начальный дисконт (%)
        /// </summary>
        [JsonProperty("start_discount")]
        public double StartDiscount { get; set; }
        /// <summary>
        /// Нижний дисконт (%)
        /// </summary>
        [JsonProperty("lower_discount")]
        public double LowerDiscount { get; set; }
        /// <summary>
        /// Верхний дисконт (%)
        /// </summary>
        [JsonProperty("upper_discount")]
        public double UpperDiscount { get; set; }
        /// <summary>
        /// Блокировка обеспечения («Да»/«Нет»)
        /// </summary>
        [JsonProperty("block_securities")]
        public double BlockSecurities { get; set; }
        /// <summary>
        /// Клиринговая комиссия (ММВБ)
        /// </summary>
        [JsonProperty("clearing_comission")]
        public double ClearingComission { get; set; }
        /// <summary>
        /// Комиссия Фондовой биржи (ММВБ)
        /// </summary>
        [JsonProperty("exchange_comission")]
        public double ExchangeComission { get; set; }
        /// <summary>
        /// Комиссия Технического центра (ММВБ)
        /// </summary>
        [JsonProperty("tech_center_comission")]
        public double TechCenterComission { get; set; }
        /// <summary>
        /// Дата расчетов
        /// </summary>
        [JsonProperty("settle_date")]
        public double SettleDate { get; set; }
        /// <summary>
        /// Валюта расчетов
        /// </summary>
        [JsonProperty("settle_currency")]
        public string SettleCurrency { get; set; }
        /// <summary>
        /// Валюта
        /// </summary>
        [JsonProperty("trade_currency")]
        public string TradeCurrency { get; set; }
        /// <summary>
        /// Код биржи в торговой системе
        /// </summary>
        [JsonProperty("exchange_code")]
        public string ExchangeCode { get; set; }
        /// <summary>
        /// Идентификатор рабочей станции
        /// </summary>
        [JsonProperty("station_id")]
        public string StationID { get; set; }
        /// <summary>
        /// Код бумаги заявки
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }
        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }
        /// <summary>
        /// Дата и время 
        /// </summary>
        [JsonProperty("datetime")]
        public QuikDateTime QuikDateTime { get; set; }
        /// <summary>
        /// Идентификатор расчетного счета/кода в клиринговой организации
        /// </summary>
        [JsonProperty("bank_acc_id")]
        public string BankAccID { get; set; }
        /// <summary>
        /// Комиссия брокера. Отображается с точностью до 2 двух знаков. Поле зарезервировано для будущего использования.
        /// </summary>
        [JsonProperty("broker_comission")]
        public double BrokerComission { get; set; }
        /// <summary>
        /// Номер витринной сделки в Торговой Системе для сделок РЕПО с ЦК и SWAP
        /// </summary>
        [JsonProperty("linked_trade")]
        public long LinkedTrade { get; set; }
        /// <summary>
        /// Период торговой сессии. Возможные значения: «0» – Открытие; «1» – Нормальный; «2» – Закрытие
        /// </summary>
        [JsonProperty("period")]
        public int Period { get; set; }

    }
}

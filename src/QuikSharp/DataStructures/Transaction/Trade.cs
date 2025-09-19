// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Описание параметров Таблицы сделок
    /// </summary>
    public class Trade : IWithLuaTimeStamp
    {
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
        /// Идентификатор пользователя, отказавшегося от сделки
        /// </summary>
        [JsonProperty("canceled_uid")]
        public double CanceledUserID { get; set; }

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
        /// Срок РЕПО, в календарных днях
        /// </summary>
        [JsonProperty("repoterm")]
        public double RepoTerm { get; set; }

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

        /// <summary>
        /// Идентификатор транзакции
        /// </summary>
        [JsonProperty("trans_id")]
        public long TransID { get; set; }

        /// <summary>
        /// Тип сделки. Возможные значения: 
        /// «1» – Обычная; 
        /// «2» – Адресная; 
        /// «3» – Первичное размещение; 
        /// «4» – Перевод денег/бумаг; 
        /// «5» – Адресная сделка первой части РЕПО; 
        /// «6» – Расчетная по операции своп; 
        /// «7» – Расчетная по внебиржевой операции своп; 
        /// «8» – Расчетная сделка бивалютной корзины; 
        /// «9» – Расчетная внебиржевая сделка бивалютной корзины; 
        /// «10» – Сделка по операции РЕПО с ЦК; 
        /// «11» – Первая часть сделки по операции РЕПО с ЦК; 
        /// «12» – Вторая часть сделки по операции РЕПО с ЦК; 
        /// «13» – Адресная сделка по операции РЕПО с ЦК; 
        /// «14» – Первая часть адресной сделки по операции РЕПО с ЦК; 
        /// «15» – Вторая часть адресной сделки по операции РЕПО с ЦК; 
        /// «16» – Техническая сделка по возврату активов РЕПО с ЦК; 
        /// «17» – Сделка по спреду между фьючерсами разных сроков на один актив; 
        /// «18» – Техническая сделка первой части от спреда между фьючерсами; 
        /// «19» – Техническая сделка второй части от спреда между фьючерсами; 
        /// «20» – Адресная сделка первой части РЕПО с корзиной; 
        /// «21» – Адресная сделка второй части РЕПО с корзиной; 
        /// «22» – Перенос позиций срочного рынка
        /// </summary>
        [JsonProperty("kind")]
        public int Kind { get; set; }

        /// <summary>
        /// Идентификатор счета в НКЦ (расчетный код)
        /// </summary>
        [JsonProperty("clearing_bank_accid")]
        public string ClearingBankAccID { get; set; }

        /// <summary>
        /// Дата и время снятия сделки
        /// </summary>
        [JsonProperty("canceled_datetime")]
        public QuikDateTime CanceledDateTime { get; set; }

        /// <summary>
        /// Идентификатор фирмы - участника клиринга
        /// </summary>
        [JsonProperty("clearing_firmid")]
        public string ClearingFirmID { get; set; }

        /// <summary>
        /// Дополнительная информация по сделке, передаваемая торговой системой
        /// </summary>
        [JsonProperty("system_ref")]
        public string SystemRef { get; set; }

        /// <summary>
        /// Идентификатор пользователя на сервере QUIK
        /// </summary>
        [JsonProperty("uid")]
        public long UID { get; set; }

        /// <summary>
        /// Приоритетное обеспечение
        /// </summary>
        [JsonProperty("lseccode")]
        public string LSecCode { get; set; }

        /// <summary>
        /// Номер ревизии заявки, по которой была совершена сделка (параметр используется только для сделок, совершенных по заявкам, к которым применена транзакция замены заявки с сохранением номера)
        /// </summary>
        [JsonProperty("order_revision_number")]
        public int OrderRevisionNumber { get; set; }

        /// <summary>
        /// Количество в заявке на момент совершения сделки, в лотах (параметр используется только для сделок, совершенных по заявкам, к которым применена транзакция замены заявки с сохранением номера)
        /// </summary>
        [JsonProperty("order_qty")]
        public int OrderQty { get; set; }

        /// <summary>
        /// Цена в заявке на момент совершения сделки (параметр используется только для сделок, совершенных по заявкам, к которым применена транзакция замены заявки с сохранением номера)
        /// </summary>
        [JsonProperty("order_price")]
        public decimal OrderPrice { get; set; }

        /// <summary>
        /// Биржевой номер заявки
        /// </summary>
        [JsonProperty("order_exchange_code")]
        public string OrderExchangeCode { get; set; }

        /// <summary>
        /// Площадка исполнения
        /// </summary>
        [JsonProperty("exec_market")]
        public string ExecMarket { get; set; }

        /// <summary>
        /// Индикатор ликвидности. Возможные значения: 
        /// «0» – не определено; 
        /// «1» – по заявке мейкера; 
        /// «2» – по заявке тейкера; 
        /// «3» – вывод ликвидности; 
        /// «4» – по заявке в период аукциона
        /// </summary>
        [JsonProperty("liquidity_indicator")]
        public int LiquidityIndicator { get; set; }

        /// <summary>
        /// Внешняя ссылка, используется для обратной связи с внешними системами
        /// </summary>
        [JsonProperty("extref")]
        public string ExtRef { get; set; }

        /// <summary>
        /// Расширенные флаги, полученные от шлюза напрямую, без вмешательства сервера QUIK. Поле не заполняется
        /// </summary>
        [JsonProperty("ext_trade_flags")]
        public int ExtTradeFlags { get; set; }

        /// <summary>
        /// UID пользователя, от имени которого совершена сделка
        /// </summary>
        [JsonProperty("on_behalf_of_uid")]
        public long OnBehalfOfUID { get; set; }

        /// <summary>
        /// Квалификатор клиента, от имени которого совершена сделка. Возможные значения: 
        /// «0» – не определено; 
        /// «1» – Natural Person; 
        /// «3» – Legal Entity
        /// </summary>
        [JsonProperty("client_qualifier")]
        public int ClientQualifier { get; set; }

        /// <summary>
        /// Краткий идентификатор клиента, от имени которого совершена сделка
        /// </summary>
        [JsonProperty("client_short_code")]
        public long ClientShortCode { get; set; }

        /// <summary>
        /// Квалификатор принявшего решение о совершении сделки. Возможные значения: 
        /// «0» – не определено; 
        /// «1» – Natural Person; 
        /// «3» – Algorithm
        /// </summary>
        [JsonProperty("investment_decision_maker_qualifier")]
        public int InvestmentDecisionMakerQualifier { get; set; }

        /// <summary>
        /// Краткий идентификатор принявшего решение о совершении сделки
        /// </summary>
        [JsonProperty("investment_decision_maker_short_code")]
        public long InvestmentDecisionMakerShortCode { get; set; }

        /// <summary>
        /// Квалификатор трейдера, исполнившего заявку, по которой совершена сделка.Возможные значения: 
        /// «0» – не определено; 
        /// «1» – Natural Person; 
        /// «3» – Algorithm
        /// </summary>
        [JsonProperty("executing_trader_qualifier")]
        public int ExecutingTraderQualifier { get; set; }

        /// <summary>
        /// Краткий идентификатор трейдера, исполнившего заявку, по которой совершена сделка
        /// </summary>
        [JsonProperty("executing_trader_short_code")]
        public long ExecutingTraderShortCode { get; set; }

        /// <summary>
        /// Признак того, что транзакция совершена по правилам пре-трейда. Возможные значения битовых флагов: 
        /// бит 0 (0x1) – RFPT; 
        /// бит 1 (0x2) – NLIQ; 
        /// бит 2 (0x4) – OILQ; 
        /// бит 3 (0x8) – PRC; 
        /// бит 4 (0x10)– SIZE; 
        /// бит 5 (0x20) – ILQD
        /// </summary>
        [JsonProperty("waiver_flag")]
        public TradeWaiverFlags WaiverFlag { get; set; }
        //public int WaiverFlag { get; set; }

        /// <summary>
        /// Идентификатор базового инструмента на сервере для multileg-инструментов 
        /// </summary>
        [JsonProperty("mleg_base_sid")]
        public long MlegBaseSID { get; set; }

        /// <summary>
        /// Квалификатор операции. Возможные значения: 
        /// «0» – не определено; 
        /// «1» – Buy; 
        /// «2» – Sell; 
        /// «3» – Sell short; 
        /// «4» – Sell short exempt; 
        /// «5» – Sell undiclosed
        /// </summary>
        [JsonProperty("side_qualifier")]
        public int SideQualifier { get; set; }

        /// <summary>
        /// OTC post-trade индикатор. Возможные значения битовых флагов: 
        /// бит 0 (0x1) – Benchmark; 
        /// бит 1 (0x2) – Agency cross;
        /// бит 2 (0x4) – Large in scale; 
        /// бит 3 (0x8) – Illiquid instrument;
        /// бит 4 (0x10) – Above specified size; 
        /// бит 5 (0x20) – Cancellations; 
        /// бит 6 (0x40) – Amendments; 
        /// бит 7 (0x80) – Special dividend;
        /// бит 8 (0x100) – Price improvement;
        /// бит 9 (0x200) – Duplicative; 
        /// бит 10 (0x400) – Not contributing to the price discovery process; 
        /// бит 11 (0x800) – Package; 
        /// бит 12 (0x1000) – Exchange for Physical
        /// </summary>
        [JsonProperty("otc_post_trade_indicator")]
        public TradeOTCPostTradeIndicatorFlags OTCPostTradeIndicator { get; set; }
        //public int OTCPostTradeIndicator { get; set; }

        /// <summary>
        /// Роль в исполнении заявки. Возможные значения: 
        /// «0» – не определено; 
        /// «1» – Agent; 
        /// «2» – Principal; 
        /// «3» – Riskless principal; 
        /// «4» – CFG give up; 
        /// «5» – Cross as agent; 
        /// «6» – Matched principal; 
        /// «7» – Proprietary; 
        /// «8» – Individual; 
        /// «9» – Agent for other member; 
        /// «10» – Mixed; 
        /// «11» – Market maker
        /// </summary>
        [JsonProperty("capacity")]
        public int Capacity { get; set; }

        /// <summary>
        /// Кросс-курс валюты цены сделки к валюте расчетов по сделке
        /// </summary>
        [JsonProperty("cross_rate")]
        public double CrossRate { get; set; }

        /// <summary>
        /// Дата фиксации котировки для расчетов. Заполняется для контрактов типа «NDF» (см. параметр operation_type)
        /// </summary>
        [JsonProperty("fixing_date")]
        public double FixingDate { get; set; }

        /// <summary>
        /// Дата, начиная с которой допускается совершение валютирования. Заполняется только для контрактов типа «FLEX FORWARD» (см. параметр operation_type)
        /// </summary>
        [JsonProperty("start_date")]
        public double StartDate { get; set; }

        /// <summary>
        /// Тип совершаемой операции. Возможные значения:
        /// «-1» – «NOT_DEFINED»;
        /// «0» – «SPOT»;
        /// «1» – «FORWARD»;
        /// «2» – «SWAP»;
        /// «6» – «NDF»;
        /// «7» – «FLEX FORWARD»
        /// </summary>
        [JsonProperty("operation_type")]
        public int OperationType { get; set; }

        /// <summary>
        /// Цена котировки по спот-инструменту в момент совершения сделки
        /// </summary>
        [JsonProperty("spot_rate")]
        public double SpotRate { get; set; }

        /// <summary>
        /// Код валюты комиссии торговой системы
        /// </summary>
        [JsonProperty("ts_commission_currency")]
        public string TSCommissionCurrency { get; set; }

        /// <summary>
        /// Код валюты комиссии брокера
        /// </summary>
        [JsonProperty("broker_commission_currency")]
        public string BrokerCommissionCurrency { get; set; }

        /// <summary>
        /// Идентификатор торговой сессии. Возможные значения:
        /// «0» –«Не определено»; 
        /// «1» –«Основная сессия»; 
        /// «2» –«Дополнительная сессия»; 
        /// «3» –«Итоги дня»
        /// </summary>
        [JsonProperty("trading_session")]
        public int TradingSession { get; set; }

        /// <summary>
        /// Идентификатор индикативной ставки
        /// </summary>
        [JsonProperty("benchmark")]
        public string Benchmark { get; set; }

        /// <summary>
        /// Тип сделки депозита. Возможные значения:
        /// «0» – сделка не типа «депозит»; 
        /// «1» – «Намерение»; 
        /// «2» –«Депозит»; 
        /// «3» –«Изъятие»; 
        /// «4» –«Закрытие планового пополнения»; 
        /// «5» –«Принудительное закрытие»; 
        /// «6» –«Перенос пополнения» 
        /// </summary>
        [JsonProperty("deposit_intent")]
        public int DepositIntent { get; set; }

        /// <summary>
        /// День Т+1 для сделок РЕПО с Открытой датой
        /// </summary>
        [JsonProperty("open_repo2date")]
        public double OpenRepo2Date { get; set; }

        /// <summary>
        /// Стоимость выкупа РЕПО с открытой датой в день T+1
        /// </summary>
        [JsonProperty("open_repo2value")]
        public double OpenRepo2Value { get; set; }
    }
}
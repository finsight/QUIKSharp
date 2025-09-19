using Newtonsoft.Json;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Описание параметров Таблицы сделок для исполнения
    /// </summary>
    public class NegTrade : IWithLuaTimeStamp
    {
        [JsonProperty("lua_timestamp")]
        public long LuaTimeStamp { get; internal set; }

        /// <summary>
        /// Номер сделки в торговой системе
        /// </summary>
        [JsonProperty("trade_num")]
        public long TradeNum { get; set; }

        /// <summary>
        /// Дата торгов
        /// </summary>
        [JsonProperty("trade_date")]
        public int TradeDate { get; set; }

        /// <summary>
        /// Дата расчетов
        /// </summary>
        [JsonProperty("settle_date")]
        public double SettleDate { get; set; }

        ///// <summary>
        ///// Набор битовых флагов
        ///// </summary>
        //[JsonProperty("flags")]
        //public OrderTradeFlags Flags { get; set; }

        /// <summary>
        /// Поручение/комментарий, обычно: код клиента/номер поручения
        /// </summary>
        [JsonProperty("brokerref")]
        public string Comment { get; set; }

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
        /// Код фирмы партнера
        /// </summary>
        [JsonProperty("cpfirmid")]
        public string CpFirmId { get; set; }

        /// <summary>
        /// Счет депо партнера
        /// </summary>
        [JsonProperty("cpaccount")]
        public string CpAccount { get; set; }

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
        /// Код расчетов
        /// </summary>
        [JsonProperty("settlecode")]
        public string SettleCode { get; set; }

        /// <summary>
        /// Отчет
        /// </summary>
        [JsonProperty("report_num")]
        public long ReportNumber { get; set; }

        /// <summary>
        /// Отчет партнера
        /// </summary>
        [JsonProperty("cpreport_num")]
        public long CpReportNumber { get; set; }

        /// <summary>
        /// Накопленный купонный доход
        /// </summary>
        [JsonProperty("accruedint")]
        public double AccruedInterest { get; set; }

        /// <summary>
        /// Номер сделки 1-ой части РЕПО
        /// </summary>
        [JsonProperty("repotradeno")]
        public long RepoTradeNo { get; set; }

        /// <summary>
        /// Цена 1-ой части РЕПО
        /// </summary>
        [JsonProperty("price1")]
        public double Price1 { get; set; }

        /// <summary>
        /// Ставка РЕПО (%)
        /// </summary>
        [JsonProperty("reporate")]
        public double RepoRate { get; set; }

        /// <summary>
        /// Цена выкупа
        /// </summary>
        [JsonProperty("price2")]
        public double Price2 { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

        /// <summary>
        /// Комиссия торговой системы
        /// </summary>
        [JsonProperty("ts_comission")]
        public double TSComission { get; set; }

        /// <summary>
        /// Остаток
        /// </summary>
        [JsonProperty("balance")]
        public double Balance { get; set; }

        /// <summary>
        /// Время исполнения
        /// </summary>
        [JsonProperty("settle_time")]
        public int SettleTime { get; set; }

        /// <summary>
        /// Сумма обязательства
        /// </summary>
        [JsonProperty("amount")]
        public double Amount { get; set; }

        /// <summary>
        /// Сумма РЕПО
        /// </summary>
        [JsonProperty("repovalue")]
        public double RepoValue { get; set; }

        /// <summary>
        /// Срок РЕПО, в календарных днях
        /// </summary>
        [JsonProperty("repoterm")]
        public double RepoTerm { get; set; }

        /// <summary>
        /// Объем выкупа РЕПО
        /// </summary>
        [JsonProperty("repo2value")]
        public double Repo2Value { get; set; }

        /// <summary>
        /// Сумма возврата РЕПО
        /// </summary>
        [JsonProperty("return_value")]
        public double ReturnValue { get; set; }

        /// <summary>
        /// Дисконт (%)
        /// </summary>
        [JsonProperty("discount")]
        public double Discount { get; set; }

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
        /// Исполнить («Да»/«Нет»)
        /// </summary>
        [JsonProperty("urgency_flag")]
        public int UrgencyFlag { get; set; }

        /// <summary>
        /// Тип. Возможные значения:
        /// «0» – «Внесистемная сделка», 
        /// «1» – «Первая часть сделки РЕПО»,
        /// «2» – «Вторая часть сделки РЕПО»,
        /// «3» – «Компенсационный взнос»,
        /// «4» – «Дефолтер: отложенные обязательства и требования»,
        /// «5» – «Пострадавший: отложенные обязательства и требования»
        /// </summary>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Направленность. Возможные значения:
        /// «1» – «Зачислить»,
        /// «2» – «Списать»
        /// </summary>
        [JsonProperty("operation_type")]
        public int OperationType { get; set; }

        /// <summary>
        /// Дисконт после взноса (%)
        /// </summary>
        [JsonProperty("expected_discount")]
        public double ExpectedDiscount { get; set; }

        /// <summary>
        /// Количество после взноса
        /// </summary>
        [JsonProperty("expected_quantity")]
        public decimal ExpectedQuantity { get; set; }

        /// <summary>
        /// Сумма РЕПО после взноса
        /// </summary>
        [JsonProperty("expected_repovalue")]
        public double ExpectedRepoValue { get; set; }

        /// <summary>
        /// Стоимость выкупа после взноса
        /// </summary>
        [JsonProperty("expected_repo2value")]
        public double ExpectedRepo2Value { get; set; }

        /// <summary>
        /// Сумма возврата после взноса
        /// </summary>
        [JsonProperty("expected_return_value")]
        public double ExpectedReturnValue { get; set; }

        /// <summary>
        /// Номер заявки в торговой системе
        /// </summary>
        [JsonProperty("order_num")]
        public long OrderNum { get; set; }

        /// <summary>
        /// Дата заключения
        /// </summary>
        [JsonProperty("report_trade_date")]
        public int ReportTradeDate { get; set; }

        /// <summary>
        /// Состояние расчетов по сделке. Возможные значения: 
        /// «1» – «Processed», 
        /// «2» – «Not processed»,
        /// «3» – «Is processing»
        /// </summary>
        [JsonProperty("settled")]
        public int Settled { get; set; }

        /// <summary>
        /// Тип клиринга. Возможные значения: 
        /// «1» – «Not set»,
        /// «2» – «Simple», 
        /// «3» – «Multilateral»
        /// </summary>
        [JsonProperty("clearing_type")]
        public int ClearingType { get; set; }

        /// <summary>
        /// Комиссия за отчет
        /// </summary>
        [JsonProperty("report_comission")]
        public double ReportComission { get; set; }

        /// <summary>
        /// Купонная выплата
        /// </summary>
        [JsonProperty("coupon_payment")]
        public double CouponPayment { get; set; }

        /// <summary>
        /// Выплата по основному долгу
        /// </summary>
        [JsonProperty("principal_payment")]
        public double PrincipalPayment { get; set; }

        /// <summary>
        /// Дата выплаты по основному долгу
        /// </summary>
        [JsonProperty("principal_payment_date")]
        public int PrincipalPaymentDate { get; set; }

        /// <summary>
        /// Дата следующего дня расчетов
        /// </summary>
        [JsonProperty("nextdaysettle")]
        public int NextDaySettle { get; set; }

        /// <summary>
        /// Валюта расчетов
        /// </summary>
        [JsonProperty("settle_currency")]
        public string SettleCurrency { get; set; }

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
        /// Сумма отступного в валюте сделки
        /// </summary>
        [JsonProperty("compval")]
        public double CompValue { get; set; }

        /// <summary>
        /// Идентификационный номер витринной сделки
        /// </summary>
        [JsonProperty("parenttradeno")]
        public long ParentTradeNo { get; set; }

        /// <summary>
        /// Расчетная организация
        /// </summary>
        [JsonProperty("bankid")]
        public string BankId { get; set; }

        /// <summary>
        /// Код позиции
        /// </summary>
        [JsonProperty("bankaccid")]
        public string BankAccID { get; set; }

        /// <summary>
        /// Количество инструментов к исполнению (в лотах)
        /// </summary>
        [JsonProperty("precisebalance")]
        public decimal PreciseBalance { get; set; }

        /// <summary>
        /// Время подтверждения в формате "ЧЧММСС"
        /// </summary>
        [JsonProperty("confirmtime")]
        public int ConfirmTime { get; set; }

        /// <summary>
        /// Расширенные флаги сделки для исполнения. Возможные значения: 
        /// «1» – «Подтверждена контрагентом»; 
        /// «2» – «Подтверждена» 
        /// </summary>
        [JsonProperty("ex_flags")]
        public int ExFlags { get; set; }

        /// <summary>
        /// Номер поручения
        /// </summary>
        [JsonProperty("confirmreport")]
        public long ConfirmReport { get; set; }

        /// <summary>
        /// Внешняя ссылка, используется для обратной связи с внешними системами
        /// </summary>
        [JsonProperty("extref")]
        public string ExtRef { get; set; }

        /// <summary>
        /// Идентификатор индикативной ставки
        /// </summary>
        [JsonProperty("benchmark")]
        public string Benchmark { get; set; }

        /// <summary>
        /// Дата изменения индикатора в формате "ГГГГММДД"
        /// </summary>
        [JsonProperty("benchmark_change_date")]
        public int BenchmarkChangeDate { get; set; }

        /// <summary>
        /// Значение индикатора, в %
        /// </summary>
        [JsonProperty("benchmark_value")]
        public decimal BenchmarkValue { get; set; }

        /// <summary>
        /// Причина отмены
        /// </summary>
        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

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

        /// <summary>
        /// Отчет на закрытие сделки РЕПО с Открытой датой
        /// </summary>
        [JsonProperty("open_repo_report_no")]
        public decimal OpenRepoReportNo { get; set; }

        /// <summary>
        /// Статус РЕПО с открытой датой. Возможные значения: 
        /// «0» – «Нет»; 
        /// «1» – «Да»; 
        /// «2» – «Срочная» 
        /// </summary>
        [JsonProperty("open_repo_status")]
        public int OpenRepoStatus { get; set; }
    }
}
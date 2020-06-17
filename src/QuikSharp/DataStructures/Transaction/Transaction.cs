// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Формат .tri-файла с параметрами транзакций
    /// Адоптированный под QLua
    /// </summary>
    public class Transaction
    {
        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении ответа на транзакцию пользователя.
        /// </summary>
        public event TransReplyHandler OnTransReply;

        internal void OnTransReplyCall(TransactionReply reply)
        {
            OnTransReply?.Invoke(reply);
            // this should happen only once per transaction id
            Trace.Assert(TransactionReply == null);
            TransactionReply = reply;
        }

        /// <summary>
        /// TransactionReply
        /// </summary>
        public TransactionReply TransactionReply { get; set; }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении новой заявки или при изменении параметров существующей заявки.
        /// </summary>
        public event OrderHandler OnOrder;

        internal void OnOrderCall(Order order)
        {
            OnOrder?.Invoke(order);
            if (Orders == null)
            {
                Orders = new List<Order>();
            }

            Orders.Add(order);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении новой стоп-заявки или при изменении параметров существующей стоп-заявки.
        /// </summary>
        public event StopOrderHandler OnStopOrder;

        internal void OnStopOrderCall(StopOrder stopOrder)
        {
            OnStopOrder?.Invoke(stopOrder);
            if (StopOrders == null)
            {
                StopOrders = new List<StopOrder>();
            }

            StopOrders.Add(stopOrder);
        }

        /// <summary>
        /// Orders
        /// </summary>
        public List<Order> Orders { get; set; }

        /// <summary>
        /// StopOrders
        /// </summary>
        public List<StopOrder> StopOrders { get; set; }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении сделки.
        /// </summary>
        public event TradeHandler OnTrade;

        internal void OnTradeCall(Trade trade)
        {
            OnTrade?.Invoke(trade);
            if (Trades == null)
            {
                Trades = new List<Trade>();
            }

            Trades.Add(trade);
        }

        /// <summary>
        /// Trades
        /// </summary>
        public List<Trade> Trades { get; set; }

        // TODO inspect with actual data
        /// <summary>
        /// Транзакция исполнена
        /// </summary>
        /// <returns></returns>
        public bool IsComepleted()
        {
            if (Orders == null || Orders.Count == 0) return false;
            var last = Orders[Orders.Count - 1];
            return !last.Flags.HasFlag(OrderTradeFlags.Active)
                   &&
                   !last.Flags.HasFlag(OrderTradeFlags.Canceled);
        }

        /// <summary>
        /// Error message returned by sendTransaction
        /// </summary>
        public string ErrorMessage { get; set; }

        ///////////////////////////////////////////////////////////////////////////////
        ///
        ///  Transaction specification properties start here
        ///
        ///////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Код класса, по которому выполняется транзакция, например TQBR. Обязательный параметр
        /// </summary>
        public string CLASSCODE { get; set; }

        /// <summary>
        /// Код инструмента, по которому выполняется транзакция, например SBER
        /// </summary>
        public string SECCODE { get; set; }

        /// <summary>
        /// Вид транзакции, имеющий одно из следующих значений:
        /// </summary>
        public TransactionAction? ACTION { get; set; }

        /// <summary>
        /// Идентификатор участника торгов (код фирмы)
        /// </summary>
        public string FIRM_ID { get; set; }

        /// <summary>
        /// Номер счета Трейдера. Параметр обязателен при «ACTION» = «KILL_ALL_FUTURES_ORDERS». Параметр чувствителен к верхнему/нижнему регистру символов.
        /// </summary>
        public string ACCOUNT { get; set; }

        /// <summary>
        /// 20-ти символьное составное поле, может содержать код клиента и текстовый комментарий с тем же разделителем, что и при вводе заявки вручную. Параметр используется только для групповых транзакций. Необязательный параметр
        /// </summary>
        public string CLIENT_CODE { get; set; }

        /// <summary>
        /// Количество лотов в заявке, обязательный параметр
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int>))]
        public int QUANTITY { get; set; }

        /// <summary>
        /// Цена заявки, за единицу инструмента. Обязательный параметр.
        /// При выставлении рыночной заявки (TYPE=M) на Срочном рынке FORTS
        /// необходимо указывать значение цены – укажите наихудшую
        /// (минимально или максимально возможную – в зависимости от направленности),
        /// заявка все равно будет исполнена по рыночной цене. Для других рынков при
        /// выставлении рыночной заявки укажите price= 0.
        /// </summary>
        [JsonConverter(typeof(DecimalG29ToStringConverter))]
        public decimal PRICE { get; set; }

        /// <summary>
        /// Направление заявки, обязательный параметр. Значения: «S» – продать, «B» – купить
        /// </summary>
        public TransactionOperation? OPERATION { get; set; }

        /// <summary>
        /// Уникальный идентификационный номер заявки, значение от 1 до 2 294 967 294
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? TRANS_ID { get; set; }

        // ReSharper restore InconsistentNaming

        // ReSharper disable InconsistentNaming

        /// <summary>
        /// Тип заявки, необязательный параметр.
        /// Значения: «L» – лимитированная (по умолчанию), «M» – рыночная
        /// </summary>
        public TransactionType? TYPE { get; set; }

        /// <summary>
        /// Признак того, является ли заявка заявкой Маркет-Мейкера. Возможные значения: «YES» или «NO». Значение по умолчанию (если параметр отсутствует): «NO»
        /// </summary>
        public YesOrNo? MARKET_MAKER_ORDER { get; set; }

        /// <summary>
        /// Условие исполнения заявки, необязательный параметр. Возможные значения:
        /// </summary>
        public ExecutionCondition? EXECUTION_CONDITION { get; set; }

        /// <summary>
        /// Объем сделки РЕПО-М в рублях
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? REPOVALUE { get; set; }

        /// <summary>
        /// Начальное значение дисконта в заявке на сделку РЕПО-М
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? START_DISCOUNT { get; set; }

        /// <summary>
        /// Нижнее предельное значение дисконта в заявке на сделку РЕПО-М
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? LOWER_DISCOUNT { get; set; }

        /// <summary>
        /// Верхнее предельное значение дисконта в заявке на сделку РЕПО-М
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? UPPER_DISCOUNT { get; set; }

        /// <summary>
        /// Стоп-цена, за единицу инструмента. Используется только при «ACTION» = «NEW_STOP_ORDER»
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? STOPPRICE { get; set; }

        /// <summary>
        /// Тип стоп-заявки. Возможные значения:
        /// </summary>
        public StopOrderKind? STOP_ORDER_KIND { get; set; }

        /// <summary>
        /// Класс инструмента условия. Используется только при «STOP_ORDER_KIND» = «CONDITION_PRICE_BY_OTHER_SEC».
        /// </summary>
        public string STOPPRICE_CLASSCODE { get; set; }

        /// <summary>
        /// Код инструмента условия. Используется только при «STOP_ORDER_KIND» = «CONDITION_PRICE_BY_OTHER_SEC»
        /// </summary>
        public string STOPPRICE_SECCODE { get; set; }

        /// <summary>
        /// Направление предельного изменения стоп-цены. Используется только при «STOP_ORDER_KIND» = «CONDITION_PRICE_BY_OTHER_SEC». Возможные значения:  «&lt;=» или «&gt;= »
        /// </summary>
        public string STOPPRICE_CONDITION { get; set; }

        /// <summary>
        /// Цена связанной лимитированной заявки. Используется только при «STOP_ORDER_KIND» = «WITH_LINKED_LIMIT_ORDER»
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? LINKED_ORDER_PRICE { get; set; }

        /// <summary>
        /// Срок действия стоп-заявки. Возможные значения: «GTC» – до отмены, «TODAY» - до окончания текущей торговой сессии, Дата в формате «ГГММДД».
        /// </summary>
        public string EXPIRY_DATE { get; set; }

        /// <summary>
        /// Цена условия «стоп-лимит» для заявки типа «Тэйк-профит и стоп-лимит»
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? STOPPRICE2 { get; set; }

        /// <summary>
        /// Признак исполнения заявки по рыночной цене при наступлении условия «стоп-лимит». Значения «YES» или «NO». Параметр заявок типа «Тэйк-профит и стоп-лимит»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? MARKET_STOP_LIMIT { get; set; }

        /// <summary>
        /// Признак исполнения заявки по рыночной цене при наступлении условия «тэйк-профит». Значения «YES» или «NO». Параметр заявок типа «Тэйк-профит и стоп-лимит»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? MARKET_TAKE_PROFIT { get; set; }

        /// <summary>
        /// Признак действия заявки типа «Тэйк-профит и стоп-лимит» в течение определенного интервала времени. Значения «YES» или «NO»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? IS_ACTIVE_IN_TIME { get; set; }

        /// <summary>
        /// Время начала действия заявки типа «Тэйк-профит и стоп-лимит» в формате «ЧЧММСС»
        /// </summary>
        [JsonConverter(typeof(HHMMSSDateTimeConverter))]
        public DateTime? ACTIVE_FROM_TIME { get; set; }

        /// <summary>
        /// Время окончания действия заявки типа «Тэйк-профит и стоп-лимит» в формате «ЧЧММСС»
        /// </summary>
        [JsonConverter(typeof(HHMMSSDateTimeConverter))]
        public DateTime? ACTIVE_TO_TIME { get; set; }

        /// <summary>
        /// Код организации – партнера по внебиржевой сделке.Применяется при «ACTION» = «NEW_NEG_DEAL», «ACTION» = «NEW_REPO_NEG_DEAL» или «ACTION» = «NEW_EXT_REPO_NEG_DEAL»
        /// </summary>
        public string PARTNER { get; set; }

        /// <summary>
        /// Номер заявки, снимаемой из торговой системы. Применяется при «ACTION» = «KILL_ORDER» или «ACTION» = «KILL_NEG_DEAL» или «ACTION» = «KILL_QUOTE»
        /// </summary>
        public string ORDER_KEY { get; set; }

        /// <summary>
        /// Номер стоп-заявки, снимаемой из торговой системы. Применяется только при «ACTION» = «KILL_STOP_ORDER»
        /// </summary>
        public string STOP_ORDER_KEY { get; set; }

        /// <summary>
        /// Код расчетов при исполнении внебиржевых заявок
        /// </summary>
        public string SETTLE_CODE { get; set; }

        /// <summary>
        /// Цена второй части РЕПО
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? PRICE2 { get; set; }

        /// <summary>
        /// Срок РЕПО. Параметр сделок РЕПО-М
        /// </summary>
        public string REPOTERM { get; set; }

        /// <summary>
        /// Ставка РЕПО, в процентах
        /// </summary>
        public string REPORATE { get; set; }

        /// <summary>
        /// Признак блокировки бумаг на время операции РЕПО («YES», «NO»)
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? BLOCK_SECURITIES { get; set; }

        /// <summary>
        /// Ставка фиксированного возмещения, выплачиваемого в случае неисполнения второй части РЕПО, в процентах
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? REFUNDRATE { get; set; }

        /// <summary>
        /// Текстовый комментарий, указанный в заявке - поручение (brokerref in Trades/Orders).
        /// Используется при снятии группы заявок
        /// </summary>
        [JsonProperty("brokerref")]
        public string Comment { get; set; }

        /// <summary>
        /// Признак крупной сделки (YES/NO). Параметр внебиржевой сделки
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? LARGE_TRADE { get; set; }

        /// <summary>
        /// Код валюты расчетов по внебиржевой сделки, например «SUR» – рубли РФ, «USD» – доллары США. Параметр внебиржевой сделки
        /// </summary>
        public string CURR_CODE { get; set; }

        /// <summary>
        /// Лицо, от имени которого и за чей счет регистрируется сделка (параметр внебиржевой сделки). Возможные значения:
        /// </summary>
        public ForAccount? FOR_ACCOUNT { get; set; }

        /// <summary>
        /// Дата исполнения внебиржевой сделки
        /// </summary>
        public string SETTLE_DATE { get; set; }

        /// <summary>
        /// Признак снятия стоп-заявки при частичном исполнении связанной лимитированной заявки. Используется только при «STOP_ORDER_KIND» = «WITH_LINKED_LIMIT_ORDER». Возможные значения: «YES» или «NO»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? KILL_IF_LINKED_ORDER_PARTLY_FILLED { get; set; }

        /// <summary>
        /// Величина отступа от максимума (минимума) цены последней сделки. Используется при «STOP_ORDER_KIND» = «TAKE_PROFIT_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? OFFSET { get; set; }

        /// <summary>
        /// Единицы измерения отступа. Возможные значения:
        /// </summary>
        public OffsetUnits? OFFSET_UNITS { get; set; }

        /// <summary>
        /// Величина защитного спрэда. Используется при «STOP_ORDER_KIND» = «TAKE_PROFIT_STOP_ORDER» или ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? SPREAD { get; set; }

        /// <summary>
        /// Единицы измерения защитного спрэда. Используется при «STOP_ORDER_KIND» = «TAKE_PROFIT_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        public OffsetUnits? SPREAD_UNITS { get; set; }

        /// <summary>
        /// Регистрационный номер заявки-условия. Используется при «STOP_ORDER_KIND» = «ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        public string BASE_ORDER_KEY { get; set; }

        /// <summary>
        /// Признак использования в качестве объема заявки «по исполнению» исполненного количества бумаг заявки-условия. Возможные значения: «YES» или «NO». Используется при «STOP_ORDER_KIND» = «ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? USE_BASE_ORDER_BALANCE { get; set; }

        /// <summary>
        /// Признак активации заявки «по исполнению» при частичном исполнении заявки-условия. Возможные значения: «YES» или «NO». Используется при «STOP_ORDER_KIND» = «ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? ACTIVATE_IF_BASE_ORDER_PARTLY_FILLED { get; set; }

        /// <summary>
        /// Идентификатор базового контракта для фьючерсов или опционов.
        /// Обязательный параметр снятия заявок на рынке FORTS
        /// </summary>
        public string BASE_CONTRACT { get; set; }

        /// <summary>
        ///  Режим перестановки заявок на рынке FORTS. Параметр операции «ACTION» = «MOVE_ORDERS» Возможные значения: «0» – оставить количество в заявках без изменения, «1» – изменить количество в заявках на новые, «2» – при несовпадении новых количеств с текущим хотя бы в одной заявке, обе заявки снимаются
        /// </summary>
        public string MODE { get; set; }

        /// <summary>
        /// Номер первой заявки
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? FIRST_ORDER_NUMBER { get; set; }

        /// <summary>
        /// Количество в первой заявке
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int?>))]
        public int? FIRST_ORDER_NEW_QUANTITY { get; set; }

        /// <summary>
        /// Цена в первой заявке
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? FIRST_ORDER_NEW_PRICE { get; set; }

        /// <summary>
        /// Номер второй заявки
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? SECOND_ORDER_NUMBER { get; set; }

        /// <summary>
        /// Количество во второй заявке
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<int?>))]
        public int? SECOND_ORDER_NEW_QUANTITY { get; set; }

        /// <summary>
        /// Цена во второй заявке
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<decimal?>))]
        public decimal? SECOND_ORDER_NEW_PRICE { get; set; }

        /// <summary>
        /// Признак снятия активных заявок по данному инструменту. Используется только при «ACTION» = «NEW_QUOTE». Возможные значения: «YES» или «NO»
        /// </summary>
        // TODO (?) Is No default here?
        public YesOrNo? KILL_ACTIVE_ORDERS { get; set; }

        /// <summary>
        /// Направление операции в сделке, подтверждаемой отчетом
        /// </summary>
        public string NEG_TRADE_OPERATION { get; set; }

        /// <summary>
        /// Номер подтверждаемой отчетом сделки для исполнения
        /// </summary>
        [JsonConverter(typeof(ToStringConverter<long?>))]
        public long? NEG_TRADE_NUMBER { get; set; }

        /// <summary>
        /// Лимит открытых позиций, при «Тип лимита» = «Ден.средства» или «Всего»
        /// </summary>
        public string VOLUMEMN { get; set; }

        /// <summary>
        /// Лимит открытых позиций, при «Тип лимита» = «Залоговые ден.средства»
        /// </summary>
        public string VOLUMEPL { get; set; }

        /// <summary>
        /// Коэффициент ликвидности
        /// </summary>
        public string KFL { get; set; }

        /// <summary>
        /// Коэффициент клиентского гарантийного обеспечения
        /// </summary>
        public string KGO { get; set; }

        /// <summary>
        /// Параметр, который определяет, будет ли загружаться величина КГО при загрузке лимитов из файла: при USE_KGO=Y – величина КГО загружает. при USE_KGO=N – величина КГО не загружается. При установке лимита на Срочном рынке Московской Биржи с принудительным понижением (см. Создание лимита) требуется указать USE_KGO= Y
        /// </summary>
        public string USE_KGO { get; set; }

        /// <summary>
        /// Признак проверки попадания цены заявки в диапазон допустимых цен.
        /// Параметр Срочного рынка FORTS. Необязательный параметр транзакций
        /// установки новых заявок по классам «Опционы ФОРТС» и «РПС: Опционы ФОРТС».
        /// Возможные значения: «YES» - выполнять проверку, «NO» - не выполнять
        /// </summary>
        public YesOrNo? CHECK_LIMITS { get; set; }

        /// <summary>
        /// Ссылка, которая связывает две сделки РЕПО или РПС. Сделка может быть заключена только между контрагентами, указавшими одинаковое значение этого параметра в своих заявках. Параметр представляет собой набор произвольный набор количеством до 10 символов (допускаются цифры и буквы). Необязательный параметр
        /// </summary>
        public string MATCHREF { get; set; }

        /// <summary>
        /// Режим корректировки ограничения по фьючерсным счетам. Возможные значения: «Y» - включен, установкой лимита изменяется действующее значение, «N» - выключен (по умолчанию), установкой лимита задается новое значение
        /// </summary>
        public string CORRECTION { get; set; }

        // ReSharper restore InconsistentNaming

        [JsonIgnore] // do not pass to Quik
        public bool IsManual { get; set; }
    }
}
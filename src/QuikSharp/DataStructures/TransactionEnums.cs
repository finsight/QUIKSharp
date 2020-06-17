// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Вид транзакции, имеющий одно из следующих значений:
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionAction>))]
    public enum TransactionAction
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// новая заявка,
        /// </summary>
        NEW_ORDER = 1,

        /// <summary>
        /// новая заявка на внебиржевую сделку,
        /// </summary>
        NEW_NEG_DEAL,

        /// <summary>
        /// новая заявка на сделку РЕПО,
        /// </summary>
        NEW_REPO_NEG_DEAL,

        /// <summary>
        /// новая заявка на сделку модифицированного РЕПО (РЕПО-М),
        /// </summary>
        NEW_EXT_REPO_NEG_DEAL,

        /// <summary>
        /// новая стоп-заявка,
        /// </summary>
        NEW_STOP_ORDER,

        /// <summary>
        /// снять заявку,
        /// </summary>
        KILL_ORDER,

        /// <summary>
        /// снять заявку на внебиржевую сделку или заявку на сделку РЕПО,
        /// </summary>
        KILL_NEG_DEAL,

        /// <summary>
        /// снять стоп-заявку,
        /// </summary>
        KILL_STOP_ORDER,

        /// <summary>
        /// снять все заявки из торговой системы,
        /// </summary>
        KILL_ALL_ORDERS,

        /// <summary>
        /// снять все стоп-заявки,
        /// </summary>
        KILL_ALL_STOP_ORDERS,

        /// <summary>
        /// снять все заявки на внебиржевые сделки и заявки на сделки РЕПО,
        /// </summary>
        KILL_ALL_NEG_DEALS,

        /// <summary>
        /// снять все заявки на рынке FORTS,
        /// </summary>
        KILL_ALL_FUTURES_ORDERS,

        /// <summary>
        /// удалить лимит открытых позиций на спот-рынке RTS Standard,
        /// </summary>
        KILL_RTS_T4_LONG_LIMIT,

        /// <summary>
        /// удалить лимит открытых позиций клиента по спот-активу на рынке RTS Standard,
        /// </summary>
        KILL_RTS_T4_SHORT_LIMIT,

        /// <summary>
        /// переставить заявки на рынке FORTS,
        /// </summary>
        MOVE_ORDERS,

        /// <summary>
        /// новая безадресная заявка,
        /// </summary>
        NEW_QUOTE,

        /// <summary>
        /// снять безадресную заявку,
        /// </summary>
        KILL_QUOTE,

        /// <summary>
        /// новая  заявка-отчет о подтверждении транзакций в режимах РПС и РЕПО,
        /// </summary>
        NEW_REPORT,

        /// <summary>
        /// новое ограничение по фьючерсному счету
        /// </summary>
        SET_FUT_LIMIT,

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Тип заявки, необязательный параметр.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionType>))]
    public enum TransactionType
    {
        /// <summary>
        /// «L» - лимитированная (по умолчанию),
        /// </summary>
        L = 0,

        /// <summary>
        /// «M» – рыночная
        /// </summary>
        M
    }

    /// <summary>
    /// YES or NO
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<YesOrNo>))]
    public enum YesOrNo
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        ///
        /// </summary>
        NO = 1,

        /// <summary>
        ///
        /// </summary>
        YES

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// YES or NO with NO default
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<YesOrNoDefault>))]
    public enum YesOrNoDefault
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        ///
        /// </summary>
        NO = 0,

        /// <summary>
        ///
        /// </summary>
        YES

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Тип заявки, необязательный параметр.
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<TransactionOperation>))]
    public enum TransactionOperation
    {
        /// <summary>
        /// «B» – купить
        /// </summary>
        B = 1,

        /// <summary>
        /// «S» – продать
        /// </summary>
        S = 2
    }

    /// <summary>
    /// Условие исполнения заявки, необязательный параметр. Возможные значения:
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<ExecutionCondition>))]
    public enum ExecutionCondition
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// «PUT_IN_QUEUE» – поставить в очередь (по умолчанию),
        /// </summary>
        PUT_IN_QUEUE = 0,

        /// <summary>
        /// «FILL_OR_KILL» – немедленно или отклонить,
        /// </summary>
        FILL_OR_KILL = 1,

        /// <summary>
        /// «KILL_BALANCE» – снять остаток
        /// </summary>
        KILL_BALANCE = 2

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Тип стоп-заявки. Возможные значения:
    /// Если параметр пропущен, то считается, что заявка имеет тип «стоп-лимит»
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<StopOrderKind>))]
    public enum StopOrderKind
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// стоп-лимит,
        /// Если параметр пропущен, то считается, что заявка имеет тип «стоп-лимит»
        /// </summary>
        SIMPLE_STOP_ORDER = 0,

        /// <summary>
        /// с условием по другой бумаге,
        /// </summary>
        CONDITION_PRICE_BY_OTHER_SEC,

        /// <summary>
        /// со связанной заявкой,
        /// </summary>
        WITH_LINKED_LIMIT_ORDER,

        /// <summary>
        /// тэйк-профит,
        /// </summary>
        TAKE_PROFIT_STOP_ORDER,

        /// <summary>
        /// тэйк-профит и стоп-лимит,
        /// </summary>
        TAKE_PROFIT_AND_STOP_LIMIT_ORDER,

        /// <summary>
        /// стоп-лимит по исполнению заявки,
        /// </summary>
        ACTIVATED_BY_ORDER_SIMPLE_STOP_ORDER,

        /// <summary>
        /// тэйк-профит по исполнению заявки,
        /// </summary>
        ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER,

        /// <summary>
        /// тэйк-профит и стоп-лимит по исполнению заявки.
        /// </summary>
        ACTIVATED_BY_ORDER_TAKE_PROFIT_AND_STOP_LIMIT_ORDER,

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Лицо, от имени которого и за чей счет регистрируется сделка
    /// (параметр внебиржевой сделки). Возможные значения:
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<ForAccount>))]
    public enum ForAccount
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// от своего имени, за свой счет,
        /// </summary>
        OWNOWN = 1,

        /// <summary>
        /// от своего имени, за счет клиента,
        /// </summary>
        OWNCLI,

        /// <summary>
        /// от своего имени, за счет доверительного управления,
        /// </summary>
        OWNDUP,

        /// <summary>
        /// от имени клиента, за счет клиента
        /// </summary>
        CLICLI,

        // ReSharper restore InconsistentNaming
    }

    /// <summary>
    /// Единицы измерения отступа. Используется при «STOP_ORDER_KIND» =
    /// «TAKE_PROFIT_STOP_ORDER» или «ACTIVATED_BY_ORDER_TAKE_PROFIT_STOP_ORDER»
    /// </summary>
    [JsonConverter(typeof(SafeEnumConverter<OffsetUnits>))]
    public enum OffsetUnits
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// «PERCENTS» – в процентах (шаг изменения – одна сотая процента),
        /// </summary>
        PERCENTS = 1,

        /// <summary>
        /// «PRICE_UNITS» – в параметрах цены (шаг изменения равен шагу цены по данному инструменту).
        /// </summary>
        PRICE_UNITS = 2

        // ReSharper restore InconsistentNaming
    }
}
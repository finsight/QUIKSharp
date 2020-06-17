// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using System;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Стоп-заявка
    /// На основе http://help.qlua.org/ch4_6_6.htm
    /// </summary>
    public class StopOrder
    {
        /// <summary>
        ///
        /// </summary>
        [JsonProperty("lua_timestamp")]
        public long LuaTimeStamp { get; internal set; }

        /// <summary>
        /// Регистрационный номер стоп-заявки на сервере QUIK
        /// </summary>
        [JsonProperty("order_num")]
        public long OrderNum { get; set; }

        /// <summary>
        /// Поручение/комментарий, обычно: код клиента/номер поручения
        /// </summary>
        [JsonProperty("brokerref")]
        public string Comment { get; set; }

        private int _flags;

        /// <summary>
        /// Набор битовых флагов.
        /// </summary>
        [JsonProperty("flags")]
        public int Flags
        {
            get { return _flags; }
            set
            {
                _flags = value;
                ParseFlags(value);
            }
        }

        /// <summary>
        /// Идентификатор транзакции.
        /// </summary>
        [JsonProperty("trans_id")]
        public long TransId { get; set; }

        /// <summary>
        /// Торговый счет
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

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
        /// Отступ от min/max
        /// </summary>
        [JsonProperty("offset")]
        public decimal Offset { get; set; }

        /// <summary>
        /// Единицы измерения отступа
        /// </summary>
        [JsonProperty("offset_unit")]
        public OffsetUnits OffsetUnit { get; set; }

        /// <summary>
        /// Защитный спред
        /// </summary>
        [JsonProperty("spread")]
        public decimal Spread { get; set; }

        /// <summary>
        /// Единицы измерения защитного спреда
        /// </summary>
        [JsonProperty("spread_unit")]
        public OffsetUnits SpreadUnit { get; set; }

        private int _stopOrderTypeInt;

        [JsonProperty("stop_order_type")]
        public int StopOrderTypeInt
        {
            get { return _stopOrderTypeInt; }
            set
            {
                _stopOrderTypeInt = value;
                StopOrderType = GetStopOrderType(value);
            }
        }

        /// <summary>
        /// Вид стоп заявки.
        /// </summary>
        [JsonIgnore]
        public StopOrderType StopOrderType { get; set; }

        private int _conditionInt;

        [JsonProperty("condition")]
        public int ConditionInt
        {
            get { return _conditionInt; }
            set
            {
                _conditionInt = value;
                Condition = GetCondition(value);
            }
        }

        /// <summary>
        /// Направленность стоп-цены.
        /// </summary>
        [JsonIgnore]
        public Condition Condition { get; set; }

        /// <summary>
        /// Стоп-цена
        /// </summary>
        [JsonProperty("condition_price")]
        public decimal ConditionPrice { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Количество в лотах
        /// </summary>
        [JsonProperty("qty")]
        public int Quantity { get; set; }

        /// <summary>
        /// Исполненное количество
        /// </summary>
        [JsonProperty("filled_qty")]
        public int FilledQuantity { get; set; }

        /// <summary>
        /// Стоп-лимит цена (для заявок типа «Тэйк-профит и стоп-лимит»)
        /// </summary>
        [JsonProperty("condition_price2")]
        public decimal ConditionPrice2 { get; set; }

        /// <summary>
        /// Номер заявки в торговой системе, зарегистрированной по наступлению условия стоп-цены.
        /// </summary>
        [JsonProperty("linkedorder")]
        public long LinkedOrder { get; set; }

        /// <summary>
        /// Заявка на продажу, иначе – на покупку.
        /// </summary>
        [JsonIgnore]
        public Operation Operation { get; set; }

        /// <summary>
        /// Состояние стоп-заявки.
        /// </summary>
        [JsonIgnore]
        public State State { get; set; }

        /// <summary>
        /// Стоп-заявка ожидает активации.
        /// </summary>
        [JsonIgnore]
        public bool IsWaitingActivation { get; set; }

        private void ParseFlags(int flags)
        {
            //Based on: http://help.qlua.org/ch9_2.htm

            if ((flags & 0x1) != 0)
                State = State.Active;
            else if ((flags & 0x2) != 0)
                State = State.Canceled;
            else
                State = State.Completed;

            Operation = (flags & 0x4) != 0 ? Operation.Sell : Operation.Buy;
            IsWaitingActivation = (flags & 0x20) != 0;
        }

        private StopOrderType GetStopOrderType(int code)
        {
            switch (code)
            {
                case 1:
                    return StopOrderType.StopLimit;

                case 6:
                    return StopOrderType.TakeProfit;

                case 9:
                    return StopOrderType.TakeProfitStopLimit;

                default:
                    return StopOrderType.NotImplemented;
            }
        }

        private Condition GetCondition(int code)
        {
            switch (code)
            {
                case 4:
                    return Condition.LessOrEqual;

                case 5:
                    return Condition.MoreOrEqual;

                default:
                    throw new Exception("Not supported code: " + code);
            }
        }
    }

    public enum StopOrderType
    {
        NotImplemented,

        /// <summary>
        /// «1» – стоп-лимит
        /// </summary>
        StopLimit,

        //«2» – условие по другому инструменту,
        //«3» – со связанной заявкой,

        /// <summary>
        ///«6» – тейк-профит
        /// </summary>
        TakeProfit,

        //«7» – стоп-лимит по исполнению активной заявки,
        //«8» –  тейк-профит по исполнению активной заявки,

        /// <summary>
        /// «9» - тэйк-профит и стоп-лимит
        /// </summary>
        TakeProfitStopLimit
    }

    /// <summary>
    /// Направленность стоп-цены. Возможные значения.
    /// </summary>
    public enum Condition
    {
        /// <summary>
        /// «4» – меньше или равно
        /// </summary>
        LessOrEqual,

        /// <summary>
        /// «5» – больше или равно
        /// </summary>
        MoreOrEqual
    }

    public enum Operation
    {
        Buy,
        Sell
    }
}
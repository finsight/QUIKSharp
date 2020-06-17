// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// На основе: http://help.qlua.org/ch4_6_11.htm
    /// Запись, которую можно получить из таблицы "Лимиты по бумагам" (depo_limits)
    /// </summary>
    public class DepoLimitEx
    {
        /// <summary>
        /// Код бумаги
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Счет депо
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

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
        /// Входящий остаток по бумагам
        /// </summary>
        [JsonProperty("openbal")]
        public long OpenBalance { get; set; }

        /// <summary>
        /// Входящий лимит по бумагам
        /// </summary>
        [JsonProperty("openlimit")]
        public long OpenLimit { get; set; }

        /// <summary>
        /// Текущий остаток по бумагам
        /// </summary>
        [JsonProperty("currentbal")]
        public long CurrentBalance { get; set; }

        /// <summary>
        /// Текущий лимит по бумагам
        /// </summary>
        [JsonProperty("currentlimit")]
        public long CurrentLimit { get; set; }

        /// <summary>
        /// Заблокировано на продажу количества лотов
        /// </summary>
        [JsonProperty("locked_sell")]
        public long LockedSell { get; set; }

        /// <summary>
        /// Заблокированного на покупку количества лотов
        /// </summary>
        [JsonProperty("locked_buy")]
        public long LockedBuy { get; set; }

        /// <summary>
        /// Стоимость ценных бумаг, заблокированных под покупку
        /// </summary>
        [JsonProperty("locked_buy_value")]
        public double LockedBuyValue { get; set; }

        /// <summary>
        /// Стоимость ценных бумаг, заблокированных под продажу
        /// </summary>
        [JsonProperty("locked_sell_value")]
        public double LockedSellValue { get; set; }

        /// <summary>
        /// Цена приобретения
        /// </summary>
        [JsonProperty("awg_position_price")]
        public double AweragePositionPrice { get; set; }

        private long _limitKindInt;

        /// <summary>
        /// Тип лимита. Возможные значения:
        /// ///«0» – обычные лимиты,
        /// ///значение не равное «0» – технологические лимиты
        /// </summary>
        [JsonProperty("limit_kind")]
        public long LimitKindInt
        {
            get { return _limitKindInt; }
            set
            {
                _limitKindInt = value;

                if (_limitKindInt == 0)
                    LimitKind = LimitKind.T0;
                else if (_limitKindInt == 1)
                    LimitKind = LimitKind.T1;
                else if (_limitKindInt == 2)
                    LimitKind = LimitKind.T2;
                else
                    LimitKind = LimitKind.NotImplemented;
            }
        }

        /// <summary>
        /// Тип лимита бумаги (Т0, Т1 или Т2).
        /// </summary>
        [JsonIgnore]
        public LimitKind LimitKind { get; private set; }
    }

    /// <summary>
    /// Тим лимита бумаги.
    /// </summary>
    public enum LimitKind
    {
        /// <summary>
        /// Тип лимита T0
        /// </summary>
        T0,

        /// <summary>
        /// Тип лимита Т1
        /// </summary>
        T1,

        /// <summary>
        /// Тип лимита Т2
        /// </summary>
        T2,

        /// <summary>
        /// Не учтенный в данной структуре тип лимита. Для деталей нужно смотреть свойство 'LimitKindInt'.
        /// </summary>
        NotImplemented
    }
}
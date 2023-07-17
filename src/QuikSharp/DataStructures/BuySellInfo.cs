// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Функция getBuySellInfo возвращает таблицу Lua с параметрами из таблицы QUIK «Купить/Продать», 
    /// означающими возможность купить либо продать указанный инструмент «sec_code» класса «class_code», 
    /// указанным клиентом «client_code» фирмы «firmid», по указанной цене «price». 
    /// Если цена равна «0», то используются лучшие значения спроса/предложения. 
    /// </summary>
    public class BuySellInfo
    {
        /// <summary>
        /// Признак маржинальности инструмента. Возможные значения:
        /// «0» – не маржинальная
        /// «1» – маржинальная
        /// </summary>
        [JsonProperty("is_margin_sec")]
        public string IsMarginSec { get; set; }

        /// <summary>
        /// Принадлежность инструмента к списку инструментов, принимаемых в обеспечение. Возможные значения: 
        /// «0» – не принимается в обеспечение; 
        /// «1» – принимается в обеспечение.
        /// </summary>
        [JsonProperty("is_asset_sec")]
        public string IsAssetSec { get; set; }

        /// <summary>
        /// Текущая позиция по инструменту, в лотах 
        /// </summary>
        [JsonProperty("balance")]
        public string Balance { get; set; }

        /// <summary>
        /// Оценка количества лотов, доступных на покупку по указанной цене *
        /// </summary>
        [JsonProperty("can_buy")]
        public string CanBuy { get; set; }

        /// <summary>
        /// Оценка количества лотов, доступных на продажу по указанной цене *
        /// </summary>
        [JsonProperty("can_sell")]
        public string CanSell { get; set; }

        /// <summary>
        /// Денежная оценка позиции по инструменту по ценам спроса/предложения
        /// </summary>
        [JsonProperty("position_valuation")]
        public string PositionValuation { get; set; }

        /// <summary>
        /// Оценка стоимости позиции по цене последней сделки 
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Оценка стоимости позиции клиента, рассчитанная по цене закрытия предыдущей торговой сессии
        /// </summary>
        [JsonProperty("open_value")]
        public string OpenValue { get; set; }

        /// <summary>
        /// Предельный размер позиции по данному инструменту, принимаемый в обеспечение длинных позиций
        /// </summary>
        [JsonProperty("lim_long")]
        public string LimLong { get; set; }

        /// <summary>
        /// Коэффициент дисконтирования, применяемый для длинных позиций по данному инструменту
        /// </summary>
        [JsonProperty("long_coef")]
        public string LongCoef { get; set; }

        /// <summary>
        /// Предельный размер короткой позиции по данному инструменту
        /// </summary>
        [JsonProperty("lim_short")]
        public string LimShort { get; set; }

        /// <summary>
        /// Коэффициент дисконтирования, применяемый для коротких позиций по данному инструменту
        /// </summary>
        [JsonProperty("short_coef")]
        public string ShortCoef { get; set; }

        /// <summary>
        /// Оценка стоимости позиции по цене последней сделки, с учетом дисконтирующих коэффициентов
        /// </summary>
        [JsonProperty("value_coef")]
        public string ValueCoef { get; set; }

        /// <summary>
        /// Оценка стоимости позиции клиента, рассчитанная по цене закрытия предыдущей торговой сессии с учетом дисконтирующих коэффициентов
        /// </summary>
        [JsonProperty("open_value_coef")]
        public string OpenValueCoef { get; set; }

        /// <summary>
        /// Процентное отношение стоимости позиции по данному инструменту к стоимости всех активов клиента, рассчитанное по текущим ценам
        /// </summary>
        [JsonProperty("share")]
        public string Share { get; set; }

        /// <summary>
        /// Средневзвешенная стоимость коротких позиций по инструментам
        /// </summary>
        [JsonProperty("short_wa_price")]
        public string ShortWAPrice { get; set; }

        /// <summary>
        /// Средневзвешенная стоимость длинных позиций по инструментам
        /// </summary>
        [JsonProperty("long_wa_price")]
        public string LongWAPrice { get; set; }

        /// <summary>
        /// Разница между средневзвешенной ценой приобретения инструментов и их рыночной оценки
        /// </summary>
        [JsonProperty("profit_loss")]
        public string ProfitLoss { get; set; }

        /// <summary>
        /// Коэффициент корреляции между инструментами
        /// </summary>
        [JsonProperty("spread_hc")]
        public string SpreadHC { get; set; }

        /// <summary>
        /// Максимально возможное количество инструментов в заявке на покупку этого инструмента на этом классе на собственные средства клиента, исходя из цены лучшего предложения
        /// </summary>
        [JsonProperty("can_buy_own")]
        public string CanBuyOwn { get; set; }

        /// <summary>
        /// Срок расчётов. Возможные значения: положительные целые числа, начиная с «0», соответствующие срокам расчётов из таблицы «Позиции по инструментам»: «0» – T0, «1» – T1, «2» – T2 и т.д.
        /// </summary>
        [JsonProperty("limit_kind")]
        public string LimitKind { get; set; }

        /// <summary>
        /// Эффективный начальный дисконт для длинной позиции. Заполняется для клиентов типа «МД»
        /// </summary>
        [JsonProperty("d_long")]
        public string DLong { get; set; }

        /// <summary>
        /// Эффективный минимальный дисконт для длинной позиции. Заполняется для клиентов типа «МД»
        /// </summary>
        [JsonProperty("d_min_long")]
        public string DMinLong { get; set; }

        /// <summary>
        /// Эффективный начальный дисконт для короткой позиции. Заполняется для клиентов типа «МД»
        /// </summary>
        [JsonProperty("d_short")]
        public string DShort { get; set; }

        /// <summary>
        /// Эффективный минимальный дисконт для короткой позиции. Заполняется для клиентов типа «МД»
        /// </summary>
        [JsonProperty("d_min_short")]
        public string DMinShort { get; set; }

        /// <summary>
        /// Тип клиента. Возможные значения: 
        /// «1» – «МЛ»; 
        /// «2» – «МП»; 
        /// «3» – «МОП»; 
        /// «4» – «МД»
        /// </summary>
        [JsonProperty("client_type")]
        public string ClientType { get; set; }

        /// <summary>
        /// Признак того, является ли инструмент разрешенным для покупки на заемные средства. Возможные значения: 
        /// «0» – не разрешен; 
        /// «1» – разрешен
        /// Заполняется для клиентов типа «МД»
        /// </summary>
        [JsonProperty("is_long_allowed")]
        public string IsLongAllowed { get; set; }

        /// <summary>
        /// Признак того, является ли инструмент  разрешенным для продажи на заемные средства. Возможные значения: 
        /// «0» – не разрешен; 
        /// «1» – разрешен
        /// Заполняется для клиентов типа «МД»
        /// </summary>
        [JsonProperty("is_short_allowed")]
        public string IsShortAllowed { get; set; }

        // (*) В зависимости от настроек сервера QUIK, величина может выражаться в лотах или в штуках. Уточните единицы измерения у обслуживающего брокера.
    }
}
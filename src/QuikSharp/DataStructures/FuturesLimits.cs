// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При получении изменений ограничений по срочному рынку функция возвращает таблицу Lua с параметрами
    /// </summary>
    public class FuturesLimits
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Торговый счет
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

        /// <summary>
        /// Тип лимита. Возможные значения:
        /// «0» – «Денежные средства»,
        /// «1» – «Залоговые денежные средства»,
        /// «2» – «Всего»,
        /// «3» – «Клиринговые рубли»,
        /// «4» – «Клиринговые залоговые рубли»,
        /// «5» – «Лимит открытых позиций на спот-рынке»
        /// </summary>
        [JsonProperty("limit_type")]
        public int LimitType { get; set; }

        /// <summary>
        /// Коэффициент ликвидности
        /// </summary>
        [JsonProperty("liquidity_coef")]
        public double LiquidityCoef { get; set; }

        /// <summary>
        /// Предыдущий лимит открытых позиций на спот-рынке
        /// </summary>
        [JsonProperty("cbp_prev_limit")]
        public double CbpPrevLimit { get; set; }

        /// <summary>
        /// Лимит открытых позиций
        /// </summary>
        [JsonProperty("cbplimit")]
        public double CbpLimit { get; set; }

        /// <summary>
        /// Текущие чистые позиции
        /// </summary>
        [JsonProperty("cbplused")]
        public double CbpLUsed { get; set; }

        /// <summary>
        /// Плановые чистые позиции
        /// </summary>
        [JsonProperty("cbplplanned")]
        public double CbpLPlanned { get; set; }

        /// <summary>
        /// Вариационная маржа
        /// </summary>
        [JsonProperty("varmargin")]
        public double VarMargin { get; set; }

        /// <summary>
        /// Накопленный купонный доход
        /// </summary>
        [JsonProperty("accruedint")]
        public double AccruedInt { get; set; }

        /// <summary>
        /// Текущие чистые позиции (под заявки)
        /// </summary>
        [JsonProperty("cbplused_for_orders")]
        public double CbpLUsedForOrders { get; set; }

        /// <summary>
        /// Текущие чистые позиции (под открытые позиции)
        /// </summary>
        [JsonProperty("cbplused_for_positions")]
        public double CbpLUsedForPositions { get; set; }

        /// <summary>
        /// Премия по опционам
        /// </summary>
        [JsonProperty("options_premium")]
        public double OptionsPremium { get; set; }

        /// <summary>
        /// Биржевые сборы
        /// </summary>
        [JsonProperty("ts_comission")]
        public double TSComission { get; set; }

        /// <summary>
        /// Коэффициент клиентского гарантийного обеспечения
        /// </summary>
        [JsonProperty("kgo")]
        public double KGO { get; set; }

        /// <summary>
        /// Валюта, в которой транслируется ограничение
        /// </summary>
        [JsonProperty("currcode")]
        public string CurrCode { get; set; }

        /// <summary>
        /// Реально начисленная в ходе клиринга вариационная маржа. Отображается с точностью до 2 двух знаков.
        /// При этом, в поле «varmargin» транслируется вариационная маржа, рассчитанная с учетом установленных границ изменения цены
        /// </summary>
        [JsonProperty("real_varmargin")]
        public double RealVarMargin { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
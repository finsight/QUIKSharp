// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Описание параметров таблицы Инструменты
    /// Результат getSecurityInfo
    /// </summary>
    public class SecurityInfo
    {
        /// <summary>
        /// Код инструмента
        /// Устаревший параметр?
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Код инструмента
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Наименование инструмента
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Краткое наименование
        /// </summary>
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Наименование класса
        /// </summary>
        [JsonProperty("class_name")]
        public string ClassName { get; set; }

        /// <summary>
        /// Номинал
        /// </summary>
        [JsonProperty("face_value")]
        public string FaceValue { get; set; }

        /// <summary>
        /// Код валюты номинала
        /// </summary>
        [JsonProperty("face_unit")]
        public string FaceUnit { get; set; }

        /// <summary>
        /// Количество значащих цифр после запятой
        /// </summary>
        [JsonProperty("scale")]
        public int Scale { get; set; }

        /// <summary>
        /// Дата погашения (в QLUA это число, но на самом деле дата записанная как YYYYMMDD),
        /// поэтому здесь сохраняем просто как строку
        /// </summary>
        [JsonProperty("mat_date")]
        public string MatDate { get; set; }

        /// <summary>
        /// Размер лота
        /// </summary>
        [JsonProperty("lot_size")]
        public int LotSize { get; set; }

        /// <summary>
        /// ISIN-код
        /// </summary>
        [JsonProperty("isin_code")]
        public string IsinCode { get; set; }

        /// <summary>
        /// Минимальный шаг цены
        /// </summary>
        [JsonProperty("min_price_step")]
        public double MinPriceStep { get; set; }

        /// <summary>
        /// Bloomberg ID
        /// </summary>
        [JsonProperty("bsid")]
        public string BsId { get; set; }

        /// <summary>
        /// CUSIP
        /// </summary>
        [JsonProperty("cusip_code")]
        public string CUSIPCode_code { get; set; }

        /// <summary>
        /// StockCode
        /// </summary>
        [JsonProperty("stock_code")]
        public string StockCode { get; set; }

        /// <summary>
        /// Размер купона
        /// </summary>
        [JsonProperty("couponvalue")]
        public double CouponValue { get; set; }

        /// <summary>
        /// Код котируемой валюты в паре
        /// </summary>
        [JsonProperty("first_currcode")]
        public string FirstCurrCode { get; set; }

        /// <summary>
        /// Код базовой валюты в паре
        /// </summary>
        [JsonProperty("second_currcode")]
        public string SecondCurrCode { get; set; }

        /// <summary>
        /// Код класса базового актива
        /// </summary>
        [JsonProperty("base_active_classcode")]
        public string BaseActiveClassCode { get; set; }

        /// <summary>
        /// Базовый актив
        /// </summary>
        [JsonProperty("base_active_seccode")]
        public string BaseActiveSecCode { get; set; }

        /// <summary>
        /// Страйк опциона
        /// </summary>
        [JsonProperty("option_strike")]
        public double OptionStrike { get; set; }

        /// <summary>
        /// Кратность при вводе количества
        /// </summary>
        [JsonProperty("qty_multiplier")]
        public double QtyMultiplier { get; set; }

        /// <summary>
        /// Валюта шага цены
        /// </summary>
        [JsonProperty("step_price_currency")]
        public string StepPriceCurrency { get; set; }

        /// <summary>
        /// SEDOL
        /// </summary>
        [JsonProperty("sedol_code")]
        public string SEDOLCode { get; set; }

        /// <summary>
        /// CFI
        /// </summary>
        [JsonProperty("cfi_code")]
        public string CFICode { get; set; }

        /// <summary>
        /// RIC
        /// </summary>
        [JsonProperty("ric_code")]
        public string RICCode { get; set; }

        /// <summary>
        /// Дата оферты
        /// Представление в виде числа `YYYYMMDD`
        /// </summary>
        [JsonProperty("buybackdate")]
        public int BuybackDate { get; set; }

        /// <summary>
        /// Цена оферты
        /// </summary>
        [JsonProperty("buybackprice")]
        public double BuybackPrice { get; set; }

        /// <summary>
        /// Уровень листинга
        /// </summary>
        [JsonProperty("list_level")]
        public int ListLevel { get; set; }

        /// <summary>
        /// Точность количества
        /// </summary>
        [JsonProperty("qty_scale")]
        public int QtyScale { get; set; }

        /// <summary>
        /// Доходность по предыдущей оценке
        /// </summary>
        [JsonProperty("yieldatprevwaprice")]
        public double YieldAtPrevWaPrice { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        [JsonProperty("regnumber")]
        public string RegNumber { get; set; }

        /// <summary>
        /// Валюта торгов
        /// </summary>
        [JsonProperty("trade_currency")]
        public string TradeCurrency { get; set; }

        /// <summary>
        /// Точность количества котируемой валюты
        /// </summary>
        [JsonProperty("second_curr_qty_scale")]
        public int SecondCurrQtyScale { get; set; }

        /// <summary>
        /// Точность количества базовой валюты
        /// </summary>
        [JsonProperty("first_curr_qty_scale")]
        public int FirstCurrQtyScale { get; set; }

        /// <summary>
        /// Накопленный купонный доход
        /// </summary>
        [JsonProperty("accruedint")]
        public double Accruedint { get; set; }

        /// <summary>
        /// Код деривативного контракта в формате QUIK
        /// </summary>
        [JsonProperty("stock_name")]
        public string StockName { get; set; }

        /// <summary>
        /// Дата выплаты купона
        /// Представление в виде числа `YYYYMMDD`
        /// </summary>
        [JsonProperty("nextcoupon")]
        public int NextCoupon { get; set; }

        /// <summary>
        /// Длительность купона
        /// </summary>
        [JsonProperty("couponperiod")]
        public int CouponPeriod { get; set; }

        /// <summary>
        /// Текущий код расчетов для инструмента
        /// </summary>
        [JsonProperty("settlecode")]
        public string SettleCode { get; set; }

        /// <summary>
        /// Дата экспирации
        /// Представление в виде числа `YYYYMMDD`
        /// </summary>
        [JsonProperty("exp_date")]
        public int ExpDate { get; set; }

        /// <summary>
        /// Дата расчетов
        /// Представление в виде числа `YYYYMMDD`
        /// </summary>
        [JsonProperty("settle_date")]
        public int SettleDate { get; set; }

        ///// <summary>
        ///// Ноги составного инструмента в формате leg_<N>
        ///// понка непонятно как реализовывать
        ///// </summary>
        //[JsonProperty("legs")]
        //public table Legs { get; set; }
    }
}
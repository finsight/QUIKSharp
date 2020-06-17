// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// Наименования параметров для функции GetParamEx и GetParamEx2
    /// </summary>
    public enum ParamNames
    {
        /// <summary>
        /// Код бумаги
        /// </summary>
        CODE,

        /// <summary>
        /// Код класса
        /// </summary>
        CLASS_CODE,

        /// <summary>
        /// Краткое название бумаги
        /// </summary>
        SHORTNAME,

        /// <summary>
        /// Полное зназвание бумаги
        /// </summary>
        LONGNAME,

        /// <summary>
        /// Кратность лота
        /// </summary>
        LOT,

        /// <summary>
        /// Тип инструмента
        /// </summary>
        SECTYPESTATIC,

        /// <summary>
        /// Тип фьючерса
        /// </summary>
        FUTURETYPE,

        /// <summary>
        /// Минимальный шаг цены
        /// </summary>
        SEC_PRICE_STEP,

        /// <summary>
        /// Название класса
        /// </summary>
        CLASSNAME,

        /// <summary>
        /// Размер лота
        /// </summary>
        LOTSIZE,

        /// <summary>
        /// Стоимость шага цены
        /// </summary>
        STEPPRICET,

        /// <summary>
        /// Стоимость шага цены
        /// </summary>
        STEPPRICE,

        /// <summary>
        /// Стоимость шага цены для клиринга
        /// </summary>
        STEPPRICECL,

        /// <summary>
        /// Стоимость шага цены для промклиринга
        /// </summary>
        STEPPRICEPRCL,

        /// <summary>
        /// Точность цены
        /// </summary>
        SEC_SCALE,

        /// <summary>
        /// Цена закрытия
        /// </summary>
        PREVPRICE,

        /// <summary>
        /// Цена первой сделки в текущей сессии
        /// </summary>
        FIRSTOPEN,

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        LAST,

        /// <summary>
        /// Цена последней сделки в текущей сессии
        /// </summary>
        LASTCLOSE,

        /// <summary>
        /// Время последней сделки
        /// </summary>
        TIME,

        /// <summary>
        /// Базовый актив
        /// </summary>
        OPTIONBASE,

        /// <summary>
        /// Класс базового актива
        /// </summary>
        OPTIONBASECLASS,

        /// <summary>
        /// Валюта номинала
        /// </summary>
        SEC_FACE_UNIT,

        /// <summary>
        /// Валюта шага цены
        /// </summary>
        CURSTEPPRICE,

        /// <summary>
        /// Лучшая цена предложения
        /// </summary>
        OFFER,

        /// <summary>
        /// Лучшая цена спроса
        /// </summary>
        BID,

        /// <summary>
        /// Количество заявок на покупку
        /// </summary>
        NUMBIDS,

        /// <summary>
        /// Количество заявок на продажу
        /// </summary>
        NUMOFFERS,

        /// <summary>
        /// Спрос по лучшей цене
        /// </summary>
        BIDDEPTH,

        /// <summary>
        /// Предложение по лучшей цене
        /// </summary>
        OFFERDEPTH,

        /// <summary>
        /// Суммарный спрос
        /// </summary>
        BIDDEPTHT,

        /// <summary>
        /// Суммарное предложение
        /// </summary>
        OFFERDEPTHT,

        /// <summary>
        /// Максимальная цена сделки
        /// </summary>
        HIGH,

        /// <summary>
        /// Минимальная цена сделки
        /// </summary>
        LOW,

        /// <summary>
        /// Максимально возможная цена
        /// </summary>
        PRICEMAX,

        /// <summary>
        /// Минимально возможная цена
        /// </summary>
        PRICEMIN,

        /// <summary>
        /// Количество открытых позиций
        /// </summary>
        NUMCONTRACTS,

        /// <summary>
        /// Гарантийное обеспечение покуптеля
        /// </summary>
        BUYDEPO,

        /// <summary>
        /// Гарантийное обеспечение продавца
        /// </summary>
        SELLDEPO,

        /// <summary>
        /// Номинал бумаги
        /// </summary>
        SEC_FACE_VALUE,

        /// <summary>
        /// Дата исполнения инструмента
        /// </summary>
        EXPDATE,

        /// <summary>
        /// Дата погашения
        /// </summary>
        MAT_DATE,

        /// <summary>
        /// Число дней до погашения
        /// </summary>
        DAYS_TO_MAT_DATE,

        /// <summary>
        /// Начало утренней сессии
        /// </summary>
        MONSTARTTIME,

        /// <summary>
        /// Окончание утренней сессии
        /// </summary>
        MONENDTIME,

        /// <summary>
        /// Начало вечерней сессии
        /// </summary>
        EVNSTARTTIME,

        /// <summary>
        /// Окончание вечерней сессии
        /// </summary>
        EVNENDTIME,

        /// <summary>
        /// Состояние сессии
        /// </summary>
        TRADINGSTATUS,

        /// <summary>
        /// Статус клиринга
        /// </summary>
        CLSTATE,

        /// <summary>
        /// Статус торговли инструментом
        /// </summary>
        STATUS,

        /// <summary>
        /// Дата торгов
        /// </summary>
        TRADE_DATE_CODE,

        /// <summary>
        /// Bloomberg ID
        /// </summary>
        BSID,

        /// <summary>
        /// CFI
        /// </summary>
        CFI_CODE,

        /// <summary>
        /// CUSIP
        /// </summary>
        CUSIP,

        /// <summary>
        /// ISIN
        /// </summary>
        ISINCODE,

        /// <summary>
        /// RIC
        /// </summary>
        RIC,

        /// <summary>
        /// SEDOL
        /// </summary>
        SEDOL,

        /// <summary>
        /// StockCode
        /// </summary>
        STOCKCODE,

        /// <summary>
        /// StockName
        /// </summary>
        STOCKNAME,

        /// <summary>
        /// Агрегированная ставка
        /// </summary>
        PERCENTRATE,

        /// <summary>
        /// Анонимная торговля
        /// </summary>
        ANONTRADE,

        /// <summary>
        /// Биржевой сбор (возможно, исключен из активных параметров)
        /// </summary>
        EXCH_PAY,

        /// <summary>
        /// Время начала аукциона
        /// </summary>
        STARTTIME,

        /// <summary>
        /// Время окончания аукциона
        /// </summary>
        ENDTIME,

        /// <summary>
        /// Время последнего изменения
        /// </summary>
        CHANGETIME,

        /// <summary>
        /// Дисконт1
        /// </summary>
        DISCOUNT1,

        /// <summary>
        /// Дисконт2
        /// </summary>
        DISCOUNT2,

        /// <summary>
        /// Дисконт3
        /// </summary>
        DISCOUNT3,

        /// <summary>
        /// Количество в последней сделке
        /// </summary>
        QTY,

        /// <summary>
        /// Количество во всех сделках
        /// </summary>
        VOLTODAY,

        /// <summary>
        /// Количество сделок за сегодня
        /// </summary>
        NUMTRADES,

        /// <summary>
        /// Комментарий
        /// </summary>
        SEC_COMMENT,

        /// <summary>
        /// Котировка последнего клиринга
        /// </summary>
        CLPRICE,

        /// <summary>
        /// Оборот в деньгах
        /// </summary>
        VALTODAY,

        /// <summary>
        /// Оборот в деньгах последней сделки
        /// </summary>
        VALUE,

        /// <summary>
        /// Пердыдущая оценка
        /// </summary>
        PREVWAPRICE,

        /// <summary>
        /// Подтип инструмента
        /// </summary>
        SECSUBTYPESTATIC,

        /// <summary>
        /// Предыдущая расчетная цена
        /// </summary>
        PREVSETTLEPRICE,

        /// <summary>
        /// Предыдущий расчетный объем
        /// </summary>
        PREVSETTLEVOL,

        /// <summary>
        /// Процент изменения от закрытия
        /// </summary>
        LASTCHANGE,

        /// <summary>
        /// Разница цены последней к предыдущей сделке
        /// </summary>
        TRADECHANGE,

        /// <summary>
        /// Разница цены последней к предыдущей сессии
        /// </summary>
        CHANGE,

        /// <summary>
        /// Расчетная цена
        /// </summary>
        SETTLEPRICE,

        /// <summary>
        /// Реальная расчетная цена
        /// </summary>
        R_SETTLEPRICE,

        /// <summary>
        /// Регистрационный номер (возможно, исключен из активных параметров)
        /// </summary>
        REGNUMBER,

        /// <summary>
        /// Средневзвешенная цена
        /// </summary>
        WAPRICE,

        /// <summary>
        /// Текущая рыночная котировка
        /// </summary>
        REALVMPRICE,

        /// <summary>
        /// Тип
        /// </summary>
        SECTYPE,

        /// <summary>
        /// Тип цены фьючерса
        /// </summary>
        ISPERCENT,

        /// <summary>
        /// Issuer
        /// </summary>
        FIRM_SHORT_NAME,

        /// <summary>
        /// Duration (дюрация)
        /// </summary>
        DURATION,

        /// <summary>
        /// YieldMaturity (Доходность к погашению)
        /// </summary>
        YIELD,

        /// <summary>
        /// Купон (размер/стоимость)
        /// </summary>
        COUPONVALUE,

        /// <summary>
        /// Периодичность выплаты купонов
        /// </summary>
        COUPONPERIOD,

        /// <summary>
        /// Дата ближайшей выплаты купона
        /// </summary>
        NEXTCOUPON
    }
}
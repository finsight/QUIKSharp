// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp
{
    // TODO Redirect these callbacks to events or rather do with events from the beginning

    /// <summary>
    /// Implements all Quik callback functions to be processed on .NET side.
    /// These functions are called by Quik inside QLUA.
    ///
    /// Функции обратного вызова
    /// Функции вызываются при получении следующих данных или событий терминалом QUIK от сервера:
    /// main - реализация основного потока исполнения в скрипте
    /// OnAccountBalance - изменение позиции по счету
    /// OnAccountPosition - изменение позиции по счету
    /// OnAllTrade - новая обезличенная сделка
    /// OnCleanUp - смена торговой сессии и при выгрузке файла qlua.dll
    /// OnClose - закрытие терминала QUIK
    /// OnConnected - установление связи с сервером QUIK
    /// OnDepoLimit - изменение бумажного лимита
    /// OnDepoLimitDelete - удаление бумажного лимита
    /// OnDisconnected - отключение от сервера QUIK
    /// OnFirm - описание новой фирмы
    /// OnFuturesClientHolding - изменение позиции по срочному рынку
    /// OnFuturesLimitChange - изменение ограничений по срочному рынку
    /// OnFuturesLimitDelete - удаление лимита по срочному рынку
    /// OnInit - инициализация функции main
    /// OnMoneyLimit - изменение денежного лимита
    /// OnMoneyLimitDelete - удаление денежного лимита
    /// OnNegDeal - новая заявка на внебиржевую сделку
    /// OnNegTrade - новая сделка для исполнения
    /// OnOrder - новая заявка или изменение параметров существующей заявки
    /// OnParam - изменение текущих параметров
    /// OnQuote - изменение стакана котировок
    /// OnStop - остановка скрипта из диалога управления
    /// OnStopOrder - новая стоп-заявка или изменение параметров существующей стоп-заявки
    /// OnTrade - новая сделка
    /// OnTransReply - ответ на транзакцию
    /// </summary>
    public interface IQuikEvents : IQuikService
    {
        /// <summary>
        /// Событие вызывается когда библиотека QuikSharp успешно подключилась к Quik'у
        /// </summary>
        event InitHandler OnConnectedToQuik;

        /// <summary>
        /// Событие вызывается когда библиотека QuikSharp была отключена от Quik'а
        /// </summary>
        event VoidHandler OnDisconnectedFromQuik;

        /// <summary>
        /// Событие вызывается при получении изменений текущей позиции по счету.
        /// </summary>
        event AccountBalanceHandler OnAccountBalance;

        /// <summary>
        /// Событие вызывается при изменении денежной позиции по счету.
        /// </summary>
        event AccountPositionHandler OnAccountPosition;

        /// <summary>
        /// Новая обезличенная сделка
        /// </summary>
        event AllTradeHandler OnAllTrade;

        /// <summary>
        /// Функция вызывается терминалом QUIK при смене сессии и при выгрузке файла qlua.dll
        /// </summary>
        event VoidHandler OnCleanUp;

        /// <summary>
        /// Функция вызывается перед закрытием терминала QUIK.
        /// </summary>
        event VoidHandler OnClose;

        /// <summary>
        /// Функция вызывается терминалом QUIK при установлении связи с сервером QUIK.
        /// </summary>
        event VoidHandler OnConnected;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений лимита по бумагам.
        /// </summary>
        event DepoLimitHandler OnDepoLimit;

        /// <summary>
        /// Функция вызывается терминалом QUIK при удалении клиентского лимита по бумагам.
        /// </summary>
        event DepoLimitDeleteHandler OnDepoLimitDelete;

        /// <summary>
        /// Функция вызывается терминалом QUIK при отключении от сервера QUIK.
        /// </summary>
        event VoidHandler OnDisconnected;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении описания новой фирмы от сервера.
        /// </summary>
        event FirmHandler OnFirm;

        /// <summary>
        /// Функция вызывается терминалом QUIK при изменении позиции по срочному рынку.
        /// </summary>
        event FuturesClientHoldingHandler OnFuturesClientHolding;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений ограничений по срочному рынку.
        /// </summary>
        event FuturesLimitHandler OnFuturesLimitChange;

        /// <summary>
        /// Функция вызывается терминалом QUIK при удалении лимита по срочному рынку.
        /// </summary>
        event FuturesLimitDeleteHandler OnFuturesLimitDelete;

        /// <summary>
        /// Depricated
        /// </summary>
        event InitHandler OnInit;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений по денежному лимиту клиента.
        /// </summary>
        event MoneyLimitHandler OnMoneyLimit;

        /// <summary>
        /// Функция вызывается терминалом QUIK при удалении денежного лимита.
        /// </summary>
        event MoneyLimitDeleteHandler OnMoneyLimitDelete;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении внебиржевой заявки.
        /// </summary>
        event EventHandler OnNegDeal;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении сделки для исполнения.
        /// </summary>
        event EventHandler OnNegTrade;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении новой заявки или при изменении параметров существующей заявки.
        /// </summary>
        event OrderHandler OnOrder;

        /// <summary>
        /// Функция вызывается терминалом QUIK при при изменении текущих параметров.
        /// </summary>
        event ParamHandler OnParam;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменения стакана котировок.
        /// </summary>
        event QuoteHandler OnQuote;

        /// <summary>
        /// Функция вызывается терминалом QUIK при остановке скрипта из диалога управления.
        /// Примечание: Значение параметра «stop_flag» – «1».После окончания выполнения функции таймаут завершения работы скрипта 5 секунд. По истечении этого интервала функция main() завершается принудительно. При этом возможна потеря системных ресурсов.
        /// </summary>
        event StopHandler OnStop;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении новой стоп-заявки или при изменении параметров существующей стоп-заявки.
        /// </summary>
        event StopOrderHandler OnStopOrder;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении сделки.
        /// </summary>
        event TradeHandler OnTrade;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении ответа на транзакцию пользователя.
        /// </summary>
        event TransReplyHandler OnTransReply;
    }
}
// Copyright (C) 2015 Victor Baybekov

using System;

namespace QuikSharp {

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
    public interface IQuikEvents : IQuikService {

		/// <summary>
		/// Событие вызывается когда библиотека QuikSharp успешно подключилась к Quik'у
		/// </summary>
		event InitHandler OnConnectedToQuik;

		/// <summary>
		/// Событие вызывается когда библиотека QuikSharp была отключена от Quik'а
		/// </summary>
		event VoidHandler OnDisconnectedFromQuik;

		event EventHandler OnAccountBalance;
        event EventHandler OnAccountPosition;
        /// <summary>
        /// Новая обезличенная сделка
        /// </summary>
        event AllTradeHandler OnAllTrade;
        event VoidHandler OnCleanUp;
        /// <summary>
        /// Функция вызывается перед закрытием терминала QUIK. 
        /// </summary>
        event VoidHandler OnClose;
        event VoidHandler OnConnected;
        event EventHandler OnDepoLimit;
        event EventHandler OnDepoLimitDelete;
        event VoidHandler OnDisconnected;
        event EventHandler OnFirm;
        event EventHandler OnFuturesClientHolding;
        event EventHandler OnFuturesLimitChange;
        event EventHandler OnFuturesLimitDelete;

		/// <summary>
		/// Depricated
		/// </summary>
        event InitHandler OnInit;

        event EventHandler OnMoneyLimit;
        event EventHandler OnMoneyLimitDelete;
        event EventHandler OnNegDeal;
        event EventHandler OnNegTrade;
        event OrderHandler OnOrder;
        event EventHandler OnParam;
        event QuoteHandler OnQuote;
        /// <summary>
        /// Функция вызывается терминалом QUIK при остановке скрипта из диалога управления. 
        /// Примечание: Значение параметра «stop_flag» – «1».После окончания выполнения функции таймаут завершения работы скрипта 5 секунд. По истечении этого интервала функция main() завершается принудительно. При этом возможна потеря системных ресурсов.
        /// </summary>
        event StopHandler OnStop;
        event TradeHandler OnTrade;
        event TransReplyHandler OnTransReply;

    }
}
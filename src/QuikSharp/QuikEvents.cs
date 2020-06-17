// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Diagnostics;

namespace QuikSharp
{
    /// <summary>
    /// A handler for events without arguments
    /// </summary>
    public delegate void VoidHandler();

    /// <summary>
    /// Обработчик события OnInit
    /// </summary>
    /// <param name="port">Порт обмена данными</param>
    public delegate void InitHandler(int port);

    /// <summary>
    ///
    /// </summary>
    /// <param name="orderbook"></param>
    public delegate void QuoteHandler(OrderBook orderbook);

    /// <summary>
    /// Обработчик события OnStop
    /// </summary>
    public delegate void StopHandler(int signal);

    /// <summary>
    /// Обработчик события OnAllTrade
    /// </summary>
    /// <param name="allTrade"></param>
    public delegate void AllTradeHandler(AllTrade allTrade);

    /// <summary>
    ///
    /// </summary>
    /// <param name="transReply"></param>
    public delegate void TransReplyHandler(TransactionReply transReply);

    /// <summary>
    /// Обработчик события OnOrder
    /// </summary>
    /// <param name="order"></param>
    public delegate void OrderHandler(Order order);

    /// <summary>
    /// Обработчик события OnTrade
    /// </summary>
    /// <param name="trade"></param>
    public delegate void TradeHandler(Trade trade);

    /// <summary>
    /// Обработчик события OnParam
    /// </summary>
    /// <param name="par">lua table with class_code, sec_code</param>
    public delegate void ParamHandler(Param par);

    /// <summary>
    /// Обработчик события OnStopOrder
    /// </summary>
    /// <param name="stopOrder"></param>
    public delegate void StopOrderHandler(StopOrder stopOrder);

    /// <summary>
    /// Обработчик события OnAccountBalance
    /// </summary>
    /// <param name="accBal"></param>
    public delegate void AccountBalanceHandler(AccountBalance accBal);

    /// <summary>
    /// Обработчик события OnAccountPosition
    /// </summary>
    /// <param name="accPos"></param>
    public delegate void AccountPositionHandler(AccountPosition accPos);

    /// <summary>
    /// Обработчик события OnDepoLimit
    /// </summary>
    /// <param name="dLimit"></param>
    public delegate void DepoLimitHandler(DepoLimitEx dLimit);

    /// <summary>
    /// Обработчик события OnDepoLimitDelete
    /// </summary>
    /// <param name="dLimitDel"></param>
    public delegate void DepoLimitDeleteHandler(DepoLimitDelete dLimitDel);

    /// <summary>
    /// Обработчик события OnFirm
    /// </summary>
    /// <param name="frm"></param>
    public delegate void FirmHandler(Firm frm);

    /// <summary>
    /// Обработчик события OnFuturesClientHolding
    /// </summary>
    /// <param name="futPos"></param>
    public delegate void FuturesClientHoldingHandler(FuturesClientHolding futPos);

    /// <summary>
    /// Обработчик события OnFuturesLimitChange
    /// </summary>
    /// <param name="futLimit"></param>
    public delegate void FuturesLimitHandler(FuturesLimits futLimit);

    /// <summary>
    /// Обработчик события OnFuturesLimitDelete
    /// </summary>
    /// <param name="limDel"></param>
    public delegate void FuturesLimitDeleteHandler(FuturesLimitDelete limDel);

    /// <summary>
    /// Обработчик события OnMoneyLimit
    /// </summary>
    /// <param name="mLimit"></param>
    public delegate void MoneyLimitHandler(MoneyLimitEx mLimit);

    /// <summary>
    /// Обработчик события OnMoneyLimitDelete
    /// </summary>
    /// <param name="mLimitDel"></param>
    public delegate void MoneyLimitDeleteHandler(MoneyLimitDelete mLimitDel);

    internal class QuikEvents : IQuikEvents
    {
        public QuikEvents(QuikService service)
        {
            QuikService = service;
        }

        public QuikService QuikService { get; private set; }

        /// <summary>
        /// Событие вызывается когда библиотека QuikSharp успешно подключилась к Quik'у
        /// </summary>
        public event InitHandler OnConnectedToQuik;

        internal void OnConnectedToQuikCall(int port)
        {
            OnConnectedToQuik?.Invoke(port);
            OnInit?.Invoke(port);
        }

        /// <summary>
        /// Событие вызывается когда библиотека QuikSharp была отключена от Quik'а
        /// </summary>
        public event VoidHandler OnDisconnectedFromQuik;

        internal void OnDisconnectedFromQuikCall()
        {
            OnDisconnectedFromQuik?.Invoke();
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK перед вызовом функции main().
        /// В качестве параметра принимает значение полного пути к запускаемому скрипту.
        /// </summary>
        public event InitHandler OnInit;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений текущей позиции по счету.
        /// </summary>
        public event AccountBalanceHandler OnAccountBalance;

        internal void OnAccountBalanceCall(AccountBalance accBal)
        {
            OnAccountBalance?.Invoke(accBal);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при изменении денежной позиции по счету.
        /// </summary>
        public event AccountPositionHandler OnAccountPosition;

        internal void OnAccountPositionCall(AccountPosition accPos)
        {
            OnAccountPosition?.Invoke(accPos);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении обезличенной сделки.
        /// </summary>
        public event AllTradeHandler OnAllTrade;

        internal void OnAllTradeCall(AllTrade allTrade) => OnAllTrade?.Invoke(allTrade);

        /// <summary>
        /// Функция вызывается терминалом QUIK при смене сессии и при выгрузке файла qlua.dll
        /// </summary>
        public event VoidHandler OnCleanUp;

        internal void OnCleanUpCall()
        {
            OnCleanUp?.Invoke();
        }

        /// <summary>
        /// Функция вызывается перед закрытием терминала QUIK.
        /// </summary>
        public event VoidHandler OnClose;

        internal void OnCloseCall()
        {
            OnClose?.Invoke();
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при установлении связи с сервером QUIK.
        /// </summary>
        public event VoidHandler OnConnected;

        internal void OnConnectedCall()
        {
            OnConnected?.Invoke();
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений лимита по бумагам.
        /// </summary>
        public event DepoLimitHandler OnDepoLimit;

        internal void OnDepoLimitCall(DepoLimitEx dLimit)
        {
            OnDepoLimit?.Invoke(dLimit);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при удалении клиентского лимита по бумагам.
        /// </summary>
        public event DepoLimitDeleteHandler OnDepoLimitDelete;

        internal void OnDepoLimitDeleteCall(DepoLimitDelete dLimitDel)
        {
            OnDepoLimitDelete?.Invoke(dLimitDel);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при отключении от сервера QUIK.
        /// </summary>
        public event VoidHandler OnDisconnected;

        internal void OnDisconnectedCall()
        {
            OnDisconnected?.Invoke();
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении описания новой фирмы от сервера.
        /// </summary>
        public event FirmHandler OnFirm;

        internal void OnFirmCall(Firm frm)
        {
            OnFirm?.Invoke(frm);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при изменении позиции по срочному рынку.
        /// </summary>
        public event FuturesClientHoldingHandler OnFuturesClientHolding;

        internal void OnFuturesClientHoldingCall(FuturesClientHolding futPos)
        {
            OnFuturesClientHolding?.Invoke(futPos);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений ограничений по срочному рынку.
        /// </summary>
        public event FuturesLimitHandler OnFuturesLimitChange;

        internal void OnFuturesLimitChangeCall(FuturesLimits futLimit)
        {
            OnFuturesLimitChange?.Invoke(futLimit);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при удалении лимита по срочному рынку.
        /// </summary>
        public event FuturesLimitDeleteHandler OnFuturesLimitDelete;

        internal void OnFuturesLimitDeleteCall(FuturesLimitDelete limDel)
        {
            OnFuturesLimitDelete?.Invoke(limDel);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменений по денежному лимиту клиента.
        /// </summary>
        public event MoneyLimitHandler OnMoneyLimit;

        internal void OnMoneyLimitCall(MoneyLimitEx mLimit)
        {
            OnMoneyLimit?.Invoke(mLimit);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при удалении денежного лимита.
        /// </summary>
        public event MoneyLimitDeleteHandler OnMoneyLimitDelete;

        internal void OnMoneyLimitDeleteCall(MoneyLimitDelete mLimitDel)
        {
            OnMoneyLimitDelete?.Invoke(mLimitDel);
        }

        public event EventHandler OnNegDeal;

        public event EventHandler OnNegTrade;

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении заявки или изменении параметров существующей заявки.
        /// </summary>
        public event OrderHandler OnOrder;

        internal void OnOrderCall(Order order)
        {
            OnOrder?.Invoke(order);
            // invoke event specific for the transaction
            string correlationId = order.TransID.ToString();

            #region Totally untested code or handling manual transactions

            if (!QuikService.Storage.Contains(correlationId))
            {
                correlationId = "manual:" + order.OrderNum + ":" + correlationId;
                var fakeTrans = new Transaction()
                {
                    Comment = correlationId,
                    IsManual = true
                    // TODO map order properties back to transaction
                    // ideally, make C# property names consistent (Lua names are set as JSON.NET properties via an attribute)
                };
                QuikService.Storage.Set<Transaction>(correlationId, fakeTrans);
            }

            #endregion Totally untested code or handling manual transactions

            var tr = QuikService.Storage.Get<Transaction>(correlationId);
            if (tr != null)
            {
                lock (tr)
                {
                    tr.OnOrderCall(order);
                }
            }

            Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and all order messages are recieved");
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при при изменении текущих параметров.
        /// </summary>
        public event ParamHandler OnParam;

        internal void OnParamCall(Param par)
        {
            OnParam?.Invoke(par);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении изменения стакана котировок.
        /// </summary>
        public event QuoteHandler OnQuote;

        internal void OnQuoteCall(OrderBook orderBook)
        {
            OnQuote?.Invoke(orderBook);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при остановке скрипта из диалога управления и при закрытии терминала QUIK.
        /// </summary>
        public event StopHandler OnStop;

        internal void OnStopCall(int signal)
        {
            OnStop?.Invoke(signal);
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении новой стоп-заявки или при изменении параметров существующей стоп-заявки.
        /// </summary>
        public event StopOrderHandler OnStopOrder;

        internal void OnStopOrderCall(StopOrder stopOrder)
        {
            //if (OnStopOrder != null) OnStopOrder(stopOrder);
            OnStopOrder?.Invoke(stopOrder);
            // invoke event specific for the transaction
            string correlationId = stopOrder.TransId.ToString();

            #region Totally untested code or handling manual transactions

            if (!QuikService.Storage.Contains(correlationId))
            {
                correlationId = "manual:" + stopOrder.OrderNum + ":" + correlationId;
                var fakeTrans = new Transaction()
                {
                    Comment = correlationId,
                    IsManual = true
                    // TODO map order properties back to transaction
                    // ideally, make C# property names consistent (Lua names are set as JSON.NET properties via an attribute)
                };
                QuikService.Storage.Set<Transaction>(correlationId, fakeTrans);
            }

            #endregion Totally untested code or handling manual transactions

            var tr = QuikService.Storage.Get<Transaction>(correlationId);
            if (tr != null)
            {
                lock (tr)
                {
                    tr.OnStopOrderCall(stopOrder);
                }
            }

            Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and all order messages are recieved");
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении сделки.
        /// </summary>
        public event TradeHandler OnTrade;

        internal void OnTradeCall(Trade trade)
        {
            OnTrade?.Invoke(trade);
            // invoke event specific for the transaction
            string correlationId = trade.Comment;

            // ignore unknown transactions
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                return;
            }

            #region Totally untested code or handling manual transactions

            if (!QuikService.Storage.Contains(correlationId))
            {
                correlationId = "manual:" + trade.OrderNum + ":" + correlationId;
                var fakeTrans = new Transaction()
                {
                    Comment = correlationId,
                    IsManual = true
                    // TODO map order properties back to transaction
                    // ideally, make C# property names consistent (Lua names are set as JSON.NET properties via an attribute)
                };
                QuikService.Storage.Set<Transaction>(correlationId, fakeTrans);
            }

            #endregion Totally untested code or handling manual transactions

            var tr = QuikService.Storage.Get<Transaction>(correlationId);
            if (tr != null)
            {
                lock (tr)
                {
                    tr.OnTradeCall(trade);
                    // persist transaction with added trade
                    QuikService.Storage.Set(correlationId, tr);
                }
            }

            // ignore unknown transactions
            //Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and all trades messages are recieved");
        }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении ответа на транзакцию пользователя.
        /// </summary>
        public event TransReplyHandler OnTransReply;

        internal void OnTransReplyCall(TransactionReply reply)
        {
            OnTransReply?.Invoke(reply);

            // invoke event specific for the transaction
            if (string.IsNullOrEmpty(reply.Comment)) //"Initialization user successful" transaction doesn't contain comment
                return;

            if (QuikService.Storage.Contains(reply.Comment))
            {
                var tr = QuikService.Storage.Get<Transaction>(reply.Comment);
                lock (tr)
                {
                    tr.OnTransReplyCall(reply);
                }
            }
            else
            {
                // NB ignore unmatched transactions
                //Trace.Fail("Transaction must exist in persistent storage until it is completed and its reply is recieved");
            }
        }
    }
}
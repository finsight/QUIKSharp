// Copyright (C) 2014 Victor Baybekov

using System;
using System.Diagnostics;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;

namespace QuikSharp {

    /// <summary>
    /// A handler for events without arguments
    /// </summary>
    public delegate void VoidHandler();

    /// <summary>
    /// Обработчик события OnInit
    /// </summary>
    /// <param name="path">Расположение скрипта QuikSharp.lua</param>
    /// <param name="port">Порт обмена данными</param>
    public delegate void InitHandler(string path, int port);

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
    /// 
    /// </summary>
    /// <param name="order"></param>
    public delegate void OrderHandler(Order order);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="trade"></param>
    public delegate void TradeHandler(Trade trade   );

    internal class QuikEvents : IQuikEvents {
        public QuikEvents(QuikService service) { QuikService = service; }
        public QuikService QuikService { get; private set; }


        public event EventHandler OnAccountBalance;
        public event EventHandler OnAccountPosition;


        public event AllTradeHandler OnAllTrade;
        internal void OnAllTradeCall(AllTrade allTrade) {
            if (OnAllTrade != null) OnAllTrade(allTrade);
        }


        public event VoidHandler OnCleanUp;
        internal void OnCleanUpCall() { if (OnCleanUp != null) OnCleanUp(); }


        public event VoidHandler OnClose;
        internal void OnCloseCall() { if (OnClose != null) OnClose(); }


        public event VoidHandler OnConnected;
        internal void OnConnectedCall() { if (OnConnected != null) OnConnected(); }


        public event EventHandler OnDepoLimit;
        public event EventHandler OnDepoLimitDelete;


        public event VoidHandler OnDisconnected;
        internal void OnDisconnectedCall() {
            if (OnDisconnected != null) OnDisconnected();
        }


        public event EventHandler OnFirm;
        public event EventHandler OnFuturesClientHolding;
        public event EventHandler OnFuturesLimitChange;
        public event EventHandler OnFuturesLimitDelete;


        public event InitHandler OnInit;
        internal void OnInitCall(string path, int port) {
            if (OnInit != null) OnInit(path, port);
        }


        public event EventHandler OnMoneyLimit;
        public event EventHandler OnMoneyLimitDelete;
        public event EventHandler OnNegDeal;
        public event EventHandler OnNegTrade;


        public event OrderHandler OnOrder;

        internal void OnOrderCall(Order order) {
            if (OnOrder != null) OnOrder(order);
            // invoke event specific for the transaction
            string correlationId = order.Comment;

            #region Totally untested code or handling manual transactions
            if (!QuikService.Storage.Contains(correlationId)) {
                Debug.Assert(order.TransID == 0);
                correlationId = "manual:" + order.OrderNum + ":" + correlationId;
                var fakeTrans = new Transaction() {
                    Comment = correlationId,
                    IsManual = true
                    // TODO map order properties back to transaction
                    // ideally, make C# property names consistent (Lua names are set as JSON.NET properties via an attribute)
                };
                QuikService.Storage.Set<Transaction>(correlationId, fakeTrans);
            }
            #endregion

            var tr = QuikService.Storage.Get<Transaction>(correlationId);
            if (tr != null) {
                tr.OnOrderCall(order);
                // persist transaction with added order
                QuikService.Storage.Set(order.Comment, tr);
            }
            Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and all order messages are recieved");
        }


        public event EventHandler OnParam;


        public event QuoteHandler OnQuote;
        internal void OnQuoteCall(OrderBook orderBook) { if (OnQuote != null) OnQuote(orderBook); }

        public event StopHandler OnStop;
        internal void OnStopCall(int signal) { if (OnStop != null) OnStop(signal); }


        public event EventHandler OnStopOrder;


        public event TradeHandler OnTrade;

        internal void OnTradeCall(Trade trade) {
            if (OnTrade != null) OnTrade(trade);
            // invoke event specific for the transaction
            string correlationId = trade.Comment;

            #region Totally untested code or handling manual transactions
            if (!QuikService.Storage.Contains(correlationId)) {
                correlationId = "manual:" + trade.OrderNum + ":" + correlationId;
                var fakeTrans = new Transaction() {
                    Comment = correlationId,
                    IsManual = true
                    // TODO map order properties back to transaction
                    // ideally, make C# property names consistent (Lua names are set as JSON.NET properties via an attribute)
                };
                QuikService.Storage.Set<Transaction>(correlationId, fakeTrans);
            }
            #endregion

            var tr = QuikService.Storage.Get<Transaction>(trade.Comment);
            if (tr != null) {
                tr.OnTradeCall(trade);
                // persist transaction with added trade
                QuikService.Storage.Set(trade.Comment, tr);
            }
            Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and all trades messages are recieved");
        }


        public event TransReplyHandler OnTransReply;
        internal void OnTransReplyCall(TransactionReply reply) {
            if (OnTransReply != null) OnTransReply(reply);
            // invoke event specific for the transaction
            var tr = QuikService.Storage.Get<Transaction>(reply.Comment);
            if (tr != null) {
                tr.OnTransReplyCall(reply);
                // persist transaction with added reply
                QuikService.Storage.Set(reply.Comment, tr);
            }
            Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and its reply is recieved");
        }

    }

}

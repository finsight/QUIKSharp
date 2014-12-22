// Copyright (C) 2014 Victor Baybekov
using System;
using QuikSharp.DataStructures;

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


    internal class QuikEvents : IQuikEvents {
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
        public event EventHandler OnOrder;
        public event EventHandler OnParam;

        public event QuoteHandler OnQuote;
        internal void OnQuoteCall(OrderBook orderBook) { if (OnQuote != null) OnQuote(orderBook); }

        public event StopHandler OnStop;
        internal void OnStopCall(int signal) { if (OnStop != null) OnStop(signal); }

        public event EventHandler OnStopOrder;
        public event EventHandler OnTrade;
        public event EventHandler OnTransReply;
    }

}

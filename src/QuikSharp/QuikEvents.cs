// Copyright (C) 2014 Victor Baybekov
using System;

namespace QuikSharp {

    public delegate void OnCloseHandler();
    public delegate void OnInitHandler(string path, int port);
    public delegate void OnStopHandler(int signal);

    internal class QuikEvents : IQuikEvents {
        public event EventHandler Stop;
        public event EventHandler OnAccountBalance;
        public event EventHandler OnAccountPosition;
        public event EventHandler OnAllTrade;
        public event EventHandler OnCleanUp;
        /// <summary>
        /// Функция вызывается перед закрытием терминала QUIK. 
        /// </summary>
        public event OnCloseHandler OnClose;
        internal void OnCloseCall() { OnClose(); }

        public event EventHandler OnConnected;
        public event EventHandler OnDepoLimit;
        public event EventHandler OnDepoLimitDelete;
        public event EventHandler OnDisconnected;
        public event EventHandler OnFirm;
        public event EventHandler OnFuturesClientHolding;
        public event EventHandler OnFuturesLimitChange;
        public event EventHandler OnFuturesLimitDelete;
        
        public event OnInitHandler OnInit;
        internal void OnInitCall(string path, int port) { OnInit(path, port); }

        public event EventHandler OnMoneyLimit;
        public event EventHandler OnMoneyLimitDelete;
        public event EventHandler OnNegDeal;
        public event EventHandler OnNegTrade;
        public event EventHandler OnOrder;
        public event EventHandler OnParam;
        public event EventHandler OnQuote;
        public event OnStopHandler OnStop;
        internal void OnStopCall(int signal) { OnStop(signal); }
        public event EventHandler OnStopOrder;
        public event EventHandler OnTrade;
        public event EventHandler OnTransReply;
    }
}

// Copyright (C) 2014 Victor Baybekov
using System;

namespace QuikSharp {

    /// <summary>
    /// Handler for events without arguments
    /// </summary>
    public delegate void VoidHandler();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="port"></param>
    public delegate void OnInitHandler(string path, int port);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="signal"></param>
    public delegate void OnStopHandler(int signal);

    internal class QuikEvents : IQuikEvents {
        public event EventHandler OnAccountBalance;
        public event EventHandler OnAccountPosition;
        public event EventHandler OnAllTrade;
        /// <summary>
        /// Функция вызывается терминалом QUIK при смене сессии и при выгрузке файла qlua.dll. 
        /// </summary>
        public event VoidHandler OnCleanUp;
        internal void OnCleanUpCall() { if (OnCleanUp != null) OnCleanUp(); }

        /// <summary>
        /// Функция вызывается перед закрытием терминала QUIK. 
        /// </summary>
        public event VoidHandler OnClose;
        internal void OnCloseCall() { if (OnClose != null) OnClose(); }

        /// <summary>
        /// Функция вызывается терминалом QUIK при установлении связи с сервером QUIK. 
        /// </summary>
        public event VoidHandler OnConnected;
        internal void OnConnectedCall() { if (OnConnected != null) OnConnected(); }
        public event EventHandler OnDepoLimit;
        public event EventHandler OnDepoLimitDelete;
        /// <summary>
        /// Функция вызывается терминалом QUIK при отключении от сервера QUIK. 
        /// </summary>
        public event VoidHandler OnDisconnected;
        internal void OnDisconnectedCall() { if (OnDisconnected != null) OnDisconnected(); }
        public event EventHandler OnFirm;
        public event EventHandler OnFuturesClientHolding;
        public event EventHandler OnFuturesLimitChange;
        public event EventHandler OnFuturesLimitDelete;
        
        public event OnInitHandler OnInit;
        internal void OnInitCall(string path, int port) { if (OnInit != null) OnInit(path, port); }

        public event EventHandler OnMoneyLimit;
        public event EventHandler OnMoneyLimitDelete;
        public event EventHandler OnNegDeal;
        public event EventHandler OnNegTrade;
        public event EventHandler OnOrder;
        public event EventHandler OnParam;
        public event EventHandler OnQuote;
        public event OnStopHandler OnStop;
        internal void OnStopCall(int signal) { if (OnStop != null) OnStop(signal); }
        public event EventHandler OnStopOrder;
        public event EventHandler OnTrade;
        public event EventHandler OnTransReply;
    }
}

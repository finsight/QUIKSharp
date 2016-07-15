// Copyright (C) 2015 Victor Baybekov

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


    internal class QuikEvents : IQuikEvents
    {
        public QuikEvents(QuikService service) { QuikService = service; }
        public QuikService QuikService { get; private set; }

		/// <summary>
		/// Событие вызывается когда библиотека QuikSharp успешно подключилась к Quik'у
		/// </summary>
		public event Action OnConnectedToQuik;
		internal void OnConnectedToQuikCall ()
		{
			if (OnConnectedToQuik != null)
				OnConnectedToQuik ();
		}

		/// <summary>
		/// Событие вызывается когда библиотека QuikSharp была отключена от Quik'а
		/// </summary>
		public event Action OnDisconnectedFromQuik;
		internal void OnDisconnectedFromQuikCall ()
		{
			if (OnDisconnectedFromQuik != null)
				OnDisconnectedFromQuik ();
		}

		public event InitHandler OnInit
		{
			add
			{
				throw new NotImplementedException ("Подписка на данный метод бессымслена. В скрипте lua будет вызван данный callback когда еще нет подключения к QuikSharp библиотеке. Используйте callback OnConnectedToQuikCall если необходимо.");
			}
			remove
			{

			}
		}

		public event EventHandler OnAccountBalance;
		public event EventHandler OnAccountPosition;

		/// <summary>
		/// Функция вызывается терминалом QUIK при получении обезличенной сделки.
		/// </summary>
		public event AllTradeHandler OnAllTrade;
        internal void OnAllTradeCall(AllTrade allTrade) {
            if (OnAllTrade != null) OnAllTrade(allTrade);
        }


        public event VoidHandler OnCleanUp;
        internal void OnCleanUpCall() { if (OnCleanUp != null) OnCleanUp(); }

		/// <summary>
		/// Функция вызывается перед закрытием терминала QUIK.
		/// </summary>
		public event VoidHandler OnClose;
        internal void OnCloseCall() { if (OnClose != null) OnClose(); }


        public event VoidHandler OnConnected;
        internal void OnConnectedCall() { if (OnConnected != null) OnConnected(); }


        public event EventHandler OnDepoLimit;
        public event EventHandler OnDepoLimitDelete;


        public event VoidHandler OnDisconnected;
        internal void OnDisconnectedCall() { if (OnDisconnected != null) OnDisconnected(); }


		public event EventHandler OnFirm;
		public event EventHandler OnFuturesClientHolding;
		public event EventHandler OnFuturesLimitChange;
		public event EventHandler OnFuturesLimitDelete;
		public event EventHandler OnMoneyLimit;
		public event EventHandler OnMoneyLimitDelete;
		public event EventHandler OnNegDeal;
		public event EventHandler OnNegTrade;


		/// <summary>
		/// Функция вызывается терминалом QUIK при получении сделки.
		/// </summary>
		public event OrderHandler OnOrder;
        internal void OnOrderCall(Order order) {
            if (OnOrder != null) OnOrder(order);
            // invoke event specific for the transaction
            string correlationId = order.TransID.ToString();

            #region Totally untested code or handling manual transactions
            if (!QuikService.Storage.Contains(correlationId)) {
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
                lock (tr) {
                    tr.OnOrderCall(order);
                }
            }
            Trace.Assert(tr != null, "Transaction must exist in persistent storage until it is completed and all order messages are recieved");
        }


        public event ParamHandler OnParam;
        internal void OnParamCall(Param par){
        if (OnParam != null) OnParam(par);
        }

		/// <summary>
		/// Функция вызывается терминалом QUIK при получении изменения стакана котировок.
		/// </summary>
		public event QuoteHandler OnQuote;
        internal void OnQuoteCall(OrderBook orderBook) { if (OnQuote != null) OnQuote(orderBook); }

		/// <summary>
		/// Функция вызывается терминалом QUIK при остановке скрипта из диалога управления и при закрытии терминала QUIK.
		/// </summary>
		public event StopHandler OnStop;
        internal void OnStopCall(int signal) { if (OnStop != null) OnStop(signal); }

        /// <summary>
        /// Функция вызывается терминалом QUIK при получении новой стоп-заявки или при изменении параметров существующей стоп-заявки.
        /// </summary>
        public event StopOrderHandler OnStopOrder;
        internal void OnStopOrderCall(StopOrder stopOrder)
        {
            //if (OnStopOrder != null) OnStopOrder(stopOrder);
            if (OnStopOrder != null) OnStopOrder(stopOrder);
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
            #endregion

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
        internal void OnTradeCall(Trade trade) {
            if (OnTrade != null) OnTrade(trade);
            // invoke event specific for the transaction
            string correlationId = trade.Comment;

            // ignore unknown transactions
            if (string.IsNullOrWhiteSpace(correlationId)) {
                return;
            }

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
                lock (tr) {
                    tr.OnTradeCall(trade);
                    // persist transaction with added trade
                    QuikService.Storage.Set(trade.Comment, tr);
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
            if (OnTransReply != null) OnTransReply(reply);

            // invoke event specific for the transaction
            if (string.IsNullOrEmpty(reply.Comment))//"Initialization user successful" transaction doesn't contain comment
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

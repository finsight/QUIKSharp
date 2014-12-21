using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikSharp.DataStructures {
    public enum EventNames {
        OnAccountBalance,
        OnAccountPosition,
        OnAllTrade,
        OnCleanUp,
        OnClose,
        OnConnected,
        OnDepoLimit,
        OnDepoLimitDelete,
        OnDisconnected,
        OnFirm,
        OnFuturesClientHolding,
        OnFuturesLimitChange,
        OnFuturesLimitDelete,
        OnInit,
        OnMoneyLimit,
        OnMoneyLimitDelete,
        OnNegDeal,
        OnNegTrade,
        OnOrder,
        OnParam,
        OnQuote,
        OnStop,
        OnStopOrder,
        OnTrade,
        OnTransReply
    }
}

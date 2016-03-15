// Copyright (C) 2015 Victor Baybekov

using System;

namespace QuikSharp.DataStructures {
    [Flags]
    public enum AllTradeFlags {
        Sell = 0x1,
        Buy = 0x2
    }
}
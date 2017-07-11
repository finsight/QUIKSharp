// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp.DataStructures
{
    [Flags]
    public enum AllTradeFlags
    {
        Sell = 0x1,
        Buy = 0x2
    }
}
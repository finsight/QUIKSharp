// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
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
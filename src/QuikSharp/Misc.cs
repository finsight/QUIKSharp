// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Net.Sockets;

namespace QuikSharp
{
    public static class NetworkUtils
    {
        /// <summary>
        /// The connection state of a socket is reflected in the Connected property,
        /// but this property is only updated with the last send or receive action.
        /// To determine the connection state before send or receive the one and only way
        /// is polling the state directly from the socket itself. The following
        /// extension class does this.
        /// </summary>
        public static bool IsConnectedNow(this Socket s)
        {
            var part1 = s.Poll(1000, SelectMode.SelectRead);
            var part2 = (s.Available == 0);
            if ((part1 && part2) || !s.Connected) return false;
            return true;
        }
    }
}
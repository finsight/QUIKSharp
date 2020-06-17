// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp
{
    /// <summary>
    /// An exception caught on Lua side with a message from Lua
    /// </summary>
    public class LuaException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public LuaException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    ///
    /// </summary>
    public class TransactionException : LuaException
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public TransactionException(string message) : base(message)
        {
        }
    }
}
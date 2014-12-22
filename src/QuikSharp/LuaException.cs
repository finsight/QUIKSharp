// Copyright (C) 2014 Victor Baybekov

using System;

namespace QuikSharp {
    /// <summary>
    /// An exception caught on Lua side with a message from Lua
    /// </summary>
    public class LuaException : Exception {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public LuaException(string message)
            : base(message) {

        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TransactionException : LuaException {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public TransactionException(string message) : base(message) { }
    }
}
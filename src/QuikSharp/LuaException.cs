using System;

namespace QuikSharp {
    /// <summary>
    /// An exception caught on Lua side with a message from Lua
    /// </summary>
    public class LuaException : Exception {
        public LuaException(string message) : base(message) {
            
        }
    }
}
using System;

namespace QuikSharp.Quik {
    /// <summary>
    /// Marker interface for message data POCOs
    /// </summary>
    interface IMessageData {

    }

    class Message<T> where T : IMessageData {
        /// <summary>
        /// Unique id to match requests and responses
        /// </summary>
        public long CorrelationId { get; set; }
        /// <summary>
        /// A name of a function to call for requests 
        /// </summary>
        public string Command { get; set; }
        /// <summary>
        /// Some messages are valid only for a short time, e.g. buy/sell orders
        /// </summary>
        public DateTime ValidUntilUtc { get; set; }

        /// <summary>
        /// A POCO with function arguments for requests and return values for responses (Lua tables map greate to POCOs)
        /// </summary>
        public T Data { get; set; }
    }
}
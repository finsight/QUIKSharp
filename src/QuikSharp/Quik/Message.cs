using System;
using Newtonsoft.Json;

namespace QuikSharp.Quik {
    /// <summary>
    /// Marker interface for message data POCOs
    /// </summary>
    interface IMessageData {
    }

    class Message<T> where T : IMessageData {
        /// <summary>
        /// Unique correlation id to match requests and responses
        /// </summary>
        [JsonProperty(PropertyName = "i")]
        public long? Id { get; set; }
        /// <summary>
        /// A name of a function to call for requests 
        /// </summary>
        [JsonProperty(PropertyName = "c")]
        public string Command { get; set; }
        /// <summary>
        /// Timestamp in milliseconds, same as in Lua `socket.gettime() * 1000`
        /// </summary>
        [JsonProperty(PropertyName = "t")]
        public long CreatedTime { get; set; }
        /// <summary>
        /// Some messages are valid only for a short time, e.g. buy/sell orders
        /// </summary>
        [JsonProperty(PropertyName = "v")]
        public DateTime? ValidUntilUtc { get; set; }
        /// <summary>
        /// A POCO with function arguments for requests and return values for responses (Lua tables map greate to POCOs)
        /// </summary>
        [JsonProperty(PropertyName = "d")]
        public T Data { get; set; }
    }
}
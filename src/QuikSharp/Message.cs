using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuikSharp {
    internal interface IMessage {
        /// <summary>
        /// Unique correlation id to match requests and responses
        /// </summary>
        long? Id { get; set; }
        /// <summary>
        /// A name of a function to call for requests 
        /// </summary>
        string Command { get; set; }
        /// <summary>
        /// Timestamp in milliseconds, same as in Lua `socket.gettime() * 1000`
        /// </summary>
        long CreatedTime { get; set; }
        /// <summary>
        /// Some messages are valid only for a short time, e.g. buy/sell orders
        /// </summary>
        DateTime? ValidUntil { get; set; }
    }

    internal abstract class BaseMessage : IMessage {
        protected static readonly long Epoch = (new DateTime(1970, 1, 1, 3, 0, 0, 0)).Ticks/10000L;
        protected BaseMessage(string command = null, DateTime? validUntil = null) {
            Id = Interlocked.Increment(ref QuikService.CorrelationId);
            Command = command ?? this.GetType().Name;
            CreatedTime = DateTime.Now.Ticks / 10000L - Epoch;
            ValidUntil = validUntil;
        }
        /// <summary>
        /// Unique correlation id to match requests and responses
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public long? Id { get; set; }
        /// <summary>
        /// A name of a function to call for requests 
        /// </summary>
        [JsonProperty(PropertyName = "cmd")]
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
        public DateTime? ValidUntil { get; set; }
    }

    /// <summary>
    /// Default implementation for a simple message with one string value
    /// </summary>
    internal class StringMessage : BaseMessage {
        public StringMessage(string message, string command, DateTime? validUntil = null) {
            Id = Interlocked.Increment(ref QuikService.CorrelationId);
            Command = command ?? GetType().Name;
            CreatedTime = DateTime.Now.Ticks / 10000L - Epoch;
            ValidUntil = validUntil;
            Data = message;
        }
        /// <summary>
        /// String message
        /// </summary>
        [JsonProperty(PropertyName = "d")]
        public string Data { get; set; }

    }

    static class MessageExtensions {
        public static async Task<TResponse> Send<TResponse>(this IMessage request, int timeout = 0)
            where TResponse : class, IMessage
            {
            var tcs = new TaskCompletionSource<IMessage>();
            if (timeout > 0) {
                var ct = new CancellationTokenSource(timeout);
                ct.Token.Register(() => tcs.TrySetCanceled(), false);
            }
            var kvp = new KeyValuePair<TaskCompletionSource<IMessage>, Type>(tcs, typeof(TResponse));
            if (request.Id == null) { request.Id = Interlocked.Increment(ref QuikService.CorrelationId); }
            QuikService.Responses[request.Id.Value] = kvp;
            // add to queue after responses dictionary
            QuikService.EnvelopeQueue.Add(request);
            var response = await tcs.Task;
            return (response as TResponse);
        }
    }

}
using System;
using System.Diagnostics;
using System.ServiceModel.Security;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuikSharp.Quik {
    /// <summary>
    /// Marker interface for message data POCOs
    /// </summary>
    interface IMessage {
    }

    class Envelope {
        private static readonly long Epoch = (new DateTime(1970, 1, 1, 3, 0, 0, 0)).Ticks/10000L;
        public Envelope(string message, string command = null, DateTime? validUntil = null) {
            Id = Interlocked.Increment(ref QuikService.CorrelationId);
            Command = command ?? message.GetType().Name;
            CreatedTime = DateTime.Now.Ticks / 10000L - Epoch;
            ValidUntil = validUntil;
            Data = message;
        }
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
        public DateTime? ValidUntil { get; set; }
        /// <summary>
        /// A POCO with function arguments for requests and return values for responses (Lua tables map greate to POCOs)
        /// </summary>
        [JsonProperty(PropertyName = "d")]
        public string Data { get; set; }

    }

    static class EnvelopeExtensions {
        // <TResponse> <IMessage>
        public static async Task<Envelope> Send(this Envelope request)
            // TODO implement optional timeout via cts.Register...
            {
            QuikService.EnvelopeQueue.Add(request);
            var tcs = new TaskCompletionSource<Envelope>(); //<IMessage>
            Debug.Assert(request.Id != null, "request.Id != null");
            QuikService.Responses[request.Id.Value] = tcs;
            var response = await tcs.Task;
            return (response as Envelope); // <TResponse>
        }
    }

}
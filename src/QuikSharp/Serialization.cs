// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Extensions for JSON.NET
    /// </summary>
    public static class JsonExtensions
    {
        private static JsonSerializer _serializer;

        [ThreadStatic]
        private static StringBuilder _stringBuilder;

        static JsonExtensions()
        {
            _serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.None,
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        /// <summary>
        ///
        /// </summary>
        public static T FromJson<T>(this string json)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                // reader will get buffer from array pool
                reader.ArrayPool = JsonArrayPool.Instance;
                var value = _serializer.Deserialize<T>(reader);
                return value;
            }
        }

        internal static IMessage FromJson(this string json, QuikService service)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                // reader will get buffer from array pool
                reader.ArrayPool = JsonArrayPool.Instance;
                var value = service.Serializer.Deserialize<IMessage>(reader);
                return value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static object FromJson(this string json, Type type)
        {
            using (var reader = new JsonTextReader(new StringReader(json)))
            {
                // reader will get buffer from array pool
                reader.ArrayPool = JsonArrayPool.Instance;
                var value = _serializer.Deserialize(reader, type);
                return value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public static string ToJson<T>(this T obj)
        {
            if (_stringBuilder == null)
            {
                _stringBuilder = new StringBuilder();
            }

            using (var writer = new JsonTextWriter(new StringWriter(_stringBuilder)))
            {
                try
                {
                    // reader will get buffer from array pool
                    writer.ArrayPool = JsonArrayPool.Instance;
                    _serializer.Serialize(writer, obj);
                    return _stringBuilder.ToString();
                }
                finally
                {
                    _stringBuilder.Clear();
                }
            }
        }

        /// <summary>
        /// Returns indented JSON
        /// </summary>
        public static string ToJsonFormatted<T>(this T obj)
        {
            var message = JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.None, // Objects
                    Formatting = Formatting.Indented,
                    // NB this is important for correctness and performance
                    // Transaction could have many null properties
                    NullValueHandling = NullValueHandling.Ignore
                });
            return message;
        }
    }

    /// <summary>
    /// Limits enum serialization only to defined values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeEnumConverter<T> : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var isDef = Enum.IsDefined(typeof(T), value);
            if (!isDef)
            {
                value = null;
            }

            base.WriteJson(writer, value, serializer);
        }
    }

    /// <summary>
    /// Serialize as string with ToString()
    /// </summary>
    public class ToStringConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var t = JToken.Load(reader);
            T target = t.Value<T>();
            return target;
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var t = JToken.FromObject(value.ToString());
            t.WriteTo(writer);
        }
    }

    /// <summary>
    /// Serialize Decimal to string without trailing zeros
    /// </summary>
    public class DecimalG29ToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(decimal));
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var t = JToken.Load(reader);
            return t.Value<decimal>();
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            decimal d = (decimal) value;
            var t = JToken.FromObject(d.ToString("G29"));
            t.WriteTo(writer);
        }
    }

    /// <summary>
    /// Convert DateTime to HHMMSS
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class HHMMSSDateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            var t = JToken.Load(reader);
            var target = t.Value<string>();
            if (target == null) return null;
            var hh = int.Parse(target.Substring(0, 2));
            var mm = int.Parse(target.Substring(2, 2));
            var ss = int.Parse(target.Substring(4, 2));
            var now = DateTime.Now;
            var dt = new DateTime(now.Year, now.Month, now.Day, hh, mm, ss);
            return dt;
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            var t = JToken.FromObject(((DateTime) value).ToString("HHmmss"));
            t.WriteTo(writer);
        }
    }

    internal class MessageConverter : JsonCreationConverter<IMessage>
    {
        private QuikService _service;

        public MessageConverter(QuikService service)
        {
            _service = service;
        }

        // we learn object type from correlation id and a type stored in responses dictionary
        // ReSharper disable once RedundantAssignment
        protected override IMessage Create(Type objectType, JObject jObject)
        {
            if (FieldExists("lua_error", jObject))
            {
                var id = jObject.GetValue("id").Value<long>();
                var cmd = jObject.GetValue("cmd").Value<string>();
                var message = jObject.GetValue("lua_error").Value<string>();
                LuaException exn;
                switch (cmd)
                {
                    case "lua_transaction_error":
                        exn = new TransactionException(message);
                        break;

                    default:
                        exn = new LuaException(message);
                        break;
                }

                KeyValuePair<TaskCompletionSource<IMessage>, Type> kvp;
                _service.Responses.TryRemove(id, out kvp);
                var tcs = kvp.Key;
                tcs.SetException(exn);
                // terminate listener task that was processing this task
                throw exn;
            }
            else if (FieldExists("id", jObject))
            {
                var id = jObject.GetValue("id").Value<long>();
                objectType = _service.Responses[id].Value;
                return (IMessage) Activator.CreateInstance(objectType);
            }
            else if (FieldExists("cmd", jObject))
            {
                // without id we have an event
                EventNames eventName;
                string cmd = jObject.GetValue("cmd").Value<string>();
                var parsed = Enum.TryParse(cmd, true, out eventName);
                if (parsed)
                {
                    switch (eventName)
                    {
                        case EventNames.OnAccountBalance:
                            return new Message<AccountBalance> {Data = new AccountBalance()};

                        case EventNames.OnAccountPosition:
                            return new Message<AccountPosition> {Data = new AccountPosition()};

                        case EventNames.OnAllTrade:
                            return new Message<AllTrade> {Data = new AllTrade()};

                        case EventNames.OnCleanUp:
                        case EventNames.OnClose:
                        case EventNames.OnConnected:
                        case EventNames.OnDisconnected:
                        case EventNames.OnInit:

                        case EventNames.OnStop:
                            return new Message<string>();

                        case EventNames.OnDepoLimit:
                            return new Message<DepoLimitEx> {Data = new DepoLimitEx()};

                        case EventNames.OnDepoLimitDelete:
                            return new Message<DepoLimitDelete> {Data = new DepoLimitDelete()};

                        case EventNames.OnFirm:
                            return new Message<Firm> {Data = new Firm()};

                        case EventNames.OnFuturesClientHolding:
                            return new Message<FuturesClientHolding> {Data = new FuturesClientHolding()};

                        case EventNames.OnFuturesLimitChange:
                            return new Message<FuturesLimits> {Data = new FuturesLimits()};

                        case EventNames.OnFuturesLimitDelete:
                            return new Message<FuturesLimitDelete> {Data = new FuturesLimitDelete()};

                        case EventNames.OnMoneyLimit:
                            return new Message<MoneyLimitEx> {Data = new MoneyLimitEx()};

                        case EventNames.OnMoneyLimitDelete:
                            return new Message<MoneyLimitDelete> {Data = new MoneyLimitDelete()};

                        case EventNames.OnNegDeal:
                            break;

                        case EventNames.OnNegTrade:
                            break;

                        case EventNames.OnOrder:
                            return new Message<Order> {Data = new Order()};

                        case EventNames.OnParam:
                            return new Message<Param> {Data = new Param()};

                        case EventNames.OnQuote:
                            return new Message<OrderBook> {Data = new OrderBook()};

                        case EventNames.OnStopOrder:
                            return new Message<StopOrder> {Data = new StopOrder()};

                        case EventNames.OnTrade:
                            return new Message<Trade> {Data = new Trade()};

                        case EventNames.OnTransReply:
                            return new Message<TransactionReply> {Data = new TransactionReply()};

                        case EventNames.NewCandle:
                            return new Message<Candle> {Data = new Candle()};

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else
                {
                    // if we have a custom event (e.g. add some processing  of standard Quik event) then we must process it here
                    switch (cmd)
                    {
                        case "lua_error":
                            return new Message<string>();

                        default:
                            //return (IMessage)Activator.CreateInstance(typeof(Message<string>));
                            throw new InvalidOperationException("Unknown command in a message: " + cmd);
                    }
                }
            }

            throw new ArgumentException("Bad message format: no cmd or lua_error fields");
        }

        private static bool FieldExists(string fieldName, JObject jObject)
        {
            return jObject[fieldName] != null;
        }
    }

    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    internal static class ZipExtentions
    {
        /// <summary>
        /// In-memory compress
        /// </summary>
        internal static byte[] GZip(this byte[] bytes)
        {
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        inStream.CopyTo(compress);
                    }

                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// Uses UTF8 bytes to zip
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static byte[] ToZipBytes(this string value)
        {
            using (var inStream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        inStream.CopyTo(compress);
                    }

                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// In-memory uncompress
        /// </summary>
        internal static byte[] UnGZip(this byte[] bytes)
        {
            byte[] outBytes;
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var deCompress = new GZipStream(inStream, CompressionMode.Decompress))
                    {
                        deCompress.CopyTo(outStream);
                    }

                    outBytes = outStream.ToArray();
                }
            }

            return outBytes;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        internal static string FromZipBytes(this byte[] bytes)
        {
            byte[] outBytes;
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var deCompress = new GZipStream(inStream, CompressionMode.Decompress))
                    {
                        deCompress.CopyTo(outStream);
                    }

                    outBytes = outStream.ToArray();
                }
            }

            return Encoding.UTF8.GetString(outBytes);
        }
    }

    public class JsonArrayPool : IArrayPool<char>
    {
        public static readonly JsonArrayPool Instance = new JsonArrayPool();

        public char[] Rent(int minimumLength)
        {
            // get char array from System.Buffers shared pool
            return ArrayPool<char>.Shared.Rent(minimumLength);
        }

        public void Return(char[] array)
        {
            // return char array to System.Buffers shared pool
            ArrayPool<char>.Shared.Return(array);
        }
    }
}
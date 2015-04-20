// Copyright (C) 2015 Victor Baybekov

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Isam.Esent.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;

namespace QuikSharp {
    /// <summary>
    /// Extensions for JSON.NET
    /// </summary>
    public static class JsonExtensions {
        /// <summary>
        /// 
        /// </summary>
        public static T FromJson<T>(this string json) {
            var obj = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None
            });
            return obj;
        }

        internal static IMessage FromJson(this string json, QuikService service) {
            var obj = JsonConvert.DeserializeObject<IMessage>(json, new MessageConverter(service));
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        public static object FromJson(this string json, Type type) {
            var obj = JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None,
                // NB this is important for correctness and performance
                // Transaction could have many null properties
                NullValueHandling = NullValueHandling.Ignore
            });
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ToJson<T>(this T obj) {

            var message = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.None, // Objects
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    // NB this is important for correctness and performance
                    // Transaction could have many null properties
                    NullValueHandling = NullValueHandling.Ignore
                });
            return message;
        }


        /// <summary>
        /// Returns indented JSON
        /// </summary>
        public static string ToJsonFormatted<T>(this T obj) {

            var message = JsonConvert.SerializeObject(obj, Formatting.Indented,
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.None, // Objects
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                    // NB this is important for correctness and performance
                    // Transaction could have many null properties
                    NullValueHandling = NullValueHandling.Ignore
                });
            return message;
        }

        internal static PersistentBlob ToBlob<T>(this T value) {
            var bytes = value.ToJson().ToZipBytes();
            var blob = new PersistentBlob(bytes);
            return blob;
        }

        internal static T FromBlob<T>(this PersistentBlob value) {
            var bytes = value.GetBytes();
            var json = bytes.FromZipBytes();
            return json.FromJson<T>();
        }
    }

    /// <summary>
    /// Limits enum serialization only to defined values
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeEnumConverter<T> : StringEnumConverter {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) {
            var isDef = Enum.IsDefined(typeof(T), value);
            if (!isDef) {
                value = null;
            }
            base.WriteJson(writer, value, serializer);
        }
    }


    /// <summary>
    /// Serialize as string with ToString()
    /// </summary>
    public class ToStringConverter<T> : JsonConverter {

        public override bool CanConvert(Type objectType) { return true; }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer) {
            var t = JToken.Load(reader);
            T target = t.Value<T>();
            return target;
        }

        public override void WriteJson(JsonWriter writer,
                                       object value,
                                       JsonSerializer serializer) {
            var t = JToken.FromObject(value.ToString());
            t.WriteTo(writer);
        }
    }

    /// <summary>
    /// Convert DateTime to HHMMSS
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class HHMMSSDateTimeConverter : JsonConverter {
        public override bool CanConvert(Type objectType) { return true; }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer) {
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
                                       JsonSerializer serializer) {
            var t = JToken.FromObject(((DateTime)value).ToString("hhmmss"));
            t.WriteTo(writer);
        }
    }

    internal class MessageConverter : JsonCreationConverter<IMessage> {
        private QuikService _service;
        public MessageConverter(QuikService service) { _service = service; }
        // we learn object type from correlation id and a type stored in responses dictionary
        // ReSharper disable once RedundantAssignment
        protected override IMessage Create(Type objectType, JObject jObject) {
            if (FieldExists("lua_error", jObject)) {
                var id = jObject.GetValue("id").Value<long>();
                var cmd = jObject.GetValue("cmd").Value<string>();
                var message = jObject.GetValue("lua_error").Value<string>();
                LuaException exn; 
                switch (cmd) { case "lua_transaction_error":
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
            } else if (FieldExists("id", jObject)) {
                var id = jObject.GetValue("id").Value<long>();
                objectType = _service.Responses[id].Value;
                return (IMessage)Activator.CreateInstance(objectType);
            } else if (FieldExists("cmd", jObject)) {
                // without id we have an event
                EventNames eventName;
                string cmd = jObject.GetValue("cmd").Value<string>();
                var parsed = Enum.TryParse(cmd, true, out eventName);
                if (parsed) {
                    switch (eventName) {
                        case EventNames.OnAccountBalance:
                            break;
                        case EventNames.OnAccountPosition:
                            break;

                        case EventNames.OnAllTrade:
                            return new Message<AllTrade> { Data = new AllTrade() };

                        case EventNames.OnCleanUp:
                        case EventNames.OnClose:
                        case EventNames.OnConnected:
                        case EventNames.OnDisconnected:
                        case EventNames.OnInit:
                        case EventNames.OnStop:
                            return new Message<string>();

                        case EventNames.OnDepoLimit:
                            break;
                        case EventNames.OnDepoLimitDelete:
                            break;

                        case EventNames.OnFirm:
                            break;
                        case EventNames.OnFuturesClientHolding:
                            break;
                        case EventNames.OnFuturesLimitChange:
                            break;
                        case EventNames.OnFuturesLimitDelete:
                            break;
                        case EventNames.OnMoneyLimit:
                            break;
                        case EventNames.OnMoneyLimitDelete:
                            break;
                        case EventNames.OnNegDeal:
                            break;
                        case EventNames.OnNegTrade:
                            break;

                        case EventNames.OnOrder:
                            return new Message<Order> {
                                Data = new Order()
                            };

                        case EventNames.OnParam:
                            break;

                        case EventNames.OnQuote:
                            return new Message<OrderBook> {
                                Data = new OrderBook()
                            };

                        case EventNames.OnStopOrder:
                            break;

                        case EventNames.OnTrade:
                            return new Message<Trade> {
                                Data = new Trade()
                            };
                        case EventNames.OnTransReply:
                            return new Message<TransactionReply> {
                                Data = new TransactionReply()
                            };
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                } else {
                    // if we have a custom event (e.g. add some processing  of standard Quik event) then we must process it here
                    switch (cmd) {
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

        private static bool FieldExists(string fieldName, JObject jObject) {
            return jObject[fieldName] != null;
        }
    }


    internal abstract class JsonCreationConverter<T> : JsonConverter {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType) {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
                                        Type objectType,
                                         object existingValue,
                                         JsonSerializer serializer) {
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
                                       JsonSerializer serializer) {
            throw new InvalidOperationException();
        }
    }


    /// <summary>
    /// 
    /// </summary>
    internal static class ZipExtentions {


        /// <summary>
        /// In-memory compress
        /// </summary>
        internal static byte[] GZip(this byte[] bytes) {
            using (var inStream = new MemoryStream(bytes)) {
                using (var outStream = new MemoryStream()) {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress)) {
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
        internal static byte[] ToZipBytes(this string value) {
            using (var inStream = new MemoryStream(Encoding.UTF8.GetBytes(value))) {
                using (var outStream = new MemoryStream()) {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress)) {
                        inStream.CopyTo(compress);
                    }
                    return outStream.ToArray();
                }
            }
        }

        /// <summary>
        /// In-memory uncompress
        /// </summary>
        internal static byte[] UnGZip(this byte[] bytes) {
            byte[] outBytes;
            using (var inStream = new MemoryStream(bytes)) {
                using (var outStream = new MemoryStream()) {
                    using (var deCompress = new GZipStream(inStream, CompressionMode.Decompress)) {
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
        internal static string FromZipBytes(this byte[] bytes) {
            byte[] outBytes;
            using (var inStream = new MemoryStream(bytes)) {
                using (var outStream = new MemoryStream()) {
                    using (var deCompress = new GZipStream(inStream, CompressionMode.Decompress)) {
                        deCompress.CopyTo(outStream);
                    }
                    outBytes = outStream.ToArray();
                }
            }
            return Encoding.UTF8.GetString(outBytes);
        }


    }

}
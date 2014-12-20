using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QuikSharp {
    internal static class JsonExtensions {
        public static T FromJson<T>(this string json) {
            var obj = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None
            });
            return obj;
        }

        public static IMessage FromJson(this string json, QuikService service) {
            var obj = JsonConvert.DeserializeObject<IMessage>(json, new MessageConverter(service));
            return obj;
        }

        public static object FromJson(this string json, Type type) {
            var obj = JsonConvert.DeserializeObject(json, type, new JsonSerializerSettings {
                TypeNameHandling = TypeNameHandling.None
            });
            return obj;
        }

        public static string ToJson<T>(this T obj) {
            
            var message = JsonConvert.SerializeObject(obj, Formatting.None,
                new JsonSerializerSettings {
                    TypeNameHandling = TypeNameHandling.None, // Objects
                    TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                });
            return message;
        }
    }

    public class LuaException : Exception {
        public LuaException(string message) : base(message) {
            
        }
    }

    internal class MessageConverter : JsonCreationConverter<IMessage> {
        private QuikService _service;
        public MessageConverter(QuikService service) { _service = service; }
        // we learn object type from correlation id and a type stored in responses dictionary
        protected override IMessage Create(Type objectType, JObject jObject) {
            if (FieldExists("lua_error", jObject)) {
                var id = jObject.GetValue("id").Value<long>();
                KeyValuePair<TaskCompletionSource<IMessage>, Type> kvp;
                _service.Responses.TryRemove(id, out kvp);
                var tcs = kvp.Key;
                var exn = new LuaException(jObject.GetValue("lua_error").Value<string>());
                tcs.SetException(new LuaException(jObject.GetValue("lua_error").Value<string>()));
                throw exn;
            }
            else if (FieldExists("id", jObject)) {
                var id = jObject.GetValue("id").Value<long>();
                objectType  = _service.Responses[id].Value;
                return (IMessage)Activator.CreateInstance(objectType);
            } else if (FieldExists("cmd", jObject)) {
                // without id we have a command
                throw new NotImplementedException();
            }
            throw new ApplicationException();
        }

        private static bool FieldExists(string fieldName, JObject jObject) {
            return jObject[fieldName] != null;
        }
    }


    // Thanks to jdavies http://stackoverflow.com/a/8031283/801189

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
            throw new NotImplementedException();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuikSharp.Quik {

    public partial interface IQuikCalls {
        /// <summary>
        /// Send 'Ping' string
        /// </summary>
        /// <returns>'Pong' string</returns>
        Task<string> Ping();

        /// <summary>
        /// Функция возвращает путь, по которому находится файл info.exe, исполняющий данный скрипт, без завершающего обратного слэша («\»). Например, C:\QuikFront. 
        /// </summary>
        /// <returns></returns>
        string GetWorkingFolder();

        /// <summary>
        /// Функция предназначена для определения состояния подключения клиентского места к серверу. Возвращает «1», если клиентское место подключено и «0», если не подключено. 
        /// </summary>
        /// <returns></returns>
        bool IsConnected();

        /// <summary>
        /// Функция возвращает путь, по которому находится запускаемый скрипт, без завершающего обратного слэша («\»). Например, C:\QuikFront\Scripts 
        /// </summary>
        /// <returns></returns>
        string GetScriptPath();

        /// <summary>
        /// Функция возвращает значения параметров информационного окна (пункт меню Связь / Информационное окно…). 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        string GetInfoParam(InfoParams param);

        /// <summary>
        /// Функция отображает сообщения в терминале QUIK. Возвращает «nil» при ошибке выполнения или при обнаружении ошибки во входных параметрах. В остальных случаях возвращает «1». 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="iconType"></param>
        /// <returns></returns>
        bool Message(string message, NotificationType iconType);
    }


    /// <summary>
    /// Service calls implementations
    /// </summary>
    public partial class QuikCalls : IQuikCalls {
        public QuikCalls() {
            QuikService.Start();
        }
        /// <summary>
        /// 
        /// </summary>
        public class PingRequest : IMessage {
            /// <summary>
            /// 
            /// </summary>
            public string Ping { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class PingResponse : IMessage {
            /// <summary>
            /// 
            /// </summary>
            public string Pong { get; set; }
        }

        public async Task<string> Ping() {
            var message = JsonConvert.SerializeObject(new PingRequest { Ping = "Ping" }, Formatting.None, new JsonSerializerSettings {
                                            TypeNameHandling = TypeNameHandling.None, // Objects
                                            TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple
                                        });
            var env = new Envelope(message); //<IMessage>
            var result = await env.Send();
            var responseMsg = JsonConvert.DeserializeObject<PingResponse>(result.Data, new JsonSerializerSettings { //<IMessage>
                                          TypeNameHandling = TypeNameHandling.None // .Objects
                                      });
            return responseMsg.Pong;
        }
        public string GetWorkingFolder() { throw new NotImplementedException(); }
        public bool IsConnected() { throw new NotImplementedException(); }
        public string GetScriptPath() { throw new NotImplementedException(); }
        public string GetInfoParam(InfoParams param) { throw new NotImplementedException(); }
        public bool Message(string message, NotificationType iconType) { throw new NotImplementedException(); }
    }
}

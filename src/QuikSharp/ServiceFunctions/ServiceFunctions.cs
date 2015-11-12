// Copyright (C) 2015 Victor Baybekov

using System;
using System.Threading.Tasks;

namespace QuikSharp {

    /// <summary>
    /// Сервисные функции
    /// </summary>
    public interface IServiceFunctions : IQuikService {

        /// <summary>
        /// Функция возвращает путь, по которому находится файл info.exe, исполняющий данный скрипт, без завершающего обратного слэша («\»). Например, C:\QuikFront. 
        /// </summary>
        /// <returns></returns>
        Task<string> GetWorkingFolder();

        /// <summary>
        /// Функция предназначена для определения состояния подключения клиентского места к серверу. Возвращает «1», если клиентское место подключено и «0», если не подключено. 
        /// </summary>
        /// <returns></returns>
        Task<bool> IsConnected();

        /// <summary>
        /// Функция возвращает путь, по которому находится запускаемый скрипт, без завершающего обратного слэша («\»). Например, C:\QuikFront\Scripts 
        /// </summary>
        /// <returns></returns>
        Task<string> GetScriptPath();

        /// <summary>
        /// Функция возвращает значения параметров информационного окна (пункт меню Связь / Информационное окно…). 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<string> GetInfoParam(InfoParams param);

        /// <summary>
        /// Функция отображает сообщения в терминале QUIK.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="iconType"></param>
        /// <returns></returns>
        Task<bool> Message(string message, NotificationType iconType);

        Task<bool> PrintDbgStr(string message);

        /// <summary>
        /// Устанавливает стартовое значение для CorrelactionId.
        /// </summary>
        /// <param name="startCorrelationId">Стартовое значение.</param>
        void InitializeCorrelationId(int startCorrelationId);
    }

    /// <summary>
    /// Service functions implementations
    /// </summary>
    public class ServiceFunctions : IServiceFunctions {
        public ServiceFunctions(int port) { QuikService = QuikService.Create(port); }

        public QuikService QuikService { get; private set; }

        public async Task<string> GetWorkingFolder() {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getWorkingFolder")));
            return response.Data;
        }

        public async Task<bool> IsConnected() {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "isConnected")));
            return response.Data == "1";
        }

        public async Task<string> GetScriptPath() {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getScriptPath")));
            return response.Data;
        }

        public async Task<string> GetInfoParam(InfoParams param) {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(param.ToString(), "getInfoParam")));
            return response.Data;
        }

        public async Task<bool> Message(string message, NotificationType iconType = NotificationType.Info) {
            switch (iconType) {
                case NotificationType.Info:
                    await QuikService.Send<Message<string>>(
                        (new Message<string>(message, "message")));
                    break;
                case NotificationType.Warning:
                    await QuikService.Send<Message<string>>(
                        (new Message<string>(message, "warning_message")));
                    break;
                case NotificationType.Error:
                    await QuikService.Send<Message<string>>(
                        (new Message<string>(message, "error_message")));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("iconType");
            }
            return true;
        }

        public async Task<bool> PrintDbgStr(string message) {
            await QuikService.Send<Message<string>>(
                (new Message<string>(message, "PrintDbgStr")));
            return true;
        }

        public void InitializeCorrelationId(int startCorrelationId)
        {
            QuikService.InitializeCorrelationId(startCorrelationId);
        }
    }
}

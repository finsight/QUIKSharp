using System;

namespace QuikSharp.ServiceFunctions {

    public interface IServiceFunctions : IQuikFunctions  {

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
    /// Service functions implementations
    /// </summary>
    internal class ServiceFunctions : IServiceFunctions {
        public ServiceFunctions(int port) { QuikService = QuikService.Create(port); }

        public QuikService QuikService { get; private set; }

        public string GetWorkingFolder() { throw new NotImplementedException(); }
        public bool IsConnected() { throw new NotImplementedException(); }
        public string GetScriptPath() { throw new NotImplementedException(); }
        public string GetInfoParam(InfoParams param) { throw new NotImplementedException(); }
        public bool Message(string message, NotificationType iconType) { throw new NotImplementedException(); }
    }
}

// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Сервисные функции
    /// </summary>
    public interface IServiceFunctions : IQuikService
    {
        /// <summary>
        /// Функция возвращает путь, по которому находится файл info.exe, исполняющий данный скрипт, без завершающего обратного слэша («\»). Например, C:\QuikFront.
        /// </summary>
        /// <returns></returns>
        Task<string> GetWorkingFolder();

        /// <summary>
        /// Функция предназначена для определения состояния подключения клиентского места к серверу. Возвращает «1», если клиентское место подключено и «0», если не подключено.
        /// </summary>
        /// <returns></returns>
        Task<bool> IsConnected(int timeout = Timeout.Infinite);

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
        /// Выводит метку на график
        /// </summary>
        /// <param name="price"></param>
        /// <param name="curDate"></param>
        /// <param name="curTime"></param>
        /// <param name="hint"></param>
        /// <param name="path"></param>
        /// <param name="tag"></param>
        /// <param name="alignment">LEFT, RIGHT, TOP, BOTTOM</param>
        /// <param name="backgnd"> On = 1, Off = 0</param>
        /// <returns>Возвращает Id метки</returns>
        Task<double> AddLabel(double price, string curDate, string curTime, string hint, string path, string tag, string alignment, double backgnd);

        /// <summary>
        /// Удаляет метку по ее Id
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="id">Id метки</param>
        /// <returns></returns>
        Task<bool> DelLabel(string tag, double id);

        /// <summary>
        /// Удаляет все метки с графика
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        Task<bool> DelAllLabels(string tag);

        /// <summary>
        /// Устанавливает стартовое значение для CorrelactionId.
        /// </summary>
        /// <param name="startCorrelationId">Стартовое значение.</param>
        void InitializeCorrelationId(int startCorrelationId);
    }

    /// <summary>
    /// Service functions implementations
    /// </summary>
    public class ServiceFunctions : IServiceFunctions
    {
        public ServiceFunctions(int port, string host)
        {
            QuikService = QuikService.Create(port, host);
        }

        public QuikService QuikService { get; private set; }

        public async Task<string> GetWorkingFolder()
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getWorkingFolder"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> IsConnected(int timeout = Timeout.Infinite)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "isConnected")), timeout).ConfigureAwait(false);
            return response.Data == "1";
        }

        public async Task<string> GetScriptPath()
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getScriptPath"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<string> GetInfoParam(InfoParams param)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(param.ToString(), "getInfoParam"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> Message(string message, NotificationType iconType = NotificationType.Info)
        {
            switch (iconType)
            {
                case NotificationType.Info:
                    await QuikService.Send<Message<string>>(
                        (new Message<string>(message, "message"))).ConfigureAwait(false);
                    break;

                case NotificationType.Warning:
                    await QuikService.Send<Message<string>>(
                        (new Message<string>(message, "warning_message"))).ConfigureAwait(false);
                    break;

                case NotificationType.Error:
                    await QuikService.Send<Message<string>>(
                        (new Message<string>(message, "error_message"))).ConfigureAwait(false);
                    break;

                default:
                    throw new ArgumentOutOfRangeException("iconType");
            }

            return true;
        }

        public async Task<bool> PrintDbgStr(string message)
        {
            await QuikService.Send<Message<string>>(
                (new Message<string>(message, "PrintDbgStr"))).ConfigureAwait(false);
            return true;
        }

        public async Task<double> AddLabel(double price, string curDate, string curTime, string hint, string path, string tag, string alignment, double backgnd)
        {
            var response = await QuikService.Send<Message<double>>(
                    (new Message<string>(price + "|" + curDate + "|" + curTime + "|" + hint + "|" + path + "|" + tag + "|" + alignment + "|" + backgnd, "addLabel")))
                .ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> DelLabel(string tag, double id)
        {
            await QuikService.Send<Message<string>>(
                (new Message<string>(tag + "|" + id, "delLabel"))).ConfigureAwait(false);
            return true;
        }

        public async Task<bool> DelAllLabels(string tag)
        {
            await QuikService.Send<Message<string>>(
                (new Message<string>(tag, "delAllLabels"))).ConfigureAwait(false);
            return true;
        }

        public void InitializeCorrelationId(int startCorrelationId)
        {
            QuikService.InitializeCorrelationId(startCorrelationId);
        }
    }
}
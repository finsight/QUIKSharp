// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using QuikSharp.DataStructures;

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
        /// Добавляет метку с заданными параметрами. Хотя бы один из параметров text или imagePath должен быть задан.
        /// </summary>
        /// <param name="chartTag">тег графика, к которому привязывается метка</param>
        /// <param name="text">Подпись метки (если подпись не требуется, то пустая строка)</param>
        /// <param name="imagePath">Путь к картинке, которая будет отображаться в качестве метки (пустая строка, если картинка не требуется).
        /// Используются картинки формата *.bmp, *.jpeg</param>
        /// <param name="alignment">Расположение картинки относительно точки. Текст распологается аналогично относительно картинки.
        /// (возможно 4 варианта: LEFT, RIGHT, TOP, BOTTOM)   -- по умолчанию LEFT</param>
        /// <param name="yValue">Значение параметра на оси Y, к которому будет привязана метка</param>
        /// <param name="strDate">Дата в формате «ГГГГММДД», к которой привязана метка</param>
        /// <param name="strTime">Время в формате «ЧЧММСС», к которому будет привязана метка</param>
        /// <param name="r">Красная компонента цвета в формате RGB. Число в интервале [0;255]  -- по умолчанию 0</param>
        /// <param name="g">Зеленая компонента цвета в формате RGB. Число в интервале [0;255]  -- по умолчанию 0</param>
        /// <param name="b">Синяя компонента цвета в формате RGB. Число в интервале [0;255]    -- по умолчанию 0</param>
        /// <param name="transparency">Прозрачность метки (картинки) в процентах. Значение должно быть в промежутке [0; 100]  -- по умолчанию = 0</param>
        /// <param name="tranBackgrnd">Прозрачность фона картинки. Возможные значения: «0» – прозрачность отключена, «1» – прозрачность включена.
        /// По умолчанию = 0. если == 1, то картинка не рисуется, и у текста исчезает фон. если ==0 то картинка рисуется и у текста есть черный фон.
        /// Если картинка отсутствует и нужен только текст, то делать = 1</param>
        /// <param name="fontName">Название шрифта (например «Arial»)  -- по умолчанию = "Arial"</param>
        /// <param name="fontHeight">Размер шрифта -- -- по умолчанию = 12. </param>
        /// <param name="hint">Текст всплывающей подсказки  </param>
        /// <returns>Возвращает Id метки</returns>
        Task<double> AddLabel(string chartTag, decimal yValue, string strDate, string strTime, string text = "", string imagePath = "", string alignment = "", string hint = "",
            int r = -1, int g = -1, int b = -1, int transparency = -1, int tranBackgrnd = -1, string fontName = "", int fontHeight = -1);

        /// <summary>
        /// Функция задает параметры для метки с указанным идентификатором. 
        /// </summary>
        /// <param name="chartTag"></param>
        /// <param name="labelId"></param>
        /// <param name="yValue"></param>
        /// <param name="strDate"></param>
        /// <param name="strTime"></param>
        /// <param name="text"></param>
        /// <param name="imagePath"></param>
        /// <param name="alignment"></param>
        /// <param name="hint"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="transparency"></param>
        /// <param name="tranBackgrnd"></param>
        /// <param name="fontName"></param>
        /// <param name="fontHeight"></param>
        /// <returns></returns>
        Task<bool> SetLabelParams(string chartTag, int labelId, decimal yValue, string strDate, string strTime, string text = "", string imagePath = "", string alignment = "", string hint = "",
            int r = -1, int g = -1, int b = -1, int transparency = -1, int tranBackgrnd = -1, string fontName = "", int fontHeight = -1);

        /// <summary>
        /// Функция возвращает таблицу с параметрами метки. В случае неуспешного завершения функция возвращает «nil».
        /// </summary>
        /// <param name="chartTag">тег графика, к которому привязывается метка</param>
        /// <param name="labelId">идентификатор метки.</param>
        /// <returns></returns>
        Task<Label> GetLabelParams(string chartTag, int labelId);


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

        public async Task<double> AddLabel(string chartTag, decimal yValue, string strDate, string strTime, string text, string imagePath,
    string alignment, string hint, int r, int g, int b, int transparency, int tranBackgrnd, string fontName, int fontHeight)
        {

            var msg = new Message<string>(chartTag + "|" + yValue + "|" + strDate + "|" + strTime + "|" + text + "|" + imagePath + "|" + alignment + "|" + hint + "|" +
                                          r + "|" + g + "|" + b + "|" + transparency + "|" + tranBackgrnd + "|" + fontName + "|" + fontHeight, "addLabel2");
            var response = await QuikService.Send<Message<double>>(msg).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> SetLabelParams(string chartTag, int labelId, decimal yValue, string strDate, string strTime, string text, string imagePath,
            string alignment, string hint, int r, int g, int b, int transparency, int tranBackgrnd, string fontName, int fontHeight)
        {
            var msg = new Message<string>(chartTag + "|" + labelId + "|" + yValue + "|" + strDate + "|" + strTime + "|" + text + "|" + imagePath + "|" + alignment + "|" + hint + "|" +
                                          r + "|" + g + "|" + b + "|" + transparency + "|" + tranBackgrnd + "|" + fontName + "|" + fontHeight, "setLabelParams");
            var response = await QuikService.Send<Message<bool>>(msg).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<Label> GetLabelParams(string chartTag, int labelId)
        {
            var msg = new Message<string>(chartTag + "|" + labelId, "getLabelParams");
            var response = await QuikService.Send<Message<Label>>(msg).ConfigureAwait(false);
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
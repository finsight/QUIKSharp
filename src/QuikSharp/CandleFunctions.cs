// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Функции для получения свечей
    /// </summary>
    public class CandleFunctions
    {
        public QuikService QuikService { get; private set; }

        public delegate void CandleHandler(Candle candle);

        /// <summary>
        /// Событие получения новой свечи. Для срабатывания необходимо подписаться с помощью метода Subscribe.
        /// </summary>
        public event CandleHandler NewCandle;

        internal void RaiseNewCandleEvent(Candle candle)
        {
            NewCandle?.Invoke(candle);
        }

        public CandleFunctions(int port, string host)
        {
            QuikService = QuikService.Create(port, host);
        }

        /// <summary>
        /// Функция предназначена для получения количества свечей по тегу
        /// </summary>
        /// <param name="graphicTag">Строковый идентификатор графика или индикатора</param>
        /// <returns></returns>
        public async Task<int> GetNumCandles(string graphicTag)
        {
            var message = new Message<string>(graphicTag, "get_num_candles");
            Message<int> response = await QuikService.Send<Message<int>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция предназначена для получения информации о свечках по идентификатору (заказ данных для построения графика плагин не осуществляет, поэтому для успешного доступа нужный график должен быть открыт). Возвращаются все доступные свечки.
        /// </summary>
        /// <param name="graphicTag">Строковый идентификатор графика или индикатора</param>
        /// <returns></returns>
        public async Task<List<Candle>> GetAllCandles(string graphicTag)
        {
            return await GetCandles(graphicTag, 0, 0, 0).ConfigureAwait(false);
        }

        /// <summary>
        /// Функция предназначена для получения информации о свечках по идентификатору (заказ данных для построения графика плагин не осуществляет, поэтому для успешного доступа нужный график должен быть открыт).
        /// </summary>
        /// <param name="graphicTag">Строковый идентификатор графика или индикатора</param>
        /// <param name="line">Номер линии графика или индикатора. Первая линия имеет номер 0</param>
        /// <param name="first">Индекс первой свечки. Первая (самая левая) свечка имеет индекс 0</param>
        /// <param name="count">Количество запрашиваемых свечек</param>
        /// <returns></returns>
        public async Task<List<Candle>> GetCandles(string graphicTag, int line, int first, int count)
        {
            var message = new Message<string>(graphicTag + "|" + line + "|" + first + "|" + count, "get_candles");
            Message<List<Candle>> response = await QuikService.Send<Message<List<Candle>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция возвращает список свечек указанного инструмента заданного интервала и параметра запрошенных данных.
        /// </summary>
        /// <param name="classCode">Класс инструмента.</param>
        /// <param name="securityCode">Код инструмента.</param>
        /// <param name="interval">Интервал свечей.</param>
        /// <param name="param">Параметр запрашиваемых свечей.</param>
        /// <returns>Список свечей.</returns>
        public async Task<List<Candle>> GetAllCandles(string classCode, string securityCode, CandleInterval interval, string param = "-")
        {
            //Параметр count == 0 говорт о том, что возвращаются все доступные свечи
            return await GetLastCandles(classCode, securityCode, interval, 0, param).ConfigureAwait(false);
        }

        /// <summary>
        /// Возвращает заданное количество свечек указанного инструмента и интервала с конца.
        /// </summary>
        /// <param name="classCode">Класс инструмента.</param>
        /// <param name="securityCode">Код инструмента.</param>
        /// <param name="interval">Интервал свечей.</param>
        /// <param name="param">Параметр запрашиваемых свечей.</param>
        /// <param name="count">Количество возвращаемых свечей с конца.</param>
        /// <returns>Список свечей.</returns>
        public async Task<List<Candle>> GetLastCandles(string classCode, string securityCode, CandleInterval interval, int count, string param = "-")
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + (int)interval + "|" + param + "|" + count, "get_candles_from_data_source");
            Message<List<Candle>> response = await QuikService.Send<Message<List<Candle>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Осуществляет подписку на получение исторических данных (свечи)
        /// </summary>
        /// <param name="classCode">Класс инструмента.</param>
        /// <param name="securityCode">Код инструмента.</param>
        /// <param name="interval">интервал свечей (тайм-фрейм).</param>
        /// <param name="param">Параметр запрашиваемых свечей.</param>
        public async Task Subscribe(string classCode, string securityCode, CandleInterval interval, string param = "-")
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + (int)interval + "|" + param, "subscribe_to_candles");
            await QuikService.Send<Message<string>>(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Отписывается от получения исторических данных (свечей)
        /// </summary>
        /// <param name="classCode">Класс инструмента.</param>
        /// <param name="securityCode">Код инструмента.</param>
        /// <param name="interval">интервал свечей (тайм-фрейм).</param>
        /// <param name="param">Параметр запрашиваемых свечей.</param>
        public async Task Unsubscribe(string classCode, string securityCode, CandleInterval interval, string param = "-")
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + (int)interval + "|" + param, "unsubscribe_from_candles");
            await QuikService.Send<Message<string>>(message).ConfigureAwait(false);
        }

        /// <summary>
        /// Проверка состояния подписки на исторические данные (свечи)
        /// </summary>
        /// <param name="classCode">Класс инструмента.</param>
        /// <param name="securityCode">Код инструмента.</param>
        /// <param name="interval">интервал свечей (тайм-фрейм).</param>
        /// <param name="param">Параметр запрашиваемых свечей.</param>
        public async Task<bool> IsSubscribed(string classCode, string securityCode, CandleInterval interval, string param = "-")
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + (int)interval + "|" + param, "is_subscribed");
            Message<bool> response = await QuikService.Send<Message<bool>>(message).ConfigureAwait(false);
            return response.Data;
        }
    }
}
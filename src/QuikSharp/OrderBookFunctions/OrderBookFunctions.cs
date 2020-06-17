// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Threading.Tasks;
using QuikSharp.DataStructures;

namespace QuikSharp
{
    /// <summary>
    /// Функции для работы со стаканом котировок
    /// </summary>
    public interface IOrderBookFunctions : IQuikService
    {
        /// <summary>
        /// Функция заказывает на сервер получение стакана по указанному классу и бумаге.
        /// </summary>
        Task<bool> Subscribe(string class_code, string sec_code);

        /// <summary>
        /// Функция заказывает на сервер получение стакана
        /// </summary>
        Task<bool> Subscribe(ISecurity security);

        /// <summary>
        /// Функция отменяет заказ на получение с сервера стакана по указанному классу и бумаге.
        /// </summary>
        Task<bool> Unsubscribe(string class_code, string sec_code);

        /// <summary>
        /// Функция отменяет заказ на получение с сервера стакана
        /// </summary>
        Task<bool> Unsubscribe(ISecurity security);

        /// <summary>
        /// Функция позволяет узнать, заказан ли с сервера стакан по указанному классу и бумаге.
        /// </summary>
        Task<bool> IsSubscribed(string class_code, string sec_code);

        /// <summary>
        /// Функция позволяет узнать, заказан ли с сервера стакан
        /// </summary>
        Task<bool> IsSubscribed(ISecurity security);

        /// <summary>
        /// Функция предназначена для получения стакана по указанному классу и инструменту
        /// </summary>
        Task<OrderBook> GetQuoteLevel2(string class_code, string sec_code);
    }

    /// <summary>
    /// Функции для работы со стаканом котировок
    /// </summary>
    public class OrderBookFunctions : IOrderBookFunctions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="port"></param>
        /// <param name="host"></param>
        public OrderBookFunctions(int port, string host)
        {
            QuikService = QuikService.Create(port, host);
        }

        /// <summary>
        ///
        /// </summary>
        public QuikService QuikService { get; private set; }

        public async Task<bool> Subscribe(ISecurity security)
        {
            return await Subscribe(security.ClassCode, security.SecCode).ConfigureAwait(false);
        }

        public async Task<bool> Subscribe(string class_code, string sec_code)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(class_code + "|" + sec_code, "Subscribe_Level_II_Quotes"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> Unsubscribe(ISecurity security)
        {
            return await Unsubscribe(security.ClassCode, security.SecCode).ConfigureAwait(false);
        }

        public async Task<bool> Unsubscribe(string class_code, string sec_code)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(class_code + "|" + sec_code, "Unsubscribe_Level_II_Quotes"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> IsSubscribed(ISecurity security)
        {
            return await IsSubscribed(security.ClassCode, security.SecCode).ConfigureAwait(false);
        }

        public async Task<bool> IsSubscribed(string class_code, string sec_code)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(class_code + "|" + sec_code, "IsSubscribed_Level_II_Quotes"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<OrderBook> GetQuoteLevel2(string class_code, string sec_code)
        {
            var response = await QuikService.Send<Message<OrderBook>>(
                (new Message<string>(class_code + "|" + sec_code, "GetQuoteLevel2"))).ConfigureAwait(false);
            return response.Data;
        }
    }
}
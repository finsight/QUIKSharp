// Copyright (C) 2014 Victor Baybekov

using System;
using System.Threading.Tasks;

namespace QuikSharp {

    /// <summary>
    /// Функции для работы со стаканом котировок
    /// </summary>
    public interface IOrderBookFunctions : IQuikFunctions {

        /// <summary>
        /// Функция заказывает на сервер получение стакана по указанному классу и бумаге. 
        /// </summary>
        Task<bool> Subscribe(string class_code, string sec_code);

        /// <summary>
        /// Функция отменяет заказ на получение с сервера стакана по указанному классу и бумаге. 
        /// </summary>
        Task<bool> Unsubscribe(string class_code, string sec_code);

        /// <summary>
        /// Функция позволяет узнать, заказан ли с сервера стакан по указанному классу и бумаге. 
        /// </summary>
        Task<bool> IsSubscribed(string class_code, string sec_code);
    }

    /// <summary>
    /// Функции для работы со стаканом котировок
    /// </summary>
    public class OrderBookFunctions : IOrderBookFunctions {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        public OrderBookFunctions(int port) { QuikService = QuikService.Create(port); }

        /// <summary>
        /// 
        /// </summary>
        public QuikService QuikService { get; private set; }


        public async Task<bool> Subscribe(string class_code, string sec_code) {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(class_code + "|" + sec_code, "Subscribe_Level_II_Quotes")));
            return response.Data;
        }

        public async Task<bool> Unsubscribe(string class_code, string sec_code) {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(class_code + "|" + sec_code, "Unsubscribe_Level_II_Quotes")));
            return response.Data;
        }

        public async Task<bool> IsSubscribed(string class_code, string sec_code) {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(class_code + "|" + sec_code, "IsSubscribed_Level_II_Quotes")));
            return response.Data;
        }
    }
}

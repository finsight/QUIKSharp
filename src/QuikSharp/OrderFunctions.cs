// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace QuikSharp
{
    /// <summary>
    /// Класс, содержащий методы работы с заявками.
    /// </summary>
    public class OrderFunctions
    {
        private QuikService QuikService { get; set; }
        private Quik Quik { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public OrderFunctions(int port, Quik quik, string host)
        {
            QuikService = QuikService.Create(port, host);
            Quik = quik;
        }

        /// <summary>
        /// Создание новой заявки.
        /// </summary>
        /// <param name="order">Инфомация о новой заявки, на основе которой будет сформирована транзакция.</param>
        public async Task<long> CreateOrder(Order order)
        {
            Transaction newOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.NEW_ORDER,
                ACCOUNT = order.Account,
                CLASSCODE = order.ClassCode,
                SECCODE = order.SecCode,
                QUANTITY = order.Quantity,
                OPERATION = order.Operation == Operation.Buy ? TransactionOperation.B : TransactionOperation.S,
                PRICE = order.Price,
                CLIENT_CODE = order.ClientCode
            };
            return await Quik.Trading.SendTransaction(newOrderTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Создание "лимитрированной"заявки.
        /// </summary>
        /// <param name="classCode">Код класса инструмента</param>
        /// <param name="securityCode">Код инструмента</param>
        /// <param name="accountID">Счет клиента</param>
        /// <param name="operation">Операция заявки (покупка/продажа)</param>
        /// <param name="price">Цена заявки</param>
        /// <param name="qty">Количество (в лотах)</param>
        public async Task<Order> SendLimitOrder(string classCode, string securityCode, string accountID, string clientCode, Operation operation, decimal price, int qty)
        {
            return await SendOrder(classCode, securityCode, accountID, clientCode, operation, price, qty, TransactionType.L).ConfigureAwait(false);
        }

        /// <summary>
        /// Создание "рыночной"заявки.
        /// </summary>
        /// <param name="classCode">Код класса инструмента</param>
        /// <param name="securityCode">Код инструмента</param>
        /// <param name="accountID">Счет клиента</param>
        /// <param name="clientCode">Код клиента</param>
        /// <param name="operation">Операция заявки (покупка/продажа)</param>
        /// <param name="qty">Количество (в лотах)</param>
        public async Task<Order> SendMarketOrder(string classCode, string securityCode, string accountID, string clientCode, Operation operation, int qty)
        {
            return await SendOrder(classCode, securityCode, accountID, clientCode, operation, 0, qty, TransactionType.M).ConfigureAwait(false);
        }

        /// <summary>
        /// Создание заявки.
        /// </summary>
        /// <param name="classCode">Код класса инструмента</param>
        /// <param name="securityCode">Код инструмента</param>
        /// <param name="accountID">Счет клиента</param>
        /// <param name="clientCode">Код клиента</param>
        /// <param name="operation">Операция заявки (покупка/продажа)</param>
        /// <param name="price">Цена заявки</param>
        /// <param name="qty">Количество (в лотах)</param>
        /// <param name="orderType">Тип заявки (L - лимитная, M - рыночная)</param>
        async Task<Order> SendOrder(string classCode, string securityCode, string accountID, string clientCode, Operation operation, decimal price, int qty, TransactionType orderType)
        {
            long res = 0;
            Transaction newOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.NEW_ORDER,
                ACCOUNT = accountID,
                CLASSCODE = classCode,
                SECCODE = securityCode,
                QUANTITY = qty,
                OPERATION = operation == Operation.Buy ? TransactionOperation.B : TransactionOperation.S,
                PRICE = price,
                TYPE = orderType,
                CLIENT_CODE = clientCode
            };
            try
            {
                res = await Quik.Trading.SendTransaction(newOrderTransaction).ConfigureAwait(false);
                Thread.Sleep(50);
                Console.WriteLine("res: " + res);
            }
            catch
            {
                //ignore
            }

            if(res < 0)
                return new Order
                {
                    RejectReason = newOrderTransaction.ErrorMessage,
                    ClassCode = classCode,
                    SecCode = securityCode,
                    Account = accountID,
                    ClientCode = clientCode,
                    Operation = operation,
                    Price = price,
                    Quantity = qty
                };

            while (true)
            {
                if (newOrderTransaction.TransactionReply != null && newOrderTransaction.TransactionReply.ErrorSource != 0)
                    return new Order
                    {
                        RejectReason =
                            newOrderTransaction.TransactionReply.ResultMsg
                            ?? $"Transaction {res} error: code {newOrderTransaction.TransactionReply.ErrorCode}, source {newOrderTransaction.TransactionReply.ErrorSource}",
                        ClassCode = classCode,
                        SecCode = securityCode,
                        Account = accountID,
                        ClientCode = clientCode,
                        Operation = operation,
                        Price = price,
                        Quantity = qty
                    };

                try
                {
                    var result = await Quik.Orders.GetOrder_by_transID(classCode, securityCode, res).ConfigureAwait(false);
                    if (result != null)
                        return result;
                }
                catch(Exception e)
                {
                    return new Order
                    {
                        RejectReason = $"Неудачная попытка получения заявки по ID-транзакции №{res}, {e.Message}",
                        ClassCode = classCode,
                        SecCode = securityCode,
                        Account = accountID,
                        ClientCode = clientCode,
                        Operation = operation,
                        Price = price,
                        Quantity = qty
                    };
                }

                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Отмена заявки.
        /// </summary>
        /// <param name="order">Информация по заявке, которую требуется отменить.</param>
        /// <returns>Номер транзакции</returns>
        public async Task<long> KillOrder(Order order)
        {
            Transaction killOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.KILL_ORDER,
                CLASSCODE = order.ClassCode,
                SECCODE = order.SecCode,
                ORDER_KEY = order.OrderNum.ToString()
            };
            return await Quik.Trading.SendTransaction(killOrderTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Отмена заявки с ожиданием завершения транзакции.
        /// </summary>
        /// <param name="order">Информация по заявке, которую требуется отменить.</param>
        /// <returns>Результат выполнения транзакции.</returns>
        public async Task<TransactionReply> KillOrderEx(Order order)
        {
            Transaction killOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.KILL_ORDER,
                CLASSCODE = order.ClassCode,
                SECCODE = order.SecCode,
                ORDER_KEY = order.OrderNum.ToString()
            };

            var res = await Quik.Trading.SendTransaction(killOrderTransaction).ConfigureAwait(false);
            if (res < 0)
                return null;

            while (killOrderTransaction.TransactionReply == null)
                Thread.Sleep(10);

            return killOrderTransaction.TransactionReply;
        }

        /// <summary>
        /// Возвращает заявку из хранилища терминала по её номеру.
        /// На основе: http://help.qlua.org/ch4_5_1_1.htm
        /// </summary>
        /// <param name="classCode">Класс инструмента.</param>
        /// <param name="orderId">Номер заявки.</param>
        /// <returns></returns>
        public async Task<Order> GetOrder(string classCode, long orderId)
        {
            var message = new Message<string>(classCode + "|" + orderId, "get_order_by_number");
            Message<Order> response = await QuikService.Send<Message<Order>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает список всех заявок.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Order>> GetOrders()
        {
            var message = new Message<string>("", "get_orders");
            Message<List<Order>> response = await QuikService.Send<Message<List<Order>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает список заявок для заданного инструмента.
        /// </summary>
        public async Task<List<Order>> GetOrders(string classCode, string securityCode)
        {
            var message = new Message<string>(classCode + "|" + securityCode, "get_orders");
            Message<List<Order>> response = await QuikService.Send<Message<List<Order>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает заявку для заданного инструмента по ID.
        /// </summary>
        public async Task<Order> GetOrder_by_transID(string classCode, string securityCode, long trans_id)
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + trans_id, "getOrder_by_ID");
            Message<Order> response = await QuikService.Send<Message<Order>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает заявку по номеру.
        /// </summary>
        public async Task<Order> GetOrder_by_Number(long order_num)
        {
            var message = new Message<string>(order_num.ToString(), "getOrder_by_Number");
            Message<Order> response = await QuikService.Send<Message<Order>>(message).ConfigureAwait(false);
            return response.Data;
        }
    }
}
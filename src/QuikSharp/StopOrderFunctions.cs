// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Функции для работы со стоп-заявками
    /// </summary>
    public class StopOrderFunctions
    {
        private QuikService QuikService { get; set; }
        private Quik Quik { get; set; }

        public delegate void StopOrderHandler(StopOrder stopOrder);

        public event StopOrderHandler NewStopOrder;

        internal void RaiseNewStopOrderEvent(StopOrder stopOrder)
        {
            NewStopOrder?.Invoke(stopOrder);
        }

        public StopOrderFunctions(int port, Quik quik, string host)
        {
            QuikService = QuikService.Create(port, host);
            Quik = quik;
        }

        /// <summary>
        /// Возвращает список всех стоп-заявок.
        /// </summary>
        /// <returns></returns>
        public async Task<List<StopOrder>> GetStopOrders()
        {
            var message = new Message<string>("", "get_stop_orders");
            Message<List<StopOrder>> response = await QuikService.Send<Message<List<StopOrder>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает список стоп-заявок для заданного инструмента.
        /// </summary>
        public async Task<List<StopOrder>> GetStopOrders(string classCode, string securityCode)
        {
            var message = new Message<string>(classCode + "|" + securityCode, "get_stop_orders");
            Message<List<StopOrder>> response = await QuikService.Send<Message<List<StopOrder>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Регистрирует стоп-заявку.
        /// </summary>
        /// <returns>Номер транзакции.</returns>
        public async Task<long> CreateStopOrder(StopOrder stopOrder)
        {
            Transaction newStopOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.NEW_STOP_ORDER,
                ACCOUNT = stopOrder.Account,
                CLASSCODE = stopOrder.ClassCode,
                SECCODE = stopOrder.SecCode,
                EXPIRY_DATE = "GTC", //до отмены
                STOPPRICE = stopOrder.ConditionPrice,
                PRICE = stopOrder.Price,
                QUANTITY = stopOrder.Quantity,
                STOP_ORDER_KIND = ConvertStopOrderType(stopOrder.StopOrderType),
                OPERATION = stopOrder.Operation == Operation.Buy ? TransactionOperation.B : TransactionOperation.S
            };
            if (stopOrder.StopOrderType == StopOrderType.TakeProfit || stopOrder.StopOrderType == StopOrderType.TakeProfitStopLimit)
            {
                newStopOrderTransaction.OFFSET = stopOrder.Offset;
                newStopOrderTransaction.SPREAD = stopOrder.Spread;
                newStopOrderTransaction.OFFSET_UNITS = stopOrder.OffsetUnit;
                newStopOrderTransaction.SPREAD_UNITS = stopOrder.SpreadUnit;
            }

            if (stopOrder.StopOrderType == StopOrderType.TakeProfitStopLimit)
            {
                newStopOrderTransaction.STOPPRICE2 = stopOrder.ConditionPrice2;
            }

            //todo: Not implemented
            //["OFFSET"]=tostring(SysFunc.toPrice(SecCode,MaxOffset)),
            //["OFFSET_UNITS"]="PRICE_UNITS",
            //["SPREAD"]=tostring(SysFunc.toPrice(SecCode,DefSpread)),
            //["SPREAD_UNITS"]="PRICE_UNITS",
            //["MARKET_STOP_LIMIT"]="YES",
            //["MARKET_TAKE_PROFIT"]="YES",
            //["STOPPRICE2"]=tostring(SysFunc.toPrice(SecCode,StopLoss)),
            //["EXECUTION_CONDITION"] = "FILL_OR_KILL",

            return await Quik.Trading.SendTransaction(newStopOrderTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Возвращает стоп-заявку для заданного инструмента по ID.
        /// </summary>
        public async Task<StopOrder> GetStopOrder_by_transID(string classCode, string securityCode, long trans_id)
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + trans_id, "getStopOrder_by_ID");
            Message<StopOrder> response = await QuikService.Send<Message<StopOrder>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает стоп-заявку по номеру.
        /// </summary>
        public async Task<StopOrder> GetStopOrder_by_Number(long order_num)
        {
            var message = new Message<string>(order_num.ToString(), "getStopOrder_by_Number");
            Message<StopOrder> response = await QuikService.Send<Message<StopOrder>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Регистрирует стоп-заявку и дожидается исполнения транзакции. В случае ошибки выкидывает исключение.
        /// </summary>        
        /// <returns>Зарегистрированный стоп-ордер.</returns>
        public async Task<StopOrder> CreateStopOrderOrThrow(StopOrder stopOrder)
        {
            Transaction newStopOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.NEW_STOP_ORDER,
                ACCOUNT = stopOrder.Account,
                CLIENT_CODE = stopOrder.ClientCode,
                CLASSCODE = stopOrder.ClassCode,
                SECCODE = stopOrder.SecCode,
                EXPIRY_DATE = "GTC", //до отмены
                STOPPRICE = stopOrder.ConditionPrice,
                PRICE = stopOrder.Price,
                QUANTITY = stopOrder.Quantity,
                STOP_ORDER_KIND = ConvertStopOrderType(stopOrder.StopOrderType),
                OPERATION = stopOrder.Operation == Operation.Buy ? TransactionOperation.B : TransactionOperation.S
            };
            if (stopOrder.StopOrderType == StopOrderType.TakeProfit || stopOrder.StopOrderType == StopOrderType.TakeProfitStopLimit)
            {
                newStopOrderTransaction.OFFSET = stopOrder.Offset;
                newStopOrderTransaction.SPREAD = stopOrder.Spread;
                newStopOrderTransaction.OFFSET_UNITS = stopOrder.OffsetUnit;
                newStopOrderTransaction.SPREAD_UNITS = stopOrder.SpreadUnit;
            }

            if (stopOrder.StopOrderType == StopOrderType.TakeProfitStopLimit)
            {
                newStopOrderTransaction.STOPPRICE2 = stopOrder.ConditionPrice2;
            }

            //todo: Not implemented
            //  ...see CreateStopOrder() method

            long res = 0;
            try
            {
                res = await Quik.Trading.SendTransaction(newStopOrderTransaction).ConfigureAwait(false);
                Thread.Sleep(500);
                Console.WriteLine("res: " + res);
            }
            catch
            {
                //ignore
            }

            if (res < 0)
                throw new InvalidOperationException($"StopOrder {stopOrder.SecCode}@{stopOrder.ClassCode} {stopOrder.Operation} {stopOrder.StopOrderType} registering failed. {newStopOrderTransaction.ErrorMessage}");

            while (true)
            {
                if (newStopOrderTransaction.TransactionReply != null && newStopOrderTransaction.TransactionReply.ErrorSource != 0)
                    throw new InvalidOperationException(!string.IsNullOrEmpty(newStopOrderTransaction.TransactionReply.ResultMsg)
                      ? $"StopOrder {stopOrder.SecCode}@{stopOrder.ClassCode} {stopOrder.Operation} {stopOrder.StopOrderType} registering failed. {newStopOrderTransaction.TransactionReply.ResultMsg}"
                      : $"StopOrder {stopOrder.SecCode}@{stopOrder.ClassCode} {stopOrder.Operation} {stopOrder.StopOrderType} registering failed. Transaction {res} error: code {newStopOrderTransaction.TransactionReply.ErrorCode}, source {newStopOrderTransaction.TransactionReply.ErrorSource}");

                try
                {
                    var result = await Quik.StopOrders.GetStopOrder_by_transID(stopOrder.ClassCode, stopOrder.SecCode, res).ConfigureAwait(false);
                    if (result != null)
                        return result;
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"StopOrder {stopOrder.SecCode}@{stopOrder.ClassCode} {stopOrder.Operation} {stopOrder.StopOrderType} registering failed. Неудачная попытка получения заявки по ID-транзакции №{res}, {e.Message}");
                }

                Thread.Sleep(10);
            }
        }

        private StopOrderKind ConvertStopOrderType(StopOrderType stopOrderType)
        {
            switch (stopOrderType)
            {
                case StopOrderType.StopLimit:
                    return StopOrderKind.SIMPLE_STOP_ORDER;

                case StopOrderType.TakeProfit:
                    return StopOrderKind.TAKE_PROFIT_STOP_ORDER;

                case StopOrderType.TakeProfitStopLimit:
                    return StopOrderKind.TAKE_PROFIT_AND_STOP_LIMIT_ORDER;

                default:
                    throw new Exception("Not implemented stop order type: " + stopOrderType);
            }
        }

        /// <summary>
        /// Отмена стоп-заявки.
        /// </summary>
        /// <param name="stopOrder">Информация по стоп-заявке, которую требуется отменить.</param>
        /// <returns>Номер транзакции</returns>
        public async Task<long> KillStopOrder(StopOrder stopOrder)
        {
            Transaction killStopOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.KILL_STOP_ORDER,
                CLASSCODE = stopOrder.ClassCode,
                SECCODE = stopOrder.SecCode,
                STOP_ORDER_KEY = stopOrder.OrderNum.ToString()
            };
            return await Quik.Trading.SendTransaction(killStopOrderTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Отмена стоп-заявки с ожиданием завершения транзакции.
        /// </summary>
        /// <param name="stopOrder">Информация по стоп-заявке, которую требуется отменить.</param>
        /// <returns>Результат выполнения транзакции</returns>
        public async Task<TransactionReply> KillStopOrderEx(StopOrder stopOrder)
        {
            Transaction killStopOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.KILL_STOP_ORDER,
                CLASSCODE = stopOrder.ClassCode,
                SECCODE = stopOrder.SecCode,
                STOP_ORDER_KEY = stopOrder.OrderNum.ToString()
            };

            var res = await Quik.Trading.SendTransaction(killStopOrderTransaction).ConfigureAwait(false);
            if (res < 0)
                return null;

            while (killStopOrderTransaction.TransactionReply == null)
                Thread.Sleep(10);

            return killStopOrderTransaction.TransactionReply;
        }
    }
}

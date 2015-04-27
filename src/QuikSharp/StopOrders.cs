using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Функции для работы со стоп-заявками
    /// </summary>
    public class StopOrders
    {
        private QuikService QuikService { get; set; }
        private Quik Quik { get; set; }

        public StopOrders(int port, Quik quik)
        {
            QuikService = QuikService.Create(port);
            Quik = quik;
        }

        /// <summary>
        /// Возвращает список всех стоп-заявок.
        /// </summary>
        /// <returns></returns>
        public async Task<List<StopOrder>> GetStopOrders()
        {
            var message = new Message<string>("", "GetStopOrders");
            Message<List<StopOrder>> response = await QuikService.Send<Message<List<StopOrder>>>(message);
            return response.Data;			
        }

        /// <summary>
        /// Возвращает список стоп-заявок для заданного инструмента.
        /// </summary>
        public async Task<List<StopOrder>> GetStopOrders(string classCode, string securityCode)
        {
            var message = new Message<string>(classCode + "|" + securityCode, "GetStopOrders");
            Message<List<StopOrder>> response = await QuikService.Send<Message<List<StopOrder>>>(message);
            return response.Data;
        }

        public async void CreateStopOrder(StopOrder stopOrder)
        {
            Transaction newStopOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.NEW_STOP_ORDER,
                ACCOUNT = stopOrder.Account,
                CLASSCODE = stopOrder.ClassCode,
                SECCODE = stopOrder.SecurityCode,
                EXPIRY_DATE = "GTC",//до отмены
                STOPPRICE = stopOrder.ConditionPrice,
                PRICE = stopOrder.Price,
                QUANTITY = stopOrder.Quantity,
                STOP_ORDER_KIND = ConverStopOrderType(stopOrder.StopOrderType),
                OPERATION = stopOrder.Operation == Operation.Buy?TransactionOperation.B : TransactionOperation.S
            };

            //todo: Not implemented
            //["OFFSET"]=tostring(SysFunc.toPrice(SecCode,MaxOffset)),
            //["OFFSET_UNITS"]="PRICE_UNITS",
            //["SPREAD"]=tostring(SysFunc.toPrice(SecCode,DefSpread)),
            //["SPREAD_UNITS"]="PRICE_UNITS",
            //["MARKET_STOP_LIMIT"]="YES",
            //["MARKET_TAKE_PROFIT"]="YES",
            //["STOPPRICE2"]=tostring(SysFunc.toPrice(SecCode,StopLoss)),
            //["EXECUTION_CONDITION"] = "FILL_OR_KILL",
    

            await Quik.Trading.SendTransaction(newStopOrderTransaction);
        }

        private StopOrderKind ConverStopOrderType(StopOrderType stopOrderType)
        {
            switch (stopOrderType)
            {
                case StopOrderType.StopLimit:
                    return StopOrderKind.SIMPLE_STOP_ORDER;
                case StopOrderType.TakeProfit:
                    return StopOrderKind.TAKE_PROFIT_STOP_ORDER;
                default:
                    throw new Exception("Not implemented stop order type: " + stopOrderType);
            }
        }

        public async void KillStopOrder(StopOrder stopOrder)
        {
            Transaction killStopOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.KILL_STOP_ORDER,
                CLASSCODE = stopOrder.ClassCode,
                SECCODE = stopOrder.SecurityCode,
                STOP_ORDER_KEY = stopOrder.OrderNum.ToString()
            };
            await Quik.Trading.SendTransaction(killStopOrderTransaction);
        }
    }
}

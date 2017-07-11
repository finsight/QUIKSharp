using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public OrderFunctions(int port, Quik quik)
        {
            QuikService = QuikService.Create(port);
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
                PRICE = order.Price
            };
            return await Quik.Trading.SendTransaction(newOrderTransaction).ConfigureAwait(false);
        }

        /// <summary>
        /// Отмена заявки.
        /// </summary>
        /// <param name="order">Информация по заявке, которую требуется отменить.</param>
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
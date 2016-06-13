using System.Threading.Tasks;
using QuikSharp.DataStructures.Transaction;
using QuikSharp.DataStructures;

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
            return await Quik.Trading.SendTransaction(newOrderTransaction);
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
            return await Quik.Trading.SendTransaction(killOrderTransaction);
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
            Message<Order> response = await QuikService.Send<Message<Order>>(message);
            return response.Data;
        }
    }
}

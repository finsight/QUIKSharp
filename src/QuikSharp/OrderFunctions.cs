using QuikSharp.DataStructures.Transaction;

namespace QuikSharp
{
    /// <summary>
    /// Класс, содержащий методы работы с заявками.
    /// </summary>
    public class OrderFunctions
    {
        private Quik Quik { get; set; }

        public OrderFunctions(Quik quik)
        {
            Quik = quik;
        }

        /// <summary>
        /// Создание новой заявки.
        /// </summary>
        /// <param name="order">Инфомация о новой заявки, на основе которой будет сформирована транзакция.</param>
        public async void CreateOrder(Order order)
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
            await Quik.Trading.SendTransaction(newOrderTransaction);
        }

        public async void KillOrder(Order order)
        {
            Transaction killOrderTransaction = new Transaction
            {
                ACTION = TransactionAction.KILL_ORDER,
                CLASSCODE = order.ClassCode,
                SECCODE = order.SecCode,
                STOP_ORDER_KEY = order.OrderNum.ToString()
            };
            await Quik.Trading.SendTransaction(killOrderTransaction);
        }
    }
}

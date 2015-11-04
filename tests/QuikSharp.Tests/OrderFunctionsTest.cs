using System;
using NUnit.Framework;
using QuikSharp.DataStructures.Transaction;

namespace QuikSharp.Tests
{
    [TestFixture]
    public class OrderFunctionsTest
    {
        [Test]
        public void GetOrderTest()
        {
            Quik quik = new Quik();

            //Заведомо не существующая заявка.
            long orderId = 123456789;
            Order order = quik.Orders.GetOrder("TQBR", orderId).Result;
            Assert.IsNull(order);

            //Заявка с таким номером должна присутствовать в таблице заявок.
            orderId = 14278245258;//вставьте свой номер
            order = quik.Orders.GetOrder("TQBR", orderId).Result;
            if (order != null)
            {
                Console.WriteLine("Order state: " + order.State);
            }
            else
            {
                Console.WriteLine("Order doesn't exsist.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuikSharp.DataStructures;

namespace QuikSharp
{
    /// <summary>
    /// Функции для получения свечей
    /// </summary>
    public class CandleFunctions
    {
        private QuikService QuikService { get; set; }

        public event EventHandler<Candle> NewCandle;

        internal void RaiseNewCandleEvent(Candle candle)
        {
            if (NewCandle != null)
                NewCandle(this, candle);
        }

        public CandleFunctions(int port)
        {
            QuikService = QuikService.Create(port);
        }

        /// <summary>
        /// Возвращает 
        /// </summary>
        /// <param name="tag">Строковый идентификатор графика или индикатора</param>
        /// <returns></returns>
        public async Task<List<Candle>> GetCandles(string tag)
        {
            var message = new Message<string>(tag, "GetCandles");
            Message<List<Candle>> response = await QuikService.Send<Message<List<Candle>>>(message);
            return response.Data;
        }

        public async void Subscribe(string classCode, string securityCode, string interval)
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + interval, "SubscribeToCandles");            
            await QuikService.Send<Message<string>>(message);
        }

        public async void Unsubscribe(string classCode, string securityCode, string interval)
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + interval, "UnsubscribeFromCandles");            
            await QuikService.Send<Message<string>>(message);     
        }

        public async Task<bool> IsSubscribed(string classCode, string securityCode, string interval)
        {
            var message = new Message<string>(classCode + "|" + securityCode + "|" + interval, "IsSubscribed");
            Message<bool> response = await QuikService.Send<Message<bool>>(message);
            return response.Data;
        }
    }
}

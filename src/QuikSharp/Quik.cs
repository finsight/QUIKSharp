// Copyright (C) 2014 Victor Baybekov

namespace QuikSharp {
    /// <summary>
    /// Quik interface in .NET
    /// </summary>
    public class Quik {
        /// <summary>
        /// 34130
        /// </summary>
        public const int DefaultPort = 34130;

        /// <summary>
        /// Quik interface in .NET constructor
        /// </summary>
        public Quik(int port = DefaultPort) {
            QuikService = QuikService.Create(port);
            Events = QuikService.Events;

            Debug = new DebugFunctions(port);
            Service = new ServiceFunctions(port);
            Class = new ClassFunctions(port);
            OrderBook = new OrderBookFunctions(port);
            Trading = new TradingFunctions(port);
        }

        private QuikService QuikService { get; set; }

        /// <summary>
        /// Debug functions
        /// </summary>
        public DebugFunctions Debug { get; set; }

        /// <summary>
        /// Функции обратного вызова
        /// </summary>
        public IQuikEvents Events { get; set; }

        /// <summary>
        /// Сервисные функции
        /// </summary>
        public IServiceFunctions Service { get; private set; }

        /// <summary>
        /// Функции для обращения к спискам доступных параметров
        /// </summary>
        public IClassFunctions Class { get; private set; }

        /// <summary>
        /// Функции для работы со стаканом котировок
        /// </summary>
        public IOrderBookFunctions OrderBook { get; set; }

        /// <summary>
        /// Функции взаимодействия скрипта Lua и Рабочего места QUIK
        /// </summary>
        public ITradingFunctions Trading { get; set; }
    }
}

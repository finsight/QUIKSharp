// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;

namespace QuikSharp
{
    /// <summary>
    /// Quik interface in .NET
    /// </summary>
    public sealed class Quik
    {
        /// <summary>
        /// 34130
        /// </summary>
        public const int DefaultPort = 34130;

        /// <summary>
        /// Quik interface in .NET constructor
        /// </summary>
        public Quik(int port = DefaultPort, IPersistentStorage storage = null)
        {
            if (storage == null) { Storage = new InMemoryStorage(); } else { Storage = storage; }
            QuikService = QuikService.Create(port);
            // poor man's DI
            QuikService.Storage = Storage;
            Events = QuikService.Events;
            Debug = new DebugFunctions(port);
            Service = new ServiceFunctions(port);
            Class = new ClassFunctions(port);
            OrderBook = new OrderBookFunctions(port);
            Trading = new TradingFunctions(port);
            StopOrders = new StopOrderFunctions(port, this);
            Orders = new OrderFunctions(port, this);
            Candles = new CandleFunctions(port);
            QuikService.Candles = Candles;
            QuikService.StopOrders = StopOrders;
        }

        // Если запуск "сервиса" (потоков работы с Lua) происходит в конструкторе Quik, то возможности остановить "сервис" нет.
        // QuikService объявлен как private.
        public void StopService()
        {
            QuikService.Stop();
        }

        private QuikService QuikService { get; set; }

        /// <summary>
        /// Quik current data is all in local time. This property allows to convert it to UTC datetime
        /// </summary>
        public TimeZoneInfo TimeZoneInfo { get; set; }

        /// <summary>
        /// Persistent transaction storage
        /// </summary>
        public IPersistentStorage Storage { get; set; }

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

        /// <summary>
        /// Функции для работы со стоп-заявками
        /// </summary>
        public StopOrderFunctions StopOrders { get; private set; }

        /// <summary>
        /// Функции для работы с заявками
        /// </summary>
        public OrderFunctions Orders { get; private set; }

        /// <summary>
        /// Функции для работы со свечами
        /// </summary>
        public CandleFunctions Candles { get; private set; }
    }
}
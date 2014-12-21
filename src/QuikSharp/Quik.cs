// Copyright (C) 2014 Victor Baybekov

namespace QuikSharp {
    public class Quik {
        public const int DefaultPort = 34130;

        public Quik(int port = DefaultPort) {
            QuikService = QuikService.Create(port);
            Debug = new DebugFunctions(port);
            Service = new ServiceFunctions(port);
            Events = QuikService.Events;
        }

        private QuikService QuikService { get; set; }

        public DebugFunctions Debug { get; set; }

        public IQuikEvents Events { get; set; }

        public IServiceFunctions Service { get; private set; }

        

    }
}

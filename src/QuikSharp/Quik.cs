using QuikSharp.ServiceFunctions;

namespace QuikSharp {
    public class Quik {
        public static readonly int DefaultPort = 34130;

        public Quik(int port = 34130) {
            QuikService = QuikService.Create(port);
            ServiceFunctions = new ServiceFunctions.ServiceFunctions(port);
        }

        internal QuikService QuikService { get; private set; }

        public IServiceFunctions ServiceFunctions { get; private set; }
    }
}

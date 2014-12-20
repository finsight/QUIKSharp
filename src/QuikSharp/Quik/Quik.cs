using QuikSharp.Quik;
using QuikSharp.Quik.ServiceFunctions;

namespace QuikSharp.Quik {
    public class Quik {
        private static readonly Quik InstancePrivate = new Quik();

        private Quik() {
            // build a hierarchy for convenience
            ServiceFunctions = new ServiceFunctions.ServiceFunctions();
        }

        public Quik Instance { get { return InstancePrivate; } }
        public IServiceFunctions ServiceFunctions { get; private set; }
    }
}

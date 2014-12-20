using System.Threading.Tasks;

namespace QuikSharp
{
    static class ServiceManager {
        private static QuikService quikService;
        public static void StartServices() {
            quikService = QuikService.Create(Quik.DefaultPort);
            Task.Run(() => Tray.Run());
        }

        public static void StopServices() {
            Tray.Instance.OnExit(null, null);
        }

        public static void RestartServices()
        {
            StopServices();
            StartServices();
        }
    }
}

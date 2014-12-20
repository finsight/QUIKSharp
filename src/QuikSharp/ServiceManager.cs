using System.Threading.Tasks;

namespace QuikSharp
{
    static class ServiceManager
    {
        public static void StartServices()
        {
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

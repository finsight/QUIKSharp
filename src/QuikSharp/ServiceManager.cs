using System.Diagnostics;
using QuikSharp.Quik;
using System.Threading.Tasks;

namespace QuikSharp
{
    static class ServiceManager
    {
        public static void StartServices()
        {
            QuikService.Start();
            Task.Run(() => Tray.Run());
        }

        public static void StopServices() {
            QuikService.Stop();
            Tray.Instance.OnExit(null, null);
        }

        public static void RestartServices()
        {
            StopServices();
            StartServices();
        }
    }
}

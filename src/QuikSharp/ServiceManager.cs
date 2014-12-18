using System.Diagnostics;
using QuikSharp.Quik;

namespace QuikSharp
{
    static class ServiceManager
    {
        private static readonly TraceSource Ts = new TraceSource("QuikSharp");
        private static QuikService _quikService;

        public static void StartServices()
        {
            _quikService = new QuikService();
            Tray.Run();
        }

        public static void StopServices()
        {
            Tray.Instance.OnExit(null, null);
        }

        public static void RestartServices()
        {
            StopServices();
            StartServices();
        }
    }
}

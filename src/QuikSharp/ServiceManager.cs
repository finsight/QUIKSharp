// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// TODO Old stuff, sort it out

namespace QuikSharp {
    public enum NotificationType {
        Error,
        Warning,
        Info
    }


    public static class NetworkUtils {

        public static IPAddress LocalIPAddress() {
            if (!NetworkInterface.GetIsNetworkAvailable()) {
                return null;
            }

            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

            return host
                .AddressList
                .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        }

        [Obsolete]
        public static bool IsRunningOnEc2() {
            return LocalIPAddress().ToString().StartsWith("10.");
        }

        [Obsolete("WTF?")]
        public static bool IsReallyConnected(this Socket s) {
            var part1 = s.Poll(1000, SelectMode.SelectRead);
            var part2 = (s.Available == 0);
            if ((part1 && part2) || !s.Connected) return false;
            return true;
        }
    }
}

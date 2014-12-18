using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using File = System.IO.File;

namespace QuikSharp.Starter
{
    public class Program
    {

        private static string configFile = @"Quik.config";
        private static QuikConfiguration qconf;

        public static void Main(string[] args)
        {

            string command = "start";
            if (args.Length > 0)
            {
                command = args[0];
            }

            do
            {
                qconf = new QuikConfiguration();

                //Console.Clear();

                switch (command.ToLower())
                {
                    case "setup":
                        Setup();
                        break;
                    case "restart":
                        KillQuik();
                        StartQuik();
                        break;
                    case "stop":
                        KillQuik();
                        break;
                    case "start":
                        StartQuik();
                        break;
                    case "exit":
                    case "quit":
                        return;
                    default:
                        break;
                }
                Console.WriteLine(@"Available commands: 'start', 'stop', 'restart', 'setup', 'exit'");
                command = Console.ReadLine().Trim().ToLower();
            } while (true);

        }


        public static void Setup()
        {


            // Get the application configuration file.
            Configuration config =
              ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            if (File.Exists(configFile)) File.Delete(configFile);

            do
            {
                qconf.Name = "";
                qconf.QuikPath = "";
                qconf.User = "";
                qconf.Password = "";

                while (string.IsNullOrEmpty(qconf.Name))
                {
                    Console.WriteLine(@"Please enter connection name, e.g. MyQuik (max 50 symbols)");
                    qconf.Name = Console.ReadLine().Trim(new char[] { '"', ' ', "'".ToCharArray()[0] });
                    if (qconf.Name.Length > 50)
                    {
                        Console.WriteLine(@"Too long");
                        qconf.Name = "";
                    }
                };

                while (string.IsNullOrEmpty(qconf.QuikPath))
                {
                    Console.WriteLine(@"Please enter the full path to Quik, e.g. C:\Quik\info.exe");
                    qconf.QuikPath = Console.ReadLine().Trim(new char[] { '"', ' ', "'".ToCharArray()[0] });
                }

                while (string.IsNullOrEmpty(qconf.User))
                {
                    Console.WriteLine(@"Please enter user name, e.g. Smith John or Иванов Иван (max 50 symbols)");
                    qconf.User = Console.ReadLine().Trim(new char[] { '"', ' ', "'".ToCharArray()[0] });
                    if (qconf.User.Length > 50)
                    {
                        Console.WriteLine(@"Too long");
                        qconf.User = "";
                    }
                }

                while (string.IsNullOrEmpty(qconf.Password))
                {
                    Console.WriteLine(@"Please enter password (it will be visible on the screen), e.g. myPa$$w0rD (max 50 symbols)");
                    qconf.Password = Console.ReadLine().Trim();
                    if (qconf.Password.Length > 50)
                    {
                        Console.WriteLine(@"Too long");
                        qconf.Password = "";
                    }

                }

                Console.WriteLine(@"Is everything correct (Y/N)?");
            } while (Console.ReadLine().ToLower().Substring(0, 1) != "y");

            //Console.Clear();



            // From MSDN: Key containers provide the most secure way to persist cryptographic keys 
            // and keep them secret from malicious third parties.
            string containerName = qconf.Name.GetStringHash();
            string salt = MachineIdentificator.GetUniqueStringID();
            var RSAProvider = RsaUtilities.GetRSAProviderFromContainer(containerName + salt, false);

            //Encrypt sensitive parts
            qconf.User = Convert.ToBase64String(
                    RSAProvider.Encrypt(Encoding.UTF8.GetBytes(qconf.User), false)
                );
            qconf.Password = Convert.ToBase64String(
                    RSAProvider.Encrypt(Encoding.UTF8.GetBytes(qconf.Password), false)
                );

            // You need to remove the old settings object before you can replace it
            config.Sections.Remove("QuikConfiguration");
            // with an updated one
            config.Sections.Add("QuikConfiguration", qconf);

            // Write the new configuration data to the XML file
            config.SaveAs(configFile, ConfigurationSaveMode.Full);

            Console.WriteLine(@"You data is saved.");


        }



        public static void StartQuik()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            Configuration config =
               ConfigurationManager.OpenMappedExeConfiguration(fileMap,
               ConfigurationUserLevel.None);
            qconf = (QuikConfiguration)config.GetSection("QuikConfiguration");

            if (qconf == null)
            {
                Main(new string[] { "-setup" });
                return;
            }

            Console.WriteLine(@"Starting '" + qconf.QuikPath + "'.");


            // From MSDN: Key containers provide the most secure way to persist cryptographic keys 
            // and keep them secret from malicious third parties.
            string containerName = qconf.Name.GetStringHash();
            string salt = MachineIdentificator.GetUniqueStringID();
            var RSAProvider = RsaUtilities.GetRSAProviderFromContainer(containerName + salt, false);


            //Decrupt sensitive parts

            qconf.User = Encoding.UTF8.GetString(RSAProvider.Decrypt(Convert.FromBase64String(qconf.User), false));
            qconf.Password = Encoding.UTF8.GetString(RSAProvider.Decrypt(Convert.FromBase64String(qconf.Password), false));



            StartProcessAndSendKeys();

            //Console.WriteLine(qconf.Name);
            //Console.WriteLine(qconf.QuikPath);
            //Console.WriteLine(qconf.User);
            //Console.WriteLine(qconf.Password);

        }

        public static void KillQuik()
        {

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            Configuration config =
               ConfigurationManager.OpenMappedExeConfiguration(fileMap,
               ConfigurationUserLevel.None);
            qconf = (QuikConfiguration)config.GetSection("QuikConfiguration");

            if (qconf == null)
            {
                Main(new string[] { "-setup" });
                return;
            }

            Console.WriteLine(@"Closing running processes of '" + qconf.QuikPath + "', if present.");

            var Quiks = Process.GetProcessesByName("info");
            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var ps = from p in Quiks
                         join mo in results.Cast<ManagementObject>()
                         on p.Id equals (int)(uint)mo["ProcessId"]
                         //where (string)mo["ExecutablePath"] == qconf.QuikPath
                         select new
                         {
                             Process = p,
                             Path = (string)mo["ExecutablePath"],
                             CommandLine = (string)mo["CommandLine"],
                         };

                foreach (var p in ps)
                {
                    p.Process.Kill();
                }
            }

        }

        [DllImport("User32.dll")]
        private static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, int msg, int Param, string s);

        public const int GW_HWNDNEXT = 2;

        [DllImport("User32.dll")]
        public static extern int GetWindow(int hwndSibling,
                                           int wFlag);

        [DllImport("user32.dll")]
        static extern IntPtr GetDlgItem(IntPtr hDlg, int nIDDlgItem);

        private static void StartProcessAndSendKeys()
        {
            var quikProcess = Process.Start(qconf.QuikPath);
            Thread.Sleep(500);
            IntPtr hwndChild = IntPtr.Zero;

            //WshShell shell = new WshShell();

            //shell.AppActivate("Идентификация пользователя");

            while (hwndChild == IntPtr.Zero)
            {
                //if (shell.AppActivate("Идентификация пользователя")) break;
                hwndChild = FindWindow(null, "Идентификация пользователя");
                Thread.Sleep(100);
            }
            //const int WM_SETTEXT = 0x000c;

            IntPtr boxHwnd = GetDlgItem(hwndChild, 0x2775);

            IntPtr edit = FindWindowEx(hwndChild, IntPtr.Zero, "Edit", "");
            
            Thread.Sleep(500);

            SendMessage((int)boxHwnd, 0x000C, 5, "login");
            SendMessage((int)boxHwnd, 0x000C, 5, "login");

            int h = (int)hwndChild;

            h = GetWindow(h, GW_HWNDNEXT);
            SendMessage(h, 0x000C, 0, "login");
            h = GetWindow(h, GW_HWNDNEXT);
            SendMessage(h, 0x000C, 0, "password");
            h = GetWindow(h, GW_HWNDNEXT);
            //SendMessage(h, WM_LBUTTONDOWN, 1, 1);
            //SendMessage(h, WM_LBUTTONUP, 1, 1);

            //SetForegroundWindow(hwndChild);
            //shell.SendKeys("(^+2)");
            //Thread.Sleep(100);
            //shell.SendKeys("login");
            //shell.SendKeys("(^+1)");
            //Thread.Sleep(100);
            //shell.SendKeys("{TAB}");
            //Thread.Sleep(100);
            //shell.SendKeys("pass" + "~");
            //Thread.Sleep(200);

            //SendKeys.SendWait("test");

        }

    }
}

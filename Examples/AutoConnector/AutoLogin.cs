using QuikSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace AutoConnector
{
    public static class AutoLogin
    {
        public static bool Enter(string login, string pass)
        {
            IntPtr hLoginWnd;

            hLoginWnd = WinowsFind.FindWindow("#32770", "Идентификация пользователя");

            if (hLoginWnd == IntPtr.Zero)
            {
                hLoginWnd = WinowsFind.FindWindow("#32770", "User identification");
            }

            if (hLoginWnd == IntPtr.Zero)
            {
                return false;
            }

            if (hLoginWnd != IntPtr.Zero)
            {
                IntPtr nBtnOk = FindWindowByIndex(hLoginWnd, 1, "Button");
                IntPtr hLogin = FindWindowByIndex(hLoginWnd, 1, "Edit");
                IntPtr nPassw = FindWindowByIndex(hLoginWnd, 2, "Edit");

                setWindowText(hLogin, login);
                setWindowText(nPassw, pass);
                WinowsFind.SetFocus(nBtnOk);
                WinowsFind.PostMessage(nBtnOk, BM_CLICK, new IntPtr(0), new IntPtr(0));
            }

            return true;
        }


        private static class WinowsFind
        {
            [DllImport("user32.dll", SetLastError = true)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string lclassName, string windowTitle);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SetFocus(IntPtr hWnd);
            [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
            [DllImport("user32.dll", SetLastError = true)]
            public static extern bool SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string s);
        }

        /// <summary>
        /// Найти дискриптор окна по индексу
        /// </summary>
        /// <param name="hWndParent"></param>
        /// <param name="index"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IntPtr FindWindowByIndex(IntPtr hWndParent, int index, string type)
        {
            if (index == 0)
                return hWndParent;
            else
            {
                int ct = 0;
                IntPtr result = IntPtr.Zero;
                do
                {
                    result = WinowsFind.FindWindowEx(hWndParent, result, type, null);
                    if (result != IntPtr.Zero)
                        ++ct;
                }
                while (ct < index && result != IntPtr.Zero);
                return result;
            }
        }

        /// <summary>
        /// Отправить текст в окно с данными пользователя
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="text"></param>
        private static void setWindowText(IntPtr hWnd, string text)
        {
            try
            {
                WinowsFind.SetFocus(hWnd);
                WinowsFind.SendMessage(hWnd, WM_SETTEXT, IntPtr.Zero, null);

                foreach (char c in text)
                {
                    Thread.Sleep(50);
                    IntPtr val = new IntPtr((Int32)c);
                    WinowsFind.PostMessage(hWnd, WM_CHAR, val, new IntPtr(0));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        const int WM_CHAR = 0x0102;
        const uint WM_SETTEXT = 0x000c;
        const int BM_CLICK = 0x00F5;
    }
}

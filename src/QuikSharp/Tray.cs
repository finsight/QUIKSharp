// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace QuikSharp
{
    // TODO Menu items and icons with notifications
    internal class Tray : Form
    {

        public static Tray Instance = new Tray();

        //[STAThread]
        public static void Run()
        {
            Application.Run(Instance);
        }

        public readonly NotifyIcon TrayIcon;

        public Tray()
        {
            // Create a simple tray menu with only one item.
            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", Program.ManualExitHandler);

            // Create a tray icon. In this example we use a
            // standard system icon for simplicity, but you
            // can of course use your own custom icon too.
            TrayIcon = new NotifyIcon
            {
                Text = "QuikSharp",
                Icon = new Icon(SystemIcons.Application, 40, 40),
                ContextMenu = trayMenu,
                Visible = true
            };

            // Add menu to tray icon and show it.
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        public void OnExit(object sender, EventArgs e)
        {
            Application.Exit(); 
        }

        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                // Release the icon resource.
                TrayIcon.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
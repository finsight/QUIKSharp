// ===========================================================================
//  QuikDDE_WinForms.cs (c) 2012 Nikolay Moroshkin, http://www.moroshkin.com/
// ===========================================================================

using System;
using System.Windows.Forms;

namespace QuikDDE_WinForms
{
  static class Program
  {
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run(new MainForm());
    }
  }
}

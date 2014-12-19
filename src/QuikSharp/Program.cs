using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace QuikSharp {

  // Шаманство с обработкой закрытия может быть нужно, если кровь из носа следует 
  // почистить за собой перед выходом, например снять все заявки или сохранить
  // необработанные данные. Взято из:
  // http://stackoverflow.com/questions/474679/capture-console-exit-c-sharp?lq=1
  // http://stackoverflow.com/questions/1119841/net-console-application-exit-event

  static class Program {
    static bool _exitSystem;

    #region Trap application termination
    [DllImport("Kernel32")]
    private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

    private delegate bool EventHandler(CtrlType sig);
    static EventHandler _handler;

    enum CtrlType {
      // ReSharper disable InconsistentNaming
      // ReSharper disable UnusedMember.Local
      CTRL_C_EVENT = 0,
      CTRL_BREAK_EVENT = 1,
      CTRL_CLOSE_EVENT = 2,
      CTRL_LOGOFF_EVENT = 5,
      CTRL_SHUTDOWN_EVENT = 6
    }

    private static bool Handler(CtrlType sig) {
      Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

      //do your cleanup here
      Cleanup();

      Thread.Sleep(1000); //simulate some cleanup delay

      Debug.Print("Cleanup complete");
      //allow main to run off
      _exitSystem = true;
      //shutdown right away so there are no lingering threads
      Environment.Exit(-1);
      return true;
    }
    #endregion
    static void Main() {
      // Some biolerplate to react to close window event, CTRL-C, kill, etc
      _handler += Handler;
      SetConsoleCtrlHandler(_handler, true);


      ServiceManager.StartServices();
      Console.WriteLine("Services are available. " +
            "Press <ENTER> to exit.");
      Console.ReadLine();
      ServiceManager.StopServices();

      //hold the console so it doesn’t run off the end
      while (!_exitSystem) {
        Thread.Sleep(500);
      }

    }

    static void Cleanup() {
      ServiceManager.StopServices();
      var ok = MessageBox.Show("Called on exit");
    }

  }



}
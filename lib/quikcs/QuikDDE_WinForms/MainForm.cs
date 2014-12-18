// =======================================================================
//    MainForm.cs (c) 2014 Nikolay Moroshkin, http://www.moroshkin.com/
// =======================================================================

using System;
using System.Windows.Forms;

using XlDde;

// Для работы данного примера требуются файлы из директории XlDde и ссылка
// на библиотеку NDde.dll (http://ndde.codeplex.com/). Перед его изучением
// рекомендуется ознакомиться с примером для консоли.

namespace QuikDDE_WinForms
{
  sealed partial class MainForm : Form
  {
    // **********************************************************************

    // Основное отличие реализации получения данных по DDE в GUI приложении
    // от консольного в том, что обращаться к элементам UI можно только из
    // основного потока приложения, тогда как данные от XlDdeServer
    // поступают из другого потока. В связи с этим требуется организовать
    // передачу данных из одного потока в другой. При этом необходимо
    // принудить поток UI эти данные обработать. Это можно сделать двумя
    // способами:

    //   1. При поступлении данных вызывать метод BeginInvoke UI контрола;
    //   2. Реализовать таймер в основном потоке, который будет проверять
    //      поступление новых данных.

    // Первый способ обладает рядом недостатков, в том числе по
    // производительности. Поэтому его следует использовать только
    // для редко обновляющихся данных. В примере он использован для
    // обновления информации о количестве подключенных к каналу
    // источников данных. Для отображения интенсивно обновляющейся
    // информации следует использовать второй способ. В примере он
    // используется для вывода передаваемых по DDE данных.

    // Реализуя любой метод передачи данных, следует помнить, что поток UI
    // никогда не должен приостанавливаться.

    // **********************************************************************

    // Идентификатор DDE сервера. Он указывается
    // при настройке экспорта таблиц в QUIK.

    const string service = "DDESample";

    // **********************************************************************

    XlDdeServer server;

    // DDE каналы. Теперь они нам потребуются в процессе работы.
    // Определены в файлах TradesChannel.cs и QuotesChannel.cs.
    TradesChannel tradesChannel;
    QuotesChannel quotesChannel;

    // Таймер, который будет проверять поступление новых данных.
    Timer updater;

    // **********************************************************************

    public MainForm()
    {
      InitializeComponent();

      server = new XlDdeServer(service);

      // Создадим и зарегистрируем обработчики передаваемых Квиком данных
      // для каждого нашего DDE канала.
      tradesChannel = new TradesChannel();
      quotesChannel = new QuotesChannel();

      tradesChannel.ConversationAdded += TradesConversationsChanged;
      tradesChannel.ConversationRemoved += TradesConversationsChanged;

      quotesChannel.ConversationAdded += QuotesConversationsChanged;
      quotesChannel.ConversationRemoved += QuotesConversationsChanged;

      server.AddChannel(tradesChannel);
      server.AddChannel(quotesChannel);

      // Зарегистрируем сам DDE сервер.
      server.Register();

      // Таймер
      updater = new Timer();
      updater.Interval = 100; // мс, этим значением не стоит злоупотреблять.
      // если нужна большая скорость обновления, то минимально разумное
      // значение - 15 мс. Меньше не имеет смысла, как с технической точки
      // зрения, так и с физиологической для человека.

      updater.Tick += new EventHandler(UpdaterTick);
      updater.Start();
    }

    // **********************************************************************

    protected override void OnClosed(EventArgs e)
    {
      // При закрытии формы выполним корректное уничтожение DDE сервера.

      server.Disconnect();
      server.Dispose();

      base.OnClosed(e);
    }

    // **********************************************************************
    // * Обработчики изменений кол-ва источников данных для каждого канала  *
    // **********************************************************************

    void TradesConversationsChanged()
    {
      this.BeginInvoke(new Action(() =>
      {
        tradesConv.Text = tradesChannel.Conversations.ToString();
      }));
    }

    // **********************************************************************

    void QuotesConversationsChanged()
    {
      this.BeginInvoke(new Action(() =>
      {
        quotesConv.Text = quotesChannel.Conversations.ToString();
      }));
    }

    // **********************************************************************
    // *                  Обработчик основных данных в UI                   *
    // **********************************************************************

    void UpdaterTick(object sender, EventArgs e)
    {
      // Проверим, есть ли какие-либо данные в очереди канала trades,
      // и если есть, выведем их в TextBox. Следует учитывать, что обращение
      // к очереди идет из другого потока, нежели того, в котором данные
      // в нее добавляются.

      if(tradesChannel.Queue.Count > 0)
      {
        string str;

        while(tradesChannel.Queue.TryDequeue(out str))
        {
          // Выводим данные, если только их количество
          // на данный момент меньше 50 - небольшой хак,
          // чтобы не выводить объемные первичные данные.

          if(tradesChannel.Queue.Count < 50)
            tradesBox.AppendText(str);
        }
      }

      // Проверим, обновились ли данные в канале quotes.
      // Если это так, обновим соответствующий TextBox.

      if(quotesChannel.Updated)
        quotesBox.Text = quotesChannel.Data;
    }

    // **********************************************************************
  }
}

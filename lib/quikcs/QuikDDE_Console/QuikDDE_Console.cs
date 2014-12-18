// ==========================================================================
//  QuikDDE_Console.cs (c) 2014 Nikolay Moroshkin, http://www.moroshkin.com/
// ==========================================================================

using System;
using XlDde;

// Для работы данного примера требуются файлы из директории XlDde
// и ссылка на библиотеку NDde.dll (http://ndde.codeplex.com/).

namespace QuikDDE_Console
{
  static class Program
  {
    // **********************************************************************

    // Идентификатор DDE сервера. Он указывается
    // при настройке экспорта таблиц в QUIK.

    const string service = "DDESample";

    // **********************************************************************
    // *              Классы DDE каналов с обработчиками данных             *
    // **********************************************************************

    // Определим классы DDE каналов. В каждом таком классе должны быть
    // перегрузки свойства Topic, которое возвращает произвольный
    // идентификатор канала (он указывается в поле "Рабочая книга" QUIK),
    // и метода ProcessTable(), в который будут передаваться получаемые
    // по DDE таблицы с данными для последующей обработки.

    // Будем получать из QUIK две таблицы: trades и quotes.


    sealed class TradesChannel : XlDdeChannel
    {
      // Идентификатор канала
      public override string Topic { get { return "trades"; } }

      // Обработчик данных
      public override void ProcessTable(XlTable xt)
      {
        // Поступили новые данные из таблицы, настроенной для экспорта
        // в "Рабочую книгу" trades.

        // Все переданные данные содержатся в переменной xt, являющейся
        // экземпляром класса XlTable, который содержит методы для их
        // распаковки.

        // xt.Rows - кол-во строк, xt.Columns - кол-во столбцов в полученной
        // таблице. Для чтения одной ячейки таблицы служит метод
        // xt.ReadValue(), который после своего вызова устанавливает
        // свойства xt.ValueType - тип прочитанных данных и xt.*Value -
        // значение в ячейке. От Квика поступают данные двух типов:
        //  XlTable.BlockType.Float (double в C#)
        //  XlTable.BlockType.String (в C# тоже string)

        // Отобразим всю полученную информацию

        for(int row = 0; row < xt.Rows; row++)
        {
          for(int col = 0; col < xt.Columns; col++)
          {
            xt.ReadValue();

            switch(xt.ValueType)
            {
              case XlTable.BlockType.Float:
                Console.Write("D:{0}\t", xt.FloatValue);
                break;

              case XlTable.BlockType.String:
                Console.Write("S:{0}\t", xt.StringValue);
                break;

              default:
                Console.Write("{0}:{1}\t", xt.ValueType, xt.WValue);
                break;
            }
          }

          Console.WriteLine();
        }
      }
    }

    // **********************************************************************

    sealed class QuotesChannel : XlDdeChannel
    {
      // В этом классе мы не будем заниматься обработкой поступающих данных,
      // а только отображать факт их получения, а также события подключения
      // и отключения источника данных к каналу.

      public override string Topic { get { return "quotes"; } }

      public QuotesChannel()
      {
        // Добавим обработчики событий подключения
        // и отключения источника данных.

        this.ConversationAdded += () => { PrintEvent("connected"); };
        this.ConversationRemoved += () => { PrintEvent("disconnected"); };
      }

      void PrintEvent(string eType)
      {
        // Выведем информацию о событии и текущее количество источников
        // данных, которые ведут передачу по этому каналу.

        Console.WriteLine("\n\'" + Topic + "\' channel " + eType
          + ". Conversations: " + this.Conversations);
      }

      public override void ProcessTable(XlTable xt)
      {
        // Поступили новые данные из таблицы, настроенной для экспорта
        // в "Рабочую книгу" quotes.

        // Ничего не будем делать, только обозначим это событие.

        Console.Write(".");
      }
    }

    // **********************************************************************
    // *                             EntryPoint                             *
    // **********************************************************************

    static void Main()
    {
      using(XlDdeServer server = new XlDdeServer(service))
      {
        // Создадим и зарегистрируем обработчики передаваемых Квиком данных
        // для каждого нашего DDE канала.
        server.AddChannel(new TradesChannel());
        server.AddChannel(new QuotesChannel());

        // Зарегистрируем сам DDE сервер.
        server.Register();

        // Теперь DDE сервер готов к работе и каждый раз при поступлении
        // данных он будет передавать их методу ProcessTable() в
        // соответствующем каналу экземпляре класса.

        Console.WriteLine("DDE server ready. Press Enter to exit.\n\n");
        Console.ReadLine();
      }
    }

    // **********************************************************************
  }
}

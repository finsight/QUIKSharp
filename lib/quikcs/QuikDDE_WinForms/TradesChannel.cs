// ==========================================================================
//   TradesChannel.cs (c) 2014 Nikolay Moroshkin, http://www.moroshkin.com/
// ==========================================================================

using System.Collections.Concurrent;
using System.Text;

using XlDde;

namespace QuikDDE_WinForms
{
  sealed class TradesChannel : XlDdeChannel
  {
    // **********************************************************************

    // Идентификатор канала.
    public override string Topic { get { return "trades"; } }

    // **********************************************************************

    // Очередь передачи данных. Важно чтобы она имела возможность
    // одновременного доступа из нескольких потоков (thread safe).
    // В противном случае необходимо использовать блокировки при работе с ней.

    public readonly ConcurrentQueue<string> Queue = new ConcurrentQueue<string>();

    // **********************************************************************

    StringBuilder sb = new StringBuilder();

    // **********************************************************************

    // Обработчик данных
    public override void ProcessTable(XlTable xt)
    {
      // Учитывая, что это простой пример, в очередь будем
      // класть уже готовую для отображения строку.

      for(int row = 0; row < xt.Rows; row++)
      {
        sb.Clear();

        for(int col = 0; col < xt.Columns; col++)
        {
          xt.ReadValue();

          switch(xt.ValueType)
          {
            case XlTable.BlockType.Float:
              sb.Append(xt.FloatValue);
              break;

            case XlTable.BlockType.String:
              sb.Append(xt.StringValue);
              break;

            default:
              sb.Append(xt.ValueType);
              sb.Append(":");
              sb.Append(xt.WValue);
              break;
          }

          sb.Append("\t");
        }

        sb.AppendLine();

        Queue.Enqueue(sb.ToString());
      }
    }

    // **********************************************************************
  }
}

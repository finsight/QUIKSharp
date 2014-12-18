// ==========================================================================
//   QuotesChannel.cs (c) 2014 Nikolay Moroshkin, http://www.moroshkin.com/
// ==========================================================================

using System.Text;
using XlDde;

namespace QuikDDE_WinForms
{
  sealed class QuotesChannel : XlDdeChannel
  {
    // **********************************************************************

    public override string Topic { get { return "quotes"; } }

    // **********************************************************************

    const int vspace = 12;

    StringBuilder sb = new StringBuilder();
    bool updated;

    // **********************************************************************

    // Для отображения стакана история его изменений не нужна, поэтому
    // будем хранить только его текущее состояние. При этом очередь
    // передачи и блокировки не нужны, однако сделаем флаг, показывающий,
    // что данные обновились.      

    public string Data { get; private set; }
    public bool Updated { get { return updated && !(updated = false); } }

    // **********************************************************************

    public override void ProcessTable(XlTable xt)
    {
      sb.Clear();

      for(int row = 0; row < xt.Rows; row++)
      {
        for(int col = 0; col < xt.Columns; col++)
        {
          xt.ReadValue();

          switch(xt.ValueType)
          {
            case XlTable.BlockType.Float:
              sb.Append(xt.FloatValue.ToString().PadLeft(vspace));
              break;

            case XlTable.BlockType.String:
              sb.Append(xt.StringValue.PadLeft(vspace));
              break;

            default:
              sb.Append(xt.ValueType.ToString().PadLeft(vspace));
              break;
          }
        }

        sb.AppendLine();
      }

      Data = sb.ToString();
      updated = true;
    }

    // **********************************************************************
  }
}

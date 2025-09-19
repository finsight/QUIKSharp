using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum NegReportFlags
    {
        /// <summary>
        /// Отчет отправлен пользователем (статус «Отправлена»)
        /// </summary>
        Sended = 0x40,

        /// <summary>
        /// Отчет получен пользователем от другого контрагента по сделке для исполнения (статус «Получена»)  
        /// </summary>
        Received = 0x80
    }
}
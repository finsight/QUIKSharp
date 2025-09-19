using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Дополнительные флаги для таблицы «Стоп-заявки»
    /// </summary>
    [Flags]
    public enum StopOrderTradeFlags
    {
        /// <summary>
        /// Использовать остаток основной заявки
        /// </summary>
        UseBalanceMainOrder = 0x1,

        /// <summary>
        /// При частичном исполнении заявки снять стоп-заявку
        /// </summary>
        PartiallyExecutedRemoveStopOrder = 0x2,

        /// <summary>
        /// Активировать стоп-заявку при частичном исполнении связанной заявки
        /// </summary>
        ActivateWhenLinkedOrderPartiallyFilled = 0x4,

        /// <summary>
        /// Отступ задан в процентах, иначе – в пунктах цены
        /// </summary>
        IndentationInPercentagesOtherwiseInPricePoints = 0x8,

        /// <summary>
        /// Защитный спред задан в процентах, иначе – в пунктах цены
        /// </summary>
        ProtectiveSpreadInPercentagesOtherwiseInPricePoints = 0x10,

        /// <summary>
        /// Срок действия стоп-заявки ограничен сегодняшним днем 
        /// </summary>
        StopOrderValidUntilToday = 0x20,

        /// <summary>
        /// Установлен интервал времени действия стоп-заявки
        /// </summary>
        TimeIntervalvaliditySet = 0x40,

        /// <summary>
        /// Выполнение тейк-профита по рыночной цене 
        /// </summary>
        TakeProfitAtMarketPrice = 0x80,

        /// <summary>
        /// Выполнение стоп-заявки по рыночной цене
        /// </summary>
        StopOrderAtMarketPrice = 0x100
    }
}
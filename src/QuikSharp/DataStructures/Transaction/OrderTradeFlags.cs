// Copyright (C) 2015 Victor Baybekov

using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Флаги для таблиц Заявки, Заявки на внебиржевые сделки, Сделки, Сделки для исполнения
    /// </summary>
    [Flags]
    public enum OrderTradeFlags {
        /// <summary>
        /// Заявка активна, иначе – не активна
        /// </summary>
        Active = 0x1,
        /// <summary>
        /// Заявка снята. Если флаг не установлен и значение бита «0» равно «0», то заявка исполнена
        /// </summary>
        Canceled = 0x2,
        /// <summary>
        /// Заявка на продажу, иначе – на покупку. Данный флаг для сделок и сделок
        ///  для исполнения определяет направление сделки (BUY/SELL)
        /// </summary>
        IsSell = 0x4,
        /// <summary>
        /// Заявка лимитированная, иначе – рыночная
        /// </summary>
        IsLimit = 0x8,
        /// <summary>
        /// Разрешить / запретить сделки по разным ценам
        /// </summary>
        AllowDiffPrice = 0x10,
        /// <summary>
        /// Исполнить заявку немедленно или снять (FILL OR KILL)
        /// </summary>
        FillOrKill = 0x20,
        /// <summary>
        /// Заявка маркет-мейкера. Для адресных заявок – заявка отправлена контрагенту
        /// </summary>
        IsMarketMakerOrSent = 0x40,
        /// <summary>
        /// Для адресных заявок – заявка получена от контрагента
        /// </summary>
        IsReceived = 0x80,
        /// <summary>
        /// Снять остаток
        /// </summary>
        IsKillBalance = 0x100,
        /// <summary>
        /// Айсберг-заявка
        /// </summary>
        Iceberg = 0x200

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Position
{
    /// <summary>
    /// Режим работы робота
    /// </summary>
    public string robotMode;
    /// <summary>
    /// ID позиции
    /// </summary>
    public string positionID;
    /// <summary>
    /// ДатаВремя входа в позицию
    /// </summary>
    public DateTime dateTimeEntrance;
    /// <summary>
    /// Цена (средняя) входа
    /// </summary>
    public decimal priceEntrance;
    /// <summary>
    /// Текущий уровень StopLoss
    /// Значение = 0, указавает на отсутствие стопа
    /// </summary>
    public decimal stopLoss;
    /// <summary>
    /// Цена выхода по стопу (цена, которая попадет в заявку, при срабатывании стопа)
    /// </summary>
    public decimal stopLossDealPrice;
    /// <summary>
    /// Текущий объем позиции
    /// "больше нуля" - лонг, "меньше нуля" - шорт, "0" - без позиции
    /// </summary>
    public int toolQty;
    /// <summary>
    /// Суммарный результат деятельности робота (включая стартовый депозит)
    /// </summary>
    public decimal summResult;

    /// <summary>
    /// ID заявки на открытие/увеличение позиции
    /// </summary>
    public long entranceOrderID;
    /// <summary>
    /// Номер заявки на открытие/увеличение позиции
    /// </summary>
    public long entranceOrderNumber;
    /// <summary>
    /// Начальное количество в заявке на открытие/увеличение позиции
    /// </summary>
    public int entranceOrderQty;
    /// <summary>
    /// ID заявки на закрытие/сокращение позиции
    /// </summary>
    public long closingOrderID;
    /// <summary>
    /// Номер заявки на закрытие/сокращение позиции
    /// </summary>
    public long closingOrderNumber;
    /// <summary>
    /// Начальное количество в заявке на закрытие/сокращение позиции
    /// </summary>
    public int closingOrderQty;

}

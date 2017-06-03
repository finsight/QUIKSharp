using QuikSharp.DataStructures;

public class Settings
{
    string robotMode;
    string lifeTimeOrder;
    CandleInterval tF;
    int koefSlip;
    int qtyOrder;
    int periodPriceChnl;

    #region Свойства
    /// <summary>
    /// Режим работы робота
    /// Виртуальный или Боевой
    /// </summary>
    public string RobotMode
    {
        get { return robotMode; }
        set { robotMode = value; }
    }
    /// <summary>
    /// Время жизни заявки
    /// Если значение = 0, заявка действует до отмены
    /// </summary>
    public string LifeTimeOrder
    {
        get { return lifeTimeOrder; }
        set { lifeTimeOrder = value; }
    }
    /// <summary>
    /// Рабочий тайм-фрейм робота
    /// </summary>
    public CandleInterval TF
    {
        get { return tF; }
        set {tF = value; }
    }
    /// <summary>
    /// Коэффициент расчета проскальзывания (Slip = KoefSlip * Step)
    /// </summary>
    public int KoefSlip
    {
        get { return koefSlip; }
        set { koefSlip = value; }
    }
    /// <summary>
    /// Размер объема заявки (лоты/контракты) для отключенного риск-менеджмента
    /// </summary>
    public int QtyOrder
    {
        get { return qtyOrder; }
        set { qtyOrder = value; }
    }
    /// <summary>
    /// Период расчета PriceChnl
    /// </summary>
    public int PeriodPriceChnl
    {
        get { return periodPriceChnl; }
        set { periodPriceChnl = value; }
    }
    #endregion

    public Settings()
    {
        robotMode = "Виртуальный";
        lifeTimeOrder = "00:01:00";
        tF = CandleInterval.M1;
        koefSlip = 3;
        qtyOrder = 1;
        periodPriceChnl = 5;
    }
}

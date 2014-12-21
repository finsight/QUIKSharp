// Copyright (C) 2014 Victor Baybekov

namespace QuikSharp.DataStructures {
    /// <summary>
    /// Таблица с параметрами обезличенной сделки 
    /// </summary>
    public class AllTrade {
        /// <summary>
        /// Номер сделки в торговой системе
        /// </summary>
        public long trade_num { get; set; }
        /// <summary>
        /// Набор битовых флагов:
        /// бит 0 (0x1)  Сделка на продажу  
        /// бит 1 (0x2)  Сделка на покупку  
        /// </summary>
        public AllTradeFlags flags { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public double price { get; set; }
        /// <summary>
        /// Количество бумаг в последней сделке в лотах
        /// </summary>
        public long qty { get; set; }
        /// <summary>
        /// Объем в денежных средствах
        /// </summary>
        public double value { get; set; }
        /// <summary>
        /// Накопленный купонный доход
        /// </summary>
        public double accruedint { get; set; }
        /// <summary>
        /// Доходность
        /// </summary>
        public double yield { get; set; }
        /// <summary>
        /// Код расчетов
        /// </summary>
        public string settlecode { get; set; }
        /// <summary>
        /// Ставка РЕПО (%)
        /// </summary>
        public double reporate { get; set; }
        /// <summary>
        /// Сумма РЕПО
        /// </summary>
        public double repovalue { get; set; }
        /// <summary>
        /// Объем выкупа РЕПО
        /// </summary>
        public double repo2value { get; set; }
        /// <summary>
        /// Срок РЕПО в днях
        /// </summary>
        public double repoterm { get; set; }
        /// <summary>
        /// Код бумаги заявки
        /// </summary>
        public string sec_code { get; set; }
        /// <summary>
        /// Код класса
        /// </summary>
        public string class_code { get; set; }
        /// <summary>
        /// Дата и время 
        /// </summary>
        public QuikDateTime datetime { get; set; }
        /// <summary>
        /// Период торговой сессии. Возможные значения:
        /// «0» – Открытие; 
        /// «1» – Нормальный; 
        /// «2» – Закрытие 
        /// </summary>
        public int period { get; set; }

    }
}

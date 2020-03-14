using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuikSharpDemo
{
    public class Quote
    {
        #region Свойства
        /// <summary>
        /// Тип котировки (offer/bid)
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Индекс записи с таблице
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// Количество
        /// </summary>
        public int Qty { get; set; }
        /// <summary>
        /// Цена
        /// </summary>
        public double Price { get; set; }
        #endregion

        public Quote()
        {
            Type    = "";
            Index   = 0;
            Qty     = 0;
            Price   = 0;
        }
        public Quote(string _type, int _index, int _qty, double _price)
        {
            Type    = _type;
            Index   = _index;
            Qty     = _qty;
            Price   = _price;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuikSharp.DataStructures;

namespace QuikSharpDemo
{
    public partial class FormOrderBook : Form
    {
        List<Quote> listQuotes;
        public FormOrderBook()
        {
            InitializeComponent();
            dataGridView_OrderBook.AutoGenerateColumns = true;
        }
        public void Renew(OrderBook _quote)
        {
            listQuotes = new List<Quote>();

            if (_quote.offer.Count() > 0)
            {
                for (int y = _quote.offer.Count() - 1; y >= 0; y--)
                {
                    listQuotes.Add(new Quote("offer", y, Convert.ToInt32(_quote.offer[y].quantity), _quote.offer[y].price));
                }
            }
            if (_quote.bid.Count() > 0)
            {
                for (int y = _quote.bid.Count() - 1; y >= 0; y--)
                {
                    listQuotes.Add(new Quote("bid", y, Convert.ToInt32(_quote.bid[y].quantity), _quote.bid[y].price));
                }
            }
            this.Text = "Стакан на: " + _quote.server_time;
            dataGridView_OrderBook.DataSource = listQuotes;
        }
    }
}

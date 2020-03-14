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
        public FormOrderBook()
        {
            InitializeComponent();
        }
        public void Renew(OrderBook _quote)
        {
            List<Quote> listQuotes = new List<Quote>();

            InitializeComponent();
            if (_quote.offer.Count() > 0)
            {
                for (int y = _quote.offer.Count() - 1; y >= 0; y--)
                {
                    listQuotes.Add(new Quote("offer", y, Convert.ToInt32(_quote.offer[y].quantity), _quote.offer[y].price));
                    //Console.WriteLine("OrderBook (offer): count - " + _quote.offer_count + ", idx - " + y + ", price - " + _quote.offer[y].price + ", qty - " + _quote.offer[y].quantity);
                    //dataGridView_OrderBook.Rows.Add();
                    //int i = dataGridView_OrderBook.Rows.Count - 1;
                    //dataGridView_OrderBook.Rows[i].Cells["Type"].Value  = "Offer";
                    //dataGridView_OrderBook.Rows[i].Cells["Index"].Value = y.ToString();
                    //dataGridView_OrderBook.Rows[i].Cells["Price"].Value = _quote.offer[y].price.ToString();
                    //dataGridView_OrderBook.Rows[i].Cells["Qty"].Value   = _quote.offer[y].quantity.ToString();
                }
            }
            if (_quote.bid.Count() > 0)
            {
                //for (int y = 0; y < _quote.bid.Count(); y++)
                for (int y = _quote.bid.Count() - 1; y >= 0; y--)
                {
                    listQuotes.Add(new Quote("bid", y, Convert.ToInt32(_quote.bid[y].quantity), _quote.bid[y].price));
                    //Console.WriteLine("OrderBook (bid): count - " + _quote.bid_count + ", idx - " + y + ", price - " + _quote.bid[y].price + ", qty - " + _quote.bid[y].quantity);
                    //dataGridView_OrderBook.Rows.Add();
                    //int i = dataGridView_OrderBook.Rows.Count - 1;
                    //dataGridView_OrderBook.Rows[i].Cells["Type"].Value  = "Bid";
                    //dataGridView_OrderBook.Rows[i].Cells["Index"].Value = y.ToString();
                    //dataGridView_OrderBook.Rows[i].Cells["Price"].Value = _quote.bid[y].price.ToString();
                    //dataGridView_OrderBook.Rows[i].Cells["Qty"].Value   = _quote.bid[y].quantity.ToString();
                }
            }
            //foreach (Quote _q in listQuotes)
            //{
            //    Console.WriteLine("Price = " + _q.Price);
            //}
            this.Text = "Стакан на: " + _quote.server_time;
            dataGridView_OrderBook.DataSource = listQuotes;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace QuikSharpDemo
{
    public partial class FormOutputTable : Form
    {
        public FormOutputTable()
        {
            InitializeComponent();
        }

        public FormOutputTable(List<Candle> _candles)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _candles;
        }

        public FormOutputTable(List<SecurityInfo> _secInfo)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _secInfo;
        }

        public FormOutputTable(List<FuturesLimits> _futuresLimits)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _futuresLimits;
        }

        public FormOutputTable(List<DepoLimit> _depoLimits)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _depoLimits;
        }

        public FormOutputTable(List<DepoLimitEx> _limits)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _limits;
        }

        public FormOutputTable(List<Order> _orders)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _orders;
        }

        public FormOutputTable(List<Trade> _trades)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _trades;
        }

        public FormOutputTable(List<PortfolioInfo> _portfolio)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _portfolio;
        }

        public FormOutputTable(List<PortfolioInfoEx> _portfolio)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _portfolio;
        }

        public FormOutputTable(List<MoneyLimit> _moneyLimit)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _moneyLimit;
        }

        public FormOutputTable(List<MoneyLimitEx> _moneyLimit)
        {
            InitializeComponent();
            dataGridViewCandles.DataSource = _moneyLimit;
        }
    }
}

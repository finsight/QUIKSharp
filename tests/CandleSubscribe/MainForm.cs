using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandleSubscribe
{
    public partial class MainForm : Form
    {
        private readonly QuikSharp.Quik connector = new QuikSharp.Quik();
        private bool is_started = false;

        public MainForm()
        {
            InitializeComponent();
            foreach (var item in Enum.GetValues(typeof(CandleInterval))) IntervalComboBox.Items.Add(item);
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            if (is_started)
            {
                is_started = false;
                StartButton.Text = "Start";

                await connector.Candles.Unsubscribe(ClassCodeTextBox.Text, SecCodeTextBox.Text, (CandleInterval)IntervalComboBox.SelectedItem);
                connector.Candles.NewCandle -= Candles_NewCandle;
                connector.Candles.UpdateCandle -= Candles_UpdateCandle;
            }
            else
            {
                is_started = true;
                StartButton.Text = "Stop";

                await connector.Candles.Subscribe(ClassCodeTextBox.Text, SecCodeTextBox.Text, (CandleInterval)IntervalComboBox.SelectedItem, OnlyNewCheckbox.Checked);
                connector.Candles.NewCandle += Candles_NewCandle;
                connector.Candles.UpdateCandle += Candles_UpdateCandle;
            }
        }

        private void Candles_NewCandle(Candle candle)
        {
            if(InvokeRequired)
            {
                Invoke(new Action(()=>
                {
                    LogTextBox.AppendText("new_candle\n");
                }));
            }
            else
            {
                LogTextBox.AppendText("new_candle\n");
            }
        }

        private void Candles_UpdateCandle(Candle candle)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() =>
                {
                    LogTextBox.AppendText("update_candle" + " / " + candle.index + "\n");
                }));
            }
            else
            {
                LogTextBox.AppendText("update_candle\n");
            }
        }
    }
}

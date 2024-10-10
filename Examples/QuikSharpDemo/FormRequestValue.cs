using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuikSharpDemo
{
    public partial class FormRequestValue : Form
    {
        public String RequestedValue { get; set; }

        public FormRequestValue()
        {
            InitializeComponent();
        }

        private void Button_OK_Click(object sender, EventArgs e)
        {
            RequestedValue = textBox_Value.Text;
            this.Close();
        }

        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            RequestedValue = "";
            this.Close();
        }
    }
}

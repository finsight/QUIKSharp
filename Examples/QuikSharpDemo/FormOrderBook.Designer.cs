namespace QuikSharpDemo
{
    partial class FormOrderBook
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView_OrderBook = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OrderBook)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView_OrderBook
            // 
            this.dataGridView_OrderBook.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_OrderBook.Location = new System.Drawing.Point(13, 13);
            this.dataGridView_OrderBook.Name = "dataGridView_OrderBook";
            this.dataGridView_OrderBook.RowHeadersVisible = false;
            this.dataGridView_OrderBook.Size = new System.Drawing.Size(520, 559);
            this.dataGridView_OrderBook.TabIndex = 0;
            // 
            // FormOrderBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 584);
            this.Controls.Add(this.dataGridView_OrderBook);
            this.Name = "FormOrderBook";
            this.Text = "FormOrderBook";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_OrderBook)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView_OrderBook;
    }
}
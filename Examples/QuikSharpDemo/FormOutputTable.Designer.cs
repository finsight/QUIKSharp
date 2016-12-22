namespace QuikSharpDemo
{
    partial class FormOutputTable
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
            this.dataGridViewCandles = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCandles)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewCandles
            // 
            this.dataGridViewCandles.AllowUserToAddRows = false;
            this.dataGridViewCandles.AllowUserToDeleteRows = false;
            this.dataGridViewCandles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCandles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCandles.Location = new System.Drawing.Point(13, 13);
            this.dataGridViewCandles.Name = "dataGridViewCandles";
            this.dataGridViewCandles.ReadOnly = true;
            this.dataGridViewCandles.Size = new System.Drawing.Size(644, 581);
            this.dataGridViewCandles.TabIndex = 0;
            // 
            // FormOutputTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 606);
            this.Controls.Add(this.dataGridViewCandles);
            this.Name = "FormOutputTable";
            this.Text = "FormCandles";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCandles)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewCandles;
    }
}
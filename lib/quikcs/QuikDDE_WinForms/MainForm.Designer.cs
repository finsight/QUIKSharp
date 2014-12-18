namespace QuikDDE_WinForms
{
  partial class MainForm
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
      if(disposing && (components != null))
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
      this.tradesBox = new System.Windows.Forms.TextBox();
      this.quotesBox = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.tradesConv = new System.Windows.Forms.Label();
      this.quotesConv = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // tradesBox
      // 
      this.tradesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.tradesBox.BackColor = System.Drawing.Color.White;
      this.tradesBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.tradesBox.Location = new System.Drawing.Point(12, 25);
      this.tradesBox.Multiline = true;
      this.tradesBox.Name = "tradesBox";
      this.tradesBox.ReadOnly = true;
      this.tradesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.tradesBox.Size = new System.Drawing.Size(674, 576);
      this.tradesBox.TabIndex = 0;
      // 
      // quotesBox
      // 
      this.quotesBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.quotesBox.BackColor = System.Drawing.Color.White;
      this.quotesBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.quotesBox.Location = new System.Drawing.Point(692, 25);
      this.quotesBox.Multiline = true;
      this.quotesBox.Name = "quotesBox";
      this.quotesBox.ReadOnly = true;
      this.quotesBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.quotesBox.Size = new System.Drawing.Size(288, 576);
      this.quotesBox.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(43, 13);
      this.label1.TabIndex = 2;
      this.label1.Text = "Trades:";
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(689, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(44, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Quotes:";
      // 
      // tradesConv
      // 
      this.tradesConv.AutoSize = true;
      this.tradesConv.Location = new System.Drawing.Point(58, 9);
      this.tradesConv.Name = "tradesConv";
      this.tradesConv.Size = new System.Drawing.Size(13, 13);
      this.tradesConv.TabIndex = 2;
      this.tradesConv.Text = "0";
      // 
      // quotesConv
      // 
      this.quotesConv.AutoSize = true;
      this.quotesConv.Location = new System.Drawing.Point(739, 9);
      this.quotesConv.Name = "quotesConv";
      this.quotesConv.Size = new System.Drawing.Size(13, 13);
      this.quotesConv.TabIndex = 2;
      this.quotesConv.Text = "0";
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(992, 613);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.quotesConv);
      this.Controls.Add(this.tradesConv);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.quotesBox);
      this.Controls.Add(this.tradesBox);
      this.Name = "MainForm";
      this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
      this.Text = "QuikDDE";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox tradesBox;
    private System.Windows.Forms.TextBox quotesBox;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label tradesConv;
    private System.Windows.Forms.Label quotesConv;
  }
}


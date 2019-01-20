namespace CandleSubscribe
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.ClassCodeTextBox = new System.Windows.Forms.TextBox();
            this.SecCodeTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.IntervalComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.OnlyNewCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Class code:";
            // 
            // ClassCodeTextBox
            // 
            this.ClassCodeTextBox.Location = new System.Drawing.Point(77, 31);
            this.ClassCodeTextBox.Name = "ClassCodeTextBox";
            this.ClassCodeTextBox.Size = new System.Drawing.Size(100, 20);
            this.ClassCodeTextBox.TabIndex = 1;
            this.ClassCodeTextBox.Text = "QJSIM";
            // 
            // SecCodeTextBox
            // 
            this.SecCodeTextBox.Location = new System.Drawing.Point(259, 31);
            this.SecCodeTextBox.Name = "SecCodeTextBox";
            this.SecCodeTextBox.Size = new System.Drawing.Size(100, 20);
            this.SecCodeTextBox.TabIndex = 3;
            this.SecCodeTextBox.Text = "SBER";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(194, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Sec code:";
            // 
            // IntervalComboBox
            // 
            this.IntervalComboBox.FormattingEnabled = true;
            this.IntervalComboBox.Location = new System.Drawing.Point(436, 30);
            this.IntervalComboBox.Name = "IntervalComboBox";
            this.IntervalComboBox.Size = new System.Drawing.Size(65, 21);
            this.IntervalComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(385, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Interval:";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(15, 69);
            this.LogTextBox.Multiline = true;
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(773, 369);
            this.LogTextBox.TabIndex = 6;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(713, 30);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 7;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // OnlyNewCheckbox
            // 
            this.OnlyNewCheckbox.AutoSize = true;
            this.OnlyNewCheckbox.Location = new System.Drawing.Point(528, 33);
            this.OnlyNewCheckbox.Name = "OnlyNewCheckbox";
            this.OnlyNewCheckbox.Size = new System.Drawing.Size(70, 17);
            this.OnlyNewCheckbox.TabIndex = 10;
            this.OnlyNewCheckbox.Text = "Only new";
            this.OnlyNewCheckbox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.OnlyNewCheckbox);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.LogTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.IntervalComboBox);
            this.Controls.Add(this.SecCodeTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ClassCodeTextBox);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Test candle subscribe";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ClassCodeTextBox;
        private System.Windows.Forms.TextBox SecCodeTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox IntervalComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LogTextBox;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox OnlyNewCheckbox;
    }
}


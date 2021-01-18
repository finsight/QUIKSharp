namespace RobotDemo
{
    partial class RobotDemo
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.groupBoxRobotSettings = new System.Windows.Forms.GroupBox();
            this.comboBoxTF = new System.Windows.Forms.ComboBox();
            this.comboBoxMode = new System.Windows.Forms.ComboBox();
            this.labelMode = new System.Windows.Forms.Label();
            this.labelTF = new System.Windows.Forms.Label();
            this.labelPeriod = new System.Windows.Forms.Label();
            this.labelQty = new System.Windows.Forms.Label();
            this.labelFormula = new System.Windows.Forms.Label();
            this.labelSlip = new System.Windows.Forms.Label();
            this.labelTiker = new System.Windows.Forms.Label();
            this.textBoxPeriod = new System.Windows.Forms.TextBox();
            this.textBoxQty = new System.Windows.Forms.TextBox();
            this.textBoxSlip = new System.Windows.Forms.TextBox();
            this.textBoxTiker = new System.Windows.Forms.TextBox();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.groupBoxToolParam = new System.Windows.Forms.GroupBox();
            this.textBoxLastPrice = new System.Windows.Forms.TextBox();
            this.labelLastPrice = new System.Windows.Forms.Label();
            this.textBoxLot = new System.Windows.Forms.TextBox();
            this.labelLot = new System.Windows.Forms.Label();
            this.textBoxStep = new System.Windows.Forms.TextBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.textBoxClassCode = new System.Windows.Forms.TextBox();
            this.labelClassCode = new System.Windows.Forms.Label();
            this.textBoxToolName = new System.Windows.Forms.TextBox();
            this.labelToolName = new System.Windows.Forms.Label();
            this.groupBoxPosition = new System.Windows.Forms.GroupBox();
            this.textBoxPositionProfit = new System.Windows.Forms.TextBox();
            this.labelPositionProfit = new System.Windows.Forms.Label();
            this.textBoxPositionSL = new System.Windows.Forms.TextBox();
            this.labelSL = new System.Windows.Forms.Label();
            this.textBoxPositionPE = new System.Windows.Forms.TextBox();
            this.labelPositionPE = new System.Windows.Forms.Label();
            this.buttonPositionReset = new System.Windows.Forms.Button();
            this.textBoxPositionQty = new System.Windows.Forms.TextBox();
            this.labelPositionQty = new System.Windows.Forms.Label();
            this.textBoxPositionDirection = new System.Windows.Forms.TextBox();
            this.labelDirection = new System.Windows.Forms.Label();
            this.groupBoxActiveOrder = new System.Windows.Forms.GroupBox();
            this.textBoxOrderDirection = new System.Windows.Forms.TextBox();
            this.labelOrderDirection = new System.Windows.Forms.Label();
            this.textBoxOrderNumber = new System.Windows.Forms.TextBox();
            this.labelOrderNumber = new System.Windows.Forms.Label();
            this.textBoxOrderPrice = new System.Windows.Forms.TextBox();
            this.labelOrderPrice = new System.Windows.Forms.Label();
            this.textBoxOrderBalance = new System.Windows.Forms.TextBox();
            this.labelOrderBalance = new System.Windows.Forms.Label();
            this.textBoxOrderQty = new System.Windows.Forms.TextBox();
            this.labelOrderQty = new System.Windows.Forms.Label();
            this.groupBoxLogs = new System.Windows.Forms.GroupBox();
            this.textBoxLogs = new System.Windows.Forms.TextBox();
            this.groupBoxDeals = new System.Windows.Forms.GroupBox();
            this.dataGridViewDeals = new System.Windows.Forms.DataGridView();
            this.ToolName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeEntr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceEntr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeExit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriceExit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Profit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SummProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timerRun = new System.Windows.Forms.Timer(this.components);
            this.groupBoxRobotSettings.SuspendLayout();
            this.groupBoxToolParam.SuspendLayout();
            this.groupBoxPosition.SuspendLayout();
            this.groupBoxActiveOrder.SuspendLayout();
            this.groupBoxLogs.SuspendLayout();
            this.groupBoxDeals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeals)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonConnect.Location = new System.Drawing.Point(13, 13);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(629, 23);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // groupBoxRobotSettings
            // 
            this.groupBoxRobotSettings.Controls.Add(this.comboBoxTF);
            this.groupBoxRobotSettings.Controls.Add(this.comboBoxMode);
            this.groupBoxRobotSettings.Controls.Add(this.labelMode);
            this.groupBoxRobotSettings.Controls.Add(this.labelTF);
            this.groupBoxRobotSettings.Controls.Add(this.labelPeriod);
            this.groupBoxRobotSettings.Controls.Add(this.labelQty);
            this.groupBoxRobotSettings.Controls.Add(this.labelFormula);
            this.groupBoxRobotSettings.Controls.Add(this.labelSlip);
            this.groupBoxRobotSettings.Controls.Add(this.labelTiker);
            this.groupBoxRobotSettings.Controls.Add(this.textBoxPeriod);
            this.groupBoxRobotSettings.Controls.Add(this.textBoxQty);
            this.groupBoxRobotSettings.Controls.Add(this.textBoxSlip);
            this.groupBoxRobotSettings.Controls.Add(this.textBoxTiker);
            this.groupBoxRobotSettings.Location = new System.Drawing.Point(13, 43);
            this.groupBoxRobotSettings.Name = "groupBoxRobotSettings";
            this.groupBoxRobotSettings.Size = new System.Drawing.Size(628, 74);
            this.groupBoxRobotSettings.TabIndex = 1;
            this.groupBoxRobotSettings.TabStop = false;
            this.groupBoxRobotSettings.Text = "Настройки робота";
            // 
            // comboBoxTF
            // 
            this.comboBoxTF.FormattingEnabled = true;
            this.comboBoxTF.Items.AddRange(new object[] {
            "Виртуальный",
            "Боевой"});
            this.comboBoxTF.Location = new System.Drawing.Point(92, 45);
            this.comboBoxTF.Name = "comboBoxTF";
            this.comboBoxTF.Size = new System.Drawing.Size(55, 21);
            this.comboBoxTF.TabIndex = 2;
            // 
            // comboBoxMode
            // 
            this.comboBoxMode.FormattingEnabled = true;
            this.comboBoxMode.Items.AddRange(new object[] {
            "Виртуальный",
            "Боевой"});
            this.comboBoxMode.Location = new System.Drawing.Point(188, 17);
            this.comboBoxMode.Name = "comboBoxMode";
            this.comboBoxMode.Size = new System.Drawing.Size(101, 21);
            this.comboBoxMode.TabIndex = 2;
            // 
            // labelMode
            // 
            this.labelMode.AutoSize = true;
            this.labelMode.Location = new System.Drawing.Point(141, 20);
            this.labelMode.Name = "labelMode";
            this.labelMode.Size = new System.Drawing.Size(45, 13);
            this.labelMode.TabIndex = 1;
            this.labelMode.Text = "Режим:";
            // 
            // labelTF
            // 
            this.labelTF.AutoSize = true;
            this.labelTF.Location = new System.Drawing.Point(12, 48);
            this.labelTF.Name = "labelTF";
            this.labelTF.Size = new System.Drawing.Size(74, 13);
            this.labelTF.TabIndex = 1;
            this.labelTF.Text = "Тайм-фрейм:";
            // 
            // labelPeriod
            // 
            this.labelPeriod.AutoSize = true;
            this.labelPeriod.Location = new System.Drawing.Point(469, 48);
            this.labelPeriod.Name = "labelPeriod";
            this.labelPeriod.Size = new System.Drawing.Size(108, 13);
            this.labelPeriod.TabIndex = 1;
            this.labelPeriod.Text = "Период PriceCannel:";
            // 
            // labelQty
            // 
            this.labelQty.AutoSize = true;
            this.labelQty.Location = new System.Drawing.Point(390, 20);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(187, 13);
            this.labelQty.TabIndex = 1;
            this.labelQty.Text = "Количество торгуемых контрактов:";
            // 
            // labelFormula
            // 
            this.labelFormula.AutoSize = true;
            this.labelFormula.Location = new System.Drawing.Point(350, 48);
            this.labelFormula.Name = "labelFormula";
            this.labelFormula.Size = new System.Drawing.Size(64, 13);
            this.labelFormula.TabIndex = 1;
            this.labelFormula.Text = "x Шаг цены";
            // 
            // labelSlip
            // 
            this.labelSlip.AutoSize = true;
            this.labelSlip.Location = new System.Drawing.Point(209, 48);
            this.labelSlip.Name = "labelSlip";
            this.labelSlip.Size = new System.Drawing.Size(113, 13);
            this.labelSlip.TabIndex = 1;
            this.labelSlip.Text = "Проскальзывание = ";
            // 
            // labelTiker
            // 
            this.labelTiker.AutoSize = true;
            this.labelTiker.Location = new System.Drawing.Point(7, 20);
            this.labelTiker.Name = "labelTiker";
            this.labelTiker.Size = new System.Drawing.Size(41, 13);
            this.labelTiker.TabIndex = 1;
            this.labelTiker.Text = "Тикер:";
            // 
            // textBoxPeriod
            // 
            this.textBoxPeriod.Location = new System.Drawing.Point(583, 45);
            this.textBoxPeriod.Name = "textBoxPeriod";
            this.textBoxPeriod.Size = new System.Drawing.Size(34, 20);
            this.textBoxPeriod.TabIndex = 0;
            this.textBoxPeriod.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxQty
            // 
            this.textBoxQty.Location = new System.Drawing.Point(583, 17);
            this.textBoxQty.Name = "textBoxQty";
            this.textBoxQty.ReadOnly = true;
            this.textBoxQty.Size = new System.Drawing.Size(34, 20);
            this.textBoxQty.TabIndex = 0;
            this.textBoxQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxSlip
            // 
            this.textBoxSlip.Location = new System.Drawing.Point(323, 45);
            this.textBoxSlip.Name = "textBoxSlip";
            this.textBoxSlip.Size = new System.Drawing.Size(24, 20);
            this.textBoxSlip.TabIndex = 0;
            this.textBoxSlip.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBoxTiker
            // 
            this.textBoxTiker.Location = new System.Drawing.Point(54, 17);
            this.textBoxTiker.Name = "textBoxTiker";
            this.textBoxTiker.Size = new System.Drawing.Size(78, 20);
            this.textBoxTiker.TabIndex = 0;
            this.textBoxTiker.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonStartStop.Location = new System.Drawing.Point(13, 123);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(629, 23);
            this.buttonStartStop.TabIndex = 0;
            this.buttonStartStop.Text = "СТАРТ";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new System.EventHandler(this.ButtonStartStop_Click);
            // 
            // groupBoxToolParam
            // 
            this.groupBoxToolParam.Controls.Add(this.textBoxLastPrice);
            this.groupBoxToolParam.Controls.Add(this.labelLastPrice);
            this.groupBoxToolParam.Controls.Add(this.textBoxLot);
            this.groupBoxToolParam.Controls.Add(this.labelLot);
            this.groupBoxToolParam.Controls.Add(this.textBoxStep);
            this.groupBoxToolParam.Controls.Add(this.labelStep);
            this.groupBoxToolParam.Controls.Add(this.textBoxClassCode);
            this.groupBoxToolParam.Controls.Add(this.labelClassCode);
            this.groupBoxToolParam.Controls.Add(this.textBoxToolName);
            this.groupBoxToolParam.Controls.Add(this.labelToolName);
            this.groupBoxToolParam.Location = new System.Drawing.Point(13, 152);
            this.groupBoxToolParam.Name = "groupBoxToolParam";
            this.groupBoxToolParam.Size = new System.Drawing.Size(629, 73);
            this.groupBoxToolParam.TabIndex = 1;
            this.groupBoxToolParam.TabStop = false;
            this.groupBoxToolParam.Text = "Параметры инструмента";
            // 
            // textBoxLastPrice
            // 
            this.textBoxLastPrice.Location = new System.Drawing.Point(105, 44);
            this.textBoxLastPrice.Name = "textBoxLastPrice";
            this.textBoxLastPrice.ReadOnly = true;
            this.textBoxLastPrice.Size = new System.Drawing.Size(83, 20);
            this.textBoxLastPrice.TabIndex = 0;
            this.textBoxLastPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelLastPrice
            // 
            this.labelLastPrice.AutoSize = true;
            this.labelLastPrice.Location = new System.Drawing.Point(6, 47);
            this.labelLastPrice.Name = "labelLastPrice";
            this.labelLastPrice.Size = new System.Drawing.Size(93, 13);
            this.labelLastPrice.TabIndex = 1;
            this.labelLastPrice.Text = "Последняя цена:";
            // 
            // textBoxLot
            // 
            this.textBoxLot.Location = new System.Drawing.Point(512, 19);
            this.textBoxLot.Name = "textBoxLot";
            this.textBoxLot.ReadOnly = true;
            this.textBoxLot.Size = new System.Drawing.Size(69, 20);
            this.textBoxLot.TabIndex = 0;
            this.textBoxLot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelLot
            // 
            this.labelLot.AutoSize = true;
            this.labelLot.Location = new System.Drawing.Point(477, 22);
            this.labelLot.Name = "labelLot";
            this.labelLot.Size = new System.Drawing.Size(29, 13);
            this.labelLot.TabIndex = 1;
            this.labelLot.Text = "Лот:";
            // 
            // textBoxStep
            // 
            this.textBoxStep.Location = new System.Drawing.Point(409, 19);
            this.textBoxStep.Name = "textBoxStep";
            this.textBoxStep.ReadOnly = true;
            this.textBoxStep.Size = new System.Drawing.Size(62, 20);
            this.textBoxStep.TabIndex = 0;
            this.textBoxStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(335, 22);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(59, 13);
            this.labelStep.TabIndex = 1;
            this.labelStep.Text = "Шаг цены:";
            // 
            // textBoxClassCode
            // 
            this.textBoxClassCode.Location = new System.Drawing.Point(272, 19);
            this.textBoxClassCode.Name = "textBoxClassCode";
            this.textBoxClassCode.ReadOnly = true;
            this.textBoxClassCode.Size = new System.Drawing.Size(53, 20);
            this.textBoxClassCode.TabIndex = 0;
            this.textBoxClassCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelClassCode
            // 
            this.labelClassCode.AutoSize = true;
            this.labelClassCode.Location = new System.Drawing.Point(198, 22);
            this.labelClassCode.Name = "labelClassCode";
            this.labelClassCode.Size = new System.Drawing.Size(68, 13);
            this.labelClassCode.TabIndex = 1;
            this.labelClassCode.Text = "Код класса:";
            // 
            // textBoxToolName
            // 
            this.textBoxToolName.Location = new System.Drawing.Point(99, 19);
            this.textBoxToolName.Name = "textBoxToolName";
            this.textBoxToolName.ReadOnly = true;
            this.textBoxToolName.Size = new System.Drawing.Size(89, 20);
            this.textBoxToolName.TabIndex = 0;
            this.textBoxToolName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelToolName
            // 
            this.labelToolName.AutoSize = true;
            this.labelToolName.Location = new System.Drawing.Point(7, 22);
            this.labelToolName.Name = "labelToolName";
            this.labelToolName.Size = new System.Drawing.Size(86, 13);
            this.labelToolName.TabIndex = 1;
            this.labelToolName.Text = "Наименование:";
            // 
            // groupBoxPosition
            // 
            this.groupBoxPosition.Controls.Add(this.textBoxPositionProfit);
            this.groupBoxPosition.Controls.Add(this.labelPositionProfit);
            this.groupBoxPosition.Controls.Add(this.textBoxPositionSL);
            this.groupBoxPosition.Controls.Add(this.labelSL);
            this.groupBoxPosition.Controls.Add(this.textBoxPositionPE);
            this.groupBoxPosition.Controls.Add(this.labelPositionPE);
            this.groupBoxPosition.Controls.Add(this.buttonPositionReset);
            this.groupBoxPosition.Controls.Add(this.textBoxPositionQty);
            this.groupBoxPosition.Controls.Add(this.labelPositionQty);
            this.groupBoxPosition.Controls.Add(this.textBoxPositionDirection);
            this.groupBoxPosition.Controls.Add(this.labelDirection);
            this.groupBoxPosition.Location = new System.Drawing.Point(13, 231);
            this.groupBoxPosition.Name = "groupBoxPosition";
            this.groupBoxPosition.Size = new System.Drawing.Size(313, 105);
            this.groupBoxPosition.TabIndex = 1;
            this.groupBoxPosition.TabStop = false;
            this.groupBoxPosition.Text = "Позиция";
            // 
            // textBoxPositionProfit
            // 
            this.textBoxPositionProfit.Location = new System.Drawing.Point(92, 71);
            this.textBoxPositionProfit.Name = "textBoxPositionProfit";
            this.textBoxPositionProfit.ReadOnly = true;
            this.textBoxPositionProfit.Size = new System.Drawing.Size(63, 20);
            this.textBoxPositionProfit.TabIndex = 0;
            this.textBoxPositionProfit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelPositionProfit
            // 
            this.labelPositionProfit.AutoSize = true;
            this.labelPositionProfit.Location = new System.Drawing.Point(8, 74);
            this.labelPositionProfit.Name = "labelPositionProfit";
            this.labelPositionProfit.Size = new System.Drawing.Size(72, 13);
            this.labelPositionProfit.TabIndex = 1;
            this.labelPositionProfit.Text = "Тек. профит:";
            // 
            // textBoxPositionSL
            // 
            this.textBoxPositionSL.Location = new System.Drawing.Point(244, 45);
            this.textBoxPositionSL.Name = "textBoxPositionSL";
            this.textBoxPositionSL.ReadOnly = true;
            this.textBoxPositionSL.Size = new System.Drawing.Size(63, 20);
            this.textBoxPositionSL.TabIndex = 0;
            this.textBoxPositionSL.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelSL
            // 
            this.labelSL.AutoSize = true;
            this.labelSL.Location = new System.Drawing.Point(160, 48);
            this.labelSL.Name = "labelSL";
            this.labelSL.Size = new System.Drawing.Size(61, 13);
            this.labelSL.TabIndex = 1;
            this.labelSL.Text = "Стоп-цена:";
            // 
            // textBoxPositionPE
            // 
            this.textBoxPositionPE.Location = new System.Drawing.Point(92, 45);
            this.textBoxPositionPE.Name = "textBoxPositionPE";
            this.textBoxPositionPE.ReadOnly = true;
            this.textBoxPositionPE.Size = new System.Drawing.Size(63, 20);
            this.textBoxPositionPE.TabIndex = 0;
            this.textBoxPositionPE.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelPositionPE
            // 
            this.labelPositionPE.AutoSize = true;
            this.labelPositionPE.Location = new System.Drawing.Point(8, 48);
            this.labelPositionPE.Name = "labelPositionPE";
            this.labelPositionPE.Size = new System.Drawing.Size(68, 13);
            this.labelPositionPE.TabIndex = 1;
            this.labelPositionPE.Text = "Цена входа:";
            // 
            // buttonPositionReset
            // 
            this.buttonPositionReset.ForeColor = System.Drawing.Color.DarkRed;
            this.buttonPositionReset.Location = new System.Drawing.Point(163, 71);
            this.buttonPositionReset.Name = "buttonPositionReset";
            this.buttonPositionReset.Size = new System.Drawing.Size(144, 23);
            this.buttonPositionReset.TabIndex = 0;
            this.buttonPositionReset.Text = "Закрыть позицию";
            this.buttonPositionReset.UseVisualStyleBackColor = true;
            this.buttonPositionReset.Click += new System.EventHandler(this.ButtonPositionReset_Click);
            // 
            // textBoxPositionQty
            // 
            this.textBoxPositionQty.Location = new System.Drawing.Point(244, 19);
            this.textBoxPositionQty.Name = "textBoxPositionQty";
            this.textBoxPositionQty.ReadOnly = true;
            this.textBoxPositionQty.Size = new System.Drawing.Size(63, 20);
            this.textBoxPositionQty.TabIndex = 0;
            this.textBoxPositionQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelPositionQty
            // 
            this.labelPositionQty.AutoSize = true;
            this.labelPositionQty.Location = new System.Drawing.Point(160, 22);
            this.labelPositionQty.Name = "labelPositionQty";
            this.labelPositionQty.Size = new System.Drawing.Size(69, 13);
            this.labelPositionQty.TabIndex = 1;
            this.labelPositionQty.Text = "Количество:";
            // 
            // textBoxPositionDirection
            // 
            this.textBoxPositionDirection.Location = new System.Drawing.Point(92, 19);
            this.textBoxPositionDirection.Name = "textBoxPositionDirection";
            this.textBoxPositionDirection.ReadOnly = true;
            this.textBoxPositionDirection.Size = new System.Drawing.Size(63, 20);
            this.textBoxPositionDirection.TabIndex = 0;
            this.textBoxPositionDirection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelDirection
            // 
            this.labelDirection.AutoSize = true;
            this.labelDirection.Location = new System.Drawing.Point(8, 22);
            this.labelDirection.Name = "labelDirection";
            this.labelDirection.Size = new System.Drawing.Size(78, 13);
            this.labelDirection.TabIndex = 1;
            this.labelDirection.Text = "Направление:";
            // 
            // groupBoxActiveOrder
            // 
            this.groupBoxActiveOrder.Controls.Add(this.textBoxOrderDirection);
            this.groupBoxActiveOrder.Controls.Add(this.labelOrderDirection);
            this.groupBoxActiveOrder.Controls.Add(this.textBoxOrderNumber);
            this.groupBoxActiveOrder.Controls.Add(this.labelOrderNumber);
            this.groupBoxActiveOrder.Controls.Add(this.textBoxOrderPrice);
            this.groupBoxActiveOrder.Controls.Add(this.labelOrderPrice);
            this.groupBoxActiveOrder.Controls.Add(this.textBoxOrderBalance);
            this.groupBoxActiveOrder.Controls.Add(this.labelOrderBalance);
            this.groupBoxActiveOrder.Controls.Add(this.textBoxOrderQty);
            this.groupBoxActiveOrder.Controls.Add(this.labelOrderQty);
            this.groupBoxActiveOrder.Location = new System.Drawing.Point(332, 231);
            this.groupBoxActiveOrder.Name = "groupBoxActiveOrder";
            this.groupBoxActiveOrder.Size = new System.Drawing.Size(310, 105);
            this.groupBoxActiveOrder.TabIndex = 1;
            this.groupBoxActiveOrder.TabStop = false;
            this.groupBoxActiveOrder.Text = "Активная заявка";
            // 
            // textBoxOrderDirection
            // 
            this.textBoxOrderDirection.Location = new System.Drawing.Point(235, 19);
            this.textBoxOrderDirection.Name = "textBoxOrderDirection";
            this.textBoxOrderDirection.ReadOnly = true;
            this.textBoxOrderDirection.Size = new System.Drawing.Size(63, 20);
            this.textBoxOrderDirection.TabIndex = 0;
            this.textBoxOrderDirection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelOrderDirection
            // 
            this.labelOrderDirection.AutoSize = true;
            this.labelOrderDirection.Location = new System.Drawing.Point(151, 22);
            this.labelOrderDirection.Name = "labelOrderDirection";
            this.labelOrderDirection.Size = new System.Drawing.Size(78, 13);
            this.labelOrderDirection.TabIndex = 1;
            this.labelOrderDirection.Text = "Направление:";
            // 
            // textBoxOrderNumber
            // 
            this.textBoxOrderNumber.Location = new System.Drawing.Point(56, 19);
            this.textBoxOrderNumber.Name = "textBoxOrderNumber";
            this.textBoxOrderNumber.ReadOnly = true;
            this.textBoxOrderNumber.Size = new System.Drawing.Size(89, 20);
            this.textBoxOrderNumber.TabIndex = 0;
            this.textBoxOrderNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelOrderNumber
            // 
            this.labelOrderNumber.AutoSize = true;
            this.labelOrderNumber.Location = new System.Drawing.Point(6, 22);
            this.labelOrderNumber.Name = "labelOrderNumber";
            this.labelOrderNumber.Size = new System.Drawing.Size(44, 13);
            this.labelOrderNumber.TabIndex = 1;
            this.labelOrderNumber.Text = "Номер:";
            // 
            // textBoxOrderPrice
            // 
            this.textBoxOrderPrice.Location = new System.Drawing.Point(82, 45);
            this.textBoxOrderPrice.Name = "textBoxOrderPrice";
            this.textBoxOrderPrice.ReadOnly = true;
            this.textBoxOrderPrice.Size = new System.Drawing.Size(63, 20);
            this.textBoxOrderPrice.TabIndex = 0;
            this.textBoxOrderPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelOrderPrice
            // 
            this.labelOrderPrice.AutoSize = true;
            this.labelOrderPrice.Location = new System.Drawing.Point(6, 48);
            this.labelOrderPrice.Name = "labelOrderPrice";
            this.labelOrderPrice.Size = new System.Drawing.Size(36, 13);
            this.labelOrderPrice.TabIndex = 1;
            this.labelOrderPrice.Text = "Цена:";
            // 
            // textBoxOrderBalance
            // 
            this.textBoxOrderBalance.Location = new System.Drawing.Point(82, 71);
            this.textBoxOrderBalance.Name = "textBoxOrderBalance";
            this.textBoxOrderBalance.ReadOnly = true;
            this.textBoxOrderBalance.Size = new System.Drawing.Size(63, 20);
            this.textBoxOrderBalance.TabIndex = 0;
            this.textBoxOrderBalance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelOrderBalance
            // 
            this.labelOrderBalance.AutoSize = true;
            this.labelOrderBalance.Location = new System.Drawing.Point(6, 74);
            this.labelOrderBalance.Name = "labelOrderBalance";
            this.labelOrderBalance.Size = new System.Drawing.Size(52, 13);
            this.labelOrderBalance.TabIndex = 1;
            this.labelOrderBalance.Text = "Остаток:";
            // 
            // textBoxOrderQty
            // 
            this.textBoxOrderQty.Location = new System.Drawing.Point(235, 45);
            this.textBoxOrderQty.Name = "textBoxOrderQty";
            this.textBoxOrderQty.ReadOnly = true;
            this.textBoxOrderQty.Size = new System.Drawing.Size(63, 20);
            this.textBoxOrderQty.TabIndex = 0;
            this.textBoxOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.AutoSize = true;
            this.labelOrderQty.Location = new System.Drawing.Point(151, 48);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(69, 13);
            this.labelOrderQty.TabIndex = 1;
            this.labelOrderQty.Text = "Количество:";
            // 
            // groupBoxLogs
            // 
            this.groupBoxLogs.Controls.Add(this.textBoxLogs);
            this.groupBoxLogs.Location = new System.Drawing.Point(13, 342);
            this.groupBoxLogs.Name = "groupBoxLogs";
            this.groupBoxLogs.Size = new System.Drawing.Size(629, 110);
            this.groupBoxLogs.TabIndex = 1;
            this.groupBoxLogs.TabStop = false;
            this.groupBoxLogs.Text = "Логи";
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.ForeColor = System.Drawing.Color.Blue;
            this.textBoxLogs.Location = new System.Drawing.Point(7, 19);
            this.textBoxLogs.Multiline = true;
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.ReadOnly = true;
            this.textBoxLogs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLogs.Size = new System.Drawing.Size(615, 85);
            this.textBoxLogs.TabIndex = 0;
            // 
            // groupBoxDeals
            // 
            this.groupBoxDeals.Controls.Add(this.dataGridViewDeals);
            this.groupBoxDeals.Location = new System.Drawing.Point(13, 458);
            this.groupBoxDeals.Name = "groupBoxDeals";
            this.groupBoxDeals.Size = new System.Drawing.Size(630, 182);
            this.groupBoxDeals.TabIndex = 1;
            this.groupBoxDeals.TabStop = false;
            this.groupBoxDeals.Text = "Сделки";
            // 
            // dataGridViewDeals
            // 
            this.dataGridViewDeals.AllowUserToAddRows = false;
            this.dataGridViewDeals.AllowUserToDeleteRows = false;
            this.dataGridViewDeals.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewDeals.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewDeals.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDeals.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ToolName,
            this.Qty,
            this.TimeEntr,
            this.PriceEntr,
            this.TimeExit,
            this.PriceExit,
            this.Profit,
            this.SummProfit});
            this.dataGridViewDeals.Location = new System.Drawing.Point(7, 20);
            this.dataGridViewDeals.MultiSelect = false;
            this.dataGridViewDeals.Name = "dataGridViewDeals";
            this.dataGridViewDeals.ReadOnly = true;
            this.dataGridViewDeals.RowHeadersVisible = false;
            this.dataGridViewDeals.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewDeals.Size = new System.Drawing.Size(616, 156);
            this.dataGridViewDeals.TabIndex = 0;
            // 
            // ToolName
            // 
            this.ToolName.HeaderText = "Инструмент";
            this.ToolName.Name = "ToolName";
            this.ToolName.ReadOnly = true;
            // 
            // Qty
            // 
            this.Qty.HeaderText = "Кол-во";
            this.Qty.Name = "Qty";
            this.Qty.ReadOnly = true;
            this.Qty.Width = 50;
            // 
            // TimeEntr
            // 
            this.TimeEntr.HeaderText = "Время входа";
            this.TimeEntr.Name = "TimeEntr";
            this.TimeEntr.ReadOnly = true;
            this.TimeEntr.Width = 60;
            // 
            // PriceEntr
            // 
            this.PriceEntr.HeaderText = "Цена входа";
            this.PriceEntr.Name = "PriceEntr";
            this.PriceEntr.ReadOnly = true;
            this.PriceEntr.Width = 80;
            // 
            // TimeExit
            // 
            this.TimeExit.HeaderText = "Время выхода";
            this.TimeExit.Name = "TimeExit";
            this.TimeExit.ReadOnly = true;
            this.TimeExit.Width = 60;
            // 
            // PriceExit
            // 
            this.PriceExit.HeaderText = "Цена выхода";
            this.PriceExit.Name = "PriceExit";
            this.PriceExit.ReadOnly = true;
            this.PriceExit.Width = 80;
            // 
            // Profit
            // 
            this.Profit.HeaderText = "Результат";
            this.Profit.Name = "Profit";
            this.Profit.ReadOnly = true;
            this.Profit.Width = 80;
            // 
            // SummProfit
            // 
            this.SummProfit.HeaderText = "Итого";
            this.SummProfit.Name = "SummProfit";
            this.SummProfit.ReadOnly = true;
            this.SummProfit.Width = 80;
            // 
            // timerRun
            // 
            this.timerRun.Interval = 250;
            this.timerRun.Tick += new System.EventHandler(this.TimerRun_Tick);
            // 
            // RobotDemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 644);
            this.Controls.Add(this.groupBoxActiveOrder);
            this.Controls.Add(this.groupBoxPosition);
            this.Controls.Add(this.groupBoxDeals);
            this.Controls.Add(this.groupBoxLogs);
            this.Controls.Add(this.groupBoxToolParam);
            this.Controls.Add(this.groupBoxRobotSettings);
            this.Controls.Add(this.buttonStartStop);
            this.Controls.Add(this.buttonConnect);
            this.Name = "RobotDemo";
            this.Text = "Демонстрационный робот";
            this.groupBoxRobotSettings.ResumeLayout(false);
            this.groupBoxRobotSettings.PerformLayout();
            this.groupBoxToolParam.ResumeLayout(false);
            this.groupBoxToolParam.PerformLayout();
            this.groupBoxPosition.ResumeLayout(false);
            this.groupBoxPosition.PerformLayout();
            this.groupBoxActiveOrder.ResumeLayout(false);
            this.groupBoxActiveOrder.PerformLayout();
            this.groupBoxLogs.ResumeLayout(false);
            this.groupBoxLogs.PerformLayout();
            this.groupBoxDeals.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDeals)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.GroupBox groupBoxRobotSettings;
        private System.Windows.Forms.ComboBox comboBoxMode;
        private System.Windows.Forms.Label labelMode;
        private System.Windows.Forms.Label labelPeriod;
        private System.Windows.Forms.Label labelQty;
        private System.Windows.Forms.Label labelSlip;
        private System.Windows.Forms.Label labelTiker;
        private System.Windows.Forms.TextBox textBoxPeriod;
        private System.Windows.Forms.TextBox textBoxQty;
        private System.Windows.Forms.TextBox textBoxSlip;
        private System.Windows.Forms.TextBox textBoxTiker;
        private System.Windows.Forms.Button buttonStartStop;
        private System.Windows.Forms.GroupBox groupBoxToolParam;
        private System.Windows.Forms.TextBox textBoxToolName;
        private System.Windows.Forms.Label labelToolName;
        private System.Windows.Forms.GroupBox groupBoxPosition;
        private System.Windows.Forms.GroupBox groupBoxActiveOrder;
        private System.Windows.Forms.GroupBox groupBoxLogs;
        private System.Windows.Forms.TextBox textBoxLogs;
        private System.Windows.Forms.GroupBox groupBoxDeals;
        private System.Windows.Forms.DataGridView dataGridViewDeals;
        private System.Windows.Forms.TextBox textBoxLastPrice;
        private System.Windows.Forms.Label labelLastPrice;
        private System.Windows.Forms.TextBox textBoxLot;
        private System.Windows.Forms.Label labelLot;
        private System.Windows.Forms.TextBox textBoxStep;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.TextBox textBoxClassCode;
        private System.Windows.Forms.Label labelClassCode;
        private System.Windows.Forms.TextBox textBoxPositionProfit;
        private System.Windows.Forms.Label labelPositionProfit;
        private System.Windows.Forms.TextBox textBoxPositionSL;
        private System.Windows.Forms.Label labelSL;
        private System.Windows.Forms.TextBox textBoxPositionPE;
        private System.Windows.Forms.Label labelPositionPE;
        private System.Windows.Forms.Button buttonPositionReset;
        private System.Windows.Forms.TextBox textBoxPositionQty;
        private System.Windows.Forms.Label labelPositionQty;
        private System.Windows.Forms.TextBox textBoxPositionDirection;
        private System.Windows.Forms.Label labelDirection;
        private System.Windows.Forms.TextBox textBoxOrderDirection;
        private System.Windows.Forms.Label labelOrderDirection;
        private System.Windows.Forms.TextBox textBoxOrderNumber;
        private System.Windows.Forms.Label labelOrderNumber;
        private System.Windows.Forms.TextBox textBoxOrderPrice;
        private System.Windows.Forms.Label labelOrderPrice;
        private System.Windows.Forms.TextBox textBoxOrderBalance;
        private System.Windows.Forms.Label labelOrderBalance;
        private System.Windows.Forms.TextBox textBoxOrderQty;
        private System.Windows.Forms.Label labelOrderQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToolName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeEntr;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceEntr;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriceExit;
        private System.Windows.Forms.DataGridViewTextBoxColumn Profit;
        private System.Windows.Forms.DataGridViewTextBoxColumn SummProfit;
        private System.Windows.Forms.ComboBox comboBoxTF;
        private System.Windows.Forms.Label labelTF;
        private System.Windows.Forms.Label labelFormula;
        private System.Windows.Forms.Timer timerRun;
    }
}


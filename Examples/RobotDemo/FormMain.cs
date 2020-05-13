using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using QuikSharp;
using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;

namespace RobotDemo
{
    public partial class RobotDemo   : Form
    {
        Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        public static Quik _quik;
        bool statusQuotation = false;
        bool isServerConnected = false;
        //bool isSubscribedToolOrderBook = false;
        //bool isSubscribedToolCandles = false;
        bool ignoreCallRun = false;
        bool started = false;
        bool entrancePause = false;
        bool closePause = false;
        bool toCloseAndStop = false;
        string secCode = "SiM0";
        string classCode = "";
        string clientCode = "";
        string directionPosition = "-";
        Tool tool;
        Settings settings;
        Position position;
        DataSet toolCandles;
        DataTable indicatorPriceChannel;
        DateTime indicatorPriceChannelDateTime;
        int counterToolQtyError;
        int qtyTrade;
        decimal currentDrawdown;
        decimal priceTrade;
        decimal summProfit;

        public RobotDemo()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            buttonStartStop.Enabled = false;
            buttonPositionReset.Enabled = false;
            timerRun.Enabled = false;
            settings = new Settings();
            position = new Position();
            textBoxTiker.Text = secCode;
            foreach (var item in Enum.GetValues(typeof(CandleInterval))) comboBoxTF.Items.Add(item);
            comboBoxTF.SelectedItem = settings.TF;
            comboBoxMode.SelectedItem = "Виртуальный";
            textBoxPeriod.Text = settings.PeriodPriceChnl.ToString();
            textBoxQty.Text = settings.QtyOrder.ToString();
            textBoxSlip.Text = settings.KoefSlip.ToString();

            string filePatch = Directory.GetCurrentDirectory() + "\\ReadMe.txt";
            try
            {
                string message = File.ReadAllText(filePatch, Encoding.GetEncoding(1251));
                MessageBox.Show(message);
            }
            catch (Exception er)
            {
                textBoxLogs.AppendText("Ошибка чтения файла ReadMe.txt!" + Environment.NewLine);
                textBoxLogs.AppendText(er.Message + Environment.NewLine);
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxLogs.AppendText("Подключаемся к терминалу Quik..." + Environment.NewLine);
                _quik = new Quik(Quik.DefaultPort, new InMemoryStorage());    // инициализируем объект Quik
                //_quik = new Quik(34136, new InMemoryStorage());    // отладочный вариант
            }
            catch
            {
                textBoxLogs.AppendText("Ошибка инициализации объекта Quik." + Environment.NewLine);
            }
            if (_quik != null)
            {
                textBoxLogs.AppendText("Экземпляр Quik создан." + Environment.NewLine);
                try
                {
                    textBoxLogs.AppendText("Получаем статус соединения с сервером..." + Environment.NewLine);
                    isServerConnected = _quik.Service.IsConnected().Result;
                    if (isServerConnected)
                    {
                        textBoxLogs.AppendText("Соединение с сервером установлено." + Environment.NewLine);
                        buttonStartStop.Enabled = true;
                    }
                    else
                    {
                        textBoxLogs.AppendText("Соединение с сервером НЕ установлено." + Environment.NewLine);
                        buttonStartStop.Enabled = false;
                    }
                }
                catch
                {
                    textBoxLogs.AppendText("Неудачная попытка получить статус соединения с сервером." + Environment.NewLine);
                }

            }
        }
        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            if (started)
            {
                buttonStartStop.Text = "START";
                timerRun.Enabled = false;
                started = false;
            }
            else
            {
                secCode = textBoxTiker.Text;
                settings.RobotMode = comboBoxMode.SelectedItem.ToString();
                settings.PeriodPriceChnl = Convert.ToInt32(textBoxPeriod.Text);
                settings.TF = (CandleInterval)comboBoxTF.SelectedItem;
                settings.KoefSlip = Convert.ToInt32(textBoxSlip.Text);
                settings.QtyOrder = Convert.ToInt32(textBoxQty.Text);
                try
                {
                    clientCode = _quik.Class.GetClientCode().Result;
                }
                catch
                {
                    clientCode = "";
                }
                textBoxLogs.AppendText("Определяем код класса инструмента " + secCode + ", по списку классов" + "..." + Environment.NewLine);
                try
                {
                    classCode = _quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB", secCode).Result;
                }
                catch
                {
                    textBoxLogs.AppendText("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно" + Environment.NewLine);
                }
                if (classCode != null && classCode != "")
                {
                    textBoxLogs.AppendText("Создаем экземпляр инструмента " + secCode + "|" + classCode + "..." + Environment.NewLine);
                    tool = new Tool(_quik, secCode, classCode, settings.KoefSlip);
                    if (tool != null && tool.Name != null && tool.Name != "")
                    {
                        textBoxLogs.AppendText("Инструмент " + tool.Name + " создан." + Environment.NewLine);
                        textBoxToolName.Text = tool.Name;
                        textBoxClassCode.Text = tool.ClassCode;
                        textBoxStep.Text = tool.Step.ToString();
                        textBoxLot.Text = tool.Lot.ToString();
                        textBoxLastPrice.Text = Math.Round(tool.LastPrice, tool.PriceAccuracy).ToString();
                        buttonStartStop.Text = "STOP";
                        timerRun.Enabled = true;
                        started = true;
                    }
                    else
                    {
                        textBoxLogs.AppendText("Не удалось создать экземпляр инструмента " + secCode + "|" + classCode + "..." + Environment.NewLine);
                    }
                }
            }
        }
        private void buttonPositionReset_Click(object sender, EventArgs e)
        {
            if (position.toolQty != 0)
            {
                //if (position.toolQty > 0) ClosePosition(tool.LastPrice + tool.Slip);
                //else ClosePosition(tool.LastPrice - tool.Slip);
                toCloseAndStop = true;
            }
        }
        private void timerRun_Tick(object sender, EventArgs e)
        {
            textBoxLastPrice.Text = Math.Round(tool.LastPrice, tool.PriceAccuracy).ToString();
            Run();
        }

        void Run()
        {
            if (!ignoreCallRun)
            {
                ignoreCallRun = true;
                try
                {
                    if (statusQuotation)
                    {
                        GetIndicators();
                        VerifingEntrancePosition();
                        VerifingClosingPosition();
                        if (position.toolQty == 0)
                        {
                            if (buttonPositionReset.Enabled) buttonPositionReset.Enabled = false;
                            if (toCloseAndStop)
                            {
                                buttonStartStop.Text = "START";
                                timerRun.Enabled = false;
                                started = false;
                                toCloseAndStop = false;
                            }
                            else if (position.entranceOrderNumber == 0 && position.entranceOrderID == 0) // Без позиции и нет выставленной заявки на вход
                            {
                                CheckConditionEntrance();
                            }
                        }
                        else // Есть открытая позиция
                        {
                            if (!buttonPositionReset.Enabled) buttonPositionReset.Enabled = true;
                            decimal newStopLoss = DefinitionCurrentStopLoss();
                            if (newStopLoss != position.stopLoss && position.toolQty != 0)
                            {
                                textBoxLogs.AppendText("Устанавливаем новый StopLoss - " + newStopLoss + Environment.NewLine);
                                position.stopLoss = newStopLoss;
                                textBoxPositionSL.Text = position.stopLoss.ToString();
                            }
                            if (position.closingOrderNumber == 0 && position.closingOrderID == 0) CheckConditionExitPosition(); // Проверка условия выхода из позиции
                        }
                    }
                    else
                    {
                        Check_statusQuotation();
                        if (!statusQuotation) GetQuotation(tool, settings.TF).Wait();
                    }
                }
                catch (Exception er)
                {
                    textBoxLogs.AppendText("Ошибка выполнения Run() - " + er.Message + Environment.NewLine);
                }
                ignoreCallRun = false;
            }
        }

        void Check_statusQuotation()
        {
            if (toolCandles != null)
            {
                if (toolCandles.Tables[tool.ClassCode + "|" + tool.SecurityCode + "|" + settings.TF] != null && toolCandles.Tables[tool.ClassCode + "|" + tool.SecurityCode + "|" + settings.TF] != null)
                {
                    statusQuotation = true;
                    _quik.Candles.NewCandle += OnNewCandle;
                }
            }
        }
        async Task GetQuotation(Tool instrument, CandleInterval tf)
        {
            List<Candle> AllCandles;
            string InstrID;
            toolCandles = new DataSet();
            try
            {
                bool _isSubscribed = false;
                while (!_isSubscribed)
                {
                    textBoxLogs.AppendText("Подписываемся на получение свечек: " + instrument.ClassCode + "|" + instrument.SecurityCode + "|" + tf + "..." + Environment.NewLine);
                    _quik.Candles.Subscribe(instrument.ClassCode, instrument.SecurityCode, tf).Wait();
                    textBoxLogs.AppendText("Проверяем состояние подписки" + "..." + Environment.NewLine);
                    _isSubscribed = _quik.Candles.IsSubscribed(instrument.ClassCode, instrument.SecurityCode, tf).Result;
                }
                textBoxLogs.AppendText("Подписка включена" + "..." + Environment.NewLine);
                textBoxLogs.AppendText("Получаем таблицу свечей" + "..." + Environment.NewLine);
                AllCandles = await _quik.Candles.GetAllCandles(instrument.ClassCode, instrument.SecurityCode, tf).ConfigureAwait(false);
                InstrID = instrument.ClassCode + "|" + instrument.SecurityCode + "|" + tf;
                if (toolCandles.Tables[InstrID] == null && AllCandles.Count > 1)
                {
                    try
                    {
                        toolCandles.Tables.Add(AllCandles_to_Table(AllCandles, InstrID));
                        toolCandles.Tables[InstrID].Columns["DateTime"].SetOrdinal(0);
                        toolCandles.Tables[InstrID].Columns["Open"].SetOrdinal(1);
                        toolCandles.Tables[InstrID].Columns["High"].SetOrdinal(2);
                        toolCandles.Tables[InstrID].Columns["Low"].SetOrdinal(3);
                        toolCandles.Tables[InstrID].Columns["Close"].SetOrdinal(4);
                        toolCandles.Tables[InstrID].Columns["Volume"].SetOrdinal(5);
                        textBoxLogs.AppendText("Сформирована таблица котировок для: " + InstrID + ". Количество свечек = " + AllCandles.Count + "..." + Environment.NewLine);
                    }
                    catch
                    {
                        textBoxLogs.AppendText("Не удалось сформировать таблицу котировок для: " + InstrID + "..." + Environment.NewLine);
                    }
                }
                _quik.Candles.NewCandle += OnNewCandle;
            }
            catch
            {
                textBoxLogs.AppendText("Ошибка подписки на инструмент." + Environment.NewLine);
            }
        }
        DataTable AllCandles_to_Table(List<Candle> AllCandles, string InstrID)
        {
            DataTable dataTable = new System.Data.DataTable(InstrID);

            DataColumn dataColumn;
            dataColumn = new DataColumn("DateTime");
            dataColumn.DataType = System.Type.GetType("System.DateTime");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Open");
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("High");
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Low");
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Close");
            dataColumn.DataType = System.Type.GetType("System.Double");
            dataTable.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Volume");
            dataColumn.DataType = System.Type.GetType("System.Int32");
            dataTable.Columns.Add(dataColumn);

            foreach (Candle cnd in AllCandles)
            {
                try
                {
                    if (cnd != null)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["DateTime"] = (DateTime)cnd.Datetime;
                        dataRow["Open"] = cnd.Open;
                        dataRow["High"] = cnd.High;
                        dataRow["Low"] = cnd.Low;
                        dataRow["Close"] = cnd.Close;
                        dataRow["Volume"] = cnd.Volume;
                        dataTable.Rows.Add(dataRow);
                    }
                }
                catch
                {
                    Console.WriteLine("Ошибка добавления строки");
                }
            }
            return dataTable;
        }
        void NewCandle_to_Table(ref DataSet toolCandles, Candle candle)
        {
            string instrID = candle.ClassCode + "|" + candle.SecCode + "|" + candle.Interval;
            if (toolCandles != null && toolCandles.Tables[instrID] != null)
            {
                if ((DateTime)candle.Datetime != (DateTime)toolCandles.Tables[instrID].Rows[toolCandles.Tables[instrID].Rows.Count-1]["DateTime"])
                {
                    DataRow dataRow = toolCandles.Tables[instrID].NewRow();
                    dataRow["DateTime"] = (DateTime)candle.Datetime;
                    dataRow["Open"] = candle.Open;
                    dataRow["High"] = candle.High;
                    dataRow["Low"] = candle.Low;
                    dataRow["Close"] = candle.Close;
                    dataRow["Volume"] = candle.Volume;
                    toolCandles.Tables[instrID].Rows.Add(dataRow);
                }
            }
        }
        void OnNewCandle(Candle candle)
        {
            if (toolCandles != null) NewCandle_to_Table(ref toolCandles, candle);
        }
        int GetPositionT2(Quik _quik, Tool instrument, string clientCode)
        {
            // возвращает чистую позицию по инструменту
            // для срочного рынка передаем номер счета, для спот-рынка код-клиента
            int qty = 0;
            if (instrument.ClassCode == "SPBFUT")
            {
                // фьючерсы
                try
                {
                    FuturesClientHolding q1 = _quik.Trading.GetFuturesHolding(instrument.FirmID, instrument.AccountID, instrument.SecurityCode, 0).Result;
                    if (q1 != null) qty = Convert.ToInt32(q1.totalNet);
                }
                catch (Exception e) { Console.WriteLine("GetPositionT2: SPBFUT, ошибка - " + e.Message); }
            }
            else
            {
                // акции
                try
                {
                    DepoLimitEx q1 = _quik.Trading.GetDepoEx(instrument.FirmID, clientCode, instrument.SecurityCode, instrument.AccountID, 2).Result;
                    if (q1 != null) qty = Convert.ToInt32(q1.CurrentBalance);
                }
                catch (Exception e) { Console.WriteLine("GetPositionT2: ошибка - " + e.Message); }
            }
            return qty;
        }
        long NewOrder(Quik _quik, Tool _tool, Operation operation, decimal price, int qty)
        {
            long res = 0;
            if (settings.RobotMode == "Боевой")
            {
                Order order_new     = new Order();
                order_new.ClassCode = _tool.ClassCode;
                order_new.SecCode   = _tool.SecurityCode;
                order_new.Operation = operation;
                order_new.Price     = price;
                order_new.Quantity  = qty;
                order_new.Account   = _tool.AccountID;
                try
                {
                    res = _quik.Orders.CreateOrder(order_new).Result;
                }
                catch
                {
                    Console.WriteLine("Неудачная попытка отправки заявки");
                }
            }
            else
            {
                res = 0;
                position.entranceOrderNumber = 8888888;
            }
            return res;
        }
        DataTable CalculateIndicator_PriceChannel(DataTable ds, int period)
        {
            DataTable indicator_dataset = new DataTable();
            DataColumn dataColumn = new DataColumn("High");
            dataColumn.DataType = System.Type.GetType("System.Double");
            indicator_dataset.Columns.Add(dataColumn);
            dataColumn = new DataColumn("Low");
            dataColumn.DataType = System.Type.GetType("System.Double");
            indicator_dataset.Columns.Add(dataColumn);
            dataColumn = new DataColumn("DateTime");
            dataColumn.DataType = System.Type.GetType("System.DateTime");
            indicator_dataset.Columns.Add(dataColumn);

            double chnlMax, chnlMin;

            try
            {
                if (ds != null && period > 0 && ds.Rows.Count > period)
                {
                    for (int ii = period; ii < ds.Rows.Count; ii++)
                    {
                        chnlMin = (double)ds.Rows[ii]["High"] * 100;
                        chnlMax = 0;
                        for (int i = ii - (period - 1); i <= ii; i++)
                        {
                            if ((double)ds.Rows[i]["Close"] > 0)
                            {
                                chnlMax = Math.Max(chnlMax, (double)ds.Rows[i]["High"]);
                                chnlMin = Math.Min(chnlMin, (double)ds.Rows[i]["Low"]);
                                //if (ii == ds.Rows.Count - 1) textBoxLogs.AppendText("i=" + i + ", High=" + (double)ds.Rows[i]["High"] + ", Low=" + (double)ds.Rows[i]["Low"] + ", chnlMax=" + chnlMax + ", chnlMin=" + chnlMin + Environment.NewLine);
                            }
                        }

                        DataRow dataRow = indicator_dataset.NewRow();
                        dataRow["High"] = chnlMax;
                        dataRow["Low"] = chnlMin;
                        dataRow["DateTime"] = (DateTime)ds.Rows[ii]["DateTime"];
                        indicator_dataset.Rows.Add(dataRow);
                    }
                    return indicator_dataset;
                }
                else
                    return null;
            }
            catch (Exception e)
            {
                textBoxLogs.AppendText("Ошибка в методе CalculateIndicator_PriceChannel: " + e.Message + Environment.NewLine);
                Console.WriteLine("Ошибка в методе CalculateIndicator_PriceChannel: " + e.Message);
                return null;
            }
        }
        void GetIndicators()
        {
            string InstrID;
            if (settings.RobotMode == "Боевой")
            {
                int toolQty = GetPositionT2(_quik, tool, clientCode);
                if (toolQty != position.toolQty)
                {
                    if (counterToolQtyError < 10) counterToolQtyError++;
                    else
                    {
                        position.toolQty = toolQty;
                        counterToolQtyError = 0;
                    }
                }
                else
                {
                    if (counterToolQtyError != 0) counterToolQtyError = 0;
                }
            }
            //else
            //{
            //    toolQty = position.toolQty;
            //}

            try
            {
                InstrID = tool.ClassCode + "|" + tool.SecurityCode + "|" + settings.TF;
                if (indicatorPriceChannel == null || indicatorPriceChannelDateTime != (DateTime)toolCandles.Tables[InstrID].Rows[toolCandles.Tables[InstrID].Rows.Count - 1]["DateTime"])
                {
                    //textBoxLogs.AppendText("Перерассчитываем индикатор `Price Channel`..." + Environment.NewLine);
                    indicatorPriceChannel = CalculateIndicator_PriceChannel(toolCandles.Tables[InstrID], settings.PeriodPriceChnl);
                    if (indicatorPriceChannel != null)
                    {
                        indicatorPriceChannelDateTime = (DateTime)toolCandles.Tables[InstrID].Rows[toolCandles.Tables[InstrID].Rows.Count - 1]["DateTime"];
                        //textBoxLogs.AppendText("High - " + indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["High"] + "; Low - " + indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["Low"] + Environment.NewLine);
                    }
                }
            }
            catch (Exception e)
            {
                textBoxLogs.AppendText("Ошибка расчета индикатора `Price Channel` - " + e.Message + Environment.NewLine);
                //Console.WriteLine(tradingSystemName + ":" + Instrument.SecurityCode + ": Ошибка GetIndicators() - " + e.Message);
            }

            if (position.toolQty == 0)
            {
                currentDrawdown = 0;
            }
            else
            {
                if (position.priceEntrance > 0)
                    currentDrawdown = Math.Round((tool.LastPrice - position.priceEntrance) * position.toolQty / tool.Step * tool.PriceStep, tool.PriceAccuracy);
                else
                    currentDrawdown = 0;
            }
            if (textBoxPositionProfit.Text == "" || Convert.ToDecimal(textBoxPositionProfit.Text) != currentDrawdown)
            {
                textBoxPositionProfit.Text = currentDrawdown.ToString();
            }
        }
        void CheckConditionEntrance()
        {
            if (!entrancePause)
            {
                if ((double)tool.LastPrice > (double)indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["High"] && (double)indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["High"] > 0)
                {
                    entrancePause = true;
                    textBoxLogs.AppendText("Сигнал на вход в позицию (long)!" + Environment.NewLine);
                    decimal priceEntrance = tool.LastPrice + tool.Slip;
                    EntrancePosition(Operation.Buy, priceEntrance, settings.QtyOrder);
                }
                else if ((double)tool.LastPrice < (double)indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["Low"] && tool.LastPrice > 0 && (double)indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["Low"] > 0)
                {
                    entrancePause = true;
                    textBoxLogs.AppendText("Сигнал на вход в позицию (short)!" + Environment.NewLine);
                    decimal priceEntrance = tool.LastPrice - tool.Slip;
                    EntrancePosition(Operation.Sell, priceEntrance, settings.QtyOrder);
                }
            }
        }
        void EntrancePosition(Operation operation, decimal price, int qty)
        {
            if (operation == Operation.Buy) directionPosition = "LONG";
            else directionPosition = "SHORT";
            if (settings.RobotMode == "Боевой")
            {
                position.entranceOrderID = NewOrder(_quik, tool, operation, price, qty);
                textBoxLogs.AppendText("ID заявки - " + position.entranceOrderID + Environment.NewLine);
            }
            else
            {
                if (operation == Operation.Buy) position.toolQty = settings.QtyOrder * tool.Lot;
                else position.toolQty = settings.QtyOrder * tool.Lot * -1;
                position.priceEntrance = price;
                position.dateTimeEntrance = DateTime.Now;
                textBoxPositionDirection.Text = directionPosition;
                textBoxPositionPE.Text = Math.Round(position.priceEntrance, tool.PriceAccuracy).ToString();
                textBoxPositionQty.Text = position.toolQty.ToString();
                NewPos2Table(tool.Name, position.toolQty, position.dateTimeEntrance, position.priceEntrance);
                entrancePause = false;
            }
        }
        void ClosePosition(decimal price)
        {
            if (settings.RobotMode == "Боевой")
            {
                if (position.toolQty > 0) position.closingOrderID = NewOrder(_quik, tool, Operation.Sell, price, Math.Abs(position.toolQty / tool.Lot));
                else if (position.toolQty < 0) position.closingOrderID = NewOrder(_quik, tool, Operation.Buy, price, Math.Abs(position.toolQty / tool.Lot));
                textBoxLogs.AppendText("ID заявки - " + position.closingOrderID + Environment.NewLine);
            }
            else
            {
                decimal profit;
                if (position.toolQty > 0) // для логовых позиций
                {
                    profit = Math.Round((price * Math.Abs(position.toolQty / tool.Lot) * tool.Lot - position.priceEntrance * Math.Abs(position.toolQty / tool.Lot) * tool.Lot) / tool.Step * tool.PriceStep, tool.PriceAccuracy);
                }
                else // для шортовых позиций
                {
                    profit = Math.Round((position.priceEntrance * Math.Abs(position.toolQty / tool.Lot) * tool.Lot - price * Math.Abs(position.toolQty / tool.Lot) * tool.Lot) / tool.Step * tool.PriceStep, tool.PriceAccuracy);
                }
                summProfit = summProfit + profit;
                position.toolQty = 0;
                position.priceEntrance = 0;
                position.stopLoss = 0;
                directionPosition = "-";
                textBoxPositionDirection.Text = "-";
                textBoxPositionPE.Text = "";
                textBoxPositionQty.Text = "";
                ClosePos2Table(DateTime.Now, price, profit, summProfit);
                closePause = false;
            }
        }
        void VerifingEntrancePosition()
        {
            if (position.entranceOrderID > 0 || position.entranceOrderNumber > 0)
            {
                if (position.entranceOrderID > 0)
                {
                    //textBoxLogs.AppendText("Получаем номер заявки по ID заявки..." + Environment.NewLine);
                    try
                    {
                        position.entranceOrderNumber = _quik.Orders.GetOrder_by_transID(tool.ClassCode, tool.SecurityCode, position.entranceOrderID).Result.OrderNum;
                        //textBoxLogs.AppendText("entranceOrderNumber - " + position.entranceOrderNumber + Environment.NewLine);
                    }
                    catch
                    {
                        //textBoxLogs.AppendText("Неудачная попытка получения номера заявки по ID заявки..." + Environment.NewLine);
                    }
                    if (position.entranceOrderNumber > 0)
                    {
                        if (position.entranceOrderID > 0) position.entranceOrderID = 0;
                    }
                }
                if (position.entranceOrderNumber > 0)
                {
                    //textBoxLogs.AppendText("Получаем заявку № " + position.entranceOrderNumber + " из журнала..." + Environment.NewLine);
                    try
                    {
                        Order order = _quik.Orders.GetOrder_by_Number(position.entranceOrderNumber).Result;
                        //textBoxLogs.AppendText("order.Balance - " + order.Balance + Environment.NewLine);
                        textBoxOrderDirection.Text = order.Operation.ToString();
                        textBoxOrderBalance.Text = order.Balance.ToString();
                        textBoxOrderNumber.Text = order.OrderNum.ToString();
                        textBoxOrderPrice.Text = order.Price.ToString();
                        textBoxOrderQty.Text = order.Quantity.ToString();
                        if (order != null && !order.Flags.HasFlag(OrderTradeFlags.Active))
                        {
                            if (order.Balance < order.Quantity)
                            {
                                priceTrade = 0;
                                qtyTrade = 0;
                                //textBoxLogs.AppendText("Получаем журнал сделок..." + Environment.NewLine);
                                List<Trade> trades = _quik.Trading.GetTrades().Result;
                                foreach (Trade trade in trades)
                                {
                                    if (trade.OrderNum == order.OrderNum)
                                    {
                                        priceTrade = (decimal)trade.Price;
                                        //textBoxLogs.AppendText("priceTrade - " + priceTrade + Environment.NewLine);
                                        qtyTrade = trade.Quantity;
                                    }
                                }
                            }
                            position.entranceOrderNumber = 0;
                        }
                        else
                        {
                            if (settings.LifeTimeOrder != "00:00:00")
                            {
                                string[] addTime = settings.LifeTimeOrder.Split(':');
                                TimeSpan duration = new TimeSpan(0, Convert.ToInt32(addTime[0]), Convert.ToInt32(addTime[1]), Convert.ToInt32(addTime[2]));
                                DateTime orderEndTime = ((DateTime)order.Datetime).Add(duration);
                                if (DateTime.Now > orderEndTime)
                                {
                                    textBoxLogs.AppendText("Отменяем просроченную заявку: " + order.OrderNum + Environment.NewLine);
                                    _quik.Orders.KillOrder(order).Wait();
                                }
                            }
                        }
                    }
                    catch
                    {
                        textBoxLogs.AppendText("Неудачная попытка получения заявки по номеру..." + Environment.NewLine);
                    }

                }
                if (position.entranceOrderID == 0 && position.entranceOrderNumber == 0)
                {
                    if (priceTrade > 0 && qtyTrade > 0)
                    {
                        //textBoxLogs.AppendText("Заявка отработала" + Environment.NewLine);
                        textBoxOrderDirection.Text = "";
                        textBoxOrderBalance.Text = "";
                        textBoxOrderNumber.Text = "";
                        textBoxOrderPrice.Text = "";
                        textBoxOrderQty.Text = "";

                        if (directionPosition == "LONG") position.toolQty = qtyTrade;
                        else position.toolQty = qtyTrade * -1;
                        position.dateTimeEntrance = DateTime.Now;
                        position.priceEntrance = priceTrade;
                        priceTrade = 0;
                        qtyTrade = 0;
                        textBoxPositionDirection.Text = directionPosition;
                        textBoxPositionPE.Text = Math.Round(position.priceEntrance, tool.PriceAccuracy).ToString();
                        textBoxPositionQty.Text = position.toolQty.ToString();
                        NewPos2Table(tool.Name, position.toolQty, position.dateTimeEntrance, position.priceEntrance);
                        entrancePause = false;
                    }
                }
            }
        }
        void VerifingClosingPosition()
        {
            if (position.closingOrderID > 0 || position.closingOrderNumber > 0)
            {
                if (position.closingOrderID > 0)
                {
                    try
                    {
                        position.closingOrderNumber = _quik.Orders.GetOrder_by_transID(tool.ClassCode, tool.SecurityCode, position.closingOrderID).Result.OrderNum;
                    }
                    catch
                    {

                    }
                    if (position.closingOrderNumber > 0)
                    {
                        if (position.closingOrderID > 0) position.closingOrderID = 0;
                    }
                }
                if (position.closingOrderNumber > 0)
                {
                    try
                    {
                        Order order = _quik.Orders.GetOrder_by_Number(position.closingOrderNumber).Result;
                        textBoxOrderDirection.Text = order.Operation.ToString();
                        textBoxOrderBalance.Text = order.Balance.ToString();
                        textBoxOrderNumber.Text = order.OrderNum.ToString();
                        textBoxOrderPrice.Text = order.Price.ToString();
                        textBoxOrderQty.Text = order.Quantity.ToString();

                        if (order != null && !order.Flags.HasFlag(OrderTradeFlags.Active))
                        {
                            if (order.Balance < order.Quantity)
                            {
                                priceTrade = 0;
                                qtyTrade = 0;
                                List<Trade> trades = _quik.Trading.GetTrades().Result;
                                foreach (Trade trade in trades)
                                {
                                    if (trade.OrderNum == order.OrderNum)
                                    {
                                        priceTrade = (decimal)trade.Price;
                                        qtyTrade = trade.Quantity;
                                    }
                                }
                            }
                            position.closingOrderNumber = 0;
                        }
                        else
                        {
                            if (settings.LifeTimeOrder != "00:00:00")
                            {
                                string[] addTime = settings.LifeTimeOrder.Split(':');
                                TimeSpan duration = new TimeSpan(0, Convert.ToInt32(addTime[0]), Convert.ToInt32(addTime[1]), Convert.ToInt32(addTime[2]));
                                DateTime orderEndTime = ((DateTime)order.Datetime).Add(duration);
                                if (DateTime.Now > orderEndTime)
                                {
                                    textBoxLogs.AppendText("Отменяем просроченную заявку: " + order.OrderNum + Environment.NewLine);
                                    _quik.Orders.KillOrder(order).Wait();
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                }
                if (position.closingOrderID == 0 && position.closingOrderNumber == 0)
                {
                    if (priceTrade > 0 && qtyTrade > 0)
                    {
                        textBoxOrderDirection.Text = "";
                        textBoxOrderBalance.Text = "";
                        textBoxOrderNumber.Text = "";
                        textBoxOrderPrice.Text = "";
                        textBoxOrderQty.Text = "";

                        decimal profit;
                        if (directionPosition == "LONG") // для логовых позиций
                        {
                            profit = Math.Round((priceTrade * qtyTrade * tool.Lot - position.priceEntrance * qtyTrade * tool.Lot) / tool.Step * tool.PriceStep, tool.PriceAccuracy);
                        }
                        else // для шортовых позиций
                        {
                            profit = Math.Round((position.priceEntrance * qtyTrade * tool.Lot - priceTrade * qtyTrade * tool.Lot) / tool.Step * tool.PriceStep, tool.PriceAccuracy);
                        }
                        summProfit = summProfit + profit;
                        position.toolQty = 0;
                        position.priceEntrance = 0;
                        position.stopLoss = 0;
                        ClosePos2Table(DateTime.Now, (decimal)priceTrade, profit, summProfit);
                        priceTrade = 0;
                        qtyTrade = 0;
                        textBoxPositionDirection.Text = "-";
                        textBoxPositionPE.Text = "";
                        textBoxPositionQty.Text = "";
                        directionPosition = "-";
                        closePause = false;
                    }
                }
            }
        }
        decimal DefinitionCurrentStopLoss()
        {
            decimal sl;
            decimal newSL;

            sl = newSL = position.stopLoss;

            try
            {
                    if (position.toolQty > 0) sl = Convert.ToDecimal(indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["Low"]);
                    else sl = Convert.ToDecimal(indicatorPriceChannel.Rows[indicatorPriceChannel.Rows.Count - 1]["High"]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка DefinitionCurrentStopLoss() - " + e.Message);
            }

            if (position.toolQty > 0)
            {
                if (sl > position.stopLoss && sl > 0) newSL = Math.Round(sl, tool.PriceAccuracy);
            }
            else
            {
                if ((sl < position.stopLoss || position.stopLoss == 0) && sl > 0) newSL = Math.Round(sl, tool.PriceAccuracy);
            }
            return newSL;
        }
        void CheckConditionExitPosition()
        {
            decimal price = 0;
            if (position.toolQty > 0)
            {
                if ((toCloseAndStop && tool.LastPrice > 0) || (tool.LastPrice < position.stopLoss && position.stopLoss > 0 && tool.LastPrice > 0)) price = tool.LastPrice - tool.Slip;
            }
            else
            {
                if ((toCloseAndStop && tool.LastPrice > 0) || (tool.LastPrice > position.stopLoss && position.stopLoss > 0 && tool.LastPrice > 0)) price = tool.LastPrice + tool.Slip;
            }

            if (price > 0 && !closePause && position.toolQty != 0)
            {
                closePause = true;
                textBoxLogs.AppendText("Сигнал на закрытие позиции!" + Environment.NewLine);
                ClosePosition(price);
            }
        }
        void NewPos2Table(string toolName, int qty, DateTime timeEntr, decimal price)
        {
            dataGridViewDeals.Rows.Add();
            int i = dataGridViewDeals.Rows.Count - 1;
            dataGridViewDeals.Rows[i].Cells["ToolName"].Value = toolName;
            dataGridViewDeals.Rows[i].Cells["Qty"].Value = qty;
            dataGridViewDeals.Rows[i].Cells["TimeEntr"].Value = timeEntr.ToShortTimeString();
            dataGridViewDeals.Rows[i].Cells["PriceEntr"].Value = Math.Round(price, tool.PriceAccuracy);
            int LastRow = dataGridViewDeals.RowCount - 1;
            dataGridViewDeals.FirstDisplayedScrollingRowIndex = LastRow;
        }
        void ClosePos2Table(DateTime timeExit, decimal _price, decimal _profit, decimal _summProfit)
        {
            int i = dataGridViewDeals.Rows.Count - 1;
            dataGridViewDeals.Rows[i].Cells["TimeExit"].Value = timeExit.ToShortTimeString();
            dataGridViewDeals.Rows[i].Cells["PriceExit"].Value = Math.Round(_price, tool.PriceAccuracy);
            dataGridViewDeals.Rows[i].Cells["Profit"].Value = _profit;
            dataGridViewDeals.Rows[i].Cells["SummProfit"].Value = _summProfit;
            int LastRow = dataGridViewDeals.RowCount - 1;
            dataGridViewDeals.FirstDisplayedScrollingRowIndex = LastRow;
        }
    }
}

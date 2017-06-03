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
    public partial class FormMain : Form
    {
        Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        public static Quik _quik;
        bool isServerConnected = false;
        bool isSubscribedToolOrderBook = false;
        bool isSubscribedToolCandles = false;
        string secCode = "SRH7";
        string classCode = "";
        string clientCode;
        decimal bid;
        decimal offer;
        Tool tool;
        OrderBook toolOrderBook;
        List<Candle> toolCandles;
        List<Order> listOrders;
        List<Trade> listTrades;
        List<DepoLimitEx> listDepoLimits;
        List<PortfolioInfoEx> listPortfolio;
        List<MoneyLimit> listMoneyLimits;
        List<MoneyLimitEx> listMoneyLimitsEx;
        FormOutputTable toolCandlesTable;
        Order order;
        FuturesClientHolding futuresPosition;

        public FormMain()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            textBoxSecCode.Text = secCode;
            textBoxClassCode.Text = classCode;
            buttonRun.Enabled = false;
            buttonCommandRun.Enabled = false;
            timerRenewForm.Enabled = false;
            listBoxCommands.Enabled = false;
            listBoxCommands.Items.Add("Получить исторические данные");
            listBoxCommands.Items.Add("Выставить заявку (без сделки)");
            listBoxCommands.Items.Add("Выставить заявку (c выполнением!!!)");
            listBoxCommands.Items.Add("Удалить активную заявку");
            listBoxCommands.Items.Add("Получить таблицу лимитов по бумаге");
            listBoxCommands.Items.Add("Получить таблицу лимитов по всем бумагам");
            listBoxCommands.Items.Add("Получить таблицу заявок");
            listBoxCommands.Items.Add("Получить таблицу сделок");
            listBoxCommands.Items.Add("Получить таблицу `Клиентский портфель`");
            listBoxCommands.Items.Add("Получить таблицы денежных лимитов");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxLogsWindow.AppendText("Подключаемся к терминалу Quik..." + Environment.NewLine);
                _quik = new Quik(Quik.DefaultPort, new InMemoryStorage());    // инициализируем объект Quik
                //_quik = new Quik(34136, new InMemoryStorage());    // отладочное подключение
            }
            catch
            {
                textBoxLogsWindow.AppendText("Ошибка инициализации объекта Quik..." + Environment.NewLine);
            }
            if (_quik != null)
            {
                textBoxLogsWindow.AppendText("Экземпляр Quik создан." + Environment.NewLine);
                try
                {
                    textBoxLogsWindow.AppendText("Получаем статус соединения с сервером...." + Environment.NewLine);
                    isServerConnected = _quik.Service.IsConnected().Result;
                    if (isServerConnected)
                    {
                        textBoxLogsWindow.AppendText("Соединение с сервером установлено." + Environment.NewLine);
                        buttonRun.Enabled = true;
                        buttonStart.Enabled = false;
                    }
                    else
                    {
                        textBoxLogsWindow.AppendText("Соединение с сервером НЕ установлено." + Environment.NewLine);
                        buttonRun.Enabled = false;
                        buttonStart.Enabled = true;
                    }
                }
                catch
                {
                    textBoxLogsWindow.AppendText("Неудачная попытка получить статус соединения с сервером." + Environment.NewLine);
                }
            }
        }
        private void buttonRun_Click(object sender, EventArgs e)
        {
            Run();
        }
        void Run()
        {
            try
            {
                secCode = textBoxSecCode.Text;
                textBoxLogsWindow.AppendText("Определяем код класса инструмента " + secCode + ", по списку классов" + "..." + Environment.NewLine);
                try
                {
                    classCode = _quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB", secCode).Result;
                }
                catch
                {
                    textBoxLogsWindow.AppendText("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно" + Environment.NewLine);
                }
                if (classCode!= null && classCode != "")
                {
                    textBoxClassCode.Text = classCode;
                    textBoxLogsWindow.AppendText("Определяем код клиента..." + Environment.NewLine);
                    clientCode = _quik.Class.GetClientCode().Result;
                    textBoxClientCode.Text = clientCode;
                    textBoxLogsWindow.AppendText("Создаем экземпляр инструмента " + secCode + "|" + classCode + "..." + Environment.NewLine);
                    tool = new Tool(_quik, secCode, classCode);
                    if (tool != null && tool.Name != null && tool.Name != "")
                    {
                        textBoxLogsWindow.AppendText("Инструмент " + tool.Name + " создан." + Environment.NewLine);
                        textBoxAccountID.Text = tool.AccountID;
                        textBoxFirmID.Text = tool.FirmID;
                        textBoxShortName.Text = tool.Name;
                        textBoxLot.Text = Convert.ToString(tool.Lot);
                        textBoxStep.Text = Convert.ToString(tool.Step);
                        textBoxGuaranteeProviding.Text = Convert.ToString(tool.GuaranteeProviding);
                        textBoxLastPrice.Text = Convert.ToString(tool.LastPrice);
                        textBoxQty.Text = Convert.ToString(GetPositionT2(_quik, tool, clientCode));
                        textBoxLogsWindow.AppendText("Подписываемся на стакан..." + Environment.NewLine);
                        _quik.OrderBook.Subscribe(tool.ClassCode, tool.SecurityCode).Wait();
                        isSubscribedToolOrderBook = _quik.OrderBook.IsSubscribed(tool.ClassCode, tool.SecurityCode).Result;
                        if (isSubscribedToolOrderBook)
                        {
                            toolOrderBook = new OrderBook();
                            textBoxLogsWindow.AppendText("Подписка на стакан прошла успешно." + Environment.NewLine);
                            textBoxLogsWindow.AppendText("Подписываемся на колбэк 'OnQuote'..." + Environment.NewLine);
                            _quik.Events.OnQuote += OnQuoteDo;
                            timerRenewForm.Enabled = true;
                            listBoxCommands.SelectedIndex = 0;
                            listBoxCommands.Enabled = true;
                            buttonCommandRun.Enabled = true;
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("Подписка на стакан не удалась." + Environment.NewLine);
                            textBoxBestBid.Text = "-";
                            textBoxBestOffer.Text = "-";
                            timerRenewForm.Enabled = false;
                            listBoxCommands.Enabled = false;
                            buttonCommandRun.Enabled = false;
                        }
                        textBoxLogsWindow.AppendText("Подписываемся на колбэк 'OnFuturesClientHolding'..." + Environment.NewLine);
                        _quik.Events.OnFuturesClientHolding += OnFuturesClientHoldingDo;
                    }
                    buttonRun.Enabled = false;
                }
            }
            catch
            {
                textBoxLogsWindow.AppendText("Ошибка получения данных по инструменту." + Environment.NewLine);
            }
        }

        void OnQuoteDo(OrderBook quote)
        {
            if (quote.sec_code == tool.SecurityCode && quote.class_code == tool.ClassCode)
            {
                toolOrderBook = quote;
                bid = Convert.ToDecimal(toolOrderBook.bid[toolOrderBook.bid.Count() - 1].price);
                offer = Convert.ToDecimal(toolOrderBook.offer[0].price);
            }
        }
        void OnFuturesClientHoldingDo(FuturesClientHolding futPos)
        {
            if (futPos.secCode == tool.SecurityCode) futuresPosition = futPos;
        }

        private void timerRenewForm_Tick(object sender, EventArgs e)
        {
            textBoxLastPrice.Text = Convert.ToString(tool.LastPrice);
            textBoxQty.Text = Convert.ToString(GetPositionT2(_quik, tool, clientCode));
            if (toolOrderBook != null && toolOrderBook.bid != null)
            {
                textBoxBestBid.Text = bid.ToString();
                textBoxBestOffer.Text = offer.ToString();
            }
            if (futuresPosition != null) textBoxVarMargin.Text = futuresPosition.varMargin.ToString(); 
        }
        private void listBoxCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCommand = listBoxCommands.SelectedItem.ToString();
            switch (selectedCommand)
            {
                case "Получить исторические данные":
                    textBoxDescription.Text = "Получить и отобразить исторические данные котировок по заданному инструменту. Тайм-фрейм = 1 Hour";
                    break;
                case "Выставить заявку (без сделки)":
                    textBoxDescription.Text = "Будет выставлена заявку на покупку 1-го лота заданного инструмента, по цене на 5% ниже текущей цены (вероятность срабатывания такой заявки достаточно низкая, чтобы успеть ее отменить)";
                    break;
                case "Выставить заявку (c выполнением!!!)":
                    textBoxDescription.Text = "Будет выставлена заявку на покупку 1-го лота заданного инструмента, по цене на 5 шагов цены выше текущей цены (вероятность срабатывания такой заявки достаточно высокая!!!)";
                    break;
                case "Выставить заявку (Удалить активную заявку)":
                    textBoxDescription.Text = "Если предварительно была выставлена заявка, заявка имеет статус 'Активна' и ее номер отображается в форме, то эта заявка будет удалена/отменена";
                    break;
                case "Получить таблицу лимитов по бумаге":
                    textBoxDescription.Text = "Получить и отобразить таблицу лимитов по бумагам. quik.Trading.GetDepoLimits(securityCode)";
                    break;
                case "Получить таблицу лимитов по всем бумагам":
                    textBoxDescription.Text = "Получить и отобразить таблицу лимитов по бумагам. quik.Trading.GetDepoLimits()";
                    break;
                case "Получить таблицу заявок":
                    textBoxDescription.Text = "Получить и отобразить таблицу всех клиентских заявок. quik.Orders.GetOrders()";
                    break;
                case "Получить таблицу сделок":
                    textBoxDescription.Text = "Получить и отобразить таблицу всех клиентских сделок. quik.Trading.GetTrades()";
                    break;
                case "Получить таблицу `Клиентский портфель`":
                    textBoxDescription.Text = "Получить и отобразить таблицу `Клиентский портфель`. quik.Trading.GetPortfolioInfoEx()";
                    break;
                case "Получить таблицы денежных лимитов":
                    textBoxDescription.Text = "Получить и отобразить таблицы денежных лимитов (стандартную и дополнительную Т2). Работает только на инструментах фондовой секции. quik.Trading.GetMoney() и quik.Trading.GetMoneyEx()";
                    break;
            }
        }
        private void buttonCommandRun_Click(object sender, EventArgs e)
        {
            string selectedCommand = listBoxCommands.SelectedItem.ToString();
            switch (selectedCommand)
            {
                case "Получить исторические данные":
                    try
                    {
                        textBoxLogsWindow.AppendText("Подписываемся на получение исторических данных..." + Environment.NewLine);
                        _quik.Candles.Subscribe(tool.ClassCode, tool.SecurityCode, CandleInterval.H1).Wait();
                        textBoxLogsWindow.AppendText("Проверяем состояние подписки..." + Environment.NewLine);
                        isSubscribedToolCandles = _quik.Candles.IsSubscribed(tool.ClassCode, tool.SecurityCode, CandleInterval.H1).Result;
                        if (isSubscribedToolCandles)
                        {
                            textBoxLogsWindow.AppendText("Получаем исторические данные..." + Environment.NewLine);
                            toolCandles = _quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.H1).Result;
                            textBoxLogsWindow.AppendText("Выводим исторические данные в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(toolCandles);
                            toolCandlesTable.Show();
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("Неудачная попытка подписки на исторические данные." + Environment.NewLine);
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения исторических данных." + Environment.NewLine);
                    }
                    break;
                case "Выставить заявку (без сделки)":
                    try
                    {
                        decimal priceInOrder = Math.Round(tool.LastPrice - tool.LastPrice / 20, tool.PriceAccuracy);
                        textBoxLogsWindow.AppendText("Выставляем заявку на покупку, по цене:" + priceInOrder + " ..." + Environment.NewLine);
                        long transactionID = NewOrder(_quik, tool, Operation.Buy, priceInOrder, 1);
                        if (transactionID > 0)
                        {
                            Thread.Sleep(500);
                            textBoxLogsWindow.AppendText("Заявка выставлена. ID транзакции - " + transactionID + Environment.NewLine);
                            try
                            {
                                listOrders = _quik.Orders.GetOrders().Result;
                                foreach (Order _order in listOrders)
                                {
                                    if (_order.TransID == transactionID && _order.ClassCode == tool.ClassCode && _order.SecCode == tool.SecurityCode)
                                    {
                                        textBoxLogsWindow.AppendText("Заявка выставлена. Номер заявки - " + _order.OrderNum + Environment.NewLine);
                                        textBoxOrderNumber.Text = _order.OrderNum.ToString();
                                        order = _order;
                                    }
                                }
                            }
                            catch (Exception er)
                            {
                                textBoxLogsWindow.AppendText("Ошибка получения номера заявки. Error: " + er.Message + Environment.NewLine);
                            }
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("Неудачная попытка выставления заявки." + Environment.NewLine);
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка выставления заявки." + Environment.NewLine);
                    }
                    break;
                case "Выставить заявку (c выполнением!!!)":
                    try
                    {
                        decimal priceInOrder = Math.Round(tool.LastPrice + tool.Step * 5, tool.PriceAccuracy);
                        textBoxLogsWindow.AppendText("Выставляем заявку на покупку, по цене:" + priceInOrder + " ..." + Environment.NewLine);
                        long transactionID = NewOrder(_quik, tool, Operation.Buy, priceInOrder, 1);
                        if (transactionID > 0)
                        {
                            textBoxLogsWindow.AppendText("Заявка выставлена. ID транзакции - " + transactionID + Environment.NewLine);
                            Thread.Sleep(500);
                            try
                            {
                                listOrders = _quik.Orders.GetOrders().Result;
                                foreach (Order _order in listOrders)
                                {
                                    if (_order.TransID == transactionID && _order.ClassCode == tool.ClassCode && _order.SecCode == tool.SecurityCode)
                                    {
                                        textBoxLogsWindow.AppendText("Заявка выставлена. Номер заявки - " + _order.OrderNum + Environment.NewLine);
                                        textBoxOrderNumber.Text = _order.OrderNum.ToString();
                                        order = _order;
                                    }
                                    else
                                    {
                                        textBoxOrderNumber.Text = "---";
                                    }
                                }
                            }
                            catch
                            {
                                textBoxLogsWindow.AppendText("Ошибка получения номера заявки." + Environment.NewLine);
                            }
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("Неудачная попытка выставления заявки." + Environment.NewLine);
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка выставления заявки." + Environment.NewLine);
                    }
                    break;
                case "Удалить активную заявку":
                    try
                    {
                        if (order != null && order.OrderNum > 0)
                        {
                            textBoxLogsWindow.AppendText("Удаляем заявку на покупку с номером - " + order.OrderNum + " ..." + Environment.NewLine);
                        }
                        long x = _quik.Orders.KillOrder(order).Result;
                        textBoxLogsWindow.AppendText("Результат - " + x + " ..." + Environment.NewLine);
                        textBoxOrderNumber.Text = "";
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка удаления заявки." + Environment.NewLine);
                    }
                    break;
                case "Получить таблицу лимитов по бумаге":
                    try
                    {
                        textBoxLogsWindow.AppendText("Получаем таблицу лимитов..." + Environment.NewLine);
                        listDepoLimits = _quik.Trading.GetDepoLimits(tool.SecurityCode).Result;

                        if (listDepoLimits.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные лимитов в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listDepoLimits);
                            toolCandlesTable.Show();
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("Бумага '" + tool.Name + "' в таблице лимитов отсутствует." + Environment.NewLine);
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения лимитов." + Environment.NewLine);
                    }
                    break;
                case "Получить таблицу лимитов по всем бумагам":
                    try
                    {
                        textBoxLogsWindow.AppendText("Получаем таблицу лимитов..." + Environment.NewLine);
                        listDepoLimits = _quik.Trading.GetDepoLimits().Result;

                        if (listDepoLimits.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные лимитов в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listDepoLimits);
                            toolCandlesTable.Show();
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения лимитов." + Environment.NewLine);
                    }
                    break;
                case "Получить таблицу заявок":
                    try
                    {
                        textBoxLogsWindow.AppendText("Получаем таблицу заявок..." + Environment.NewLine);
                        listOrders = _quik.Orders.GetOrders().Result;

                        if (listOrders.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные о заявках в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listOrders);
                            toolCandlesTable.Show();
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения заявок." + Environment.NewLine);
                    }
                    break;
                case "Получить таблицу сделок":
                    try
                    {
                        textBoxLogsWindow.AppendText("Получаем таблицу сделок..." + Environment.NewLine);
                        listTrades = _quik.Trading.GetTrades().Result;

                        if (listTrades.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные о сделках в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listTrades);
                            toolCandlesTable.Show();
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения сделок." + Environment.NewLine);
                    }
                    break;
                case "Получить таблицу `Клиентский портфель`":
                    try
                    {
                        textBoxLogsWindow.AppendText("Получаем таблицу `Клиентский портфель`..." + Environment.NewLine);
                        listPortfolio = new List<PortfolioInfoEx>();
                        if (classCode == "SPBFUT") listPortfolio.Add(_quik.Trading.GetPortfolioInfoEx(tool.FirmID, tool.AccountID, 0).Result);
                        else listPortfolio.Add(_quik.Trading.GetPortfolioInfoEx(tool.FirmID, clientCode, 2).Result);

                        if (listPortfolio.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные о портфеле в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listPortfolio);
                            toolCandlesTable.Show();
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("В таблице `Клиентский портфель` отсутствуют записи." + Environment.NewLine);
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения клиентского портфеля." + Environment.NewLine);
                    }
                    break;
                case "Получить таблицы денежных лимитов":
                    try
                    {
                        textBoxLogsWindow.AppendText("Получаем таблицу денежных лимитов..." + Environment.NewLine);
                        listMoneyLimits = new List<MoneyLimit>();
                        listMoneyLimits.Add(_quik.Trading.GetMoney(clientCode, tool.FirmID, "EQTV", "SUR").Result);

                        if (listMoneyLimits.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные о денежных лимитах в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listMoneyLimits);
                            toolCandlesTable.Show();
                        }

                        textBoxLogsWindow.AppendText("Получаем расширение таблицы денежных лимитов..." + Environment.NewLine);
                        listMoneyLimitsEx = new List<MoneyLimitEx>();
                        listMoneyLimitsEx.Add(_quik.Trading.GetMoneyEx(tool.FirmID, clientCode, "EQTV", "SUR", 2).Result);

                        if (listMoneyLimitsEx.Count > 0)
                        {
                            textBoxLogsWindow.AppendText("Выводим данные о денежных лимитах в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listMoneyLimitsEx);
                            toolCandlesTable.Show();
                        }
                    }
                    catch
                    {
                        textBoxLogsWindow.AppendText("Ошибка получения денежных лимитов." + Environment.NewLine);
                    }
                    break;
            }
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
            Order order_new = new Order();
            order_new.ClassCode = _tool.ClassCode;
            order_new.SecCode = _tool.SecurityCode;
            order_new.Operation = operation;
            order_new.Price = price;
            order_new.Quantity = qty;
            order_new.Account = _tool.AccountID;
            try
            {
                res = _quik.Orders.CreateOrder(order_new).Result;
            }
            catch
            {
                Console.WriteLine("Неудачная попытка отправки заявки");
            }
            return res;
        }
    }
}

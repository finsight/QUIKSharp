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
using QuikSharpDemo.Properties;

namespace QuikSharpDemo
{
    public partial class FormMain : Form
    {
        readonly Char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];
        public static Quik _quik;
        int portConnection = Quik.DefaultPort;
        bool isServerConnected = false;
        bool isSubscribedToolOrderBook = false;
        bool isSubscribedToolCandles = false;
        //string secCode = "SiU3";
        string secCode = "SU26225RMFS1";
        string classCode = "";
        string clientCode;
        decimal bid;
        decimal offer;
        Tool tool;
        OrderBook toolOrderBook;
        List<Candle> toolCandles;
        List<Order> listOrders;
        List<Trade> listTrades;
        List<AllTrade> listAllTrades;
        List<SecurityInfo> listSecurityInfo;
        List<DepoLimitEx> listDepoLimits;
        List<FuturesLimits> listFuturesLimits;
        List<FuturesClientHolding> listFuturesClientHoldings;
        List<PortfolioInfoEx> listPortfolio;
        List<MoneyLimit> listMoneyLimits;
        List<MoneyLimitEx> listMoneyLimitsEx;
        List<BuySellInfo> listBuySellInfo;
        FormOutputTable toolCandlesTable;
        FormOrderBook toolOrderBookTable;
        FormRequestValue requestValue;
        Order order;
        FuturesLimits futLimit;
        FuturesClientHolding futuresPosition;
        DateTime renewOrderBookTime;
        delegate void TextBoxTextDelegate(TextBox tb ,string text);
        delegate void TextBoxAppendTextDelegate(TextBox tb, string text);

        public FormMain()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            textBoxSecCode.Text                 = secCode;
            textBoxClassCode.Text               = classCode;
            numericUpDownPortConnection.Value   = Convert.ToDecimal(portConnection);
            buttonRun.Enabled                   = false;
            buttonCommandRun.Enabled            = false;
            timerRenewForm.Enabled              = false;
            listBoxCommands.Enabled             = false;
            listBoxCommands.Items.Add("Получить исторические данные");
            listBoxCommands.Items.Add("Получить исторические данные (с параметром `bid`)");
            listBoxCommands.Items.Add("Выставить лимитрированную заявку (без сделки)");
            listBoxCommands.Items.Add("Выставить лимитрированную заявку (c выполнением!!!)");
            listBoxCommands.Items.Add("Выставить рыночную заявку (c выполнением!!!)");
            listBoxCommands.Items.Add("Удалить активную заявку");
            listBoxCommands.Items.Add("Получить заявку по номеру");
            listBoxCommands.Items.Add("Получить заявку по ID транзакции");
            listBoxCommands.Items.Add("Получить информацию по бумаге");
            listBoxCommands.Items.Add("Получить таблицу лимитов по бумаге");
            listBoxCommands.Items.Add("Получить таблицу лимитов по всем бумагам");
            listBoxCommands.Items.Add("Получить таблицу по фьючерсным лимитам");
            listBoxCommands.Items.Add("Получить таблицу позиций по клиентским счетам (фьючерсы)");
            listBoxCommands.Items.Add("Получить таблицу заявок");
            listBoxCommands.Items.Add("Получить таблицу сделок");
            listBoxCommands.Items.Add("Получить таблицу обезличенных сделок");
            listBoxCommands.Items.Add("Получить таблицу `Клиентский портфель`");
            listBoxCommands.Items.Add("Получить таблицы денежных лимитов");
            listBoxCommands.Items.Add("Получить таблицу Купить/Продать");
            listBoxCommands.Items.Add("Получить стакан заявок");
            listBoxCommands.Items.Add("Связка ParamRequest + OnParam + GetParamEx2");
            listBoxCommands.Items.Add("CancelParamRequest");
            listBoxCommands.Items.Add("Отменить заказ на получение стакана");
            listBoxCommands.Items.Add("Выставить стоп-заявку типа тейк-профит и стоп-лимит");
            listBoxCommands.Items.Add("Рассчитать максимальное количество лотов в заявке");
            listBoxCommands.Items.Add("Получить дату торговой сессии");
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            try
            {
                portConnection = Convert.ToUInt16(numericUpDownPortConnection.Value);
                textBoxLogsWindow.AppendText("Подключаемся к терминалу Quik (порт: " + portConnection.ToString() + ")..." + Environment.NewLine);
                if (checkBoxRemoteHost.Checked) _quik = new Quik(Quik.DefaultPort, new InMemoryStorage(), textBoxHost.Text);    // инициализируем объект Quik с использованием удаленного IP-адреса терминала
                else _quik = new Quik(portConnection, new InMemoryStorage());    // инициализируем объект Quik с использованием локального расположения терминала (по умолчанию)
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
                        textBoxLogsWindow.AppendText("Определяем код клиента..." + Environment.NewLine);
                        //clientCode              = _quik.Class.GetClientCode().Result;
                        List<string> codes = _quik.Class.GetClientCodes().Result;
                        if (codes.Count > 0)
                        {
                            comboBox_ClientCode.Items.AddRange(codes.ToArray<object>());
                            comboBox_ClientCode.SelectedItem = comboBox_ClientCode.Items[0];
                            clientCode = comboBox_ClientCode.SelectedItem.ToString();
                        }

                        textBoxLogsWindow.AppendText("Определяем аккаунт..." + Environment.NewLine);
                        List<TradesAccounts> accounts = _quik.Class.GetTradeAccounts().Result;
                        if (accounts.Count > 0)
                        {
                            foreach(TradesAccounts account in accounts)
                            {
                                textBoxLogsWindow.AppendText("Найден аккаунт: " + "firmID-"+account.Firmid + ", TrdaccId-" + account.TrdaccId + ", MainTrdaccid-" + account.MainTrdaccid + Environment.NewLine);
                            }
                            //comboBox_ClientCode.Items.AddRange(codes.ToArray<object>());
                            //comboBox_ClientCode.SelectedItem = comboBox_ClientCode.Items[0];
                            //clientCode = comboBox_ClientCode.SelectedItem.ToString();
                        }

                        buttonRun.Enabled = true;
                        buttonStart.Enabled = false;
                    }
                    else
                    {
                        textBoxLogsWindow.AppendText("Соединение с сервером НЕ установлено." + Environment.NewLine);
                        buttonRun.Enabled = false;
                        buttonStart.Enabled = true;
                    }
                    // для отладки
                    //Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
                    //Trace.Listeners.Add(new TextWriterTraceListener("TraceLogging.log"));
                    // для отладки
                }
                catch
                {
                    textBoxLogsWindow.AppendText("Неудачная попытка получить статус соединения с сервером." + Environment.NewLine);
                }
            }
        }
        private void ButtonRun_Click(object sender, EventArgs e)
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
                    classCode = textBoxClassCode.Text == "" ? _quik.Class.GetSecurityClass("SPBFUT,TQBR,TQBS,TQNL,TQLV,TQNE,TQOB,SPBXM", secCode).Result : textBoxClassCode.Text.ToUpper();
                }
                catch
                {
                    textBoxLogsWindow.AppendText("Ошибка определения класса инструмента. Убедитесь, что тикер указан правильно" + Environment.NewLine);
                }
                if (classCode!= null && classCode != "")
                {
                    textBoxClassCode.Text   = classCode;
                    textBoxLogsWindow.AppendText("Создаем экземпляр инструмента " + secCode + "|" + classCode + "..." + Environment.NewLine);
                    tool                    = new Tool(_quik, secCode, classCode);
                    if (tool != null && tool.Name != null && tool.Name != "")
                    {
                        textBoxLogsWindow.AppendText("Инструмент " + tool.Name + " создан." + Environment.NewLine);
                        textBoxAccountID.Text           = tool.AccountID;
                        textBoxFirmID.Text              = tool.FirmID;
                        textBoxShortName.Text           = tool.Name;
                        textBoxLot.Text                 = Convert.ToString(tool.Lot);
                        textBoxStep.Text                = Convert.ToString(tool.Step);
                        textBoxGuaranteeProviding.Text  = Convert.ToString(tool.GuaranteeProviding);
                        textBoxLastPrice.Text           = Convert.ToString(tool.LastPrice);
                        textBoxQty.Text                 = Convert.ToString(GetPositionT2(_quik, tool, clientCode));
                        textBoxLogsWindow.AppendText("Подписываемся на стакан..." + Environment.NewLine);
                        _quik.OrderBook.Subscribe(tool.ClassCode, tool.SecurityCode).Wait();
                        isSubscribedToolOrderBook       = _quik.OrderBook.IsSubscribed(tool.ClassCode, tool.SecurityCode).Result;
                        if (isSubscribedToolOrderBook)
                        {
                            toolOrderBook                   = new OrderBook();
                            textBoxLogsWindow.AppendText("Подписка на стакан прошла успешно." + Environment.NewLine);
                            textBoxLogsWindow.AppendText("Подписываемся на колбэк 'OnQuote'..." + Environment.NewLine);
                            _quik.Events.OnQuote += OnQuoteDo;
                            timerRenewForm.Enabled          = true;
                            listBoxCommands.SelectedIndex   = 0;
                            listBoxCommands.Enabled         = true;
                            buttonCommandRun.Enabled        = true;
                        }
                        else
                        {
                            textBoxLogsWindow.AppendText("Подписка на стакан не удалась." + Environment.NewLine);
                            textBoxBestBid.Text             = "-";
                            textBoxBestOffer.Text           = "-";
                            timerRenewForm.Enabled          = false;
                            listBoxCommands.Enabled         = false;
                            buttonCommandRun.Enabled        = false;
                        }
                        textBoxLogsWindow.AppendText("Подписываемся на колбэк 'OnFuturesClientHolding'..." + Environment.NewLine);
                        _quik.Events.OnFuturesClientHolding += OnFuturesClientHoldingDo;
                        textBoxLogsWindow.AppendText("Подписываемся на колбэк 'OnDepoLimit'..." + Environment.NewLine);
                        _quik.Events.OnDepoLimit += OnDepoLimitDo;
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
                renewOrderBookTime  = DateTime.Now;
                toolOrderBook       = quote;
                bid                 = Convert.ToDecimal(toolOrderBook.bid[toolOrderBook.bid.Count() - 1].price);
                offer               = Convert.ToDecimal(toolOrderBook.offer[0].price);
            }
        }
        void OnFuturesClientHoldingDo(FuturesClientHolding futPos)
        {
            if (futPos.secCode == tool.SecurityCode) futuresPosition = futPos;
        }
        void OnDepoLimitDo(DepoLimitEx depLimit)
        {
            AppendText2TextBox(textBoxLogsWindow, "Вызвано событие OnDepoLimit (изменение бумажного лимита)..." + Environment.NewLine);
            AppendText2TextBox(textBoxLogsWindow, "Заблокировано на покупку количества лотов - " + depLimit.LockedBuy + Environment.NewLine);
        }
        void OnParamDo(Param _param)
        {
            if (_param.ClassCode == tool.ClassCode && _param.SecCode == tool.SecurityCode)
            {
                double bid = Convert.ToDouble(_quik.Trading.GetParamEx2(tool.ClassCode, tool.SecurityCode, ParamNames.BID).Result.ParamValue.Replace('.', separator));
                AppendText2TextBox(textBoxLogsWindow, "Вызвано событие OnParam. Актуальное значение параметра 'BID' = " + bid + Environment.NewLine);
            }
        }
        void OnNewCandleDo(Candle _candle)
        {
            Trace.Assert(_candle != null, DateTime.Now + ": Trace: candle = NULL");
            Trace.WriteLine(DateTime.Now + ": Trace: OnNewCandleDo()");
            try
            {
                if (_candle.ClassCode == tool.ClassCode && _candle.SecCode == tool.SecurityCode && _candle.Interval == CandleInterval.M15)
                {
                    Trace.WriteLine("Trace: Получена новая свеча -  'Time' = " + DateTime.Now + ", 'TimeCandle' = " + (DateTime)_candle.Datetime + ", 'Open' = " + _candle.Open + ", 'Close' = " + _candle.Close);
                    AppendText2TextBox(textBoxLogsWindow, "Получена новая свеча: 'Time' = " + DateTime.Now + ", 'TimeCandle' = " + (DateTime)_candle.Datetime + ", 'Open' = " + _candle.Open + ", 'Close' = " + _candle.Close + Environment.NewLine);
                }
            }
            catch (Exception er)
            {
                Trace.WriteLine("Trace: Ошибка в OnNewCandleDo() - " + er.ToString());
            }
        }
        void OnStopOrderDo(StopOrder _stopOrder)
        {
            Trace.Assert(_stopOrder != null, DateTime.Now + ": Trace: stopOrder = NULL");
            Trace.WriteLine(DateTime.Now + ": Trace: OnStopOrderDo()");
            try
            {
                if (_stopOrder != null && _stopOrder.OrderNum > 0)
                {
                    Trace.WriteLine("Trace: Вызвано событие OnStopOrder - 'Time' = " + DateTime.Now + ", 'OrderNum' = " + _stopOrder.OrderNum + ", 'State' = " + _stopOrder.State);
                    AppendText2TextBox(textBoxLogsWindow, "Вызвано событие OnStopOrder - 'Time' = " + DateTime.Now + ", 'OrderNum' = " + _stopOrder.OrderNum + ", 'State' = " + _stopOrder.State + Environment.NewLine);
                }
            }
            catch (Exception er)
            {
                Trace.WriteLine("Trace: Ошибка в OnStopOrderDo() - " + er.ToString());
            }
        }

        private void TimerRenewForm_Tick(object sender, EventArgs e)
        {
            textBoxLastPrice.Text = Convert.ToString(tool.LastPrice);
            textBoxQty.Text = Convert.ToString(GetPositionT2(_quik, tool, clientCode));
            if (toolOrderBook != null && toolOrderBook.bid != null)
            {
                Text2TextBox(textBox_RenewTime, renewOrderBookTime.ToLongTimeString());
                Text2TextBox(textBoxBestBid, bid.ToString());
                Text2TextBox(textBoxBestOffer, offer.ToString());
                if (toolOrderBookTable != null) toolOrderBookTable.Renew(toolOrderBook);
            }
            if (futuresPosition != null) textBoxVarMargin.Text = futuresPosition.varMargin.ToString();
            //Trace.Flush();
        }
        private void ListBoxCommands_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCommand = listBoxCommands.SelectedItem.ToString();
            switch (selectedCommand)
            {
                case "Получить исторические данные":
                    textBoxDescription.Text = "Получить и отобразить исторические данные котировок по заданному инструменту. Тайм-фрейм = 15 Minute";
                    break;
                case "Получить исторические данные (с параметром `bid`)":
                    textBoxDescription.Text = "Получить и отобразить исторические данные котировок по заданному инструменту и параметру `bid`. Тайм-фрейм = 15 Minute";
                    break;
                case "Выставить лимитрированную заявку (без сделки)":
                    textBoxDescription.Text = "Будет выставлена заявку на покупку 1-го лота заданного инструмента, по цене на 5% ниже текущей цены (вероятность срабатывания такой заявки достаточно низкая, чтобы успеть ее отменить)";
                    break;
                case "Выставить лимитрированную заявку (c выполнением!!!)":
                    textBoxDescription.Text = "Будет выставлена заявку на покупку 1-го лота заданного инструмента, по цене на 5 шагов цены выше текущей цены (вероятность срабатывания такой заявки достаточно высокая!!!)";
                    break;
                case "Выставить рыночную заявку (c выполнением!!!)":
                    textBoxDescription.Text = "Будет выставлена заявку на покупку 1-го лота заданного инструмента, \"по рынку\" (Заявка будет автоматически исполнена по текущим доступным ценам!!!)";
                    break;
                case "Выставить заявку (Удалить активную заявку)":
                    textBoxDescription.Text = "Если предварительно была выставлена заявка, заявка имеет статус 'Активна' и ее номер отображается в форме, то эта заявка будет удалена/отменена";
                    break;
                case "Получить заявку по номеру":
                    textBoxDescription.Text = "Попытаться получить заявку по номеру, который укажет пользователь";
                    break;
                case "Получить заявку по ID транзакции)":
                    textBoxDescription.Text = "Попытаться получить заявку по ID транзакции, который укажет пользователь";
                    break;
                case "Получить таблицу лимитов по бумаге":
                    textBoxDescription.Text = "Получить и отобразить таблицу лимитов по бумагам. quik.Trading.GetDepoLimits(securityCode)";
                    break;
                case "Получить информацию по бумаге":
                    textBoxDescription.Text = "Получить и отобразить таблицу c информацией по бумаге. quik.Class.GetSecurityInfo(classCode, securityCode)";
                    break;
                case "Получить таблицу лимитов по всем бумагам":
                    textBoxDescription.Text = "Получить и отобразить таблицу лимитов по бумагам. quik.Trading.GetDepoLimits()";
                    break;
                case "Получить таблицу по фьючерсным лимитам":
                    textBoxDescription.Text = "Получить и отобразить таблицу лимитов по фьючерсам. quik.Trading.GetFuturesLimit(firmId, accId, limitType, currCode)";
                    break;
                case "Получить таблицу позиций по клиентским счетам (фьючерсы)":
                    textBoxDescription.Text = "Получить и отобразить таблицу \"Позиции по клиенским счетам (фьючерсы)\". quik.Trading.GetFuturesClientHoldings()";
                    break;
                case "Получить таблицу заявок":
                    textBoxDescription.Text = "Получить и отобразить таблицу всех клиентских заявок. quik.Orders.GetOrders()";
                    break;
                case "Получить таблицу сделок":
                    textBoxDescription.Text = "Получить и отобразить таблицу всех клиентских сделок. quik.Trading.GetTrades()";
                    break;
                case "Получить таблицу обезличенных сделок":
                    textBoxDescription.Text = "Получить и отобразить таблицу обезличенных сделок по инструменту. quik.Trading.GetAllTrades()";
                    break;
                case "Получить таблицу `Клиентский портфель`":
                    textBoxDescription.Text = "Получить и отобразить таблицу `Клиентский портфель`. quik.Trading.GetPortfolioInfoEx()";
                    break;
                case "Получить таблицы денежных лимитов":
                    textBoxDescription.Text = "Получить и отобразить таблицы денежных лимитов (стандартную и дополнительную Т2). Работает только на инструментах фондовой секции. quik.Trading.GetMoney() и quik.Trading.GetMoneyEx()";
                    break;
                case "Получить таблицу Купить/Продать":
                    textBoxDescription.Text = "Получить и отобразить таблицу с параметрами из таблицы QUIK «Купить/Продать», означающими возможность купить либо продать указанный инструмент «sec_code» класса «class_code», указанным клиентом «client_code» фирмы «firmid», по указанной цене «price». Если цена равна «0», то используются лучшие значения спроса/предложения. quik.Trading.GetBuySellInfo() и quik.Trading.GetBuySellInfoEx()";
                    break;
                case "Получить стакан заявок":
                    textBoxDescription.Text = "Получить и отобразить стакан заявок в виде таблицы (данные на момент вызова функции. Без обновления)";
                    break;
                case "Связка ParamRequest + OnParam + GetParamEx2":
                    textBoxDescription.Text = "Демонстрация работы связки ParamRequest + OnParam + GetParamEx2";
                    break;
                case "CancelParamRequest":
                    textBoxDescription.Text = "Отменяем подписку на обновление параметра и отключаем обработку события OnParam";
                    break;
                case "Отменить заказ на получение стакана":
                    textBoxDescription.Text = "Вызываем функцию отмены заказа стакана по инструменту";
                    break;
                case "Выставить стоп-заявку типа тейк-профит и стоп-лимит":
                    textBoxDescription.Text = "Выставляем стоп-заявку типа тейк-профит и стоп-лимит. Закрываем short. Тейк-профит по цене минус 50 шагов цены от последней сделки. Стоп-лимит - плюс 40 шагов цены. Для тейпрофита для отступа используем тип шаг цены, для спреда - процент.";
                    break;
                case "Рассчитать максимальное количество лотов в заявке":
                    textBoxDescription.Text = "Получить максимальное количество лото в заявке по текущему инструменту. (`покупка` по текущей цене, лимитированной заявкой)";
                    break;
                case "Получить дату торговой сессии":
                    textBoxDescription.Text = "Получить дату текущей торговой сессии";
                    break;

            }
        }
        private void ButtonCommandRun_Click(object sender, EventArgs e)
        {
            CallCommand();
        }
        private async Task CallCommand()
        {
            string selectedCommand = listBoxCommands.SelectedItem.ToString();
            switch (selectedCommand)
            {
                case "Получить исторические данные":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Подписываемся на получение исторических данных..." + Environment.NewLine);
                        _quik.Candles.Subscribe(tool.ClassCode, tool.SecurityCode, CandleInterval.M15).Wait();
                        AppendText2TextBox(textBoxLogsWindow, "Проверяем состояние подписки..." + Environment.NewLine);
                        isSubscribedToolCandles = _quik.Candles.IsSubscribed(tool.ClassCode, tool.SecurityCode, CandleInterval.M15).Result;
                        if (isSubscribedToolCandles)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Получаем исторические данные..." + Environment.NewLine);
                            toolCandles = _quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.M15).Result;
                            AppendText2TextBox(textBoxLogsWindow, "Выводим исторические данные в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(toolCandles);
                            toolCandlesTable.Show();
                            _quik.Candles.NewCandle += OnNewCandleDo;
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка подписки на исторические данные." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения исторических данных." + Environment.NewLine); }
                    break;
                case "Получить исторические данные (с параметром `bid`)":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Подписываемся на получение исторических данных..." + Environment.NewLine);
                        _quik.Candles.Subscribe(tool.ClassCode, tool.SecurityCode, CandleInterval.M15, "bid").Wait();
                        AppendText2TextBox(textBoxLogsWindow, "Проверяем состояние подписки..." + Environment.NewLine);
                        isSubscribedToolCandles = _quik.Candles.IsSubscribed(tool.ClassCode, tool.SecurityCode, CandleInterval.M15, "bid").Result;
                        if (isSubscribedToolCandles)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Получаем исторические данные..." + Environment.NewLine);
                            toolCandles = _quik.Candles.GetAllCandles(tool.ClassCode, tool.SecurityCode, CandleInterval.M15, "bid").Result;
                            AppendText2TextBox(textBoxLogsWindow, "Выводим исторические данные в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(toolCandles);
                            toolCandlesTable.Show();
                            _quik.Candles.NewCandle += OnNewCandleDo;
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка подписки на исторические данные." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения исторических данных." + Environment.NewLine); }
                    break;
                case "Выставить лимитрированную заявку (без сделки)":
                    try
                    {
                        decimal priceInOrder = Math.Round(tool.LastPrice - tool.LastPrice / 50, tool.PriceAccuracy);
                        AppendText2TextBox(textBoxLogsWindow, "Выставляем заявку на покупку, по цене:" + priceInOrder + " ..." + Environment.NewLine);
                        order = await _quik.Orders.SendLimitOrder(tool.ClassCode, tool.SecurityCode, tool.AccountID, Operation.Buy, priceInOrder, 1, ExecutionCondition.PUT_IN_QUEUE, clientCode).ConfigureAwait(false);
                        if (order.OrderNum > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. ID транзакции - " + order.TransID + Environment.NewLine);
                            AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. Номер заявки - " + order.OrderNum + Environment.NewLine);
                            AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. Код клиента - " + order.ClientCode + Environment.NewLine);
                            Text2TextBox(textBoxOrderNumber, order.OrderNum.ToString());
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка размещения заявки. Error: " + order.RejectReason + Environment.NewLine);
                    }
                    catch (Exception er) { textBoxLogsWindow.AppendText("Ошибка процедуры размещения заявки. Error: " + er.Message + Environment.NewLine); }
                    break;
                case "Выставить лимитрированную заявку (c выполнением!!!)":
                    try
                    {
                        decimal priceInOrder = Math.Round(tool.LastPrice + tool.Step * 5, tool.PriceAccuracy);
                        AppendText2TextBox(textBoxLogsWindow, "Выставляем заявку на покупку, по цене:" + priceInOrder + " ..." + Environment.NewLine);
                        long transactionID = (await _quik.Orders.SendLimitOrder(tool.ClassCode, tool.SecurityCode, tool.AccountID, Operation.Buy, priceInOrder, 1, ExecutionCondition.PUT_IN_QUEUE, clientCode).ConfigureAwait(false)).TransID;
                        if (transactionID > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. ID транзакции - " + transactionID + Environment.NewLine);
                            Thread.Sleep(500);
                            try
                            {
                                listOrders = _quik.Orders.GetOrders().Result;
                                foreach (Order _order in listOrders)
                                {
                                    if (_order.TransID == transactionID && _order.ClassCode == tool.ClassCode && _order.SecCode == tool.SecurityCode)
                                    {
                                        AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. Номер заявки - " + _order.OrderNum + Environment.NewLine);
                                        Text2TextBox(textBoxOrderNumber, _order.OrderNum.ToString());
                                        order = _order;
                                    }
                                    else
                                        Text2TextBox(textBoxOrderNumber, "---");
                                }
                            }
                            catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения номера заявки." + Environment.NewLine); }
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка выставления заявки." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка выставления заявки." + Environment.NewLine); }
                    break;
                case "Выставить рыночную заявку (c выполнением!!!)":
                    try
                    {
                        decimal priceInOrder = Math.Round(tool.LastPrice + tool.Step * 5, tool.PriceAccuracy);
                        AppendText2TextBox(textBoxLogsWindow, "Выставляем рыночную заявку на покупку..." + Environment.NewLine);
                        long transactionID = (await _quik.Orders.SendMarketOrder(tool.ClassCode, tool.SecurityCode, tool.AccountID, Operation.Buy, 1).ConfigureAwait(false)).TransID;
                        if (transactionID > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. ID транзакции - " + transactionID + Environment.NewLine);
                            Thread.Sleep(500);
                            try
                            {
                                listOrders = _quik.Orders.GetOrders().Result;
                                foreach (Order _order in listOrders)
                                {
                                    if (_order.TransID == transactionID && _order.ClassCode == tool.ClassCode && _order.SecCode == tool.SecurityCode)
                                    {
                                        textBoxLogsWindow.AppendText("Заявка выставлена. Номер заявки - " + _order.OrderNum + Environment.NewLine);
                                        Text2TextBox(textBoxOrderNumber, _order.OrderNum.ToString());
                                        order = _order;
                                    }
                                    else
                                        Text2TextBox(textBoxOrderNumber, "---");
                                }
                            }
                            catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения номера заявки." + Environment.NewLine); }
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка выставления заявки." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка выставления заявки." + Environment.NewLine); }
                    break;
                case "Удалить активную заявку":
                    try
                    {
                        if (order != null && order.OrderNum > 0) AppendText2TextBox(textBoxLogsWindow, "Удаляем заявку на покупку с номером - " + order.OrderNum + " ..." + Environment.NewLine);
                        long x = _quik.Orders.KillOrder(order).Result;
                        AppendText2TextBox(textBoxLogsWindow, "Результат - " + x + " ..." + Environment.NewLine);
                        Text2TextBox(textBoxOrderNumber, "");
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка удаления заявки." + Environment.NewLine); }
                    break;
                case "Получить заявку по номеру":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Запрашиваем номер заявки у пользователя..." + Environment.NewLine);
                        requestValue = new FormRequestValue();
                        requestValue.ShowDialog();
                        String requestedValue = requestValue.RequestedValue;
                        requestValue.Dispose();
                        if (requestedValue.Length > 0)
                        {
                            long orderNumber = 0;
                            try
                            {
                                orderNumber = Convert.ToInt64(requestedValue.Replace(" ", ""));
                            }
                            catch
                            {
                                AppendText2TextBox(textBoxLogsWindow, "Полученное от пользователя значение не удалось преобразовать в номер заявки. Запрос не будет выполнен." + Environment.NewLine);
                            }
                            if (orderNumber > 0)
                            {
                                AppendText2TextBox(textBoxLogsWindow, "Ищем заявку с номером - " + orderNumber + " ..." + Environment.NewLine);
                                Order findOrder = _quik.Orders.GetOrder_by_Number(orderNumber).Result;
                                if (findOrder != null) 
                                {
                                    listOrders = new List<Order>
                                    {
                                        findOrder
                                    };
                                    AppendText2TextBox(textBoxLogsWindow, "Выводим данные о заявке в таблицу..." + Environment.NewLine);
                                    toolCandlesTable = new FormOutputTable(listOrders);
                                    toolCandlesTable.Show();
                                }
                                else
                                {
                                    AppendText2TextBox(textBoxLogsWindow, "Не удалось найти заявку по указанному номеру..." + Environment.NewLine);
                                }
                            }
                        }
                        else
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Номер заявки не указан. Запрос не будет выполнен." + Environment.NewLine);
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения заявки по номеру." + Environment.NewLine); }
                    break;
                case "Получить заявку по ID транзакции":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Запрашиваем ID транзакции у пользователя..." + Environment.NewLine);
                        requestValue = new FormRequestValue();
                        requestValue.ShowDialog();
                        String requestedValue = requestValue.RequestedValue;
                        requestValue.Dispose();
                        if (requestedValue.Length > 0)
                        {
                            long transID = 0;
                            try
                            {
                                transID = Convert.ToInt64(requestedValue.Replace(" ", ""));
                            }
                            catch
                            {
                                AppendText2TextBox(textBoxLogsWindow, "Полученное от пользователя значение не удалось преобразовать в ID транзакции. Запрос не будет выполнен." + Environment.NewLine);
                            }
                            if (transID > 0)
                            {
                                AppendText2TextBox(textBoxLogsWindow, "Ищем заявку с ID транзакции - " + transID + " ..." + Environment.NewLine);
                                Order findOrder = _quik.Orders.GetOrder_by_transID(tool.ClassCode, tool.SecurityCode, transID).Result;
                                if (findOrder != null)
                                {
                                    listOrders = new List<Order>
                                    {
                                        findOrder
                                    };
                                    AppendText2TextBox(textBoxLogsWindow, "Выводим данные о заявке в таблицу..." + Environment.NewLine);
                                    toolCandlesTable = new FormOutputTable(listOrders);
                                    toolCandlesTable.Show();
                                }
                                else
                                {
                                    AppendText2TextBox(textBoxLogsWindow, "Не удалось найти заявку по указанному ID транзакции..." + Environment.NewLine);
                                }
                            }
                        }
                        else
                        {
                            AppendText2TextBox(textBoxLogsWindow, "ID транзакции не указан. Запрос не будет выполнен." + Environment.NewLine);
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения заявки по ID транзакции." + Environment.NewLine); }
                    break;
                case "Получить информацию по бумаге":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу информации..." + Environment.NewLine);
                        listSecurityInfo = new List<SecurityInfo> { _quik.Class.GetSecurityInfo(tool.ClassCode, tool.SecurityCode).Result };

                        if (listSecurityInfo.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listSecurityInfo);
                            toolCandlesTable.Show();
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Информация по бумаге '" + tool.Name + "' отсутствует." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения лимитов." + Environment.NewLine); }
                    break;
                case "Получить таблицу лимитов по бумаге":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу лимитов..." + Environment.NewLine);
                        listDepoLimits = _quik.Trading.GetDepoLimits(tool.SecurityCode).Result;

                        if (listDepoLimits.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные лимитов в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listDepoLimits);
                            toolCandlesTable.Show();
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Бумага '" + tool.Name + "' в таблице лимитов отсутствует." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения лимитов." + Environment.NewLine); }
                    break;
                case "Получить таблицу лимитов по всем бумагам":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу лимитов..." + Environment.NewLine);
                        listDepoLimits = _quik.Trading.GetDepoLimits().Result;

                        if (listDepoLimits.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные лимитов в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listDepoLimits);
                            toolCandlesTable.Show();
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения лимитов." + Environment.NewLine); }
                    break;
                case "Получить таблицу по фьючерсным лимитам":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу фьючерсных лимитов..." + Environment.NewLine);
                        futLimit = _quik.Trading.GetFuturesLimit(tool.FirmID, tool.AccountID, 0, "SUR").Result;

                        if (futLimit != null)
                        {
                            listFuturesLimits = new List<FuturesLimits> { futLimit };
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные лимитов в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listFuturesLimits);
                            toolCandlesTable.Show();
                        }
                        else
                        {
                            Console.WriteLine("futuresLimit = null");
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения лимитов." + Environment.NewLine); }
                    break;
                case "Получить таблицу позиций по клиентским счетам (фьючерсы)":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу фьючерсных позиций..." + Environment.NewLine);
                        listFuturesClientHoldings = _quik.Trading.GetFuturesClientHoldings().Result;

                        if (listFuturesClientHoldings.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные по фьючерсным позициям в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listFuturesClientHoldings);
                            toolCandlesTable.Show();
                        }
                        else
                        {
                            Console.WriteLine("futuresLimit = null");
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения лимитов." + Environment.NewLine); }
                    break;
                case "Получить таблицу заявок":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу заявок..." + Environment.NewLine);
                        listOrders = _quik.Orders.GetOrders().Result;

                        if (listOrders.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о заявках в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listOrders);
                            toolCandlesTable.Show();
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения заявок." + Environment.NewLine); }
                    break;
                case "Получить таблицу сделок":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу сделок..." + Environment.NewLine);
                        listTrades = _quik.Trading.GetTrades().Result;

                        if (listTrades.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о сделках в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listTrades);
                            toolCandlesTable.Show();
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения сделок." + Environment.NewLine); }
                    break;
                case "Получить таблицу обезличенных сделок":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу обезличенных сделок..." + Environment.NewLine);
                        listAllTrades = _quik.Trading.GetAllTrades(tool.ClassCode, tool.SecurityCode).Result;

                        if (listAllTrades.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о сделках в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listAllTrades);
                            toolCandlesTable.Show();
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения сделок." + Environment.NewLine); }
                    break;
                case "Получить таблицу `Клиентский портфель`":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу `Клиентский портфель`..." + Environment.NewLine);
                        listPortfolio = new List<PortfolioInfoEx>();
                        if (classCode == "SPBFUT")
                        {
                            listPortfolio.Add(_quik.Trading.GetPortfolioInfoEx(tool.FirmID, tool.AccountID, 0).Result);
                        }
                        else
                        {
                            listPortfolio.Add(_quik.Trading.GetPortfolioInfoEx(tool.FirmID, clientCode, 0).Result);
                            listPortfolio.Add(_quik.Trading.GetPortfolioInfoEx(tool.FirmID, clientCode, 1).Result);
                            listPortfolio.Add(_quik.Trading.GetPortfolioInfoEx(tool.FirmID, clientCode, 2).Result);
                        }

                        if (listPortfolio.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о портфеле в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listPortfolio);
                            toolCandlesTable.Show();
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "В таблице `Клиентский портфель` отсутствуют записи." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения клиентского портфеля." + Environment.NewLine); }
                    break;
                case "Получить таблицы денежных лимитов":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу денежных лимитов..." + Environment.NewLine);
                        listMoneyLimits = new List<MoneyLimit> { _quik.Trading.GetMoney(clientCode, tool.FirmID, "EQTV", "SUR").Result };

                        if (listMoneyLimits.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о денежных лимитах в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listMoneyLimits);
                            toolCandlesTable.Show();
                        }

                        AppendText2TextBox(textBoxLogsWindow, "Получаем расширение таблицы денежных лимитов..." + Environment.NewLine);
                        listMoneyLimitsEx = new List<MoneyLimitEx> { _quik.Trading.GetMoneyEx(tool.FirmID, clientCode, "EQTV", "SUR", 2).Result };

                        if (listMoneyLimitsEx.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о денежных лимитах в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listMoneyLimitsEx);
                            toolCandlesTable.Show();
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения денежных лимитов." + Environment.NewLine); }
                    break;
                case "Получить таблицу Купить/Продать":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем таблицу `Купить/Продать`..." + Environment.NewLine);
                        listBuySellInfo = new List<BuySellInfo>();
                        listBuySellInfo.Add(_quik.Trading.GetBuySellInfoEx(tool.FirmID, classCode == "SPBFUT" ? tool.AccountID : clientCode, tool.ClassCode, tool.SecurityCode, 0).Result);

                        if (listBuySellInfo.Count > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о параметрах Купить/Продать в таблицу..." + Environment.NewLine);
                            toolCandlesTable = new FormOutputTable(listBuySellInfo);
                            toolCandlesTable.Show();
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "В таблице `Купить/Продать` отсутствуют записи." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения таблицы Купить/Продать." + Environment.NewLine); }
                    break;
                case "Получить стакан заявок":
                    try
                    {
                        if (toolOrderBook != null)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Получаем данные о стакане с помощью функции GetQuoteLevel2..." + Environment.NewLine);
                            OrderBook orderBookCurrent = _quik.OrderBook.GetQuoteLevel2(tool.ClassCode, tool.SecurityCode).Result;
                            AppendText2TextBox(textBoxLogsWindow, "Выводим данные о стакане заявок в таблицу..." + Environment.NewLine);
                            //Console.WriteLine("Count offer = " + orderBookCurrent.offer_count);
                            //toolOrderBookTable = new FormOrderBook(toolOrderBook);
                            //toolOrderBookTable = new FormOrderBook(orderBookCurrent);
                            toolOrderBookTable = new FormOrderBook();
                            toolOrderBookTable.Show();
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения денежных лимитов." + Environment.NewLine); }
                    break;
                case "Связка ParamRequest + OnParam + GetParamEx2":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Подписываемся на получение обновляемого параметра 'BID', через ParamRequest..." + Environment.NewLine);
                        bool pReq = _quik.Trading.ParamRequest(tool.ClassCode, tool.SecurityCode, ParamNames.BID).Result;
                        if (pReq)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Подписываемся на колбэк 'OnParam'..." + Environment.NewLine);
                            _quik.Events.OnParam += OnParamDo;
                        }
                        else
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка подписки на обновление параметра..." + Environment.NewLine);
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка работы в связке ParamRequest + OnParam + GetParamEx2." + Environment.NewLine); }
                    break;
                case "CancelParamRequest":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Отменяем подписку на получение обновляемого параметра 'BID', через ParamRequest..." + Environment.NewLine);
                        bool pReq = _quik.Trading.CancelParamRequest(tool.ClassCode, tool.SecurityCode, ParamNames.BID).Result;
                        if (pReq)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Отменяем подписку на колбэк 'OnParam'..." + Environment.NewLine);
                            _quik.Events.OnParam -= OnParamDo;
                        }
                        else
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка отписки на обновление параметра..." + Environment.NewLine);
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка работы в связке ParamRequest + OnParam + GetParamEx2." + Environment.NewLine); }
                    break;
                case "Отменить заказ на получение стакана":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Отменяем заказ на получение с сервера стакана по указанному классу и инструменту..." + Environment.NewLine);
                        bool resultUnsub = await _quik.OrderBook.Unsubscribe(tool.ClassCode, tool.SecurityCode).ConfigureAwait(false);
                        int count = 0;
                        while (!resultUnsub && count < 10)
                        {
                            Thread.Sleep(500);
                            resultUnsub = await _quik.OrderBook.Unsubscribe(tool.ClassCode, tool.SecurityCode).ConfigureAwait(false);
                            count++;
                        }
                        int i = 0;
                        while(isSubscribedToolOrderBook && i < 10)
                        {
                            Thread.Sleep(500);
                            isSubscribedToolOrderBook = _quik.OrderBook.IsSubscribed(tool.ClassCode, tool.SecurityCode).Result;
                            i++;
                        }
                        if (isSubscribedToolOrderBook)
                        {
                            toolOrderBook                   = new OrderBook();
                            AppendText2TextBox(textBoxLogsWindow, "Отмена подписки на стакан не удалась." + Environment.NewLine);
                        }
                        else
                        {
                            toolOrderBook                   = null;
                            AppendText2TextBox(textBoxLogsWindow, "Отмена подписки на стакан прошла успешно." + Environment.NewLine);
                            bid                             = 0;
                            offer                           = 0;
                            Text2TextBox(textBoxBestBid, "-");
                            Text2TextBox(textBoxBestOffer, "-");
                        }
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка в функции отмены заказа стакана." + Environment.NewLine); }
                    break;
                case "Выставить стоп-заявку типа тейк-профит и стоп-лимит":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Подписываемся на событие OnStopOrder..." + Environment.NewLine);
                        _quik.Events.OnStopOrder += OnStopOrderDo;
                        decimal priceInOrder = Math.Round(tool.LastPrice, tool.PriceAccuracy);
                        StopOrder orderNew = new StopOrder()
                        {
                            Account         = tool.AccountID,
                            ClassCode       = tool.ClassCode,
                            ClientCode      = clientCode,
                            SecCode         = secCode,
                            Offset          = 5,
                            OffsetUnit      = OffsetUnits.PRICE_UNITS,
                            Spread          = 0.1M,
                            SpreadUnit      = OffsetUnits.PERCENTS,
                            StopOrderType   = StopOrderType.TakeProfitStopLimit,
                            Condition       = Condition.LessOrEqual,
                            ConditionPrice  = Math.Round(priceInOrder - 50 * tool.Step, tool.PriceAccuracy),
                            ConditionPrice2 = Math.Round(priceInOrder + 40 * tool.Step, tool.PriceAccuracy),
                            Price           = Math.Round(priceInOrder + 45 * tool.Step, tool.PriceAccuracy),
                            Operation       = Operation.Buy,
                            Quantity        = 1
                        };
                        AppendText2TextBox(textBoxLogsWindow, "Выставляем стоп-заявку на покупку, по цене:" + priceInOrder + " ..." + Environment.NewLine);
                        long transID = await _quik.StopOrders.CreateStopOrder(orderNew).ConfigureAwait(false);
                        if (transID > 0)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Заявка выставлена. ID транзакции - " + transID + Environment.NewLine);
                            Thread.Sleep(500);
                            try
                            {
                                var listStopOrders = _quik.StopOrders.GetStopOrders().Result;
                                foreach (StopOrder stopOrder in listStopOrders)
                                {
                                    if (stopOrder.TransId == transID && stopOrder.ClassCode == tool.ClassCode && stopOrder.SecCode == tool.SecurityCode)
                                        AppendText2TextBox(textBoxLogsWindow,"Стоп-заявка выставлена. Номер стоп-заявки - " + stopOrder.OrderNum + Environment.NewLine);
                                }
                            }
                            catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка получения номера стоп-заявки." + Environment.NewLine); }
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная попытка выставления стоп-заявки." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка выставления стоп-заявки." + Environment.NewLine); }
                    break;
                case "Рассчитать максимальное количество лотов в заявке":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем максимальное количество лотов в заявке для текущей цены..." + Environment.NewLine);
                        CalcBuySellResult res = await _quik.Trading.CalcBuySell(tool.ClassCode, tool.SecurityCode, clientCode, tool.AccountID, (double)tool.LastPrice, true, false).ConfigureAwait(false);
                        if (res != null)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Количество лотов = " + res.Qty + ", Комиссия = " + res.Comission + Environment.NewLine);
                            Thread.Sleep(500);
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная получения данных о максимальном количество лотов в заявке." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка выполнения функции CalcBuySell." + Environment.NewLine); }
                    break;
                case "Получить дату торговой сессии":
                    try
                    {
                        AppendText2TextBox(textBoxLogsWindow, "Получаем дату торговой сессии..." + Environment.NewLine);
                        DateTime res = await _quik.Trading.GetTradeDate().ConfigureAwait(false);
                        if (res != null)
                        {
                            AppendText2TextBox(textBoxLogsWindow, "Дата торговой сессии = " + res.ToShortDateString() + Environment.NewLine);
                            Thread.Sleep(500);
                        }
                        else AppendText2TextBox(textBoxLogsWindow, "Неудачная получения даты торговой сессии." + Environment.NewLine);
                    }
                    catch { AppendText2TextBox(textBoxLogsWindow, "Ошибка выполнения функции GetTradeDate." + Environment.NewLine); }
                    break;
            }
        }

        private void Text2TextBox(TextBox _tb, string _text)
        {
            if (_tb.InvokeRequired)
            {
                TextBoxTextDelegate d = new TextBoxTextDelegate(Text2TextBox);
                _tb.Invoke(d, new object[] { _tb, _text });
            }
            else _tb.Text = _text;
        }
        private void AppendText2TextBox(TextBox _tb, string _text)
        {
            if (_tb.InvokeRequired)
            {
                TextBoxAppendTextDelegate d = new TextBoxAppendTextDelegate(AppendText2TextBox);
                _tb.Invoke(d, new object[] { _tb, _text });
            }
            else _tb.AppendText(_text);
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
        private void CheckBoxRemoteHost_CheckedChanged(object sender, EventArgs e) { textBoxHost.Enabled = checkBoxRemoteHost.Checked; }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Trace.Close();
        }

        private void ComboBox_ClientCode_SelectedIndexChanged(object sender, EventArgs e) { clientCode = comboBox_ClientCode.SelectedItem.ToString(); }
    }
}

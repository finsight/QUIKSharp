// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Функции взаимодействия скрипта Lua и Рабочего места QUIK
    /// getDepo - функция для получения информации по бумажным лимитам
    /// getMoney - функция для получения информации по денежным лимитам
    /// getMoneyEx - функция для получения информации по денежным лимитам указанного типа
    /// getFuturesLimit - функция для получения информации по фьючерсным лимитам
    /// getFuturesHolding - функция для получения информации по фьючерсным позициям
    /// paramRequest - Функция заказывает получение параметров Таблицы текущих торгов
    /// cancelParamRequest - Функция отменяет заказ на получение параметров Таблицы текущих торгов
    /// getParamEx - функция для получения значений Таблицы текущих значений параметров
    /// getParamEx2 - функция для получения всех значений Таблицы текущих значений параметров
    /// getTradeDate - функция для получения даты торговой сессии
    /// sendTransaction - функция для работы с заявками
    /// CulcBuySell - функция для расчета максимально возможного количества лотов в заявке
    /// getPortfolioInfo - функция для получения значений параметров таблицы «Клиентский портфель»
    /// getPortfolioInfoEx - функция для получения значений параметров таблицы «Клиентский портфель» с учетом вида лимита
    /// getBuySellInfo - функция для получения параметров таблицы «Купить/Продать»
    /// getBuySellInfoEx - функция для получения параметров (включая вид лимита) таблицы «Купить/Продать»
    /// getTrdAccByClientCode - Функция возвращает торговый счет срочного рынка, соответствующий коду клиента фондового рынка с единой денежной позицией
    /// getClientCodeByTrdAcc - Функция возвращает код клиента фондового рынка с единой денежной позицией, соответствующий торговому счету срочного рынка
    /// isUcpClient - Функция предназначена для получения признака, указывающего имеет ли клиент единую денежную позицию
    /// </summary>
    public interface ITradingFunctions : IQuikService
    {
        /// <summary>
        /// Функция для получения информации по бумажным лимитам
        /// </summary>
        Task<DepoLimit> GetDepo(string clientCode, string firmId, string secCode, string account);

        /// <summary>
        /// Функция для получения информации по бумажным лимитам указанного типа
        /// </summary>
        Task<DepoLimitEx> GetDepoEx(string firmId, string clientCode, string secCode, string accID, int limitKind);

        /// <summary>
        /// Возвращает список записей из таблицы 'Лимиты по бумагам'.
        /// </summary>
        Task<List<DepoLimitEx>> GetDepoLimits();

        /// <summary>
        /// Возвращает список записей из таблицы 'Лимиты по бумагам', отфильтрованных по коду инструмента.
        /// </summary>
        /// <param name="secCode">Код инструментаю</param>
        /// <returns></returns>
        Task<List<DepoLimitEx>> GetDepoLimits(string secCode);

        /// <summary>
        /// Функция для получения информации по денежным лимитам
        /// </summary>
        ///
        Task<MoneyLimit> GetMoney(string clientCode, string firmId, string tag, string currCode);

        /// <summary>
        ///  функция для получения информации по денежным лимитам указанного типа
        /// </summary>
        Task<MoneyLimitEx> GetMoneyEx(string firmId, string clientCode, string tag, string currCode, int limitKind);

        /// <summary>
        ///  функция для получения информации по денежным лимитам всех торговых счетов (кроме фьючерсных) и валют
        ///  Лучшее место для получения связки clientCode + firmid
        /// </summary>
        Task<List<MoneyLimitEx>> GetMoneyLimits();

        /// <summary>
        ///  функция для получения информации по фьючерсным лимитам
        /// </summary>
        Task<FuturesLimits> GetFuturesLimit(string firmId, string accId, int limitType, string currCode);

        /// <summary>
        ///  функция для получения информации по фьючерсным лимитам всех клиентских счетов
        /// </summary>
        Task<List<FuturesLimits>> GetFuturesClientLimits();

        /// <summary>
        ///  функция для получения информации по фьючерсным позициям
        /// </summary>
        Task<FuturesClientHolding> GetFuturesHolding(string firmId, string accId, string secCode, int posType);

        /// <summary>
        /// Функция получения доски опционов
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <returns></returns>
        Task<List<OptionBoard>> GetOptionBoard(string classCode, string secCode);

        /// <summary>
        /// Функция заказывает получение параметров Таблицы текущих торгов
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<bool> ParamRequest(string classCode, string secCode, string paramName);

        Task<bool> ParamRequest(string classCode, string secCode, ParamNames paramName);

        /// <summary>
        /// Функция отменяет заказ на получение параметров Таблицы текущих торгов
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<bool> CancelParamRequest(string classCode, string secCode, string paramName);

        Task<bool> CancelParamRequest(string classCode, string secCode, ParamNames paramName);

        /// <summary>
        /// Функция для получения значений Таблицы текущих значений параметров
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        Task<ParamTable> GetParamEx(string classCode, string secCode, string paramName, int timeout = Timeout.Infinite);

        Task<ParamTable> GetParamEx(string classCode, string secCode, ParamNames paramName, int timeout = Timeout.Infinite);

        /// <summary>
        /// Функция для получения всех значений Таблицы текущих значений параметров
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<ParamTable> GetParamEx2(string classCode, string secCode, string paramName);

        Task<ParamTable> GetParamEx2(string classCode, string secCode, ParamNames paramName);

        /// <summary>
        /// функция для получения таблицы сделок по заданному инструменту
        /// </summary>
        Task<List<Trade>> GetTrades();

        /// <summary>
        /// функция для получения таблицы сделок по заданному инструменту
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <returns></returns>
        Task<List<Trade>> GetTrades(string classCode, string secCode);

        /// <summary>
        /// функция для получения таблицы сделок номеру заявки
        /// </summary>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        Task<List<Trade>> GetTrades_by_OdrerNumber(long orderNum);

        ///// <summary>
        /////  функция для получения информации по инструменту
        ///// </summary>
        //Task<string> getSecurityInfo();
        ///// <summary>
        /////  функция для получения даты торговой сессии
        ///// </summary>
        //Task<string> getTradeDate();

        /// <summary>
        /// Функция отправляет транзакцию на сервер QUIK и сохраняет ее в словаре транзакций
        /// с идентификатором trans_id. Возвращает идентификатор
        /// транзакции trans_id (позитивное число) в случае успеха или индентификатор,
        /// умноженный на -1 (-trans_id) (негативное число) в случае ошибки. Также в случае
        /// ошибки функция созраняет текст ошибки в свойтво ErrorMessage транзакции.
        /// </summary>
        Task<long> SendTransaction(Transaction transaction);

        ///// <summary>
        /////  функция для расчета максимально возможного количества лотов в заявке
        ///// </summary>
        //Task<string> CulcBuySell();
        /// <summary>
        ///  функция для получения значений параметров таблицы «Клиентский портфель»
        /// </summary>
        Task<PortfolioInfo> GetPortfolioInfo(string firmId, string clientCode);

        /// <summary>
        ///  функция для получения значений параметров таблицы «Клиентский портфель» с учетом вида лимита
        ///  Для получения значений параметров таблицы «Клиентский портфель» для клиентов срочного рынка без единой денежной позиции
        ///  необходимо указать в качестве «clientCode» – торговый счет на срочном рынке, а в качестве «limitKind» – 0.
        /// </summary>
        Task<PortfolioInfoEx> GetPortfolioInfoEx(string firmId, string clientCode, int limitKind);

        ///// <summary>
        /////  функция для получения параметров таблицы «Купить/Продать»
        ///// </summary>
        //Task<string> getBuySellInfo();
        ///// <summary>
        /////  функция для получения параметров (включая вид лимита) таблицы «Купить/Продать»
        ///// </summary>
        //Task<string> getBuySellInfoEx();

        /// <summary>
        /// Функция возвращает торговый счет срочного рынка, соответствующий коду клиента фондового рынка с единой денежной позицией
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        Task<string> GetTrdAccByClientCode(string firmId, string clientCode);

        /// <summary>
        /// Функция возвращает код клиента фондового рынка с единой денежной позицией, соответствующий торговому счету срочного рынка
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="trdAccId"></param>
        /// <returns></returns>
        Task<string> GetClientCodeByTrdAcc(string firmId, string trdAccId);

        /// <summary>
        /// Функция предназначена для получения признака, указывающего имеет ли клиент единую денежную позицию
        /// </summary>
        /// <param name="firmId">идентификатор фирмы фондового рынка</param>
        /// <param name="client">код клиента фондового рынка или торговый счет срочного рынка</param>
        /// <returns></returns>
        Task<bool> IsUcpClient(string firmId, string client);
    }

    /// <summary>
    /// Функции взаимодействия скрипта Lua и Рабочего места QUIK
    /// </summary>
    public class TradingFunctions : ITradingFunctions
    {
        public TradingFunctions(int port, string host)
        {
            QuikService = QuikService.Create(port, host);
        }

        public QuikService QuikService { get; private set; }

        //public async Task<string[]> GetClassesList() {
        //    var response = await QuikService.Send<Message<string>>(
        //        (new Message<string>("", "getClassesList")));
        //    return response.Data == null
        //        ? new string[0]
        //        : response.Data.TrimEnd(',').Split(new[] { "," }, StringSplitOptions.None);
        //}

        //public async Task<ClassInfo> GetClassInfo(string classID) {
        //    var response = await QuikService.Send<Message<ClassInfo>>(
        //        (new Message<string>(classID, "getClassInfo")));
        //    return response.Data;
        //}

        //public async Task<SecurityInfo> GetSecurityInfo(string classCode, string secCode) {
        //    var response = await QuikService.Send<Message<SecurityInfo>>(
        //        (new Message<string>(classCode + "|" + secCode, "getSecurityInfo")));
        //    return response.Data;
        //}

        //public async Task<SecurityInfo> GetSecurityInfo(ISecurity security) {
        //    return await GetSecurityInfo(security.ClassCode, security.SecCode);
        //}

        //public async Task<string[]> GetClassSecurities(string classID) {
        //    var response = await QuikService.Send<Message<string>>(
        //        (new Message<string>(classID, "getClassSecurities")));
        //    return response.Data == null
        //        ? new string[0]
        //        : response.Data.TrimEnd(',').Split(new[] { "," }, StringSplitOptions.None);
        //}
        public async Task<DepoLimit> GetDepo(string clientCode, string firmId, string secCode, string account)
        {
            var response = await QuikService.Send<Message<DepoLimit>>(
                (new Message<string>(clientCode + "|" + firmId + "|" + secCode + "|" + account, "getDepo"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<DepoLimitEx> GetDepoEx(string firmId, string clientCode, string secCode, string accID, int limitKind)
        {
            var response = await QuikService.Send<Message<DepoLimitEx>>(
                (new Message<string>(firmId + "|" + clientCode + "|" + secCode + "|" + accID + "|" + limitKind, "getDepoEx"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает список всех записей из таблицы 'Лимиты по бумагам'.
        /// </summary>
        public async Task<List<DepoLimitEx>> GetDepoLimits()
        {
            var message = new Message<string>("", "get_depo_limits");
            Message<List<DepoLimitEx>> response = await QuikService.Send<Message<List<DepoLimitEx>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Возвращает список записей из таблицы 'Лимиты по бумагам', отфильтрованных по коду инструмента.
        /// </summary>
        public async Task<List<DepoLimitEx>> GetDepoLimits(string secCode)
        {
            var message = new Message<string>(secCode, "get_depo_limits");
            Message<List<DepoLimitEx>> response = await QuikService.Send<Message<List<DepoLimitEx>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция для получения информации по денежным лимитам.
        /// </summary>
        public async Task<MoneyLimit> GetMoney(string clientCode, string firmId, string tag, string currCode)
        {
            var response = await QuikService.Send<Message<MoneyLimit>>(
                (new Message<string>(clientCode + "|" + firmId + "|" + tag + "|" + currCode, "getMoney"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция для получения информации по денежным лимитам указанного типа.
        /// </summary>
        public async Task<MoneyLimitEx> GetMoneyEx(string firmId, string clientCode, string tag, string currCode, int limitKind)
        {
            var response = await QuikService.Send<Message<MoneyLimitEx>>(
                (new Message<string>(firmId + "|" + clientCode + "|" + tag + "|" + currCode + "|" + limitKind, "getMoneyEx"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        ///  функция для получения информации по денежным лимитам всех торговых счетов (кроме фьючерсных) и валют.
        ///  Лучшее место для получения связки clientCode + firmid
        /// </summary>
        public async Task<List<MoneyLimitEx>> GetMoneyLimits()
        {
            var response = await QuikService.Send<Message<List<MoneyLimitEx>>>(
                (new Message<string>("", "getMoneyLimits"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция заказывает получение параметров Таблицы текущих торгов
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public async Task<bool> ParamRequest(string classCode, string secCode, string paramName)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "paramRequest"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> ParamRequest(string classCode, string secCode, ParamNames paramName)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "paramRequest"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция отменяет заказ на получение параметров Таблицы текущих торгов
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public async Task<bool> CancelParamRequest(string classCode, string secCode, string paramName)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "cancelParamRequest"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> CancelParamRequest(string classCode, string secCode, ParamNames paramName)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "cancelParamRequest"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция для получения значений Таблицы текущих значений параметров
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public async Task<ParamTable> GetParamEx(string classCode, string secCode, string paramName, int timeout = Timeout.Infinite)
        {
            var response = await QuikService.Send<Message<ParamTable>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "getParamEx")), timeout).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<ParamTable> GetParamEx(string classCode, string secCode, ParamNames paramName, int timeout = Timeout.Infinite)
        {
            var response = await QuikService.Send<Message<ParamTable>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "getParamEx")), timeout).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция для получения всех значений Таблицы текущих значений параметров
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public async Task<ParamTable> GetParamEx2(string classCode, string secCode, string paramName)
        {
            var response = await QuikService.Send<Message<ParamTable>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "getParamEx2"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<ParamTable> GetParamEx2(string classCode, string secCode, ParamNames paramName)
        {
            var response = await QuikService.Send<Message<ParamTable>>(
                (new Message<string>(classCode + "|" + secCode + "|" + paramName, "getParamEx2"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция для получения информации по фьючерсным лимитам
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="accId"></param>
        /// <param name="limitType"></param>
        /// <param name="currCode"></param>
        /// <returns></returns>
        public async Task<FuturesLimits> GetFuturesLimit(string firmId, string accId, int limitType, string currCode)
        {
            var response = await QuikService.Send<Message<FuturesLimits>>(
                (new Message<string>(firmId + "|" + accId + "|" + limitType + "|" + currCode, "getFuturesLimit"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        ///  функция для получения информации по фьючерсным лимитам всех клиентских счетов
        /// </summary>
        public async Task<List<FuturesLimits>> GetFuturesClientLimits()
        {
            var response = await QuikService.Send<Message<List<FuturesLimits>>>(
                (new Message<string>("", "getFuturesClientLimits"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Функция для получения информации по фьючерсным позициям
        /// </summary>
        /// <param name="firmId"></param>
        /// <param name="accId"></param>
        /// <param name="secCode"></param>
        /// <param name="posType"></param>
        /// <returns></returns>
        public async Task<FuturesClientHolding> GetFuturesHolding(string firmId, string accId, string secCode, int posType)
        {
            var response = await QuikService.Send<Message<FuturesClientHolding>>(
                (new Message<string>(firmId + "|" + accId + "|" + secCode + "|" + posType, "getFuturesHolding"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<List<OptionBoard>> GetOptionBoard(string classCode, string secCode)
        {
            var message = new Message<string>(classCode + "|" + secCode, "getOptionBoard");
            Message<List<OptionBoard>> response =
                await QuikService.Send<Message<List<OptionBoard>>>(message).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<List<Trade>> GetTrades()
        {
            var response = await QuikService.Send<Message<List<Trade>>>(
                (new Message<string>("", "get_trades"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<List<Trade>> GetTrades(string classCode, string secCode)
        {
            var response = await QuikService.Send<Message<List<Trade>>>(
                (new Message<string>(classCode + "|" + secCode, "get_trades"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<List<Trade>> GetTrades_by_OdrerNumber(long orderNum)
        {
            var response = await QuikService.Send<Message<List<Trade>>>(
                (new Message<string>(orderNum.ToString(), "get_Trades_by_OrderNumber"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<PortfolioInfo> GetPortfolioInfo(string firmId, string clientCode)
        {
            var response = await QuikService.Send<Message<PortfolioInfo>>(
                (new Message<string>(firmId + "|" + clientCode, "getPortfolioInfo"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<PortfolioInfoEx> GetPortfolioInfoEx(string firmId, string clientCode, int limitKind)
        {
            var response = await QuikService.Send<Message<PortfolioInfoEx>>(
                (new Message<string>(firmId + "|" + clientCode + "|" + limitKind, "getPortfolioInfoEx"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<string> GetTrdAccByClientCode(string firmId, string clientCode)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(firmId + "|" + clientCode, "GetTrdAccByClientCode"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<string> GetClientCodeByTrdAcc(string firmId, string trdAccId)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(firmId + "|" + trdAccId, "GetClientCodeByTrdAcc"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<bool> IsUcpClient(string firmId, string client)
        {
            var response = await QuikService.Send<Message<bool>>(
                (new Message<string>(firmId + "|" + client, "IsUcpClient"))).ConfigureAwait(false);
            return response.Data;
        }

        /*public async Task<ClassInfo> GetClassInfo(string classID) {
            var response = await QuikService.Send<Message<ClassInfo>>(
                (new Message<string>(classID, "getClassInfo")));
            return response.Data;
        }*/

        /// <summary>
        /// Send a single transaction to Quik server
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<long> SendTransaction(Transaction transaction)
        {
            Trace.Assert(!transaction.TRANS_ID.HasValue, "TRANS_ID should be assigned automatically in SendTransaction functions");

            //transaction.TRANS_ID = QuikService.GetNewUniqueId();
            transaction.TRANS_ID = QuikService.GetUniqueTransactionId();

            //    Console.WriteLine("Trans Id from function = {0}", transaction.TRANS_ID);

            //Trace.Assert(transaction.CLIENT_CODE == null,
            //    "Currently we use Comment to store correlation id for a transaction, " +
            //    "its reply, trades and orders. Support for comments will be added later if needed");
            //// TODO Comments are useful to kill all orders with a single KILL_ALL_ORDERS
            //// But see e.g. this http://www.quik.ru/forum/import/27073/27076/

            //transaction.CLIENT_CODE = transaction.TRANS_ID.Value.ToString();

            if (transaction.CLIENT_CODE == null) transaction.CLIENT_CODE = transaction.TRANS_ID.Value.ToString();

            //this can be longer than 20 chars.
            //transaction.CLIENT_CODE = QuikService.PrependWithSessionId(transaction.TRANS_ID.Value);

            try
            {
                var response = await QuikService.Send<Message<bool>>(
                    (new Message<Transaction>(transaction, "sendTransaction"))).ConfigureAwait(false);
                Trace.Assert(response.Data);

                // store transaction
                QuikService.Storage.Set(transaction.CLIENT_CODE, transaction);

                return transaction.TRANS_ID.Value;
            }
            catch (TransactionException e)
            {
                transaction.ErrorMessage = e.Message;
                // dirty hack: if transaction was sent we return its id,
                // else we return negative id so the caller will know that
                // the transaction was not sent
                return (-transaction.TRANS_ID.Value);
            }
        }
    }
}
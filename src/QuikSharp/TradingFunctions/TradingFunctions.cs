// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using QuikSharp.DataStructures.Transaction;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// getParamEx - функция для получения значений Таблицы текущих значений параметров
    /// getTradeDate - функция для получения даты торговой сессии
    /// sendTransaction - функция для работы с заявками
    /// CulcBuySell - функция для расчета максимально возможного количества лотов в заявке
    /// getPortfolioInfo - функция для получения значений параметров таблицы «Клиентский портфель»
    /// getPortfolioInfoEx - функция для получения значений параметров таблицы «Клиентский портфель» с учетом вида лимита
    /// getBuySellInfo - функция для получения параметров таблицы «Купить/Продать»
    /// getBuySellInfoEx - функция для получения параметров (включая вид лимита) таблицы «Купить/Продать»
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

        ///// <summary>
        /////  функция для получения информации по фьючерсным лимитам
        ///// </summary>
        //Task<string> getFuturesLimit();
        ///// <summary>
        /////  функция для получения информации по фьючерсным позициям
        ///// </summary>
        Task<FuturesClientHolding> GetFuturesHolding(string firmId, string accId, string secCode, int posType);

        /// <summary>
        /// Функция получения доски опционов
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <returns></returns>
        Task<List<OptionBoard>> GetOptionBoard(string classCode, string secCode);

        /// <summary>
        /// Функция для получения значений Таблицы текущих значений параметров
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        Task<ParamTable> GetParamEx(string classCode, string secCode, string paramName);
        Task<ParamTable> GetParamEx(string classCode, string secCode, ParamNames paramName);

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
    }

    /// <summary>
    /// Функции взаимодействия скрипта Lua и Рабочего места QUIK
    /// </summary>
    public class TradingFunctions : ITradingFunctions
    {
        public TradingFunctions(int port)
        {
            QuikService = QuikService.Create(port);
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
        /// Функция для получения значений Таблицы текущих значений параметров
        /// </summary>
        /// <param name="classCode"></param>
        /// <param name="secCode"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        public async Task<ParamTable> GetParamEx(string classCode, string secCode, string paramName)
        {
            var response = await QuikService.Send<Message<ParamTable>>(
                    (new Message<string>(classCode + "|" + secCode + "|" + paramName, "getParamEx"))).ConfigureAwait(false);
            return response.Data;
        }
        public async Task<ParamTable> GetParamEx(string classCode, string secCode, ParamNames paramName)
        {
            var response = await QuikService.Send<Message<ParamTable>>(
                    (new Message<string>(classCode + "|" + secCode + "|" + paramName, "getParamEx"))).ConfigureAwait(false);
            return response.Data;
        }

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

            transaction.TRANS_ID = QuikService.GetNewUniqueId();

            //    Console.WriteLine("Trans Id from function = {0}", transaction.TRANS_ID);

            Trace.Assert(transaction.CLIENT_CODE == null,
                "Currently we use Comment to store correlation id for a transaction, " +
                "its reply, trades and orders. Support for comments will be added later if needed");
            // TODO Comments are useful to kill all orders with a single KILL_ALL_ORDERS
            // But see e.g. this http://www.quik.ru/forum/import/27073/27076/

            transaction.CLIENT_CODE = transaction.TRANS_ID.Value.ToString();

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
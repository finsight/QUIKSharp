// Copyright (C) 2014 Victor Baybekov

using System;
using System.Threading.Tasks;

namespace QuikSharp {

    /// <summary>
    /// Функции взаимодействия скрипта Lua и Рабочего места QUIK
    /// getDepo - функция для получения информации по бумажным лимитам 
    /// getDepoEx - функция для получения информации по бумажным лимитам указанного типа 
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
    public interface ITradingFunctions : IQuikFunctions {

        /// <summary>
        /// Функция для получения информации по бумажным лимитам
        /// </summary>
        Task<DepoLimit> GetDepo(string clientCode, string firmId, string secCode, string account);

        /// <summary>
        /// Функция для получения информации по бумажным лимитам указанного типа
        /// </summary>
        Task<DepoLimitEx> GetDepoEx(string firmId, string clientCode, string secCode, string accID, int limitKind);

        /// <summary>
        /// Функция для получения информации по денежным лимитам
        /// </summary>
        /// 
        Task<MoneyLimit> GetMoney(string clientCode, string firmId, string tag, string currCode);
        /// <summary>
        ///  функция для получения информации по денежным лимитам указанного типа
        /// </summary>
        Task<MoneyLimitEx> GetMoneyEx(string firmId, string clientCode, string tag, string curr_code, int limitKind);
        ///// <summary>
        /////  функция для получения информации по фьючерсным лимитам
        ///// </summary>
        //Task<string> getFuturesLimit();
        ///// <summary>
        /////  функция для получения информации по фьючерсным позициям
        ///// </summary>
        //Task<string> getFuturesHolding();
        ///// <summary>
        /////  функция для получения значений Таблицы текущих значений параметров
        ///// </summary>
        //Task<string> getParamEx();

        ///// <summary>
        /////  функция для получения информации по инструменту
        ///// </summary>
        //Task<string> getSecurityInfo();
        ///// <summary>
        /////  функция для получения даты торговой сессии
        ///// </summary>
        //Task<string> getTradeDate();

        /// <summary>
        /// Функция отправляет транзакцию на сервер QUIK. В случае ошибки 
        /// обработки транзакции в терминале QUIK Task выдаст ошибку типа 
        /// LuaException с описанием ошибки. 
        /// Функция асинхронно ждет результат транзакции, возвращаемый OnTransReply.
        /// </summary>
        Task<TransactionReply> SendTransaction(TransactionSpecification transaction);

        ///// <summary>
        /////  функция для расчета максимально возможного количества лотов в заявке
        ///// </summary>
        //Task<string> CulcBuySell();
        ///// <summary>
        /////  функция для получения значений параметров таблицы «Клиентский портфель»
        ///// </summary>
        //Task<string> getPortfolioInfo();
        ///// <summary>
        /////  функция для получения значений параметров таблицы «Клиентский портфель» с учетом вида лимита
        ///// </summary>
        //Task<string> getPortfolioInfoEx();
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
    public class TradingFunctions : ITradingFunctions {
        public TradingFunctions(int port) { QuikService = QuikService.Create(port); }

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
        public Task<DepoLimit> GetDepo(string clientCode, string firmId, string secCode, string account) { throw new NotImplementedException(); }
        public Task<DepoLimitEx> GetDepoEx(string firmId, string clientCode, string secCode, string accID, int limitKind) { throw new NotImplementedException(); }
        public Task<MoneyLimit> GetMoney(string clientCode, string firmId, string tag, string currCode) { throw new NotImplementedException(); }
        public Task<MoneyLimitEx> GetMoneyEx(string firmId, string clientCode, string tag, string curr_code, int limitKind) { throw new NotImplementedException(); }


        public async Task<TransactionReply> SendTransaction(TransactionSpecification transaction) {
            // this is what Send doesn anyway, but to illustrate
            // that Lua will set a message id inside OnTransReply
            // and we will receive OnTransReply table right here if 
            // sendTransaction was successful 
            if (!transaction.TRANS_ID.HasValue) {
                transaction.TRANS_ID = QuikService.GetNewUniqueId();
            }
            var response = await QuikService.Send<Message<TransactionReply>>(
                (new Message<TransactionSpecification>(transaction, "sendTransaction")));
            return response.Data;
        }
    }
}

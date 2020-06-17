// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using QuikSharp.DataStructures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuikSharp
{
    /// <summary>
    /// Функции для обращения к спискам доступных параметров
    /// </summary>
    public interface IClassFunctions : IQuikService
    {
        /// <summary>
        /// Функция предназначена для получения списка кодов классов, переданных с сервера в ходе сеанса связи.
        /// </summary>
        /// <returns></returns>
        Task<string[]> GetClassesList();

        /// <summary>
        /// Функция предназначена для получения информации о классе.
        /// </summary>
        /// <param name="classID"></param>
        Task<ClassInfo> GetClassInfo(string classID);

        /// <summary>
        /// Функция предназначена для получения информации по бумаге.
        /// </summary>
        Task<SecurityInfo> GetSecurityInfo(string classCode, string secCode);

        /// <summary>
        /// Функция предназначена для получения информации по бумаге.
        /// </summary>
        Task<SecurityInfo> GetSecurityInfo(ISecurity security);

        /// <summary>
        /// Функция предназначена для получения списка кодов бумаг для списка классов, заданного списком кодов.
        /// </summary>
        Task<string[]> GetClassSecurities(string classID);

        /// <summary>
        /// Функция предназначена для определения класса по коду инструмента из заданного списка классов.
        /// </summary>
        Task<string> GetSecurityClass(string classesList, string secCode);

        /// <summary>
        /// Функция возвращает код клиента.
        /// </summary>
        Task<string> GetClientCode();

        /// <summary>
        /// Функция возвращает таблицу с описанием торгового счета для запрашиваемого кода класса.
        /// </summary>
        Task<string> GetTradeAccount(string classCode);

        /// <summary>
        /// Функция возвращает таблицу всех счетов в торговой системе.
        /// </summary>
        /// <returns></returns>
        Task<List<TradesAccounts>> GetTradeAccounts();
    }

    /// <summary>
    /// Функции для обращения к спискам доступных параметров
    /// </summary>
    public class ClassFunctions : IClassFunctions
    {
        public ClassFunctions(int port, string host)
        {
            QuikService = QuikService.Create(port, host);
        }

        public QuikService QuikService { get; private set; }

        public async Task<string[]> GetClassesList()
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getClassesList"))).ConfigureAwait(false);
            return response.Data == null
                ? new string[0]
                : response.Data.TrimEnd(',').Split(new[] {","}, StringSplitOptions.None);
        }

        public async Task<ClassInfo> GetClassInfo(string classID)
        {
            var response = await QuikService.Send<Message<ClassInfo>>(
                (new Message<string>(classID, "getClassInfo"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<SecurityInfo> GetSecurityInfo(string classCode, string secCode)
        {
            var response = await QuikService.Send<Message<SecurityInfo>>(
                (new Message<string>(classCode + "|" + secCode, "getSecurityInfo"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<SecurityInfo> GetSecurityInfo(ISecurity security)
        {
            return await GetSecurityInfo(security.ClassCode, security.SecCode).ConfigureAwait(false);
        }

        public async Task<string[]> GetClassSecurities(string classID)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(classID, "getClassSecurities"))).ConfigureAwait(false);
            return response.Data == null
                ? new string[0]
                : response.Data.TrimEnd(',').Split(new[] {","}, StringSplitOptions.None);
        }

        public async Task<string> GetSecurityClass(string classesList, string secCode)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(classesList + "|" + secCode, "getSecurityClass"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<string> GetClientCode()
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getClientCode"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<string> GetTradeAccount(string classCode)
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(classCode, "getTradeAccount"))).ConfigureAwait(false);
            return response.Data;
        }

        public async Task<List<TradesAccounts>> GetTradeAccounts()
        {
            var response = await QuikService.Send<Message<List<TradesAccounts>>>(
                (new Message<string>("", "getTradeAccounts"))).ConfigureAwait(false);
            return response.Data;
        }
    }
}
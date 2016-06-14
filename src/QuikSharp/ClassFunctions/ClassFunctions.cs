// Copyright (C) 2015 Victor Baybekov

using System;
using System.Threading.Tasks;
using QuikSharp.DataStructures;

namespace QuikSharp {

    /// <summary>
    /// Функции для обращения к спискам доступных параметров
    /// </summary>
    public interface IClassFunctions : IQuikService {

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
    }

    /// <summary>
    /// Функции для обращения к спискам доступных параметров
    /// </summary>
    public class ClassFunctions : IClassFunctions {
        public ClassFunctions(int port) { QuikService = QuikService.Create(port); }

        public QuikService QuikService { get; private set; }

        
        public async Task<string[]> GetClassesList() {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "getClassesList")));
            return response.Data == null 
                ? new string[0]
                : response.Data.TrimEnd(',').Split(new [] {","}, StringSplitOptions.None);
        }

        public async Task<ClassInfo> GetClassInfo(string classID) {
            var response = await QuikService.Send<Message<ClassInfo>>(
                (new Message<string>(classID, "getClassInfo")));
            return response.Data;
        }

        public async Task<SecurityInfo> GetSecurityInfo(string classCode, string secCode) {
            var response = await QuikService.Send<Message<SecurityInfo>>(
                (new Message<string>(classCode + "|" + secCode, "getSecurityInfo")));
            return response.Data;
        }

        public async Task<SecurityInfo> GetSecurityInfo(ISecurity security) {
            return await GetSecurityInfo(security.ClassCode, security.SecCode);
        }

        public async Task<string[]> GetClassSecurities(string classID) {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>(classID, "getClassSecurities")));
            return response.Data == null 
                ? new string[0]
                : response.Data.TrimEnd(',').Split(new[] { "," }, StringSplitOptions.None);
        }
    }
}

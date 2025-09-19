using System;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum NegDealsFlags
    {
        /// <summary>
        /// Валюты на адресной заявке заполнены в соответствии с общими правилами заполнения валют
        /// </summary>
        CurrenciesFilledGeneralRules = 0x1,

        /// <summary>
        /// Признак заявки типа РЕПО с открытой датой 
        /// </summary>
        OpenDatedRepoOrderFlag = 0x2
    }
}
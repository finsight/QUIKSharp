using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При удалении лимита по срочному рынку функция возвращает таблицу Lua с параметрами
    /// </summary>
    public class FuturesLimitDelete
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код торгового счета
        /// </summary>
        [JsonProperty("trdaccid")]
        public string TrdAccId { get; set; }

        /// <summary>
        /// Тип лимита
        /// </summary>
        [JsonProperty("limit_type")]
        public string LimitType { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
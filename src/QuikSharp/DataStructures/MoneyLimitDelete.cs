using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// При удалении клиентского лимита по бумагам функция возвращает таблицу Lua с параметрами
    /// </summary>
    public class MoneyLimitDelete
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// Код валюты
        /// </summary>
        [JsonProperty("currcode")]
        public string CurrCode { get; set; }

        /// <summary>
        /// Тэг расчетов
        /// </summary>
        [JsonProperty("tag")]
        public string Tag { get; set; }

        /// <summary>
        /// Код клиента
        /// </summary>
        [JsonProperty("client_code")]
        public string ClientCode { get; set; }

        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Тип лимита. Возможные значения:
        /// «0» – обычные лимиты,
        /// иначе – технологические лимиты
        /// </summary>
        [JsonProperty("limit_kind")]
        public int LimitKind { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
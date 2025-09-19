using Newtonsoft.Json;

namespace QuikSharp.DataStructures.Transaction
{
    /// <summary>
    /// Описание параметров таблицы Отчеты по сделкам для исполнения
    /// </summary>
    public class NegDealReport : IWithLuaTimeStamp
    {
        [JsonProperty("lua_timestamp")]
        public long LuaTimeStamp { get; internal set; }

        /// <summary>
        /// Отчет
        /// </summary>
        [JsonProperty("report_num")]
        public long ReportNumber { get; set; }

        /// <summary>
        /// Дата отчета
        /// </summary>
        [JsonProperty("report_date")]
        public int ReportDate { get; set; }

        /// <summary>
        /// Набор битовых флагов
        /// </summary>
        [JsonProperty("flags")]
        public NegReportFlags Flags { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [JsonProperty("userid")]
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор фирмы
        /// </summary>
        [JsonProperty("firmid")]
        public string FirmId { get; set; }

        /// <summary>
        /// Счет депо
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Код фирмы партнера
        /// </summary>
        [JsonProperty("cpfirmid")]
        public string CpFirmId { get; set; }

        /// <summary>
        /// Код торгового счета партнера
        /// </summary>
        [JsonProperty("cpaccount")]
        public string CpAccount { get; set; }

        /// <summary>
        /// Количество инструментов, в лотах
        /// </summary>
        [JsonProperty("qty")]
        public int Quantity { get; set; }

        /// <summary>
        /// Объем сделки, выраженный в рублях
        /// </summary>
        [JsonProperty("value")]
        public decimal Value { get; set; }

        /// <summary>
        /// Время снятия заявки
        /// </summary>
        [JsonProperty("withdraw_time")]
        public int WithdrawTime { get; set; }

        /// <summary>
        /// Тип отчета
        /// </summary>
        [JsonProperty("report_type")]
        public int ReportType { get; set; }

        /// <summary>
        /// Вид отчета
        /// </summary>
        [JsonProperty("report_kind")]
        public int ReportKind { get; set; }

        /// <summary>
        /// Объем комиссии по сделке, выраженный в руб
        /// </summary>
        [JsonProperty("commission")]
        public decimal Commission { get; set; }

        /// <summary>
        /// Код инструмента
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// Код класса
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }

        /// <summary>
        /// Время отчета
        /// </summary>
        [JsonProperty("report_time")]
        public int ReportTime { get; set; }

        /// <summary>
        /// Дата и время отчета
        /// </summary>
        [JsonProperty("report_date_time")]
        public QuikDateTime ReportDateTime { get; set; }
    }
}
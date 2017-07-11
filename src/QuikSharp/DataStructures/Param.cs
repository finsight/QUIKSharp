using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    public class Param
    {
        /// <summary>
        /// sec_code  STRING  Код бумаги
        /// </summary>
        [JsonProperty("sec_code")]
        public string SecCode { get; set; }

        /// <summary>
        /// class_code  STRING  Код бумаги
        /// </summary>
        [JsonProperty("class_code")]
        public string ClassCode { get; set; }
    }
}
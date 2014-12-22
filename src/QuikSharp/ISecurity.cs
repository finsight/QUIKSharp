using Newtonsoft.Json;

namespace QuikSharp {
    /// <summary>
    /// 
    /// </summary>
    public interface ISecurity {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("class_code")]
        string ClassCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("sec_code")]
        string SecCode { get; set; }
    }
}

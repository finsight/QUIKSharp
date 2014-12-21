using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace QuikSharp {
    public interface ISecurity {
        [JsonProperty("class_code")]
        string ClassCode { get; set; }

        [JsonProperty("sec_code")]
        string SecCode { get; set; }
    }
}

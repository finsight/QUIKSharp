// Copyright (C) 2015 Victor Baybekov

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

		/// <summary>
		/// Свойство возвращает коммбинацию ClassCode и SecCode в выбраном пользователем формате, 
		///  например: $"{ClassCode}@{SecCode}" или $"{ClassCode}-{SecCode}"
		/// </summary>
		string FullCode
		{
			get;
		}

	}
}

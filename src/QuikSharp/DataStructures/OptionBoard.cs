// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    /// OptionBoard structure
    /// </summary>
    public class OptionBoard
    {
        /// <summary>
        /// Strike
        /// </summary>
        [JsonProperty("Strike")]
        public double Strike { get; set; }

        /// <summary>
        /// Code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Volatility
        /// </summary>
        [JsonProperty("Volatility")]
        public double Volatility { get; set; }

        /// <summary>
        /// OptionBase
        /// </summary>
        [JsonProperty("OPTIONBASE")]
        public string OPTIONBASE { get; set; }

        /// <summary>
        /// Offer
        /// </summary>
        [JsonProperty("OFFER")]
        public double OFFER { get; set; }

        /// <summary>
        /// Longname
        /// </summary>
        [JsonProperty("Longname")]
        public string Longname { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [JsonProperty("Name")]
        public string Name { get; set; }

        /// <summary>
        /// OptionType
        /// </summary>
        [JsonProperty("OPTIONTYPE")]
        public string OPTIONTYPE { get; set; }

        /// <summary>
        /// ShortName
        /// </summary>
        [JsonProperty("shortname")]
        public string Shortname { get; set; }

        /// <summary>
        /// Bid
        /// </summary>
        [JsonProperty("BID")]
        public double BID { get; set; }

        /// <summary>
        /// DaysToMatDate
        /// </summary>
        [JsonProperty("DAYS_TO_MAT_DATE")]
        public int DAYSTOMATDATE { get; set; }
    }
}
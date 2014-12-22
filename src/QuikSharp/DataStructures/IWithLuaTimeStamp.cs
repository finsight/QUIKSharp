// Copyright (C) 2014 Victor Baybekov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures {
    /// <summary>
    /// 
    /// </summary>
    public interface IWithLuaTimeStamp {
        /// <summary>
        /// Lua timestamp
        /// </summary>
        [JsonProperty("lua_timestamp")]
        long LuaTimeStamp { get; }
    }
}

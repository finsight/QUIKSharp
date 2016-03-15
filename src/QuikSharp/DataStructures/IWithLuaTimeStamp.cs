// Copyright (C) 2015 Victor Baybekov

using Newtonsoft.Json;

namespace QuikSharp.DataStructures {
    /// <summary>
    /// 
    /// </summary>
    public interface IWithLuaTimeStamp {
        // TODO change to TimeStamp without refactoring and add cast to DateTime
        // then replace all assignments. 
        /// <summary>
        /// Lua timestamp
        /// </summary>
        [JsonProperty("lua_timestamp")]
        long LuaTimeStamp { get; }
    }
}

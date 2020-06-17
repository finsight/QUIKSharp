// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using Newtonsoft.Json;

namespace QuikSharp.DataStructures
{
    /// <summary>
    ///
    /// </summary>
    public interface IWithLuaTimeStamp
    {
        // TODO change to TimeStamp without refactoring and add cast to DateTime
        // then replace all assignments.
        /// <summary>
        /// Lua timestamp
        /// </summary>
        [JsonProperty("lua_timestamp")]
        long LuaTimeStamp { get; }
    }
}
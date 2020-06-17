// Copyright (c) 2014-2020 QUIKSharp Authors https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE.txt in the project root for license information.

using System.Diagnostics;
using System.Threading.Tasks;

namespace QuikSharp
{
    public class DebugFunctions : IQuikService
    {
        public DebugFunctions(int port, string host)
        {
            QuikService = QuikService.Create(port, host);
        }

        public QuikService QuikService { get; private set; }

        private class PingRequest : Message<string>
        {
            public PingRequest()
                : base("Ping", "ping", null)
            {
            }
        }

        private class PingResponse : Message<string>
        {
            public PingResponse()
                : base("Pong", "ping", null)
            {
            }
        }

        public async Task<string> Ping()
        {
            // could have used StringMessage directly. This is an example of how to define DTOs for custom commands
            var response = await QuikService.Send<PingResponse>((new PingRequest())).ConfigureAwait(false);
            Trace.Assert(response.Data == "Pong");
            return response.Data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="msg"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> Echo<T>(T msg)
        {
            // could have used StringMessage directly. This is an example of how to define DTOs for custom commands
            var response = await QuikService.Send<Message<T>>(
                (new Message<T>(msg, "echo"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// This method returns LuaException and demonstrates how Lua errors are caught
        /// </summary>
        /// <returns></returns>
        public async Task<string> DivideStringByZero()
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "divide_string_by_zero"))).ConfigureAwait(false);
            return response.Data;
        }

        /// <summary>
        /// Check if running inside Quik
        /// </summary>
        public async Task<bool> IsQuik()
        {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "is_quik"))).ConfigureAwait(false);
            return response.Data == "1";
        }
    }
}
using System.Diagnostics;
using System.Threading.Tasks;

namespace QuikSharp.Quik {
    public class DebugFunctions : IQuikFunctions {
        public DebugFunctions() {
            QuikService.Start();
        }
        class PingRequest : StringMessage {
            public PingRequest()
                : base("Ping", "ping", null) { }
        }

        class PingResponse : StringMessage {
            public PingResponse()
                : base("Pong", "ping", null) { }
        }

        public async Task<string> Ping() {
            // could have used StringMessage directly. This is an example of how to define DTOs for custom commands
            var response = await (new PingRequest()).Send<PingResponse>();
            Trace.Assert(response.Data == "Pong");
            return response.Data;
        }

        /// <summary>
        /// This method returns LuaException and demonstrates how Lua errors are caught
        /// </summary>
        /// <returns></returns>
        public async Task<string> DivideStringByZero() {
            var response = await (new StringMessage("", "divide_string_by_zero")).Send<StringMessage>();
            return response.Data;
        }
    }
}

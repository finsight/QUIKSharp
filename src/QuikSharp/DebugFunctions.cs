using System.Diagnostics;
using System.Threading.Tasks;

namespace QuikSharp {
    public class DebugFunctions : IQuikFunctions {
        public DebugFunctions(int port) { QuikService = new QuikService(port); }

        public QuikService QuikService { get; private set; }
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
            var response = await QuikService.Send<PingResponse>((new PingRequest()));
            Trace.Assert(response.Data == "Pong");
            return response.Data;
        }

        /// <summary>
        /// This method returns LuaException and demonstrates how Lua errors are caught
        /// </summary>
        /// <returns></returns>
        public async Task<string> DivideStringByZero() {
            var response = await QuikService.Send<StringMessage>((new StringMessage("", "divide_string_by_zero")));
            return response.Data;
        }

    }
}

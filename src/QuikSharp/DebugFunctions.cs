using System.Diagnostics;
using System.Threading.Tasks;

namespace QuikSharp {
    public class DebugFunctions : IQuikFunctions {
        public DebugFunctions(int port) { QuikService = QuikService.Create(port); }

        public QuikService QuikService { get; private set; }
        class PingRequest : Message<string> {
            public PingRequest()
                : base("Ping", "ping", null) { }
        }

        class PingResponse : Message<string> {
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
            var response = await QuikService.Send<Message<string>>((new Message<string>("", "divide_string_by_zero")));
            return response.Data;
        }

        /// <summary>
        /// Check if running inside Quik
        /// </summary>
        public async Task<bool> IsQuik() {
            var response = await QuikService.Send<Message<string>>(
                (new Message<string>("", "is_quik")));
            return response.Data == "1";
        }

    }
}

--~ (c) Victor Baybekov, 2014 All Rights Reserved
package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local util = require("qsutils")
local qf = require("qsfunctions")
require("qscallbacks")

local is_started = true

function main()
	log("Entered main function", 1)
	while is_started do
		-- if not connected, connect
		util.connect()
		-- when connected, process queue
		-- receive message,
		local requestMsg = receiveRequest()
		if requestMsg then
			-- if ok, process message
			local responseMsg = qf.dispatch_and_process(requestMsg)
			if responseMsg then
				-- send message
				local res = sendResponse(responseMsg)
			else
				log("Could not dispatch and process request: "..requestMsg, 3)
			end
		else
			delay(1)
		end
	end
end

if not is_quik() then
	log("Hello, QuikSharp! Running outside Quik.")
	main()
	logfile:close()
end


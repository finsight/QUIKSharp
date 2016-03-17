<<<<<<< HEAD
--~ Copyright Ⓒ 2015 Victor Baybekov
=======
--~ Copyright Ⓒ 2014 Victor Baybekov
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

-- is running from Quik
function is_quik()
    if getScriptPath then return true else return false end
end
<<<<<<< HEAD
script_path = "."
if is_quik() then
    script_path = getScriptPath()
	package.loadlib(getScriptPath()  .. "\\clibs\\lua51.dll", "main")
end
package.path = package.path .. ";" .. script_path .. "\\?.lua;" .. script_path .. "\\?.luac"..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath .. ";" .. script_path .. '\\clibs\\?.dll'..";"..'.\\clibs\\?.dll'

=======
if is_quik() then
    package.path = package.path .. ";" .. getScriptPath() .. "\\?.lua;" .. getScriptPath() .. "\\?.luac"
    package.cpath = package.cpath .. ";" .. getScriptPath() .. '\\clibs\\?.dll'
else
    package.path = package.path .. ";" .. ".\\?.lua;" .. ".\\?.luac"
    package.cpath = package.cpath .. ";" .. '.\\clibs\\?.dll'
end
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
local util = require("qsutils")
local qf = require("qsfunctions")
require("qscallbacks")

local is_started = true

function do_main()
<<<<<<< HEAD
    log("Entered main function", 0)
=======
    log("Entered main function")
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
    while is_started do
        -- if not connected, connect
        util.connect()
        -- when connected, process queue
        -- receive message,
        local requestMsg = receiveRequest()
        if requestMsg then
            -- if ok, process message
            -- dispatch_and_process never throws, it returns lua errors wrapped as a message
            local responseMsg, err = qf.dispatch_and_process(requestMsg)
            if responseMsg then
                -- send message
                local res = sendResponse(responseMsg)
            else
                log("Could not dispatch and process request: " .. err, 3)
            end
        else
            delay(1)
        end
    end
end

<<<<<<< HEAD
--- catch errors
=======
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function main()
    local status, err = pcall(do_main)
    if status then
        log("finished")
    else
        log(err, 3)
    end
end

if not is_quik() then
    log("Hello, QuikSharp! Running outside Quik.")
    do_main()
    logfile:close()
end


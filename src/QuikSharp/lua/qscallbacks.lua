--~ (c) Victor Baybekov, 2014 All Rights Reserved
package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local util = require("qsutils")

local qscallbacks = {}

function OnQuikSharpDisconnected()
    -- TODO any recovery or risk management logic here
end

function OnInit()
    log("Hello, QuikSharp! Running inside Quik from the path: "..getScriptPath())
end

function OnStop(s)
    log("Bye, QuikSharp!")
    is_started = false
    pcall(logfile:close(logfile))
    if missed_values_file then
        pcall(missed_values_file:close(missed_values_file))
        missed_values_file = nil
        missed_values_file_name = nil
    end
    --	send disconnect
    return 1000
end

return qscallbacks
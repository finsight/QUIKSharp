--~ (c) Victor Baybekov, 2014 All Rights Reserved
package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local qsfunctions = {}


------------------------------
-- Service functions
------------------------------
local service = {}
--- Echoes its message
-- @param msg message table
-- @return same msg table
function service.ping(msg)
    -- need to know data structure the caller gives
    msg.t = nil
    if msg.d.Ping == "Ping" then
        msg.d = {}
        msg.d.Pong =  "Pong"
        return msg
    else
        local ret = msg.d.Ping.." is not Ping"
        msg.d = {}
        msg.d.Pong =  ret
        return msg
    end
end
qsfunctions.service = service


function qsfunctions.dispatch_and_process(msg)
    return qsfunctions.service.ping(msg)
end

return qsfunctions
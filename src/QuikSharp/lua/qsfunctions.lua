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
function service.echo(msg)
return msg
end
qsfunctions.service = service


function qsfunctions.dispatch_and_process(msg)
    return qf.service.echo(msg)
end

return qsfunctions
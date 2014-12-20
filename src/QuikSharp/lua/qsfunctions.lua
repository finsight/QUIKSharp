--~ (c) Victor Baybekov, 2014 All Rights Reserved
package.path = package.path .. ";" .. ".\\?.lua;" .. ".\\?.luac"
package.cpath = package.cpath .. ";" .. '.\\clibs\\?.dll'

local qsfunctions = {}

function qsfunctions.dispatch_and_process(msg)
    if qsfunctions[msg.cmd] then
        -- dispatch a command simply by a table lookup
        -- in qsfunctions method names must match commands
        local status, result = pcall(qsfunctions[msg.cmd], msg)
        if status then
            return result
        else
            msg.cmd = "lua_error"
            msg.lua_error = "Lua error: " .. result
            return msg
        end
    else
        msg.cmd = "lua_error"
        msg.lua_error = "Command not implemented in Lua qsfunctions module: " .. msg.cmd
        return msg
    end
end


------------------------------
-- Service functions
------------------------------
--- Echoes its message
-- @param msg message table
-- @return same msg table
function qsfunctions.ping(msg)
    -- need to know data structure the caller gives
    msg.t = nil
    if msg.d == "Ping" then
        msg.d = "Pong"
        return msg
    else
        msg.d = msg.d .. " is not Ping"
        return msg
    end
end

--- Test error handling
function qsfunctions.divide_string_by_zero(msg)
    msg.d = "asd" / 1
    return msg
end


return qsfunctions
--~ (c) Victor Baybekov, 2014 All Rights Reserved
package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'
local socket = require ("socket")
local json = require ("dkjson")
require ("list")

local qsutils = {}

--- Sleep that always works
function delay(msec)
    if sleep then
        pcall(sleep, msec)
    else
        pcall(socket.select, nil, nil, msec / 1000)
    end
end

-- high precision current time
function timemsec()
    local st, res = pcall(socket.gettime)
    if st then
        return (res) * 1000
    else
        log("unexpected error in timemsec", 3)
        error("unexpected error in timemsec")
    end
end

-- is running from Quik
function is_quik()
    if getScriptPath then return true else return false end
end

os.execute("mkdir " .. "logs")
logfile = io.open ("logs/QuikSharp.log", "a")
missed_values_file = nil
missed_values_file_name = nil

function log(msg, level)
    if level ~= 2 and level ~= 3 then
        level = 1
    else
        -- only warnings and recoverable errors to Quik
    end
        if message then
            pcall(message, msg, level)
        end
    local logLine = "LOG "..level..": "..msg
    print(logLine)
    pcall(logfile.write, logfile, timemsec().." "..logLine.."\n")
    pcall(logfile.flush, logfile)
end

-- current connection state
local is_connected = false


-- a queue for responses/callbacks that cannot be sent
--local response_queue = List.new()
-- if cannot receive or send then put all responses to the queue
-- then reconnect from the main function and consume the queue + reset last_disconnected_time
--local last_disconnected_time
-- we need a limit to avoid out of memory exceptions, but at the same time we must
-- keep some data even if we had to disconnect the server temporary. E.g. history of limit order books is
-- not available from Quik and we do not want to lose it too often.
--local flush_timeout_sec = 10 * 60 -- 10 minutes
--local function should_flush()
--    if os.time() - last_disconnected_time < flush_timeout_sec then
--        return false
--    else
--        return true
--    end
--end

local requestPort = 34130
local responsePort = 34131
local requestClient
local responseClient

-- block until connection
local function connectClient(port)
    print('Connecting to port '..port..' ...')
    local i = 0
    while true do
        local tcp = socket.tcp()
        tcp:settimeout(2)
        local status, ret = pcall(tcp.connect, tcp, 'localhost', port)
        if status and ret then -- and client 
            return tcp
        else
            delay(100)
            i = i + 1
            if math.fmod(i,1) == 0 then log('Connection attempt #'..i) end
        end
    end
    return c
end

function qsutils.connect()
    -- TODO bad logic, keep it for a while as an example how to use queue
--    if not is_connected and should_flush() then
--        local pop_msg = List.popleft(response_queue)
--        while pop_msg and pop_msg.t / 1000 < os.time() - flush_timeout_sec do
--            -- write to file
--            pop_msg = List.popleft(response_queue)
--        end
--    end

    if not is_connected then
        log('Connecting...')
        if requestClient then
            -- Quik crashes without pcall
            pcall(requestClient.close, requestClient)
        end
        requestClient = connectClient(requestPort)
        if responseClient then
            -- Quik crashes without pcall
            pcall(responseClient.close, responseClient)
        end
        responseClient = connectClient(responsePort)
        if requestClient and responseClient then
            is_connected = true
            log 'Connected!'
            if missed_values_file then
                missed_values_file:flush()
                missed_values_file:close()
                missed_values_file = nil
                local previous_file_name = missed_values_file_name
                missed_values_file_name = nil
                for line in io.lines(previous_file_name) do
                    responseClient:send(line..'\n')
                end
            end
        end
    end
end

local function disconnected()
    is_connected = false
--    last_disconnected_time = os.time()
    print('Disconnecting...')
    if requestClient then
        pcall(requestClient.close, requestClient)
        requestClient = nil
    end
    if responseClient then
        pcall(responseClient.close, responseClient)
        responseClient = nil
    end
end

--- get a decoded message as a table
function receiveRequest()
    if not is_connected then
        return nil, "not conencted"
    end
    local status, requestString= pcall(requestClient.receive, requestClient)
    if status and requestString then
        local msg_table, pos, err = json.decode(requestString, 1, json.null)
        if err then
            log(err, 3)
            return nil, err
        else
            --log(requestString)
            return msg_table
        end
    else
        disconnected()
        return nil, err
    end
end

function sendResponse(msg_table)
    -- if not set explicitly then set CreatedTime "t" property here
    if not msg_table.t then msg_table.t = timemsec() end
    local responseString = json.encode (msg_table, { indent = false })
    if is_connected then
        local status, res = pcall(responseClient.send, responseClient, responseString..'\n')
        if status and res then
            --log(responseString)
            return true
        else
            disconnected()
            return nil, err
        end
    end
    -- we need this break instead of else because we could lose connection inside the previous if
    if not is_connected then
        if not missed_values_file then
            missed_values_file_name = "logs/MissedValues."..os.time()..".log"
            missed_values_file = io.open(missed_values_file_name, "a")
        end
        missed_values_file:write(responseString..'\n')
        return nil, "Message added to the response queue"
    end
end

return qsutils
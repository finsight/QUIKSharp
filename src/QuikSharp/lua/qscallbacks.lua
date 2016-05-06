--~ Copyright Ⓒ 2015 Victor Baybekov

package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local util = require("qsutils")

local qscallbacks = {}

--- Мы сохраняем пропущенные значения только если скрипт работает, но соединение прервалось
-- Если скрипт останавливается, то мы удаляем накопленные пропущенные значения
-- QuikSharp должен работать пока работает Квик, он не рассчитан на остановку внутри Квика.
-- При этом клиент может подключаться и отключаться сколько угодно и всегда получит пропущенные
-- сообщения после переподключения (если хватит места на диске)
local function CleanUp()
    -- close log
    pcall(logfile:close(logfile))
    -- discard missed values if any
    if missed_values_file then
        pcall(missed_values_file:close(missed_values_file))
        missed_values_file = nil
        pcall(os.remove, missed_values_file_name)
        missed_values_file_name = nil
    end
end

--- Функция вызывается когда соединение с QuikSharp клиентом обрывается
function OnQuikSharpDisconnected()
    -- TODO any recovery or risk management logic here
end

--- Функция вызывается когда скрипт ловит ошибку в функциях обратного вызова
function OnError(message)
    msg.cmd = "lua_error"
    msg.data = "Lua error: " .. message
    sendCallback(msg)
end



--- Функция вызывается терминалом QUIK при получении обезличенной сделки.
function OnAllTrade(alltrade)
    if is_connected then
        local msg = {}
        msg.t = timemsec()
        msg.cmd = "OnAllTrade"
        msg.data = alltrade
        sendCallback(msg)
    end
end

--- Функция вызывается перед закрытием терминала QUIK.
function OnClose()
    if is_connected then
        local msg = {}
        msg.cmd = "OnClose"
        msg.t = timemsec()
        msg.data = ""
        sendCallback(msg)
    end
    CleanUp()
end

--- Функция вызывается терминалом QUIK перед вызовом функции main().
-- В качестве параметра принимает значение полного пути к запускаемому скрипту.
function OnInit(script_path)
    if is_connected then
        local msg = {}
        msg.cmd = "OnInit"
        msg.t = timemsec()
        msg.data = script_path
        sendCallback(msg)
    end
    log("Hello, QuikSharp! Running inside Quik from the path: "..getScriptPath(), 1)
end

--- Функция вызывается терминалом QUIK при получении сделки.
function OnOrder(order)
    local msg = {}
    msg.t = timemsec()
    msg.id = nil -- значение в order.trans_id
    msg.data = order
    msg.cmd = "OnOrder"
    sendCallback(msg)
end

--- Функция вызывается терминалом QUIK при получении изменения стакана котировок.
function OnQuote(class_code, sec_code)
    if true then -- is_connected
        local msg = {}
        msg.cmd = "OnQuote"
        msg.t = timemsec()
        local server_time = getInfoParam("SERVERTIME")
        local status, ql2 = pcall(getQuoteLevel2, class_code, sec_code)
        if status then
            msg.data = ql2
            msg.data.class_code = class_code
            msg.data.sec_code = sec_code
            msg.data.server_time = server_time
            sendCallback(msg)
        else
            OnError(ql2)
        end
    end
end

--- Функция вызывается терминалом QUIK при остановке скрипта из диалога управления и при закрытии терминала QUIK.
function OnStop(s)
    is_started = false

    if is_connected then
        local msg = {}
        msg.cmd = "OnStop"
        msg.t = timemsec()
        msg.data = s
        sendCallback(msg)
    end
    log("Bye, QuikSharp!")
    CleanUp()
    --	send disconnect
    return 1000
end

--- Функция вызывается терминалом QUIK при получении сделки.
function OnTrade(trade)
    local msg = {}
    msg.t = timemsec()
    msg.id = nil -- значение в OnTrade.trans_id
    msg.data = trade
    msg.cmd = "OnTrade"
    sendCallback(msg)
end

--- Функция вызывается терминалом QUIK при получении ответа на транзакцию пользователя.
function OnTransReply(trans_reply)
    local msg = {}
    msg.t = timemsec()
    msg.id = nil -- значение в trans_reply.trans_id
    msg.data = trans_reply
    msg.cmd = "OnTransReply"
    sendCallback(msg)
end

function OnStopOrder(stop_order)
	local msg = {}
    msg.t = timemsec()
    msg.data = stop_order
    msg.cmd = "OnStopOrder"
    sendCallback(msg)
end

return qscallbacks
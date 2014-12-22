--~ Copyright Ⓒ 2014 Victor Baybekov

package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local util = require("qsutils")

local qscallbacks = {}

--- Функция вызывается когда соединение с QuikSharp клиентом обрывается
function OnQuikSharpDisconnected()

    -- TODO any recovery or risk management logic here
end


--- Функция вызывается терминалом QUIK при получении обезличенной сделки.
function OnAllTrade(alltrade)
    if is_connected then
        local msg = {}
        msg.cmd = "OnAllTrade"
        msg.t = timemsec()
        msg.data = alltrade
        sendResponse(msg)
    end
end


--- Функция вызывается перед закрытием терминала QUIK.
function OnClose()
    if is_connected then
        local msg = {}
        msg.cmd = "OnClose"
        msg.t = timemsec()
        msg.data = ""
        sendResponse(msg)
    end
end

--- Функция вызывается терминалом QUIK перед вызовом функции main().
-- В качестве параметра принимает значение полного пути к запускаемому скрипту.
function OnInit(script_path)
    if is_connected then
        local msg = {}
        msg.cmd = "OnInit"
        msg.t = timemsec()
        msg.data = script_path
        sendResponse(msg)
    end
    log("Hello, QuikSharp! Running inside Quik from the path: "..getScriptPath(), 1)
end

--- Функция вызывается терминалом QUIK при получении изменения стакана котировок.
function OnQuote(class_code, sec_code)
    if is_connected then
        local msg = {}
        msg.cmd = "OnQuote"
        -- msg.t = timemsec()
        ql2 = getQuoteLevel2(class_code, sec_code)
        msg.data = ql2
        msg.data.class_code = class_code
        msg.data.sec_code = sec_code
        sendResponse(msg)
    end
end

--- Функция вызывается терминалом QUIK при остановке скрипта из диалога управления и при закрытии терминала QUIK.
function OnStop(s)
    if is_connected then
        local msg = {}
        msg.cmd = "OnStop"
        msg.t = timemsec()
        msg.data = s
        sendResponse(msg)
    end

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

--- подробнее см. qsfunctions.sendTransaction
function OnTransReply(trans_reply)
    local msg = {}
    -- это сообщение будет отправлено в то задание, которое изначально отправило транзакцию
    msg.id = trans_reply.trans_id
    msg.data = trans_reply
    msg.cmd = "sendTransaction"
    return 1000
end


return qscallbacks
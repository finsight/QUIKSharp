--~ Copyright â’¸ 2015 Victor Baybekov

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


---------------------
-- Debug functions --
---------------------

--- Returns Pong to Ping
-- @param msg message table
-- @return same msg table
function qsfunctions.ping(msg)
    -- need to know data structure the caller gives
    msg.t = 0 -- avoid time generation. Could also leave original
    if msg.data == "Ping" then
        msg.data = "Pong"
        return msg
    else
        msg.data = msg.data .. " is not Ping"
        return msg
    end
end

--- Echoes its message
function qsfunctions.echo(msg)
    return msg
end

--- Test error handling
function qsfunctions.divide_string_by_zero(msg)
    msg.data = "asd" / 0
    return msg
end

--- Is running inside quik
function qsfunctions.is_quik(msg)
    if getScriptPath then msg.data = 1 else msg.data = 0 end
    return msg
end



-----------------------
-- Service functions --
-----------------------

<<<<<<< HEAD
--- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ îïðåäåëåíèÿ ñîñòîÿíèÿ ïîäêëþ÷åíèÿ êëèåíòñêîãî ìåñòà â
-- ñåðâåðó. Âîçâðàùàåò "1", åñëè êëèåíòñêîå ìåñòî ïîäêëþ÷åíî è "0", åñëè íå ïîäêëþ÷åíî.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€ÐµÐ´Ð½Ð°Ð·Ð½Ð°Ñ‡ÐµÐ½Ð° Ð´Ð»Ñ Ð¾Ð¿Ñ€ÐµÐ´ÐµÐ»ÐµÐ½Ð¸Ñ ÑÐ¾ÑÑ‚Ð¾ÑÐ½Ð¸Ñ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¸Ñ ÐºÐ»Ð¸ÐµÐ½Ñ‚ÑÐºÐ¾Ð³Ð¾ Ð¼ÐµÑÑ‚Ð° Ðº
-- ÑÐµÑ€Ð²ÐµÑ€Ñƒ. Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Â«1Â», ÐµÑÐ»Ð¸ ÐºÐ»Ð¸ÐµÐ½Ñ‚ÑÐºÐ¾Ðµ Ð¼ÐµÑÑ‚Ð¾ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¾ Ð¸ Â«0Â», ÐµÑÐ»Ð¸ Ð½Ðµ Ð¿Ð¾Ð´ÐºÐ»ÑŽÑ‡ÐµÐ½Ð¾.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.isConnected(msg)
    -- set time when function was called
    msg.t = timemsec()
    msg.data = isConnected()
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ âîçâðàùàåò ïóòü ïî êîòîðîìó íàõîäòñÿ ôàéë info.exe, èñïîëíÿþùèé äàííûé ñêðèïò,
-- áåç çàâåðøàþùåãî îáðàòíîãî ñëýøà ("\"). Íàïðèìåð, C:\QuikFront.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Ð¿ÑƒÑ‚ÑŒ, Ð¿Ð¾ ÐºÐ¾Ñ‚Ð¾Ñ€Ð¾Ð¼Ñƒ Ð½Ð°Ñ…Ð¾Ð´Ð¸Ñ‚ÑÑ Ñ„Ð°Ð¹Ð» info.exe, Ð¸ÑÐ¿Ð¾Ð»Ð½ÑÑŽÑ‰Ð¸Ð¹ Ð´Ð°Ð½Ð½Ñ‹Ð¹
-- ÑÐºÑ€Ð¸Ð¿Ñ‚, Ð±ÐµÐ· Ð·Ð°Ð²ÐµÑ€ÑˆÐ°ÑŽÑ‰ÐµÐ³Ð¾ Ð¾Ð±Ñ€Ð°Ñ‚Ð½Ð¾Ð³Ð¾ ÑÐ»ÑÑˆÐ° (Â«\Â»). ÐÐ°Ð¿Ñ€Ð¸Ð¼ÐµÑ€, C:\QuikFront.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getWorkingFolder(msg)
    -- set time when function was called
    msg.t = timemsec()
    msg.data = getWorkingFolder()
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ âîçâðàùàåò ïóòü ïî êîòîðîìó íàõîäèòñÿ èñïîëíÿåìûé ñêðèïò,
-- áåç çàâåðøàþùåãî îáðàòíîãî ñëýøà ("\"). Íàïðèìåð, C:\QuikFront\Scripts.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Ð¿ÑƒÑ‚ÑŒ, Ð¿Ð¾ ÐºÐ¾Ñ‚Ð¾Ñ€Ð¾Ð¼Ñƒ Ð½Ð°Ñ…Ð¾Ð´Ð¸Ñ‚ÑÑ Ð·Ð°Ð¿ÑƒÑÐºÐ°ÐµÐ¼Ñ‹Ð¹ ÑÐºÑ€Ð¸Ð¿Ñ‚, Ð±ÐµÐ· Ð·Ð°Ð²ÐµÑ€ÑˆÐ°ÑŽÑ‰ÐµÐ³Ð¾
-- Ð¾Ð±Ñ€Ð°Ñ‚Ð½Ð¾Ð³Ð¾ ÑÐ»ÑÑˆÐ° (Â«\Â»). ÐÐ°Ð¿Ñ€Ð¸Ð¼ÐµÑ€, C:\QuikFront\Scripts.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getScriptPath(msg)
    -- set time when function was called
    msg.t = timemsec()
    msg.data = getScriptPath()
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ âîçâðàùàåò çíà÷åíèÿ ïàðàìåòðîâ èíôîðìàöèîííîãî îêíà
-- (ïóíêò ìåíþ Ñâÿçü / Èíôîðìàöèîííîå îêíî...).
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Ð·Ð½Ð°Ñ‡ÐµÐ½Ð¸Ñ Ð¿Ð°Ñ€Ð°Ð¼ÐµÑ‚Ñ€Ð¾Ð² Ð¸Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ð¾Ð½Ð½Ð¾Ð³Ð¾ Ð¾ÐºÐ½Ð° (Ð¿ÑƒÐ½ÐºÑ‚ Ð¼ÐµÐ½ÑŽ
-- Ð¡Ð²ÑÐ·ÑŒ / Ð˜Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ð¾Ð½Ð½Ð¾Ðµ Ð¾ÐºÐ½Ð¾â€¦).
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getInfoParam(msg)
    -- set time when function was called
    msg.t = timemsec()
    msg.data = getInfoParam(msg.data)
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ îòîáðàæàåò ñîîáùåíèÿ â òåðìèíàëå QUIK.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¾Ñ‚Ð¾Ð±Ñ€Ð°Ð¶Ð°ÐµÑ‚ ÑÐ¾Ð¾Ð±Ñ‰ÐµÐ½Ð¸Ñ Ð² Ñ‚ÐµÑ€Ð¼Ð¸Ð½Ð°Ð»Ðµ QUIK.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.message(msg)
    log(msg.data, 1)
    msg.data = ""
    return msg
end
function qsfunctions.warning_message(msg)
    log(msg.data, 2)
    msg.data = ""
    return msg
end
function qsfunctions.error_message(msg)
    log(msg.data, 3)
    msg.data = ""
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ ïðèîñòàíàâëèâàåò âûïîëíåíèå ñêðèïòà.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€Ð¸Ð¾ÑÑ‚Ð°Ð½Ð°Ð²Ð»Ð¸Ð²Ð°ÐµÑ‚ Ð²Ñ‹Ð¿Ð¾Ð»Ð½ÐµÐ½Ð¸Ðµ ÑÐºÑ€Ð¸Ð¿Ñ‚Ð°.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.sleep(msg)
    delay(msg.data)
    msg.data = ""
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ äëÿ âûâîäà îòëàäî÷íîé èíôîðìàöèè.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð´Ð»Ñ Ð²Ñ‹Ð²Ð¾Ð´Ð° Ð¾Ñ‚Ð»Ð°Ð´Ð¾Ñ‡Ð½Ð¾Ð¹ Ð¸Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ð¸. 
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.PrintDbgStr(msg)
    log(msg.data, 0)
    msg.data = ""
    return msg
end

---------------------
-- Class functions --
---------------------

<<<<<<< HEAD
--- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ ïîëó÷åíèÿ ñïèñêà êîäîâ êëàññîâ, ïåðåäàííûõ ñ ñåðâåðà â õîäå ñåàíñà ñâÿçè.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€ÐµÐ´Ð½Ð°Ð·Ð½Ð°Ñ‡ÐµÐ½Ð° Ð´Ð»Ñ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ ÑÐ¿Ð¸ÑÐºÐ° ÐºÐ¾Ð´Ð¾Ð² ÐºÐ»Ð°ÑÑÐ¾Ð², Ð¿ÐµÑ€ÐµÐ´Ð°Ð½Ð½Ñ‹Ñ… Ñ ÑÐµÑ€Ð²ÐµÑ€Ð° Ð² Ñ…Ð¾Ð´Ðµ ÑÐµÐ°Ð½ÑÐ° ÑÐ²ÑÐ·Ð¸.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getClassesList(msg)
    msg.data = getClassesList()
--    if  msg.data then log(msg.data) else log("getClassesList returned nil") end
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ ïîëó÷åíèÿ èíôîðìàöèè î êëàññå.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€ÐµÐ´Ð½Ð°Ð·Ð½Ð°Ñ‡ÐµÐ½Ð° Ð´Ð»Ñ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ Ð¸Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ð¸ Ð¾ ÐºÐ»Ð°ÑÑÐµ.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getClassInfo(msg)
    msg.data = getClassInfo(msg.data)
--    if msg.data then log(msg.data.name) else log("getClassInfo  returned nil") end
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ ïîëó÷åíèÿ ñïèñêà êîäîâ áóìàã äëÿ ñïèñêà êëàññîâ, çàäàííîãî ñïèñêîì êîäîâ.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€ÐµÐ´Ð½Ð°Ð·Ð½Ð°Ñ‡ÐµÐ½Ð° Ð´Ð»Ñ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ ÑÐ¿Ð¸ÑÐºÐ° ÐºÐ¾Ð´Ð¾Ð² Ð±ÑƒÐ¼Ð°Ð³ Ð´Ð»Ñ ÑÐ¿Ð¸ÑÐºÐ° ÐºÐ»Ð°ÑÑÐ¾Ð², Ð·Ð°Ð´Ð°Ð½Ð½Ð¾Ð³Ð¾ ÑÐ¿Ð¸ÑÐºÐ¾Ð¼ ÐºÐ¾Ð´Ð¾Ð².
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getClassSecurities(msg)
    msg.data = getClassSecurities(msg.data)
--    if msg.data then log(msg.data) else log("getClassSecurities returned nil") end
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ ïîçâîëÿåò óçíàòü, çàêàçàí ëè ñ ñåðâåðà ñòàêàí ïî óêàçàííîìó êëàññó è áóìàãå.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ð¾Ð·Ð²Ð¾Ð»ÑÐµÑ‚ ÑƒÐ·Ð½Ð°Ñ‚ÑŒ, Ð·Ð°ÐºÐ°Ð·Ð°Ð½ Ð»Ð¸ Ñ ÑÐµÑ€Ð²ÐµÑ€Ð° ÑÑ‚Ð°ÐºÐ°Ð½ Ð¿Ð¾ ÑƒÐºÐ°Ð·Ð°Ð½Ð½Ð¾Ð¼Ñƒ ÐºÐ»Ð°ÑÑÑƒ Ð¸ Ð±ÑƒÐ¼Ð°Ð³Ðµ.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getSecurityInfo(msg)
    local spl = split(msg.data, "|")
    local class_code, sec_code = spl[1], spl[2]
    msg.data = getSecurityInfo(class_code, sec_code)
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ îïðåäåëåíèÿ êëàññà ïî êîäó èíñòðóìåíòà èç çàäàííîãî ñïèñêà êëàññîâ.
function qsfunctions.getSecurityClass(msg)
    local spl = split(msg.data, "|")
    local classes_list, sec_code = spl[1], spl[2]

	for class_code in string.gmatch(classes_list,"%a+") do
		if getSecurityInfo(class_code,sec_code) then
			msg.data = class_code
			return msg
		end
	end
	msg.data = ""
	return msg
end

--- Ôóíêöèÿ âîçâðàùàåò êîä êëèåíòà
function qsfunctions.getClientCode(msg)
	for i=0,getNumberOf("MONEY_LIMITS")-1 do
		local clientcode = getItem("MONEY_LIMITS",i).client_code
		if clientcode ~= nil then
			msg.data = clientcode
			return msg
		end
    end
	return msg
end

--- Ôóíêöèÿ âîçâðàùàåò òîðãîâûé ñ÷åò äëÿ çàïðàøèâàåìîãî êîäà êëàññà
function qsfunctions.getTradeAccount(msg)
	for i=0,getNumberOf("trade_accounts")-1 do
		local trade_account = getItem("trade_accounts",i)
		if string.find(trade_account.class_codes,msg.data,1,1) then
			msg.data = trade_account.trdaccid
			return msg
		end
	end
	return msg
end



---------------------------------------------------------------------
-- Order Book functions (Ôóíêöèè äëÿ ðàáîòû ñî ñòàêàíîì êîòèðîâîê) --
---------------------------------------------------------------------

--- Ôóíêöèÿ çàêàçûâàåò íà ñåðâåð ïîëó÷åíèå ñòàêàíà ïî óêàçàííîìó êëàññó è áóìàãå.
=======


---------------------------------------------------------------------
-- Order Book functions (Ð¤ÑƒÐ½ÐºÑ†Ð¸Ð¸ Ð´Ð»Ñ Ñ€Ð°Ð±Ð¾Ñ‚Ñ‹ ÑÐ¾ ÑÑ‚Ð°ÐºÐ°Ð½Ð¾Ð¼ ÐºÐ¾Ñ‚Ð¸Ñ€Ð¾Ð²Ð¾Ðº) --
---------------------------------------------------------------------

--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð·Ð°ÐºÐ°Ð·Ñ‹Ð²Ð°ÐµÑ‚ Ð½Ð° ÑÐµÑ€Ð²ÐµÑ€ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ðµ ÑÑ‚Ð°ÐºÐ°Ð½Ð° Ð¿Ð¾ ÑƒÐºÐ°Ð·Ð°Ð½Ð½Ð¾Ð¼Ñƒ ÐºÐ»Ð°ÑÑÑƒ Ð¸ Ð±ÑƒÐ¼Ð°Ð³Ðµ.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.Subscribe_Level_II_Quotes(msg)
    local spl = split(msg.data, "|")
    local class_code, sec_code = spl[1], spl[2]
    msg.data = Subscribe_Level_II_Quotes(class_code, sec_code)
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ îòìåíÿåò çàêàç íà ïîëó÷åíèå ñòàêàíà ïî óêàçàííîìó êëàññó è áóìàãå.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¾Ñ‚Ð¼ÐµÐ½ÑÐµÑ‚ Ð·Ð°ÐºÐ°Ð· Ð½Ð° Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ðµ Ñ ÑÐµÑ€Ð²ÐµÑ€Ð° ÑÑ‚Ð°ÐºÐ°Ð½Ð° Ð¿Ð¾ ÑƒÐºÐ°Ð·Ð°Ð½Ð½Ð¾Ð¼Ñƒ ÐºÐ»Ð°ÑÑÑƒ Ð¸ Ð±ÑƒÐ¼Ð°Ð³Ðµ.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.Unsubscribe_Level_II_Quotes(msg)
    local spl = split(msg.data, "|")
    local class_code, sec_code = spl[1], spl[2]
    msg.data = Unsubscribe_Level_II_Quotes(class_code, sec_code)
    return msg
end

<<<<<<< HEAD
--- Ôóíêöèÿ ïîçâîëÿåò óçíàòü, çàêàçàí ëè ñ ñåðâåðà ñòàêàí ïî óêàçàííîìó êëàññó è áóìàãå.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ð¾Ð·Ð²Ð¾Ð»ÑÐµÑ‚ ÑƒÐ·Ð½Ð°Ñ‚ÑŒ, Ð·Ð°ÐºÐ°Ð·Ð°Ð½ Ð»Ð¸ Ñ ÑÐµÑ€Ð²ÐµÑ€Ð° ÑÑ‚Ð°ÐºÐ°Ð½ Ð¿Ð¾ ÑƒÐºÐ°Ð·Ð°Ð½Ð½Ð¾Ð¼Ñƒ ÐºÐ»Ð°ÑÑÑƒ Ð¸ Ð±ÑƒÐ¼Ð°Ð³Ðµ.
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.IsSubscribed_Level_II_Quotes(msg)
    local spl = split(msg.data, "|")
    local class_code, sec_code = spl[1], spl[2]
    msg.data = IsSubscribed_Level_II_Quotes(class_code, sec_code)
    return msg
end

-----------------------
-- Trading functions --
-----------------------

<<<<<<< HEAD
-- Ôóíêöèÿ îòïðàâëÿåò òðàíçàêöèþ íà ñåðâåð è âîçâðàùàåò ïóñòîå ñîîáùåíèå, êîòîðîå
-- áóäåò ïðîèãíîðèðîâàíî. Âìåñòî íåãî, îòïðàâèòåëü áóäåò æäàòü ñîáûòèÿ
-- OnTransReply, èç êîòîðîãî ïî TRANS_ID îí ïîëó÷èò ðåçóëüòàò îòïðàâëåííîé òðàíçàêöèè.
=======
--- Ð¾Ñ‚Ð¿Ñ€Ð°Ð²Ð»ÑÐµÑ‚ Ñ‚Ñ€Ð°Ð½Ð·Ð°ÐºÑ†Ð¸ÑŽ Ð½Ð° ÑÐµÑ€Ð²ÐµÑ€ Ð¸ Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Ð¿ÑƒÑÑ‚Ð¾Ðµ ÑÐ¾Ð¾Ð±Ñ‰ÐµÐ½Ð¸Ðµ, ÐºÐ¾Ñ‚Ð¾Ñ€Ð¾Ðµ
-- Ð±ÑƒÐ´ÐµÑ‚ Ð¿Ñ€Ð¾Ð¸Ð³Ð½Ð¾Ð¸Ñ€Ð¾Ð²Ð°Ð½Ð¾. Ð’Ð¼ÐµÑÑ‚Ð¾ Ð½ÐµÐ³Ð¾, Ð¾Ñ‚Ð¿Ñ€Ð°Ð²Ð¸Ñ‚ÐµÐ»ÑŒ Ð±ÑƒÐ´ÐµÑ‚ Ð¶Ð´Ð°Ñ‚ÑŒ ÑÐ¾Ð±Ñ‹Ñ‚Ð¸Ñ
-- OnTransReply, Ð¸Ð· ÐºÐ¾Ñ‚Ð¾Ñ€Ð¾Ð³Ð¾ Ð¿Ð¾ TRANS_ID Ð¾Ð½ Ð¿Ð¾Ð»ÑƒÑ‡Ð¸Ñ‚ Ñ€ÐµÐ·ÑƒÐ»ÑŒÑ‚Ð°Ñ‚ Ð¾Ñ‚Ð¿Ñ€Ð°Ð²Ð»ÐµÐ½Ð½Ð¾Ð¹ Ñ‚Ñ€Ð°Ð½Ð·Ð°ÐºÑ†Ð¸Ð¸
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.sendTransaction(msg)
    local res = sendTransaction(msg.data)
    if res~="" then
        -- error handling
        msg.cmd = "lua_transaction_error"
        msg.lua_error = res
        return msg
    else
        -- transaction sent
        msg.data = true
        return msg
    end
end

<<<<<<< HEAD
-- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ ïîëó÷åíèÿ çíà÷åíèé âñåõ ïàðàìåòðîâ áèðæåâîé èíôîðìàöèè èç òàáëèöû òåêóùèõ çíà÷åíèé ïàðàìåòðîâ.
-- Ñ ïîìîùüþ ýòîé ôóíêöèè ìîæíî ïîëó÷èòü ëþáîå èç çíà÷åíèé òàáëèöû òåêóùèõ çíà÷åíèé ïàðàìåòðîâ äëÿ çàäàííûõ êîäîâ êëàññà è áóìàãè.
=======
--- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€ÐµÐ´Ð½Ð°Ð·Ð½Ð°Ñ‡ÐµÐ½Ð° Ð´Ð»Ñ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ Ð·Ð½Ð°Ñ‡ÐµÐ½Ð¸Ð¹ Ð²ÑÐµÑ… Ð¿Ð°Ñ€Ð°Ð¼ÐµÑ‚Ñ€Ð¾Ð² Ð±Ð¸Ñ€Ð¶ÐµÐ²Ð¾Ð¹ Ð¸Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ð¸ Ð¸Ð· Ð¢Ð°Ð±Ð»Ð¸Ñ†Ñ‹ Ñ‚ÐµÐºÑƒÑ‰Ð¸Ñ… Ð·Ð½Ð°Ñ‡ÐµÐ½Ð¸Ð¹ Ð¿Ð°Ñ€Ð°Ð¼ÐµÑ‚Ñ€Ð¾Ð². 
-- Ð¡ Ð¿Ð¾Ð¼Ð¾Ñ‰ÑŒÑŽ ÑÑ‚Ð¾Ð¹ Ñ„ÑƒÐ½ÐºÑ†Ð¸Ð¸ Ð¼Ð¾Ð¶Ð½Ð¾ Ð¿Ð¾Ð»ÑƒÑ‡Ð¸Ñ‚ÑŒ Ð»ÑŽÐ±Ð¾Ðµ Ð¸Ð· Ð·Ð½Ð°Ñ‡ÐµÐ½Ð¸Ð¹ Ð¢Ð°Ð±Ð»Ð¸Ñ†Ñ‹ Ñ‚ÐµÐºÑƒÑ‰Ð¸Ñ… Ð·Ð½Ð°Ñ‡ÐµÐ½Ð¸Ð¹ Ð¿Ð°Ñ€Ð°Ð¼ÐµÑ‚Ñ€Ð¾Ð² Ð´Ð»Ñ Ð·Ð°Ð´Ð°Ð½Ð½Ñ‹Ñ… ÐºÐ¾Ð´Ð¾Ð² ÐºÐ»Ð°ÑÑÐ° Ð¸ Ð±ÑƒÐ¼Ð°Ð³Ð¸. 
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

function qsfunctions.getParamEx(msg)
    local spl = split(msg.data, "|")
    local class_code, sec_code, param_name = spl[1], spl[2], spl[3]
    msg.data = getParamEx(class_code, sec_code, param_name)
    return msg
end

<<<<<<< HEAD
-- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ ïîëó÷åíèÿ èíôîðìàöèè ïî áóìàæíûì ëèìèòàì.
=======
-- Ð¤ÑƒÐ½ÐºÑ†Ð¸Ñ Ð¿Ñ€ÐµÐ´Ð½Ð°Ð·Ð½Ð°Ñ‡ÐµÐ½Ð° Ð´Ð»Ñ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ Ð¸Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸Ð¸ Ð¿Ð¾ Ð±ÑƒÐ¼Ð°Ð¶Ð½Ñ‹Ð¼ Ð»Ð¸Ð¼Ð¸Ñ‚Ð°Ð¼. 
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getDepo(msg)
    local spl = split(msg.data, "|")
    local clientCode, firmId, secCode, account = spl[1], spl[2], spl[3], spl[4]
    msg.data = getDepo(clientCode, firmId, secCode, account)
    return msg
end

<<<<<<< HEAD
-- Ôóíêöèÿ ïðåäíàçíà÷åíà äëÿ ïîëó÷åíèÿ èíôîðìàöèè ïî áóìàæíûì ëèìèòàì.
function qsfunctions.getDepoEx(msg)
    local spl = split(msg.data, "|")
    local firmId, clientCode, secCode, account, limit_kind = spl[1], spl[2], spl[3], spl[4], spl[5]
    msg.data = getDepoEx(firmId, clientCode, secCode, account, tonumber(limit_kind))
    return msg
end

=======
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.getFuturesHolding(msg)
    local spl = split(msg.data, "|")
    local firmId, accId, secCode, posType = spl[1], spl[2], spl[3], spl[4]
	local result, err = getFuturesHolding(firmId, accId, secCode, posType*1)
	if result then
		msg.data = result
	else
<<<<<<< HEAD
		--log("Futures holding returns nil", 3)
=======
		log("Futures holding returns nil", 3)
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
		msg.data = nil
	end
    return msg
end

<<<<<<< HEAD
-- Ôóíêöèÿ âîçâðàùàåò òàáëèöó çàÿâîê (âñþ èëè ïî çàäàííîìó èíñòðóìåíòó)
function qsfunctions.get_orders(msg)
	if msg.data ~= "" then
		local spl = split(msg.data, "|")
		class_code, sec_code = spl[1], spl[2]
	end

	local orders = {}
	for i = 0, getNumberOf("orders") - 1 do
		local order = getItem("orders", i)
		if msg.data == "" or (order.class_code == class_code and order.sec_code == sec_code) then
			table.insert(orders, order)
		end
	end
	msg.data = orders
	return msg
end

-- Ôóíêöèÿ âîçâðàùàåò çàÿâêó ïî çàäàííîìó èíñòðóìåíòó è ID-òðàíçàêöèè
function qsfunctions.getOrder_by_ID(msg)
	if msg.data ~= "" then
		local spl = split(msg.data, "|")
		class_code, sec_code, trans_id = spl[1], spl[2], spl[3]
	end

	local order_num = 0
	local res
	for i = 0, getNumberOf("orders") - 1 do
		local order = getItem("orders", i)
		if order.class_code == class_code and order.sec_code == sec_code and order.trans_id == tonumber(trans_id) and order.order_num > order_num then
			order_num = order.order_num
			res = order
		end
	end
	msg.data = res
	return msg
end

-- Ôóíêöèÿ âîçâðàùàåò çàÿâêó ïî íîìåðó
function qsfunctions.getOrder_by_Number(msg)
	for i=0,getNumberOf("orders")-1 do
		local order = getItem("orders",i)
		if order.order_num == tonumber(msg.data) then
			msg.data = order
			return msg
		end
	end
	return msg
end

-- Ôóíêöèÿ âîçâðàùàåò òàáëèöó ñäåëîê (âñþ èëè ïî çàäàííîìó èíñòðóìåíòó)
function qsfunctions.get_trades(msg)
	if msg.data ~= "" then
		local spl = split(msg.data, "|")
		class_code, sec_code = spl[1], spl[2]
	end

	local trades = {}
	for i = 0, getNumberOf("trades") - 1 do
		local trade = getItem("trades", i)
		if msg.data == "" or (trade.class_code == class_code and trade.sec_code == sec_code) then
			table.insert(trades, trade)
		end
	end
	msg.data = trades
	return msg
end

-- Ôóíêöèÿ âîçâðàùàåò òàáëèöó ñäåëîê ïî íîìåðó çàÿâêè
function qsfunctions.get_Trades_by_OrderNumber(msg)
	local order_num = tonumber(msg.data)

	local trades = {}
	for i = 0, getNumberOf("trades") - 1 do
		local trade = getItem("trades", i)
		if trade.order_num == order_num then
			table.insert(trades, trade)
		end
	end
	msg.data = trades
	return msg
end

--function qsfunctions.getQuikTable(msg)
--	msg.data = SearchItems(msg.data, 0, getNumberOf(msg.data)-1)
--	return msg
--end

=======
--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Ð·Ð°ÑÐ²ÐºÑƒ Ð¿Ð¾ ÐµÑ‘ Ð½Ð¾Ð¼ÐµÑ€Ñƒ ---
--- ÐÐ° Ð¾ÑÐ½Ð¾Ð²Ðµ http://help.qlua.org/ch4_5_1_1.htm ---
function qsfunctions.get_order_by_number(msg)
	local spl = split(msg.data, "|")
	local class_code = spl[1]
	local order_id = tonumber(spl[2])
	msg.data = getOrderByNumber(class_code, order_id)
	return msg
end

--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ ÑÐ¿Ð¸ÑÐ¾Ðº Ð·Ð°Ð¿Ð¸ÑÐµÐ¹ Ð¸Ð· Ñ‚Ð°Ð±Ð»Ð¸Ñ†Ñ‹ 'Ð›Ð¸Ð¼Ð¸Ñ‚Ñ‹ Ð¿Ð¾ Ð±ÑƒÐ¼Ð°Ð³Ð°Ð¼'
--- ÐÐ° Ð¾ÑÐ½Ð¾Ð²Ðµ http://help.qlua.org/ch4_6_11.htm Ð¸ http://help.qlua.org/ch4_5_3.htm
function qsfunctions.get_depo_limits(msg)
	local sec_code = msg.data
	local count = getNumberOf("depo_limits")
	local depo_limits = {}
	for i = 0, count - 1 do
		local depo_limit = getItem("depo_limits", i)
		if msg.data == "" or depo_limit.sec_code == sec_code then
			table.insert(depo_limits, depo_limit)
		end
	end
	msg.data = depo_limits
	return msg
end

>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
--------------------------
-- Stop order functions --
--------------------------

<<<<<<< HEAD
--- Ôóíêöèÿ âîçâðàùàåò ñïèñîê ñòîï-çàÿâîê
=======
--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ ÑÐ¿Ð¸ÑÐ¾Ðº ÑÑ‚Ð¾Ð¿-Ð·Ð°ÑÐ²Ð¾Ðº
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.get_stop_orders(msg)
	if msg.data ~= "" then
		local spl = split(msg.data, "|")
		class_code, sec_code = spl[1], spl[2]
	end

	local count = getNumberOf("stop_orders")
	local stop_orders = {}
	for i = 0, count - 1 do
		local stop_order = getItem("stop_orders", i)
		if msg.data == "" or (stop_order.class_code == class_code and stop_order.sec_code == sec_code) then
			table.insert(stop_orders, stop_order)
		end
	end
	msg.data = stop_orders
	return msg
end

-------------------------
--- Candles functions ---
-------------------------

<<<<<<< HEAD
--- Âîçâðàùàåì âñå ñâå÷è ïî èäåíòèôèêàòîðó ãðàôèêà. Ãðàôèê äîëæåí áûòü îòêðûò.
=======
--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÐ¼ Ð²ÑÐµ ÑÐ²ÐµÑ‡Ð¸ Ð¿Ð¾ Ð¸Ð´ÐµÐ½Ñ‚Ð¸Ñ„Ð¸ÐºÐ°Ñ‚Ð¾Ñ€Ñƒ Ð³Ñ€Ð°Ñ„Ð¸ÐºÐ°. Ð“Ñ€Ð°Ñ„Ð¸Ðº Ð´Ð¾Ð»Ð¶ÐµÐ½ Ð±Ñ‹Ñ‚ÑŒ Ð¾Ñ‚ÐºÑ€Ñ‹Ñ‚
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.get_candles(msg)
	log("Called get_candles" .. msg.data, 2)
	local spl = split(msg.data, "|")
	local tag = spl[1]
	local line = tonumber(spl[2])
	local first_candle = tonumber(spl[3])
	local count = tonumber(spl[4])
	if count == 0 then
		count = getNumCandles(tag) * 1
	end
	log("Count: " .. count, 2)
	local t,n,l = getCandlesByIndex(tag, line, first_candle, count)
	log("Candles table size: " .. n, 2)
	log("Label: " .. l, 2)
	local candles = {}
	for i = 0, count - 1 do
		table.insert(candles, t[i])
	end
	msg.data = candles
	return msg
end

<<<<<<< HEAD
--- Ñëîâàðü îòêðûòûõ ïîäïèñîê (datasources) íà ñâå÷è
data_sources = {}
last_indexes = {}

--- Ïîäïèñàòüñÿ íà ïîëó÷åíèå ÂÑÅÕ ñâå÷åé ïî çàäàííîìó èíñòðóìåíòó è èíòåðâàëó
function qsfunctions.subscribe_to_candles(msg)
	local class, sec, interval = get_candles_param(msg)
	local key = get_key(class, sec, interval)
	data_sources[key], error_descr = CreateDataSource(class, sec, interval)

	if(error_descr ~= nil) then
		msg.cmd = "lua_create_data_source_error"
		msg.lua_error = error_descr
		return msg
	end

	if data_sources[key] == nil then
		msg.cmd = "lua_create_data_source_error"
		msg.lua_error = "Can't create data source for " .. class .. ", " .. sec .. ", " .. tostring(interval)
	else
		--data_sources[key] = ds
		last_indexes[key] = data_sources[key]:Size()
		--------------------------

		all_candles = {}
		for i = 1, data_sources[key]:Size()-1 do
			local candle = {}
			candle.low   = data_sources[key]:L(i)
			candle.close = data_sources[key]:C(i)
			candle.high = data_sources[key]:H(i)
			candle.open = data_sources[key]:O(i)
			candle.volume = data_sources[key]:V(i)
			candle.datetime = data_sources[key]:T(i)

			candle.sec = sec
			candle.class = class
			candle.interval = interval

			table.insert(all_candles, candle)
		end
		msg.data = all_candles
		--------------------------
		data_sources[key]:SetUpdateCallback(
			function(index)
				data_source_callback(index, class, sec, interval)
=======
--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÐ¼ Ð²ÑÐµ ÑÐ²ÐµÑ‡Ð¸ Ð¿Ð¾ Ð·Ð°Ð´Ð°Ð½Ð½Ð¾Ð¼Ñƒ Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚Ñƒ Ð¸ Ð¸Ð½Ñ‚ÐµÑ€Ð²Ð°Ð»Ñƒ
function qsfunctions.get_candles_from_data_source(msg)
	local ds, is_error = create_data_source(msg)
	if not is_error then
		--- Ð´Ð°Ñ‚Ð°ÑÐ¾Ñ€Ñ Ð¸Ð·Ð½Ð°Ñ‡Ð°Ð»ÑŒÐ½Ð¾ Ð¿Ñ€Ð¸Ñ…Ð¾Ð´Ð¸Ñ‚ Ð¿ÑƒÑÑ‚Ð¾Ð¹, Ð½ÑƒÐ¶Ð½Ð¾ Ð½ÐµÐºÐ¾Ñ‚Ð¾Ñ€Ð¾Ðµ Ð²Ñ€ÐµÐ¼Ñ Ð¿Ð¾Ð´Ð¾Ð¶Ð´Ð°Ñ‚ÑŒ Ð¿Ð¾ÐºÐ° Ð¾Ð½ Ð·Ð°Ð¿Ð¾Ð»Ð½Ð¸Ñ‚ÑŒÑÑ Ð´Ð°Ð½Ð½Ñ‹Ð¼Ð¸
		repeat sleep(1) until ds:Size() > 0

		local count = tonumber(split(msg.data, "|")[4]) --- Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÐ¼ Ð¿Ð¾ÑÐ»ÐµÐ´Ð½Ð¸Ðµ count ÑÐ²ÐµÑ‡ÐµÐ¹. Ð•ÑÐ»Ð¸ Ñ€Ð°Ð²ÐµÐ½ 0, Ñ‚Ð¾ Ð²Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÐ¼ Ð²ÑÐµ Ð´Ð¾ÑÑ‚ÑƒÐ¿Ð½Ñ‹Ðµ ÑÐ²ÐµÑ‡Ð¸.
		local class, sec, interval = get_candles_param(msg)
		local candles = {}
		local start_i = count == 0 and 1 or math.max(1, ds:Size() - count + 1)
		for i = start_i, ds:Size() do
			local candle = fetch_candle(ds, i)
			candle.sec = sec
			candle.class = class
			candle.interval = interval
			table.insert(candles, candle)
		end
		ds:Close()
		msg.data = candles
	end
	return msg
end

function create_data_source(msg)
	local class, sec, interval = get_candles_param(msg)
	local ds, error_descr = CreateDataSource(class, sec, interval)
	local is_error = false
	if(error_descr ~= nil) then
		msg.cmd = "lua_create_data_source_error"
		msg.lua_error = error_descr
		is_error = true
	elseif ds == nil then
		msg.cmd = "lua_create_data_source_error"
		msg.lua_error = "Can't create data source for " .. class .. ", " .. sec .. ", " .. tostring(interval)
		is_error = true
	end
	return ds, is_error
end

function fetch_candle(data_source, index)
	local candle = {}
	candle.low   = data_source:L(index)
	candle.close = data_source:C(index)
	candle.high = data_source:H(index)
	candle.open = data_source:O(index)
	candle.volume = data_source:V(index)
	candle.datetime = data_source:T(index)
	return candle
end

--- Ð¡Ð»Ð¾Ð²Ð°Ñ€ÑŒ Ð¾Ñ‚ÐºÑ€Ñ‹Ñ‚Ñ‹Ñ… Ð¿Ð¾Ð´Ð¿Ð¸ÑÐ¾Ðº (datasources) Ð½Ð° ÑÐ²ÐµÑ‡Ð¸
data_sources = {}
last_indexes = {}

--- ÐŸÐ¾Ð´Ð¿Ð¸ÑÐ°Ñ‚ÑŒÑÑ Ð½Ð° Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ ÑÐ²ÐµÑ‡ÐµÐ¹ Ð¿Ð¾ Ð·Ð°Ð´Ð°Ð½Ð½Ð¾Ð¼Ñƒ Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚ Ð¸ Ð¸Ð½Ñ‚ÐµÑ€Ð²Ð°Ð»Ñƒ
function qsfunctions.subscribe_to_candles(msg)
	local ds, is_error = create_data_source(msg)
	if not is_error then
		local class, sec, interval = get_candles_param(msg)
		local key = get_key(class, sec, interval)
		data_sources[key] = ds
		last_indexes[key] = ds:Size()
		ds:SetUpdateCallback(
			function(index) 
				data_source_callback(index, class, sec, interval) 
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
			end)
	end
	return msg
end

function data_source_callback(index, class, sec, interval)
	local key = get_key(class, sec, interval)
	if index ~= last_indexes[key] then
		last_indexes[key] = index

<<<<<<< HEAD
		local candle = {}
		candle.low   = data_sources[key]:L(index - 1)
		candle.close = data_sources[key]:C(index - 1)
		candle.high = data_sources[key]:H(index - 1)
		candle.open = data_sources[key]:O(index - 1)
		candle.volume = data_sources[key]:V(index - 1)
		candle.datetime = data_sources[key]:T(index - 1)

=======
		local candle = fetch_candle(data_sources[key], index - 1)
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
		candle.sec = sec
		candle.class = class
		candle.interval = interval

		local msg = {}
        msg.t = timemsec()
        msg.cmd = "NewCandle"
        msg.data = candle
        sendCallback(msg)
	end
end

<<<<<<< HEAD
--- Îòïèñàòüñÿ îò ïîëó÷åíèÿ ñâå÷åé ïî çàäàííîìó èíñòðóìåíòó è èíòåðâàëó
=======
--- ÐžÑ‚Ð¿Ð¸ÑÐ°Ñ‚ÑŒ Ð¾Ñ‚ Ð¿Ð¾Ð»ÑƒÑ‡ÐµÐ½Ð¸Ñ ÑÐ²ÐµÑ‡ÐµÐ¹ Ð¿Ð¾ Ð·Ð°Ð´Ð°Ð½Ð½Ð¾Ð¼Ñƒ Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚Ñƒ Ð¸ Ð¸Ð½Ñ‚ÐµÑ€Ð²Ð°Ð»Ñƒ
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.unsubscribe_from_candles(msg)
	local class, sec, interval = get_candles_param(msg)
	local key = get_key(class, sec, interval)
	data_sources[key]:Close()
	data_sources[key] = nil
	last_indexes[key] = nil
	return msg
end

<<<<<<< HEAD
--- Ïðîâåðèòü îòêðûòà ëè ïîäïèñêà íà çàäàííûé èíñòðóìåíò è èíòåðâàë
=======
--- ÐŸÑ€Ð¾Ð²ÐµÑ€Ð¸Ñ‚ÑŒ Ð¾Ñ‚ÐºÑ€Ñ‹Ñ‚Ð° Ð»Ð¸ Ð¿Ð¾Ð´Ð¿Ð¸ÑÐºÐ° Ð½Ð° Ð·Ð°Ð´Ð°Ð½Ð½Ñ‹Ð¹ Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚ Ð¸ Ð¸Ð½Ñ‚ÐµÑ€Ð²Ð°Ð»
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function qsfunctions.is_subscribed(msg)
	local class, sec, interval = get_candles_param(msg)
	local key = get_key(class, sec, interval)
	for k, v in pairs(data_sources) do
		if key == k then
			msg.data = true;
			return  msg
		end
	end
	msg.data = false
	return msg
end

<<<<<<< HEAD
--- Âîçâðàùàåò èç msg èíôîðìàöèþ î èíñòðóìåíòå íà êîòîðûé ïîäïèñûâàåìñÿ è èíòåðâàëå
=======
--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ Ð¸Ð· msg Ð¸Ð½Ñ„Ð¾Ñ€Ð¼Ð°Ñ†Ð¸ÑŽ Ð¾ Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚Ðµ Ð½Ð° ÐºÐ¾Ñ‚Ð¾Ñ€Ñ‹Ð¹ Ð¿Ð¾Ð´Ð¿Ð¸ÑÑ‹Ð²Ð°ÐµÐ¼ÑÑ Ð¸ Ð¸Ð½Ñ‚ÐµÑ€Ð²Ð°Ð»Ðµ
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function get_candles_param(msg)
	local spl = split(msg.data, "|")
	return spl[1], spl[2], tonumber(spl[3])
end

<<<<<<< HEAD
--- Âîçâðàùàåò óíèêàëüíûé êëþ÷ äëÿ èíñòðóìåíòà íà êîòîðûé ïîäïèñûâàåìñÿ è èíòåðâàëà
=======
--- Ð’Ð¾Ð·Ð²Ñ€Ð°Ñ‰Ð°ÐµÑ‚ ÑƒÐ½Ð¸ÐºÐ°Ð»ÑŒÐ½Ñ‹Ð¹ ÐºÐ»ÑŽÑ‡ Ð´Ð»Ñ Ð¸Ð½ÑÑ‚Ñ€ÑƒÐ¼ÐµÐ½Ñ‚Ð° Ð½Ð° ÐºÐ¾Ñ‚Ð¾Ñ€Ñ‹Ð¹ Ð¿Ð¾Ð´Ð¿Ð¸ÑÑ‹Ð²Ð°ÐµÐ¼ÑÑ Ð¸ Ð¸Ð½ÐµÑ‚Ñ€Ð²Ð°Ð»Ð°
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350
function get_key(class, sec, interval)
	return class .. "|" .. sec .. "|" .. tostring(interval)
end

<<<<<<< HEAD
return qsfunctions
=======
return qsfunctions
>>>>>>> 91b29cc115763bff30f3ed949bc7a2bf88d3b350

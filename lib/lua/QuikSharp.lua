--~ (c) Victor Baybekov, 2014 All Rights Reserved

package.path = package.path..";"..".\\?.lua;"..".\\?.luac"
package.cpath = package.cpath..";"..'.\\clibs\\?.dll'

local sleepMock = function(sec)
	socket.select(nil, nil, sec/1000)
end

if not sleep then
	sleep = sleepMock
end

local socket = require ("socket")
local json = require ("dkjson")

local servicePort = 34130
local callbackPort = 34131
local serviceClient = nil
local callbackClient = nil

-- block until connection
local function connect(port)
	print('Connecting to port '..port..' ...')

	local i = 0
	while true do
		c, err = socket.connect('localhost', port)
		if c then
			break
		else
			sleep(100)
			i = i + 1
			message('Connection attempt #'..i)
		end
	end
	return c
end




local function reconnect()
	print('Reconnecting...')
	if serviceClient then
		serviceClient:close()
		serviceClient = connect(servicePort)
	end
	if callbackClient then
		callbackClient:close()
		callbackClient = connect(callbackPort)
	end
	if serviceClient and callbackClient then
		print 'connected!'
	end
end

local function disconnect()
	print('Disconnecting...')
	if serviceClient then
		serviceClient:close()
	end
	if callbackClient then
		callbackClient:close()
	end
end

local function callService(requestLine)
	print('Calling service with: '..requestLine)
	local res, err = serviceClient:send(requestLine..'\n')
	if res then
		responseLine, err = serviceClient:receive()
		if responseLine then
			return responseLine, nil
		else
			print('Cannot get service response')
			return nil, err
		end
	else
		print('Cannot call the service')
		return nil, err
	end
end



local function listenService(callback)
	requestLine, err = callbackClient:receive()
	if requestLine then
		print('Callback request: '..requestLine)
		responseLine, err = callback(requestLine)
		if responseLine then
			res, err = callbackClient:send(responseLine..'\n')
			if res then
				print('Callback response: '..responseLine)
				return true, nil
			else
				print('Could not send: '..responseLine)
				return nil, err
			end
		else
			return nil, err
		end
	else
		return nil, err
	end
end


echo = function(requestLine)
	return 'Echoed: '..requestLine
end




is_run = true
function main()
	serviceClient = connect(servicePort)
	callbackClient = connect(callbackPort)

	i = 0
	while is_run do --and i < 1000 do
		res, err = callService('Hello, Lua! #'..i)
		res, err = listenService(echo)
		--sleep(100)
		if not res then
			reconnect()
		end
		i = i + 1
	end;
--~ 	while is_run do
--~ 		--print('start listening')
--~ 		res, err = listenService(echo)
--~ 	 	if not res then
--~ 	 		print('Reconnecting...')
--~ 			reconnect()
--~ 		end
--~ 	end
end

function OnInit()
	--message('OnInit', 1)
end

function OnStop()
    is_run = false
	disconnect()
end

--main()

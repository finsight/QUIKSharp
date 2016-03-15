if string.find(package.path,'C:\\Program Files (x86)\\Lua\\5.1\\lua\\?.lua')==nil then
   package.path=package.path..';C:\\Program Files (x86)\\Lua\\5.1\\lua\\?.lua;'
end
if string.find(package.path,'C:\\Program Files\\Lua\\5.1\\lua\\?.lua')==nil then
   package.path=package.path..';C:\\Program Files\\Lua\\5.1\\lua\\?.lua;'
end 

require("QL")

log="Costs_Counter.log"

spot_comission=0.0007
fut_comission=1.85

is_run=false
trades={} -- "транспортная" таблица для сделок. С ее помощью мы будем обрабатывать данные не в коллбэке Квика а в главном потоке ВМ Луа
-- GUI
--создаем таблицу Квик
t=QTable:new()

function OnStop()
  is_run = false
  toLog(log,'OnStop. Script finished manually')
  message ("Script finished manually", 2)
end
function OnInit(path)
  log=getScriptPath()..'\\'..log
  is_run=true
end
function OnTrade(trade)
	trades[#trades+1]=trade
end

function CountandAdd(trade)
	local l,op,bc,rp,qty=t:AddLine(),'',0,0,0
	t:SetValue(l,'Time',datetime2string(trade.datetime))
	t:SetValue(l,'Account',trade.account)
	t:SetValue(l,'Client_code',trade.client_code)
	t:SetValue(l,'Security',trade.sec_code)
	if trade.class_code=='GTS' then
		qty=trade.qty
	else
		qty=trade.qty*getParam(trade.sec_code,'LOTSIZE')
	end
	if string.find(FUT_OPT_CLASSES,trade.class_code)~=nil then bc=qty*fut_comission else bc=trade.value*spot_comission end
	if tradeflags2table(trade.flags).operation=='S' then 
		op='Buy' 
		rp=(trade.value-trade.exchange_comission-bc)/qty
	else 
		rp=(trade.value+trade.exchange_comission+bc)/qty
		op='Sell' 
	end
	t:SetValue(l,'Operation',op)
	t:SetValue(l,'Deal_price',trade.price)
	t:SetValue(l,'Quantity',qty)
	t:SetValue(l,'Volume',trade.value)
	t:SetValue(l,'Stock_comission',trade.exchange_comission)
	t:SetValue(l,'Broker_comission',bc)
	t:SetValue(l,'Full_comission',(trade.exchange_comission+bc))
	t:SetValue(l,'Real_price',rp)
end

function main()
	--добавляем нужные столбцы: 	
	t:AddColumn("Time",QTABLE_STRING_TYPE ,20)
	t:AddColumn("Account",QTABLE_STRING_TYPE,20)
	t:AddColumn("Client_code",QTABLE_STRING_TYPE,20)
	t:AddColumn("Security",QTABLE_STRING_TYPE,20)
	t:AddColumn("Operation",QTABLE_STRING_TYPE,5)
	t:AddColumn("Deal_price",QTABLE_DOUBLE_TYPE ,15)	
	t:AddColumn("Quantity",QTABLE_DOUBLE_TYPE,15)
	t:AddColumn("Volume",QTABLE_DOUBLE_TYPE,20)
	t:AddColumn("Stock_comission",QTABLE_DOUBLE_TYPE,20)
	t:AddColumn("Broker_comission",QTABLE_DOUBLE_TYPE,20)
	t:AddColumn("Full_comission",QTABLE_DOUBLE_TYPE,20)
	t:AddColumn("Real_price",QTABLE_DOUBLE_TYPE,20)
	-- назначаем название для таблицы
	t:SetCaption('Costs_Counter')
	-- показываем таблицу
	t:Show()
	-- добавляем пустую строку
	line=t:AddLine()
		
	local i=0
	for i=0,getNumberOf('trades') do
		row=getItem('trades',i)
		CountandAdd(row)
	end
	toLog(log,"Old trades processed.")

	while is_run do
		if #trades~=0 then
			local t=table.remove(trades,1)
			if t==nil then toLog(log,"Nil trade on remove") else CountandAdd(t) end
		else 
			sleep(1)
		end
	end	
end
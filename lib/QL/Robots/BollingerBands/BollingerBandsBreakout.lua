require "QL"
require 'luaxml'
require 'iuplua'

VERSION='0.1'
log='full_BB.log'
set_path='settings.xml'
is_run=false
security=''
class=''
acc=''
clc=''
slippage=0
stop_slippage=0
take=0
stop=0
last=0
chart=''
position=false
stopnum=0
takenum=0
open_price=0
open_vol=0
newlast={}
trades={}
orders={}
stops={}
transactions={}
orders_to_process={}

empty_str=[[<table>
	<security value="SBER" />
	<volume value="2" />
	<take value="10" />
	<stop value="5" />
	<slippage value="3" />
	<stopslippage value="15" />
	<account value='100101' />
	<clientcode value='100' />
	<graphname value='bb' />
</table>]]

function OnInitDo()
	toLog(log,"Try to open settings "..set_path)
	local f=io.open(set_path)
	if f==nil then f=io.open(set_path,"w") f:close() else f:close() end
	local file=xml.load(set_path)
	toLog(log,"XML loaded")
	if file==nil then
		--message("Begemot can`t open settings file!",3)
		toLog(log,"File can`t be openned! File would be created.")
		file=xml.eval(empty_str)
	end 
	toLog(log,"File oppened")
	ret, file:find("security").value,file:find("account").value,file:find("clientcode").value,file:find("volume").value,
	file:find("slippage").value,file:find("take").value,file:find("stop").value,file:find("stopslippage").value,file:find("graphname").value=
      iup.GetParam("Begemot "..VERSION, nil,
                  "Код бумаги: %s\n"..
				  "Счет: %s\n"..
				  "Код клиента: %s\n"..
				  "Объем: %i\n"..
				  "Отступ: %i\n"..
				  "Тэйк-профит (в шагах цены): %i\n"..
				  "Стоп-лосс (в шагах цены): %i\n"..
				  "Проскальзывание стопов: %i\n"..
				  "Идентификатор графика: %s\n",
				  file:find("security").value,file:find("account").value,file:find("clientcode").value,file:find("volume").value,file:find("slippage").value,
	file:find("take").value,file:find("stop").value,file:find("stopslippage").value,file:find("graphname").value)
	toLog(log,"GetSettingsParam done")
	if (not ret) then
		iup.Message("Bollinger Bands Breakout "..VERSION,"Запуск скрипта отменен.")
		toLog(log,"Cancelled on GetSettingsParam")
		return false
	end
	file:save(set_path)
	toLog(log,'Settings file------')
	toLog(log,file)
	toLog(log,'------------')
	security=file:find("security").value
	class=getSecurityInfo('',security).class_code
	minstep=getParam(security,"SEC_PRICE_STEP")
	volume=tonumber(file:find("volume").value)
	slippage=tonumber(file:find("slippage").value)*minstep
	take=tonumber(file:find("take").value)*minstep
	stop=tonumber(file:find("stop").value)*minstep
	acc=file:find("account").value
	clc=file:find("clientcode").value
	chart=file:find("graphname").value
	stop_slippage=tonumber(file:find("stopslippage").value)*minstep
	toLog(log,'Settings: sec='..security..' cl='..class..' minstep='..minstep..' vol='..volume..' slip='..slippage..' take='..take..' stop='..stop..' stopslip='..stop_slippage)
	if not isChartExist(chart) then
		toLog(log,'Chart doesn`t exist') 
		iup.Message("Bollinger Bands Breakout "..VERSION,"График не может быть найден")
		return false
	end
	toLog(log,"Settings loaded")
	return true
end
function OnOrderDo(order)
	if transactions[order.trans_id]~=nil then
		toLog(log,'Open order received. Num='..order.order_num..' Balance='..order.balance)
		orders_to_process[order.order_num]=transactions[order.trans_id]
		table.remove(transactions,order.order_num)
		if order.balance==0 and getRowFromTable("trades","ordernum",order.order_num)~=nil then 
			toLog(log,"Found trade for lag order. Start OnTradeDo()") 
			for i=0,getNumberOf('trades') do
				if getItem('trades',i).order_num==order.order_num then
					local tr=getItem('trades',i)
					toLog(log,'Process trade#'..tr.trade_num)
					OnTradeDo(tr)
				end
			end		
		end
	end
end
function OnStopOrderDo(stop)
	if position and transactions[stop.trans_id]=='stop' then
		toLog(log,'Update on stop#'..stop.ordernum)
		stopnum=stop.order_num
		if stoporderflags2table(stop.flags).done then
			toLog(log,'Stop order done')
			position=false
			stopnum=0
			if takenum~=0 then
				_,res=killStopOrder(takenum,security,class)
				toLog(log,res)
				takenum=0
			else
				toLog(log,'Take-profit order not received yet. Wait and cancell it.')
				message('Take-profit order not received yet. Wait and cancell it.',3)
			end
			table.remove(transactions,stop.trans_id)
		end
	end
	if position and transactions[stop.trans_id]=='take' then
		toLog(log,'Update on take#'..stop.ordernum)
		takenum=stop.order_num
		if stoporderflags2table(stop.flags).done then
			toLog(log,'Take profit order done')
			position=false
			takenum=0
			if stopnum~=0 then
				_,res=killStopOrder(stopnum,security,class)
				toLog(log,res)
				stopnum=0
			else
				toLog(log,'Stop order not received yet. Wait and cancell it.')
				message('Stop order not received yet. Wait and cancell it.',3)
			end
			table.remove(transactions,stop.trans_id)
		end
	end
end
function OnTradeDo(trade)
	if orders_to_process[trade.order_num]~=nil then
		toLog(log,"New trade for open order#"..trade.order_num..'. Trade#'..trade.trade_num..' Qty='..trade.qty)
		open_price=(open_price*open_vol+trade.qty*trade.price)/(open_vol+trade.qty)
		open_vol=open_vol+trade.qty
		if open_vol==getOrderByNumber(class,trade.order_num).qty then
			--position opennd.e send take&stop
			if orders_to_process[trade.ordernum]=='S' then
				sdir='B'
				smul=1
				tdir='S'
				tmul=-1
			else
				sdir='S'
				smul=-1
				tdir='B'
				tmul=1
			end
			table.remove(orders_to_process,trade.ordernum)
			toLog(log,'Position openned.Price='..open_price)
			local tr,res=sendStop(class,security,sdir,open_price+smul*stop,open_price+smul*stop+smul*stop_slippage,volume,acc,'GTC',clc,'BBstop')
			if tr~=nil then
				transactions[tr]='stop'
			end
			toLog(log,res)
			local tr,res=sendTake(class,security,tdir,open_price+tmul*take,volume,stop_slippage,'PRICE_UNITS',0,"PRICE_UNITS",acc,'GTC',clc,'BBtake')
			if tr~=nil then
				transactions[tr]='take'
			end
			toLog(log,res)
		end
	end
end
function OnNewLastDo(lastprice)
	if not position then
		toLog(log,'Check to open. New Last='..lastprice)
		toLog(log,'line1='..getLastCandle(chart,1).close)
		toLog(log,'closetype='..type(getLastCandle(chart,1).close)..' slip='..type(slippage)..' last='..type(lastprice))
		toLog(log,'line0='..getLastCandle(chart,0).close)
		if getLastCandle(chart,1).close+slippage<lastprice then
			--sell
			local tr,res=sendMarket(class,security,"S",volume,acc,clc,'BBbsell')
			if tr~=nil then
				position=true
				table.insert(transactions,tr,'S')
			end
			toLog(log,res)
		end
		if getLastCandle(chart,0).close-slippage>lastprice then
			--buy
			local tr,res=sendMarket(class,security,"B",volume,acc,clc,'BBbuy')
			if tr~=nil then
				position=true
				table.insert(transactions,tr,'B')
			end
			toLog(log,res)
		end
	end
end
--quik callbacks
function OnStop()
	toLog(log,'Stop pressed')
	is_run=false
end
function OnParam(cl,sec)
	if is_run and cl==class and sec==security and last~=getParam(security,'LAST') then
		last=getParam(security,'LAST')
		--toLog(log,'type='..type(last))
		table.insert(newlast,last)
	end
end
function OnOrder(order)
	if is_run and order.sec_code==security then
		table.insert(orders,order)
	end
end
function OnTrade(trade)
	if is_run and trade.sec_code==security then
		table.insert(trades,trade)
	end
end
function OnTransReply(reply)
	if reply.status~='3' then
		toLog(log,reply)
	end
end
function OnStopOrder(stop)
	if is_run and stop.sec_code==security then
		table.insert(stops,stop)
	end
end

function main()
	is_run=OnInitDo()
	toLog(log,'Main started')
	while is_run do
		if #orders~=0 then
			local trrep=table.remove(orders,1)
			if trrep~=nil then OnOrderDo(trrep) else toLog(log,"Nil Order on remove") end
		elseif #trades~=0 then
			local trrep=table.remove(trades,1)
			if trrep~=nil then OnTradeDo(trrep) else toLog(log,"Nil Trade on remove") end
		elseif #stops~=0 then
			local trrep=table.remove(stops,1)
			if trrep~=nil then OnStopOrderDo(trrep) else toLog(log,"Nil Stop on remove") end
		elseif #newlast~=0 then
			local trrep=table.remove(newlast,1)
			if trrep~=nil then OnNewLastDo(trrep) else toLog(log,"Nil Last on remove") end
		else
			sleep(1)
		end
	end
	toLog(log,'Main ended')
	iup.ExitLoop()
	iup.Close()
end
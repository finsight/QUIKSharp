if string.find(package.path,'C:\\Program Files (x86)\\Lua\\5.1\\lua\\?.lua')==nil then
   package.path=package.path..';C:\\Program Files (x86)\\Lua\\5.1\\lua\\?.lua;'
end
if string.find(package.path,'C:\\Program Files\\Lua\\5.1\\lua\\?.lua')==nil then
   package.path=package.path..';C:\\Program Files\\Lua\\5.1\\lua\\?.lua;'
end 

require"QL"
require"iuplua"
log="auto_stop.log"

--Блок пользовательских переменных. Забиваем в визуальный интерфейс, который помнит ранее введённые данные.
account="game997"
cl_code="game997"
sec="MSICHS"
stop_level=5 --расстояние в шагах цены от индикатора, на котором срабатывает стоп.
slippage=10 --проскальзывание при исполнении ордера в шагах цены. 
chart="auto_stop"

table_mask={}
table_mask.client_code=cl_code
table_mask.seccode=sec
is_run = true

function OnStop(s)
  is_run = false
  toLog(log,'OnStop. Script finished manually')
  message ("auto_stop finished manually", 2)
end

function main()
	log=getScriptPath()..'\\'..log
	toLog(log,"Start main")
	class=getSecurityInfo("",sec).class_code
	--Получаем размер лота
	lotsize=getParamEx(class,sec,"lotsize").param_value
	--Получаем цену последней сделки
	last=getParamEx(class,sec,"last").param_value
	step=getParamEx(class,sec,'SEC_PRICE_STEP').param_value
	stop_level=stop_level*step
	if string.find(FUT_OPT_CLASSES,class)~=nil then acc=account else acc=cl_code end
  while is_run do
	--получаем значение индикатора. Одинаково для Moving Average и Parabolic SAR
	n = getNumCandles (chart)
	if n==0 or n==nil then
		toLog(log,'Can`t get data from chart '..chart)
		message('Не можем получить данные с графика '..chart,1)
		is_run=false
		break
	end
	t,n,s = getCandlesByIndex(chart,0,n-1,1)
	indicator=t[0].close

	sell_out_trigger=toPrice(sec, indicator-stop_level) --цена, при которой спускаем курок на продажу лонга
	buy_out_trigger=toPrice(sec, indicator+stop_level) --цена, при которой спускаем курок на откуп шорта
	
	--Получаем текущий баланс по торгуемому инструменту

	balance=getPosition(sec,acc)
	close_order_size=math.abs(balance/lotsize) --считаем размер ордера-закрывашки
	toLog(log,balance)

	--ЗАКРЫТИЕ ПОЗИЦИЙ

	if balance>0 and last < sell_out_trigger then 
		toLog (log, 'Sell out signal. Balance='..balance..' Last='..last..' SellTrigerPrice='..sell_out_trigger)
		comment="sell_out!" --комментарий: продаём!
		sell_out=sell_out_trigger-slippage
		_,reply=sendLimit(class,sec,"S",sell_out,close_order_size,account,cl_code,comment)
		toLog(log, reply)
	end 
	
	if balance<0 and last > buy_out_trigger then 
		toLog (log, 'Buy out signal. Balance='..balance..' Last='..last..' SellTrigerPrice='..buy_out_trigger)
		comment="buy_out!" --комментарий: продаём!
		buy_out=buy_out_trigger+slippage
		_,reply=sendLimit(class,sec,"B",buy_out,close_order_size,account,cl_code,comment)
		toLog(log, reply)
	end 
	sleep(1000)
	--killAllOrders(table_mask) --рекомендуется после срабатывания стопа принудительно почистить все заявки в стакане по этому инструменту чтобы вдруг не осталось лишних, в том числе "забытых" по неосторожности. Бывает, что оставшиеся в стакане заявки блокируют средства и не дают продать по стопу. killAllOrders даст гарантию что на следующем круге цикла мы точно выйдем из позиции.
  end
  toLog(log,"Main ended")
end
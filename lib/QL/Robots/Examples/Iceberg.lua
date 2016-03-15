--[[Робот "Айсберг" предназначен для трейдера, который хочет мелкими пакетами купить или продать крупный объём. Робот работает по такому алгоритму:
1. Трейдер задаёт торговый инструмент и размер позиции, которую хочет иметь
2. Робот выставляет в стакан "лучшим" заданный объём малого пакета этого инструмента (верхушку айсберга) и ждёт акцепта.
3. Если его обгоняют, он тоже обгоняет. Если позади него появляется пустое место, куда можно передвинуться, он передвигается. Так реализуется механизм Best Execution.
4. После акцепта робот выставляет новый малый объём в стакан.
5. Так продолжается до тех пор, пока текущий баланс не сравняется с желаемым балансом.

Код оптимизирован для работы на рынке акций Московской и Украинской биржи.
Для срочного рынка НЕ ПОДХОДИТ.
--]]

if string.find(package.path,'C:\\Program Files (x86)\\Lua\\5.1\\lua\\?.lua')==nil then
   package.path=package.path..';C:\\Program Files (x86)\\Lua\\5.1\\lua\\?.lua;'
end
if string.find(package.path,'C:\\Program Files\\Lua\\5.1\\lua\\?.lua')==nil then
   package.path=package.path..';C:\\Program Files\\Lua\\5.1\\lua\\?.lua;'
end

require("QL")
require("iuplua")

log="Iceberg.log"

--Вводим номер торгового счёта
account="game1777"

--Вводим код клиента
client_code="game1777"

--К заявкам будем дописывать код "Iceberg"
comment="Iceberg"

--Вводим торговый инструмент
_,security,class = iup.GetParam("Trade instrument", nil, "Ticker: %s\n Class: %s\n","","")

toLog(log,'Start script with sec='..tostring(security)..' class='..tostring(class))
table_mask={}
table_mask.seccode=security

--class=getSecurityInfo("",security).class_code

--Получаем шаг цены
step=getParamEx(class,security,"SEC_PRICE_STEP").param_value
--toLog(log, "step="..step)

--[[Выбираем, размер позиции в штуках акций, который хотим иметь. Если нужно, чтобы робот Айсберг закрыл позицию, пишем 0.
Пример 1. На балансе 0 акций GAZP. Хотим, чтобы робот набрал позицию в размере 10 000 акций. Значит пишем 10 000.
Пример 2. На балансе 0 акций SBER . Хотим чтобы робот зашортил 5000 акций. Значит пишем -5000.
Пример 3. На балансе 100 000 акций ALMK. Хотим, чтобы робот закрыл эту позицию в ноль. Значит пишем 0.
Пример 4. На балансе шорт -100 акций LKOH. Хотим чтобы робот откупил этот шорт и набрал лонг 50 акций. Значит пишем 0.
--]]

_, final_balance=iup.GetParam("Final Balance", nil, "Enter the final balance: %s\n","")
final_balance=tonumber(final_balance)

--toLog(log, final_balance)

--Берём из ТТП количество акций в лоте
lotsize=getParamEx(class,security,"lotsize").param_value

--Задаём размер айсберга, видимый в стакане
_, iceberg_size=iup.GetParam("Iceberg Size", nil, "Enter small volume: %s\n","")
iceberg_size=tonumber(iceberg_size)

--Задаём максимальную пороговую цену покупки (минимальную пороговую цену продажи)
_, threshold=iup.GetParam("Threshold price", nil, "Enter max buy price (min sell price): %s\n","")
threshold=tonumber(threshold)


is_run = true

function get_balance(security, client_code)
      local n=getNumberOf("depo_limits")
      ----toLog (log, "number_of_depo_limits= "..n)
      for i=0,n-1 do
            limit = getItem("depo_limits", i)
            ----toLog (log, limit)
            if limit~=nil and limit.sec_code == security and limit.client_code == client_code then
               return limit.currentbal
            end
      end
      return 0
end

function OnStop()
  is_run = false
  --toLog(log,'OnStop. Script finished manually')
  reply,ms=killAllOrders(table_mask)
  message ("Script finished manually", 2)
end

function main()

while is_run do

	--Получаем стакан. Берём первых и вторых лучших.

	local qt = getQuoteLevel2(class, security)
	local bid_1 = qt.bid[tonumber(qt.bid_count)].price
	local bid_1_q = qt.bid[tonumber(qt.bid_count)].quantity
	local offer_1 = qt.offer[1].price
	local offer_1_q = qt.offer[1].quantity
	local bid_2 = qt.bid[tonumber(qt.bid_count)-1].price
	local offer_2 = qt.offer[2].price

	balance=get_balance(security, client_code)
	toLog (log, "balance "..balance)
	toLog(log, "final_balance "..final_balance)
	if final_balance>balance then
		mode="BUY MODE"
		distance_lots=((final_balance-balance)/lotsize)
		toLog (log, distance_lots)
		if distance_lots>=iceberg_size then
			order_volume=iceberg_size
			else
			order_volume=distance_lots
		end

		if bid_1+step<threshold then
			buy_price=bid_1+step
			else
			buy_price=threshold
		end

		send_limit_buy, reply=sendLimit(class,security,"B",buy_price,order_volume,account,client_code,comment)

	elseif final_balance<balance then
		mode="SELL MODE"
		distance_lots=((balance-final_balance)/lotsize)
		toLog (log, distance_lots)
		if distance_lots>=iceberg_size then
			order_volume=iceberg_size
			else
			order_volume=distance_lots
		end

		if offer_1-step>=threshold then
			sell_price=offer_1-step
			else
			sell_price=threshold
		end

		send_limit_sell, reply=sendLimit(class,security,"S",sell_price,order_volume,account,client_code,comment)
	else
		mode="WE HAPPY"
		iup.Message('Finish','You have '..balance.." "..security)
		_,_=killAllOrders(table_mask)
		is_run=false
	end
	toLog (log, mode)

	if is_run then
	sleep(10000) -- засыпаем на 10 секунд
	_,_=killAllOrders(table_mask)
	sleep(1000)
	end
end
end

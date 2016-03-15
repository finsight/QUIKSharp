--[[

Ссылка на книгу Куртис Фейс 
Путь Черепах: Из дилетантов в легендарные трейдеры

http://narod.ru/disk/44642845001.c7d241e5c905fa6ea63c44d3f4d4e30e/K.Fejs%20Put%20Cherepah.%20Iz%20diletantov%20v%20legendarnye%20trejdery.rar.html


Запуск советника по торговой стратегии черепах.

1. Как запустить робота на Lua в Quik. Общая инструкция. 
http://forum.qlua.org/topic34.html

2. В качества детектора пробивания ценового максимума или минимума используем индикатор Price Channel. Настраиваем его так:
http://radikal.ua/data/upload/6895e/4efc3/4bcfd54945.gif
(указываем период 20 или 55 в зависимости от выбора Системы 1 или Системы 2)

http://radikal.ua/data/upload/69fda/6895e/97f1e9df78.gif
(в качестве идентификатора пишем маленькую английскую t. Потому что черепахи :)))) )

3. Графику цены тоже присваиваем идентификатор:

http://radikal.ua/data/upload/4efc3/c2184/3ff1b83c96.gif

(идентификатор - маленькая английская p)

4. Добавляем в текущую таблицу параметров акции и фьючерсы, которые хотим мониторить. Ставим якорь между текущей таблицей параметров и графиком, чтобы удобно было переходить с графика на график. Листаем, ищем сигналы. Внимательно смотрим, чтобы робота не выбивало при листании. Если выбило, удаляем старую таблицу робота TURTLE ADVISER и запускаем его по новой. Признак выбивания - красный квадрат.
 
Вот примерный вид рабочего пространства:
 
http://radikal.ua/data/upload/69fda/04012/3dc9e5a0c2.gif
 
--]]

require"QL"
log="TURTLES.log"
--идентификаторы графиков
chart_inst="t" --идентификатор индикатора Price Channel
price_chart="p"	--идентификатор графика цены Price
is_run = true

function OnStop()
  is_run = false
  --toLog(log,'OnStop. Script finished manually')
  -- уничтожаем таблицу Квик
  t:delete()
end

function main()
	log=getScriptPath()..'\\'..log
	--toLog(log,"Start main")
	--создаем таблицу Квик
	t=QTable:new()
	-- добавляем 2 столбца
	t:AddColumn("SIGNAL",QTABLE_STRING_TYPE,45)


	-- назначаем название для таблицы
	t:SetCaption('TURTLE ADVISER')
	-- показываем таблицу
	t:Show()
	
	-- добавляем пустую строку
	line=t:AddLine()
	
	while is_run do
	sleep (10)
	
	--Получаем цену последней сделки с графика цены
	num = getNumCandles (price_chart)	
	
	if num==nil or num==0 then
		while num==nil or num==0 do
			sleep (100)
			num = getNumCandles (price_chart)
		end
	end
	
	t_cur = getCandlesByIndex(price_chart,0,num-1,1)
	if t_cur[0]==nil then
		while t_cur[0]==nil do
			sleep (100)
			num = getNumCandles (price_chart)	
			t_cur = getCandlesByIndex(price_chart,0,num-1,1)
		end
	end
	
	last_price=t_cur[0].close
	
	--Получаем данные индикатора "Price Channel"

		price_channel_n = getNumCandles (chart_inst)
		if price_channel_n==nil or price_channel_n==0 then
			while price_channel_n==nil or price_channel_n==0 do
				sleep (100)

				price_channel_n = getNumCandles (chart_inst)
			end
		end
				
		if price_channel_n~=nil then
		
			--текущий верхний Price Channel
			if getCandlesByIndex(chart_inst,0,price_channel_n-1,1)[0]~=nil then 
				line_10 = getCandlesByIndex(chart_inst,0,price_channel_n-1,1)[0].close
			end
			
			--предыдущий верхний Price Channel
			if getCandlesByIndex(chart_inst,0,price_channel_n-2,1)[0]~=nil then
				line_11 = getCandlesByIndex(chart_inst,0,price_channel_n-2,1)[0].close 
			end
			
			--позапредыдущий верхний Price Channel
			if getCandlesByIndex(chart_inst,0,price_channel_n-3,1)[0]~=nil then
				line_12 = getCandlesByIndex(chart_inst,0,price_channel_n-3,1)[0].close 
			end
			
			--поза-позапредыдущий верхний Price Channel
			if getCandlesByIndex(chart_inst,0,price_channel_n-4,1)[0]~=nil then
				line_13 = getCandlesByIndex(chart_inst,0,price_channel_n-4,1)[0].close
			end
			
			--текущий нижний Price Channel
			if getCandlesByIndex(chart_inst,2,price_channel_n-1,1)[0]~=nil then
				line_30 = getCandlesByIndex(chart_inst,2,price_channel_n-1,1)[0].close 
			end
			
			--предыдущий нижний Price Channel
			if getCandlesByIndex(chart_inst,2,price_channel_n-2,1)[0]~=nil then
				line_31 = getCandlesByIndex(chart_inst,2,price_channel_n-2,1)[0].close
			end
		
			
			--позапредыдущий нижний Price Channel
			if getCandlesByIndex(chart_inst,2,price_channel_n-3,1)[0]~=nil then
				line_32 = getCandlesByIndex(chart_inst,2,price_channel_n-3,1)[0].close
			end
			
			--поза-позапредыдущий нижний Price Channel
			if getCandlesByIndex(chart_inst,2,price_channel_n-4,1)[0]~=nil then
				line_33 = getCandlesByIndex(chart_inst,2,price_channel_n-4,1)[0].close
			end
			
		end
		
		--Детектор сигнала
		SIGNAL="NO SIGNAL"
	
		if (last_price>line_11 and line_11==line_12) or (last_price>line_12 and line_12==line_13) then
			SIGNAL="BUY"
		end
		
		if (last_price<line_31 and line_31==line_32) or (last_price<line_32 and line_32==line_33) then
			SIGNAL="SELL"
		end
			
		-- заполняем значения для ячеек таблицы
		t:SetValue(line,"SIGNAL",SIGNAL)
		sleep(100)
	end
end
require "QL" --подключаем библиотеку
require "iuplua" --подключаем библиотеку

--Задаём глобальные переменные.
is_run = false --переменная для запуска-остановки функции main
log="AvrVol.log" --задаём лог-файл
ticker_list ="UNAF,CEEN,ALMK,MSICH" --задаём список инструментов, по которым считаем средний спред
data={} -- таблица в которой будем хранить всю необходимую информацию
trades={} -- "транспортная" таблица для обезличенных сделок. С ее помощью мы будем обрабатывать данные не к коллбэке Квика а в главном потоке ВМ Луа
--создаём объекты интерфейса
mainbox=iup.vbox{} -- главный вертикальный бокс в котором будут хранится контролы для каждой бумаги
Dialog=iup.dialog{mainbox; title="Average Volume", size="THIRDxTHIRD"} -- главное диалоговое окно
--задаём действия при закрытии визуального интерфейса
function Dialog:close_cb()
	toLog(log,"Interface close button pressed")
	is_run = false
end
--Функция OnInit вызывается терминалом QUIK перед вызовом функции main(). В качестве параметра принимает значение полного пути к запускаемому скрипту. 
function OnInit(path)
  is_run = true
  -- изменяем путь к файлу до полного абсолютного
  log=getScriptPath().."\\"..log
  data_file=getScriptPath().."\\AverageVolume_"..getTradeDate()..".csv"
end
--Функция вызывается терминалом QUIK при остановке скрипта из диалога управления. 
function OnStop()
  toLog(log,"Script stopped")
  is_run = false
end
-- коллбэк Квика который вызывается при появлении новой обезличенной сделки
function OnAllTrade(trade)
	-- если скрипт не запущен или сделка по бумаге не из нашего списка - то сделка не обрабатывается
	if is_run and data[trade.seccode]~=nil then
		trades[#trades+1]=trade
	end
end
--Ключевая функция скрипта. В ней идёт расчёт данных и их отображение и запись в файл
function AvrVol(trade)
	-- для ускорения обращения к данным создаем локальную переменную
	local dat=data[trade.seccode]
	toLog(log,"New trade for "..trade.seccode.." Num="..trade.tradenum)
	-- расчитываем значения необходимые для расчетов средних
	dat.trade_num=dat.trade_num+1
	dat.vol=dat.vol+trade.qty
	dat.avr_vol=dat.vol/dat.trade_num
	-- определяем направление сделки (функция из библиотеки QL)
	local dir=tradeflags2table(trade.flags).operation
	dat[dir..'trade_num']=dat[dir..'trade_num']+1
	dat[dir..'vol']=dat[dir..'vol']+trade.qty
	dat['avr_'..dir..'vol']=dat[dir..'vol']/dat[dir..'trade_num']
	-- открываем файл на дозапись
	local file=io.open(data_file,'a')
	-- записываем строчку с новыми значениями
	file:write(trade.seccode..","..dat.avr_vol..","..dat.avr_Bvol..","..dat.avr_Svol.."\n") --пишем результат расчёта среднего значения в файл
	file:flush()
	-- закрываем файл
	file:close()
	-- изменяем соответствующие значения в интерфейсе. указатели на нужные контролы мы храним в таблице с данными
	dat.avr_lbl.title='Average='..string.format('%.2f',dat.avr_vol)
	dat.bavr_lbl.title='Buy Average='..string.format('%.2f',dat.avr_Bvol)
	dat.savr_lbl.title='Sell Average='..string.format('%.2f',dat.avr_Svol)
	toLog(log," Average calculated. New values "..trade.seccode.." Average Volume="..dat.avr_vol.." Average Buy Volume="..dat.avr_Bvol.." Average Sell Volume="..dat.avr_Svol)
end
-- функция инициализации. ее задача создать все необходимые объекты
function OnInitDo(list)
	toLog(log,"Start initialization...")
	local sec=""
	local ticker_lbl,avr_lbl,barv_lbl,savr_lbl,hbox
	-- цикл по строке с тикерами
	for sec in string.gmatch(list,"%a+") do
		-- создание переменных для хранения данных для тикера sec
		data[sec]={}
		data[sec].avr_vol=0
		data[sec].vol=0
		data[sec].trade_num=0
		data[sec].avr_Bvol=0
		data[sec].Bvol=0
		data[sec].Btrade_num=0
		data[sec].avr_Svol=0
		data[sec].Svol=0
		data[sec].Strade_num=0
		-- создание визуальных контролов
		ticker_lbl=iup.label{title=sec,expand="YES"}
		data[sec].avr_lbl=iup.label{title='Average=',expand="YES"}
		data[sec].bavr_lbl=iup.label{title='Buy Average=',expand="YES"}
		data[sec].savr_lbl=iup.label{title='Sell Average=',expand="YES"}
		hbox=iup.hbox{ticker_lbl,data[sec].avr_lbl,data[sec].bavr_lbl,data[sec].savr_lbl}
		-- добавление созданного горизонтального бокса с контролами в главный бокс интерфейса
		if iup.Append(mainbox,hbox)==nil then toLog(log,"Can`t append interface element") return false end
		toLog(log,sec..' added to list')
	end
	-- создание файла с расчетными данными
	local f=io.open(data_file,'w+')
	if f==nil then 
		toLog(log,"Can`t create data file at "..data_file)
		return false
	end
	f:write('TICKER,AVERAGE_VOLUME,AVERAGE_BUY_VOLUME,AVERAGE_SELL_VOLUME\n') 
	f:flush() 
	f:close() 
	toLog(log,'Initialization ended')
	return true
end
--Главная функция скрипта, которая крутится в бесконечном цикле
function main()
	is_run=OnInitDo(ticker_list)
	if is_run then
		toLog(log,"Main started")
		Dialog:show() --вызываем метод show графической библиотеки
		local i=0
		toLog(log,"#all_trades="..getNumberOf('all_trades'))
		for i=0,getNumberOf('all_trades') do
			row=getItem('all_trades',i)
			if data[row.seccode]~=nil then AvrVol(row) end
		end
		toLog(log,"Old trades processed.")
	end
	while is_run do
		if #trades~=0 then
			local t=table.remove(trades,1)
			if t==nil then toLog(log,"Nil trade on remove") else AvrVol(t) end
		else 
			iup.LoopStep()
			sleep(1)
		end
	end
	Dialog:destroy()
	iup.ExitLoop()
	iup.Close()
	toLog(log,"Main ended")
end
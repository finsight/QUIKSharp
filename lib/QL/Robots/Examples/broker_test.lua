require "QL"
require 'iuplua'

log='BrokerTest.log'
go=false
is_run=false
class='SPBFUT'
sec='RIM3'
acc=' '
clc=' '
vol=1
num_orders=10
orders={}
index=0
last_send_tr=-666
last_cancell_tr=-666
--GUI
pbar=iup.progressbar{value=0}
stop_btn=iup.button{title='Stop'}
function stop_btn:action()
	toLog(log,'Stop button pressed')
	go=false
	is_run=false
	return iup.DEFAULT
end
pvbox=iup.vbox{pbar,stop_btn}
pvbox.alignment='ACENTER'
pvbox.gap='10'
progress_dlg=iup.dialog{pvbox;title='Broker latency test'}
function progress_dlg:close_cb()
	toLog(log,'Dialog closed')
	is_run=false
	go=false
end
max_ping=iup.label{title='0.000',expand='YES'}
avr_ping=iup.label{title='0.000',expand='YES'}
min_send=iup.label{title='0.000',expand='YES'}
max_send=iup.label{title='0.000',expand='YES'}
avr_send=iup.label{title='0.000',expand='YES'}
min_cancell=iup.label{title='0.000',expand='YES'}
avr_cancell=iup.label{title='0.000',expand='YES'}
max_cancell=iup.label{title='0.000',expand='YES'}
mark=iup.label{title='0',font='Arial, Bold 44',fgcolor='0 0 0'}
mvbox=iup.vbox{iup.label{title='Оценка скорости (0-5)   '},mark}
mvbox.alignment="ACENTER"
mvbox.gap='15'
result_dlg=iup.dialog{iup.hbox{iup.vbox{max_ping,avr_ping,min_send,max_send,avr_send,min_cancell,avr_cancell,max_cancell},mvbox};title='Broker latency test',size='240x100'}
function result_dlg:close_cb()
	toLog(log,'Result dialog closed')
	iup.ExitLoop()
	return iup.CLOSE
end
--
function OnTransReply(reply)
	--toLog(log,'Reply')
	if reply.trans_id==last_send_tr then
		--toLog(log,'Send tr received. Onum='..tostring(reply.order_num)..' TRId='..reply.trans_id)
		--toLog(log,reply)
		orders[index].tr_receive_time=os.clock()
	end
	if reply.trans_id==last_cancell_tr then
		--toLog(log,'Cancell tr received')
		--toLog(log,reply)
		orders[index].cancell_tr_receive_time=os.clock()
	end
end
function OnOrder(order)
	if order.trans_id==last_send_tr then
		if bit.band(bit.tobit(order.flags),1)~=0  then
			if last_cancell_tr==-666 then
				toLog(log,'Order received')
				orders[index].ord_receive_time=os.clock()
				local tr,mes=killOrder(order.order_num,sec,class)
				if tr~=nil then last_cancell_tr=tr orders[index].cancell_send_time=os.clock() end
				toLog(log,mes)
			end
		else
			toLog(log,'Cancelled order received')
			orders[index].ord_cancell_time=os.clock()
			last_cancell_tr=-666
			last_send_tr=-666
			if index==num_orders then
				toLog(log,'All sent')
				is_run=false
			else
				pbar.value=pbar.value+1
				go=true
			end
		end
	end
end
function main()
	toLog(log,'Start main')
	ret,sec,acc,clc,vol,num_orders=iup.GetParam('Broker latency test',nil,
	"Бумага: %s\n"..
	"Номер счета: %s\n"..
	"Код клиента: %s\n"..
	"Объем заявок: %i\n"..
	"Количество транзакций: %i\n",sec,acc,clc,vol,num_orders)
	if not ret then
		iup.Message("FAMA 3 LE","Запуск скрипта отменен.")
		toLog(log,"Cancelled on GetSettingsParam")
	else
		toLog(log,'Satrt to run test...')
		class=getClass(sec)
		if getQuoteLevel2(class,sec).bid==nil then
			iup.Message('Broker latency test',' Oткройте стакан по инструменту '..sec)
			iup.Close()
			return
		end
		pbar.max=num_orders
		progress_dlg:show()
		is_run=true
		go=true
	end
	while is_run do
		if go then
			toLog(log,'Start to send new transaction.')
			local qt=getQuoteLevel2(class,sec)
			tr,mes=sendLimit(class,sec,'B',qt.bid[1].price,vol,acc,clc,'BrokerTest')
			if tr~=nil then index=index+1 orders[index]={} orders[index].transaction=tr orders[index].tr_send_time=os.clock() last_send_tr=tr 
			else 
				iup.Message('Broker latency test',mes)
				go=false
				is_run=false
				iup.Close()
				return
			end
			toLog(log,mes)
			go=false
		end
		iup.LoopStep()
	end
	progress_dlg:hide()
	local max=math.max
	local min=math.min
	local send_max_lat,send_avr_lat,send_min_lat=0,0,9999999
	local cancell_max_lat,cancell_avr_lat,cancell_min_lat=0,0,9999999
	for i=1,#orders do
	--' d1='..(orders[i].tr_recive_time-orders[i].tr_send_time)..
		v=orders[i]
		v.d1=(v.tr_receive_time-v.tr_send_time)
		v.d2=(v.ord_receive_time-v.tr_send_time)
		v.d3=(v.cancell_tr_receive_time-v.cancell_send_time)
		v.d4=(v.ord_cancell_time-v.cancell_send_time)
		toLog(log,'Order#'..i..' STR d1='..string.format('%.3f',v.d1)..' SOrd d2='..string.format('%.3f',v.d2)..' CTR d3='..string.format('%.3f',v.d3)..' COrd d4='..string.format('%.3f',v.d4))
		send_max_lat=max(send_max_lat,v.d2)
		send_min_lat=min(send_min_lat,v.d2)
		cancell_max_lat=max(cancell_max_lat,v.d4)
		cancell_min_lat=min(cancell_min_lat,v.d4)
		send_avr_lat=(send_avr_lat+v.d2)/2
		cancell_avr_lat=(cancell_avr_lat+v.d4)/2
	end
	toLog(log,"MAXPINGDURATION="..GetInfoParam("MAXPINGDURATION"))
	max_ping.title='Макс. пинг до сервера - '..GetInfoParam("MAXPINGDURATION")
	toLog(log,"AVGPINGDURATION="..GetInfoParam("AVGPINGDURATION"))
	avr_ping.title='Средний пинг до сервера - '..GetInfoParam("AVGPINGDURATION")
	min_send.title='Мин. задержка постановки - '..string.format('%.3f',send_min_lat)
	max_send.title='Макс. задержка постановки - '..string.format('%.3f',send_max_lat)
	avr_send.title='Сред. задержка постановки - '..string.format('%.3f',send_avr_lat)
	min_cancell.title='Мин. задержка снятия - '..string.format('%.3f',cancell_min_lat)
	avr_cancell.title='Сред. задержка снятия - '..string.format('%.3f',cancell_avr_lat)
	max_cancell.title='Макс. задержка снятия - '..string.format('%.3f',cancell_max_lat)
	local avr=(send_avr_lat+cancell_avr_lat)/2
	if string.find(FUT_OPT_CLASSES,class)~=nil then
		if avr<0.2 then
			mark.title='5'
			mark.fgcolor='0 255 0'
		elseif avr<0.4 then
			mark.title='4'
			mark.fgcolor='102 205 0'
		elseif avr<0.6 then
			mark.title='3'
			mark.fgcolor='154 205 50'
		elseif avr<0.8 then
			mark.title='2'
			mark.fgcolor='255 127 36'
		elseif avr<1 then
			mark.title='1'
			mark.fgcolor='255 64 64'
		else
			mark.title='0'
			mark.fgcolor='139 35 35'
		end
	else
		if avr<0.4 then
			mark.title='5'
			mark.fgcolor='0 255 0'
		elseif avr<0.8 then
			mark.title='4'
			mark.fgcolor='102 205 0'
		elseif avr<1.2 then
			mark.title='3'
			mark.fgcolor='154 205 50'
		elseif avr<1.6 then
			mark.title='2'
			mark.fgcolor='255 127 36'
		elseif avr<2 then
			mark.title='1'
			mark.fgcolor='255 64 64'
		else
			mark.title='0'
			mark.fgcolor='139 35 35'
		end
	end
	result_dlg:show()
	iup.MainLoop()
	--toLog(log,'After main loop')
	iup.ExitLoop()
	iup.Close()
end
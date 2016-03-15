work_time = {100500, 135900, 140400, 184000} -- интервалы времени, когда мониторить стаканы
class = 'SPBFUT'
sec = {'RIH3', 'SiH3', 'SRH3', 'EDH3', 'VBH3', 'GDH3', 'BRH3'} -- инструменты для слежения (соотв. стаканы должны быть открыты)
max_depth = 10 -- глубина просмотра стакана

------------- глобальные переменные ---------------------------------

status = "Скрипт запущен"
f = {}

----------------------Функции обратного вызова------------------------------------------------------------------

function OnInit()
	if status ~= "Скрипт остановлен" then
		for i = 1, #sec, 1 do
			f[sec[i]] = io.open('begemot_log_' .. sec[i] .. '.csv', 'a+')
			f[sec[i]]:write('time;avr_bid;avr_offer;max_bid;max_offer;spread' .. '\n')
			f[sec[i]]:flush()
		end
		message (status, 1)
	end
end--OnInit

function OnStop()
    status = "Скрипт остановлен"
	for i = 1, #sec, 1 do
		f[sec[i]]:close()
	end
	message (status, 1)
end--OnStop

function main()
    while status ~= "Скрипт остановлен" do
		if isConnected() == 1 then
			local now =	string.gsub(getInfoParam('SERVERTIME'),':','')*1
			if (now >= work_time[1] and now <= work_time[2]) or (now >= work_time[3] and now <= work_time[4]) then
				for i = 1, #sec, 1 do
					ql2 = getQuoteLevel2(class, sec[i])

					avr_bid = 0
					max_bid = 0
					bid_count = math.min(ql2.bid_count, max_depth)
					for i = 0, bid_count-1, 1 do
						avr_bid = avr_bid + ql2.bid[ql2.bid_count-i].quantity
						max_bid = math.max(max_bid, ql2.bid[ql2.bid_count-i].quantity)
					end
					avr_bid = avr_bid / bid_count

					avr_offer = 0
					max_offer = 0
					offer_count = math.min(ql2.offer_count, max_depth)
					for i = 1, offer_count, 1 do
						avr_offer = avr_offer + ql2.offer[i].quantity
						max_offer = math.max(max_offer, ql2.offer[i].quantity)
					end
					avr_offer = avr_offer / offer_count

					spread = 0
					if tonumber(ql2.bid_count) > 0 and tonumber(ql2.offer_count) > 0 then
						spread = ql2.offer[1].price - ql2.bid[tonumber(ql2.bid_count)].price
					end

					f[sec[i]]:write(now .. ';' .. avr_bid .. ';' .. avr_offer .. ';' .. max_bid .. ';' .. max_offer .. ';' .. spread .. '\n')
					f[sec[i]]:flush()
				end
			end
		end
		sleep(1000)
	end
end--main

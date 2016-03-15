-- version 0.5.2
-- bug FIXed in TradeBid, TradeOffer, FindBidClosePrice
-- 0.3 transaction sync mechanizm
-- 0.3.1 reduce exess transactions
-- 0.3.2 bug in change orders on update
-- 0.4.0 moved from OnAllTrade to OnParam, no class in settings
-- 0.4.1 new condition for close order
-- 0.4.2 interface at settings load
-- 0.4.3 bug in close
-- GUI & initialization method changed
-- 0.5.1 bad close slippage parameter
-- 0.5.2 close script if table closed
require"QL"
--require"luaxml"
require'iuplua'
VERSION='0.5.0'
log="begemot4free.log"
settings_file="settings.lua"
--watch_list={}
transactions={}
quotes={}
orders={}
all_trades={}
on_param={}
trans_replies={}
bad_transactions={}
is_run=false
tbl=QTable:new()


function setStatus(type)
	if type=='Bid' then tbl:SetValue(watch_list.line,'BidStatus',watch_list.status_bid) end
	if type=='Offer' then tbl:SetValue(watch_list.line,'OfferStatus',watch_list.status_offer) end
end
function OnTableClose(t_id, msg,  par1, par2)
	if msg==QTABLE_CLOSE then
		toLog(log,'Table closed. Stop script')
		message('Begemot stopped.',3)
		OnStop()
	end
end
function getSettings(path)
	toLog(log,"Try to open settings "..path)
	local f=io.open(path)
	if f==nil then
		toLog(log,"File can`t be openned! File would be created.")
		watch_list={}
		watch_list.code=' '
		watch_list.class=' '
		watch_list.volume_offer=0
		watch_list.volume_bid=0
		watch_list.tp=0
		watch_list.volume=0
		watch_list.account=' '
		watch_list.client_code=' '
		watch_list.bidEnable=0
		watch_list.offerEnable=0
		watch_list.bad_close_slippage=1
	else f:close() dofile(path) end
	toLog(log,"settings loaded")
	toLog(log,watch_list)
	ret, watch_list.code,watch_list.class,watch_list.offerEnable,watch_list.bidEnable,watch_list.volume_bid,watch_list.volume_offer,watch_list.tp,
	watch_list.bad_close_slippage,watch_list.volume,watch_list.account,watch_list.client_code=
      iup.GetParam("Begemot4Free "..VERSION, nil,
                  "Код бумаги: %s\n"..
				  "Код класса: %s\n"..
				  "Разрешить торговлю от Аска: %b\n"..
				  "Разрешить торговлю от Бида: %b\n"..
				  "Объем бегемота для Бид: %i\n"..
				  "Объем бегемота для Аск : %i\n"..
				  "Тэйк-профит (в шагах цены): %i\n"..
				  "Проскальзывание екстренного закрытия: %i\n"..
				  "Объем заявок: %i\n"..
				  "Номер счета: %s\n"..
				  "Код клиента: %s\n",
				  watch_list.code,watch_list.class,watch_list.offerEnable,watch_list.bidEnable,watch_list.volume_bid,watch_list.volume_offer,watch_list.tp,
	watch_list.bad_close_slippage,watch_list.volume,watch_list.account,watch_list.client_code)
	toLog(log,"GetSettingsParam done")
	if (not ret) then
		--iup.Message("Begemot "..VERSION,"Запуск скрипта отменен.")
		message("Begemot4Free "..VERSION.." Запуск скрипта отменен.",3)
		toLog(log,"Cancelled on GetSettingsParam")
		return false
	end
	watch_list.class=trim(watch_list.class)
	watch_list.code=trim(watch_list.code)
	watch_list.class=trim(watch_list.class)
	watch_list.client_code=trim(watch_list.client_code)
	watch_list.account=trim(watch_list.account)
	watch_list.position_bid=0
	watch_list.position_offer=0
	watch_list.status_bid=""
	watch_list.status_offer=""
	watch_list.order_bid={}
	watch_list.trans_bid=0
	watch_list.trans_offer=0
	watch_list.order_offer={}
	watch_list.open_price_offer=0
	watch_list.open_price_bid=0
	watch_list.bad_bid=false
	watch_list.bad_offer=false
	watch_list.minstep=getParamEx(watch_list.class,watch_list.code,"SEC_PRICE_STEP").param_value
	tbl:SetCaption("Begemot "..VERSION)
	tbl:AddColumn('Security',QTABLE_CACHED_STRING_TYPE,15)
	tbl:AddColumn('BidEnable',QTABLE_CACHED_STRING_TYPE,15)
	tbl:AddColumn('BidStatus',QTABLE_STRING_TYPE,15)
	tbl:AddColumn('OfferEnable',QTABLE_CACHED_STRING_TYPE,15)
	tbl:AddColumn('OfferStatus',QTABLE_STRING_TYPE,15)
	tbl:SetTableNotificationCallback(OnTableClose)
	tbl:Show()
	local l=tbl:AddLine()
	watch_list.line=l
	tbl:SetValue(l,'Security',watch_list.code)
	if watch_list.bidEnable==1 then tbl:SetValue(l,'BidEnable','Включен') tbl:SetColor(l,'BidEnable',LIGHT_GREEN,nil,LIGHT_GREEN,nil) else tbl:SetValue(l,'BidEnable','Выключен') end
	if watch_list.offerEnable==1 then tbl:SetValue(l,'OfferEnable','Включен') tbl:SetColor(l,'OfferEnable',LIGHT_GREEN,nil,LIGHT_GREEN,nil) else tbl:SetValue(l,'OfferEnable','Выключен') end
	toLog(log,"Settings loaded")
	toLog(log,watch_list)
	return true
end
function findBegemot(type,table,count,sec)
	local i
	--local st=os.clock()
	--toLog(log,"findBegemot started type="..type.." sec="..sec.." count="..count)
	if type=="bid" then
		for i=0,count-1,1 do
			--toLog(log,"Bid "..(count-i).." vol="..table[count-i].quantity.." price="..table[count-i].price)
			if tonumber(table[count-i].quantity)>=watch_list.volume_bid then
				--toLog(log,"beg found ="..table[count-i].price.." wtch_vol="..watch_list[sec].volume_bid)
				return tonumber(table[count-i].price)
			end
		end
		return 0
	else
		for i=1,count,1 do
			--toLog(log,"Offer "..i.." vol="..table[i].quantity.." price="..table[i].price)
			if tonumber(table[i].quantity)>=watch_list.volume_offer then
				--toLog(log,"beg found ="..table[i].price.." wtch_vol="..watch_list[sec].volume_offer)
				return tonumber(table[i].price)
			end
		end
		return 0
	end
	--toLog(log,"findBegemot ended. "..(os.clock()-st))
end
function AnalyzeBegemot(sec_code,old_value,new_value)
	if old_value==0 and new_value>0 then
		toLog(log,"Begemot found! sec="..sec_code.." Price="..new_value)
		return new_value
	elseif old_value~=0 and new_value==0 then
		toLog(log,"Begemot escaped! sec="..sec_code)
		return 0
	elseif new_value~=0 and old_value~=0 and old_value~=new_value then
		toLog(log,"Begemot moved! sec="..sec_code.." old_price="..watch_list[sec_code].position_bid.." new_price="..new_value)
		return new_value
	else
		return old_value
	end
end
function TradeBid(cur_begbid,new_begbid,new_begoffer,soffer,sec_code)
	toLog(log,"Trade BId started. CBBid="..cur_begbid.." NBBid="..new_begbid.." NBOffer="..new_begoffer.." SOffer="..soffer.." Sec="..sec_code)
	local boffer=tonumber(getParamEx(watch_list.class,sec_code,"OFFER").param_value)
	local boffer_volume=tonumber(getParamEx(watch_list.class,sec_code,"OFFERDEPTH").param_value)
	-- если бегемот исчез и есть заявка на открытие - снять
	if watch_list.status_bid=="open" and new_begbid==0 then
		toLog(log,"Bid. если бегемот исчез и есть заявка на открытие - снять ")
		local trid,ms=killOrder(watch_list.order_bid.ordernum)
		if trid~=nil then transactions[trid]="bid" watch_list.status_bid="wait" setStatus('Bid') end
		toLog(log,ms)
	-- если бегемот появился и "условия"- выставить заявку
	elseif new_begbid~=0 and watch_list.status_bid=="" and (new_begoffer==0 or new_begoffer>new_begbid+(1+watch_list.tp)*watch_list.minstep) then
		toLog(log,"BId. если бегемот появился и условия- выставить заявку")
		local trid,ms=sendLimit(watch_list.class,sec_code,"B",toPrice(sec_code,new_begbid+watch_list.minstep),watch_list.volume,watch_list.account,watch_list.client_code,"BegemotOpenBid")
		if trid~=nil then	transactions[trid]="bid" watch_list.status_bid="waitopen" watch_list.trans_bid=trid setStatus('Bid') end
		toLog(log,ms)
	-- если бегемот передвинулся - передвинуть заявку
	elseif new_begbid~=0 and cur_begbid~=0 and cur_begbid~=new_begbid and watch_list.status_bid=="open" then
		toLog(log,"Bid. если бегемот передвинулся - передвинуть заявку. num="..watch_list.order_bid.ordernum.." pr="..toPrice(sec_code,new_begbid+watch_list.minstep))
		local trid,ms=moveOrder(0,watch_list.order_bid.ordernum,toPrice(sec_code,new_begbid+watch_list.minstep))
		if trid~=nil then transactions[trid]="bid" watch_list.status_bid="waitopen" watch_list.trans_bid=trid setStatus('Bid') end
		toLog(log,ms)
	-- если стоим на закрытие и ниже повился бегемот - передвигаемся под него
	elseif watch_list.status_bid=="close" and new_begoffer<watch_list.order_bid.price and new_begoffer~=0 and (not watch_list.bad_bid) then
		toLog(log,"BId. если стоим на закрытие и ниже повился бегемот - передвигаемся под него")
		local trid,ms=moveOrder(0,watch_list.order_bid.ordernum,toPrice(sec_code,new_begoffer-watch_list.minstep))
		if trid~=nil then transactions[trid]="bid" watch_list.status_bid="waitclose" watch_list.trans_bid=trid setStatus('Bid') end
		toLog(log,ms)
	-- если стоим на закрытие и бегемота нет и можно "улучшить" место оставаясь лучшим офером - передвигаемся
	elseif watch_list.status_bid=="close" and watch_list.order_bid.price<soffer-watch_list.minstep and watch_list.order_bid.qty==boffer_volume then
		toLog(log,"BId. если стоим на закрытие и бегемота нет и можно улучшить место оставаясь лучшим офером - передвигаемся ")
		trid,ms=moveOrder(0,watch_list.order_bid.ordernum,toPrice(sec_code,soffer-watch_list.minstep))
		if trid~=nil then transactions[trid]="bid" watch_list.status_bid="waitclose" watch_list.trans_bid=trid setStatus('Bid') end
		toLog(log,ms)
	-- если стоим на закрытие, прошла сделка хуже и перед нами появилась заявка - переставится
	elseif watch_list.status_bid=='close' and watch_list.bad_bid and boffer<watch_list.order_bid.price then
		toLog(log,"Bid. если стоим на закрытие, прошла сделка хуже и перед нами появилась заявка - переставится")
		trid,ms=moveOrder(0,watch_list.order_bid.ordernum,toPrice(sec_code,boffer-watch_list.minstep))
		if trid~=nil then transactions[trid]="bid" watch_list.status_bid="waitclose" watch_list.trans_bid=trid setStatus('Bid') end
		toLog(log,ms)
	end
	--toLog(log,"TradeBid ended. "..(os.clock()-st))
end
function TradeOffer(cur_begoffer,new_begoffer,new_begbid,sbid,sec_code)
	--local st=os.clock()
	toLog(log,"Trade Offer started. CBOffer="..cur_begoffer.." NBOffer="..new_begoffer.." NBBid="..new_begbid.." SBid="..sbid.." Sec="..sec_code)
	local bbid=tonumber(getParamEx(watch_list.class,sec_code,"BID").param_value)
	local bbid_volume=tonumber(getParamEx(watch_list.class,sec_code,"BIDDEPTH").param_value)
	-- если бегемот исчез и есть заявка на открытие - снять
	if watch_list.status_offer=="open" and new_begoffer==0 then
		toLog(log,"Offer. если бегемот исчез и есть заявка на открытие - снять ")
		local trid,ms=killOrder(watch_list.order_offer.ordernum)
		if trid~=nil then transactions[trid]="offer" watch_list.status_offer="wait" setStatus('Offer') end
		toLog(log,ms)
	-- если бегемот появился и "условия"- выставить заявку
	elseif new_begoffer~=0 and watch_list.status_offer=="" and (new_begbid==0 or new_begbid<new_begoffer-(1+watch_list.tp)*watch_list.minstep) then
		toLog(log,"Offer. если бегемот появился и условия- выставить заявку")
		local trid,ms=sendLimit(watch_list.class,sec_code,"S",toPrice(sec_code,new_begoffer-watch_list.minstep),watch_list.volume,watch_list.account,watch_list.client_code,"BegemotOpenOffer")
		if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitopen" watch_list.trans_offer=trid setStatus('Offer')end
		toLog(log,ms)
	-- если бегемот передвинулся - передвинуть заявку
	elseif new_begoffer~=0 and cur_begoffer~=0 and cur_begoffer~=new_begoffer and watch_list.status_offer=="open" then
		toLog(log,"Offer. если бегемот передвинулся - передвинуть заявку. num="..watch_list.order_offer.ordernum.." pr="..toPrice(sec_code,new_begoffer-watch_list.minstep))
		local trid,ms=moveOrder(0,watch_list.order_offer.ordernum,toPrice(sec_code,new_begoffer-watch_list.minstep))
		if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitopen" watch_list.trans_offer=trid setStatus('Offer')end
		toLog(log,ms)
	-- если стоим на закрытие и ниже повился бегемот - передвигаемся под него
	elseif watch_list.status_offer=="close" and new_begbid>watch_list.order_offer.price and new_begbid~=0 then
		toLog(log,"Offer. если стоим на закрытие и ниже повился бегемот - передвигаемся под него")
		local trid,ms=moveOrder(0,watch_list.order_offer.ordernum,toPrice(sec_code,new_begbid+watch_list.minstep))
		if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitclose" watch_list.trans_offer=trid setStatus('Offer')end
		toLog(log,ms)
	-- если стоим на закрытие и можно "улучшить" место оставаясь лучшим офером - передвигаемся
	elseif watch_list.status_offer=="close" and watch_list.order_offer.price>sbid+watch_list.minstep and watch_list.order_offer.qty==bbid_volume then
		toLog(log,"Offer. если стоим на закрытие и бегемота нет и можно улучшить место оставаясь лучшим офером - передвигаемся ")
		trid,ms=moveOrder(0,watch_list.order_offer.ordernum,toPrice(sec_code,sbid+watch_list.minstep))
		if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitclose" watch_list.trans_offer=trid setStatus('Offer')end
		toLog(log,ms)
	-- если стоим на закрытие, прошла сделка хуже и перед нами появилась заявка - переставится
	elseif watch_list.status_offer=='close' and watch_list.bad_offer and bbid>watch_list.order_offer.price then
		toLog(log,"Offer. если стоим на закрытие, прошла сделка хуже и перед нами появилась заявка - переставится")
		trid,ms=moveOrder(0,watch_list.order_offer.ordernum,toPrice(sec_code,bbid+watch_list.minstep))
		if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitclose" watch_list.trans_offer=trid setStatus('Offer')end
		toLog(log,ms)
	end
	--toLog(log,"Trade Offer ended. "..(os.clock()-st))
end
function FindOfferClosePrice(security,price)
	local tp_level=watch_list.tp*watch_list.minstep
	local bbid=tonumber(getParamEx(watch_list.class,security,"BID").param_value)
	if price-tp_level>bbid then
		toLog(log,"tp="..(bbid+watch_list.minstep))
		return bbid+watch_list.minstep
	else
		local ql2=getQuoteLevel2(watch_list.class,security)
		beg=findBegemot("bid",ql2.bid,ql2.bid_count,security)
		if beg==0 then
			toLog(log,"tp="..(price-tp_level))
			return price-tp_level
		else
			if price+tp_level<beg then
				toLog(log,"tp="..(beg+watch_list.minstep))
				return beg+watch_list.minstep
			else
				toLog(log,"tp="..(price-tp_level))
				return price-tp_level
			end
		end
	end
end
function FindBidClosePrice(security,price)
	local tp_level=watch_list.tp*watch_list.minstep
	local bask=tonumber(getParamEx(watch_list.class,security,"OFFER").param_value)
	if price+tp_level<bask then
		toLog(log,"tp="..(bask-watch_list.minstep))
		return bask-watch_list.minstep
	else
		local ql2=getQuoteLevel2(watch_list.class,security)
		beg=findBegemot("ask",ql2.offer,ql2.bid_count,security)
		if beg==0 then
			toLog(log,"tp="..(price+tp_level))
			return price+tp_level
		else
			if price+tp_level>beg then
				toLog(log,"tp="..(beg-watch_list.minstep))
				return beg-watch_list.minstep
			else
				toLog(log,"tp="..price+tp_level)
				return price+tp_level
			end
		end
	end
end

function OnQuoteDo(class_code,sec_code)
	local st=os.clock()
	local ql2=getQuoteLevel2(class_code,sec_code)
	if ql2==nil or tonumber(ql2.offer_count)==0 or tonumber(ql2.bid_count)==0 then toLog(log,"------- Can`t get glass for "..class_code..sec_code) is_run=false return end
	local begbid,begoffer=0,0
	if ql2.bid_count~=0 and watch_list.volume_bid~=0 then begbid=findBegemot("bid",ql2.bid,ql2.bid_count,sec_code)	end
	if ql2.offer_count~=0 and watch_list.volume_offer~=0 then begoffer=findBegemot("offer",ql2.offer,ql2.offer_count,sec_code)	end
	if watch_list.bidEnable==1 then TradeBid(watch_list.position_bid,begbid,begoffer,tonumber(ql2.offer[2].price),sec_code) end
	if watch_list.offerEnable==1 then TradeOffer(watch_list.position_offer,begoffer,begbid,tonumber(ql2.bid[tonumber(ql2.bid_count)-1].price),sec_code) end
	watch_list.position_bid=begbid
	watch_list.position_offer=begoffer
	toLog(log,"OnQuote. "..(os.clock()-st))
end
function OnOrderDo(order)
	local st=os.clock()
	if order==nil then toLog(log,"Nil order") return end
	toLog(log,"OnOrder start. TrId="..order.trans_id.." Num="..order.ordernum.." Status="..tostring(orderflags2table(order.flags).active))
	if watch_list.order_bid.ordernum~=nil then
		toLog(log,"Bid status="..watch_list.status_bid.." OrderNum="..watch_list.order_bid.ordernum)
	else
		toLog(log,"Bid status="..watch_list.status_bid)
	end
	if watch_list.order_offer.ordernum~=nil then
		toLog(log,"Offer status="..watch_list.status_offer.." OrderNum="..watch_list.order_offer.ordernum)
	else
		toLog(log,"Offer status="..watch_list.status_offer)
	end
	if bad_transactions[order.trans_id]~="" and bad_transactions[order.trans_id]~=nil then
		toLog(log,"Bad transaction arrived ID="..order.trans_id.." Status="..bad_transactions[order.trans_id])
		toLog(log,order)
		toLog(log,orderflags2table(order.flags))
		if orderflags2table(order.flags).active then
			local tr,ms=killOrder(order.ordernum,order.seccode,order.class_code)
			if tr~=nil then bad_transactions[tr]="cancell"..bad_transactions[order.trans_id] end
			toLog(log,ms)
		end
		if orderflags2table(order.flags).done then toLog(log,"ERROR! Exess transaction done") end
		-- do smthng with done orders
		bad_transactions[order.trans_id]=""
	end
	if transactions[order.trans_id]=="bid" then
		toLog(log,"New bid order. Cur_status="..watch_list.status_bid.." LastTransID="..watch_list.trans_bid)
		if watch_list.trans_bid==order.trans_id --[[or watch_list.trans_bid==0]] then
			watch_list.order_bid={}
			watch_list.order_bid=order
			watch_list.status_bid=string.gsub(watch_list.status_bid,"wait","")
			setStatus('Bid')
		end
		if order.balance==0 then
			transactions[order.trans_id]=""
			watch_list.open_price_bid=order.price
			toLog(log,watch_list.status_bid.." order filled! Balance="..order.balance)
			if order.trans_id~=watch_list.trans_bid and watch_list.trans_bid~=0 then
				toLog(log,"Warning! Exess transaction sended ID="..watch_list.trans_bid)
				bad_transactions[watch_list.trans_bid]=watch_list.status_bid
				--watch_list.trans_bid=0
				watch_list.trans_bid=order.trans_id
				watch_list.order_bid={}
				watch_list.order_bid=order
				watch_list.status_bid=string.gsub(watch_list.status_bid,"wait","")
				setStatus('Bid')
			end
			if watch_list.status_bid=="open" or watch_list.status_bid=="" then
				watch_list.open_price_bid=order.price
				local pr=FindBidClosePrice(order.seccode,order.price)
				local trid,ms=sendLimit(order.class_code,order.seccode,"S",toPrice(order.seccode,pr),watch_list.volume,watch_list.account,watch_list.client_code,"BegemotCloseBid")
				if trid~=nil then transactions[trid]="bid" watch_list.status_bid="waitclose" watch_list.trans_bid=trid setStatus('Bid') end
				toLog(log,ms)
			elseif watch_list.status_bid=="close" then
				toLog(log,"Start new cycle.")
				watch_list.order_bid={}
				watch_list.open_price_bid=0
				watch_list.status_bid=""
				setStatus('Bid')
				watch_list.bad_bid=false
			end
		end
		--if order.trans_id==watch_list.trans_bid then watch_list.trans_bid=0 end
		if orderflags2table(order.flags).cancelled then
			transactions[order.trans_id]=""
			if watch_list.status_bid=="" then --[[watch_list.trans_bid=0]] toLog(log,"Bid order cancelled") watch_list.order_bid={} end
		end
	elseif transactions[order.trans_id]=="offer" then
		toLog(log,"New offer order. Cur_status="..watch_list.status_offer.." LastTransID="..watch_list.trans_offer)
		if watch_list.trans_offer==order.trans_id --[[or watch_list.trans_offer==0]] then
			watch_list.order_offer={}
			watch_list.order_offer=order
			watch_list.status_offer=string.gsub(watch_list.status_offer,"wait","")
			setStatus('Offer')
		end
		if order.balance==0 then
			transactions[order.trans_id]=""
			toLog(log,watch_list.status_offer.." order filled! Balance="..order.balance)
			if order.trans_id~=watch_list.trans_offer and watch_list.trans_offer~=0 then
				toLog(log,"Warning! Exess transaction sended ID="..watch_list.trans_offer)
				bad_transactions[watch_list.trans_offer]=watch_list.status_offer
				--watch_list.trans_offer=0
				watch_list.trans_offer=order.trans_id
				watch_list.order_offer={}
				watch_list.order_offer=order
				watch_list.status_offer=string.gsub(watch_list.status_offer,"wait","")
				setStatus('Offer')
			end
			if watch_list.status_offer=="open" or watch_list.status_offer=="" then
				watch_list.open_price_offer=order.price
				local pr=FindOfferClosePrice(order.seccode,order.price)
				local trid,ms=sendLimit(order.class_code,order.seccode,"B",toPrice(order.seccode,pr),watch_list.volume,watch_list.account,watch_list.client_code,"closeoffer")
				if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitclose" watch_list.trans_offer=trid setStatus('Offer') end
				toLog(log,ms)
			elseif watch_list.status_offer=="close" then
				toLog(log,"Start new cycle.")
				watch_list.order_offer={}
				watch_list.open_price_offer=0
				watch_list.status_offer=""
				watch_list.bad_offer=false
				setStatus('Offer')
			end
		end
		--if order.trans_id==watch_list.trans_offer then watch_list.trans_offer=0 end
		if orderflags2table(order.flags).cancelled then
			--transactions[order.trans_id]=""
			if watch_list.status_offer=="" then --[[watch_list.trans_offer=0]] toLog(log,"Offer order cancelled") watch_list.order_offer={} end
		end
	else
		toLog(log,"____ some shit on OnOrder()_____")
		toLog(log,order)
		toLog(log,"________________________________")
	end
	toLog(log,"Final BidStatus="..watch_list.status_bid.." OfferStatus="..watch_list.status_offer)
	toLog(log,"OnOrder end. "..(os.clock()-st))
end
function OnAllTradeDo(trade)
	local st=os.clock()
	local s="OnAllTrade start. Price="..trade
	if watch_list.status_bid=="close" then s=s..' BidOpenPrice='..watch_list.open_price_bid end
	if watch_list.status_offer=="close" then s=s..' OfferOpenPrice='..watch_list.open_price_offer end
	toLog(log,s)
	--toLog(log,trade)
	-- check bad data
	if watch_list.status_bid=="close" and trade<watch_list.open_price_bid-watch_list.bad_close_slippage*watch_list.minstep and (not watch_list.bad_bid) then
		toLog(log,"Trade lower then bid open price. Trade="..trade.." OpenPrice="..watch_list.open_price_bid.." S="..watch_list.status_bid)
		watch_list.bad_bid=true
		local bask=tonumber(getParamEx(watch_list.class,watch_list.code,"OFFER").param_value)
		if watch_list.order_bid.price>bask then
			toLog(log,"Move order to be first")
			local trid,ms=moveOrder(0,watch_list.order_bid.ordernum,toPrice(watch_list.code,bask-watch_list.minstep))
			if trid~=nil then transactions[trid]="bid" watch_list.status_bid="waitclose" watch_list.trans_bid=trid setStatus('Bid') end
			toLog(log,ms)
		end
	end
	if watch_list.status_offer=="close" and trade>watch_list.open_price_bid+watch_list.bad_close_slippage*watch_list.minstep and (not watch_list.bad_offer) then
		toLog(log,"Trade upper then offer open price. Trade="..trade.." OpenPrice="..watch_list.open_price_offer.." S="..watch_list.status_offer)
		watch_list.bad_offer=true
		local bbid=tonumber(getParamEx(watch_list.class,watch_list.code,"BID").param_value)
		if watch_list.order_offer.price<bbid then
			toLog(log,"Move order to be first")
			local trid,ms=moveOrder(0,watch_list.order_offer.ordernum,toPrice(watch_list.code,bbid+watch_list.minstep))
			if trid~=nil then transactions[trid]="offer" watch_list.status_offer="waitclose" watch_list.trans_offer=trid setStatus('Offer') end
			toLog(log,ms)
		end
	end
	toLog(log,"OnAllTrade. "..(os.clock()-st))
end
function OnTransReplyDo(reply)
	if is_run and reply.R~=nil then
		if transactions[reply.R]=="cancellopenbid" then
			toLog(log,"OnTransReply found cancellbid")
			watch_list.open_order_bid={}
			transactions[reply.R]=""
		elseif transactions[reply.R]=="cancellopenoffer" then
			toLog(log,"OnTransReply found cancelloffer")
			watch_list.open_order_offer={}
			transactions[reply.R]=""
		elseif reply.status~=3 and transactions[reply.R]~=nil then
			toLog(log,"Error on transaction "..reply.R.." "..transactions[reply.R])
			toLog(log,reply.result_msg)
			transactions[reply.R]=""
		end
	end
end
function OnInitDo()
	is_run=getSettings(getScriptPath().."\\"..settings_file)
	toLog(log,"Is_run="..tostring(is_run))
	is_run=true
	toLog(log,"Initialization finished. ")
	if is_run then OnQuoteDo(watch_list.class,watch_list.code) end
end

function OnStop()
	toLog(log,"Stop button pressed!")
	is_run=false
	if watch_list.bidEnable==1 and watch_list.order_bid~=nil then _,_=killOrder(watch_list.order_bid.ordernum) end
	if watch_list.offerEnable==1 and watch_list.order_offer~=nil then _,_=killOrder(watch_list.order_offer.ordernum) end
end
function OnInit()
	log=getScriptPath().."\\"..log
	toLog(log,"Initialization...")
end
function OnQuote(class,sec)
	if is_run and watch_list.code==sec then
		local tmp={
		["class"]=class,
		["security"]=sec
		}
		quotes[#quotes+1]=tmp
	elseif class==nil or sec==nil then
		toLog(log,"Nil update OnQuote")
	end
end
function OnOrder(order)
	if is_run and watch_list.code==order.seccode then
		orders[#orders+1]=order
		--table.insert(orders,order)
	elseif order==nil then
		toLog(log,"Nil update on Order")
	end
end
function OnParam(pclass,psec)
	if not is_run or psec~=watch_list.code then return end
	local t=tonumber(getParamEx(pclass,psec,"LAST").param_value)
	if last_trade~=t then
		on_param[#on_param+1]=t
		--table.insert(on_param,t)
		last_trade=t
	end
end
function OnTransReply(reply)
	if is_run then
		if reply==nil then toLog(log,"Nil update on transreply") return end
		if reply.code~=3 then toLog(log,reply.result_msg) end
		--table.insert(trans_replies,reply)
	end
end

function main()
	OnInitDo()
	toLog(log,"Main start")
	while is_run do
		if #trans_replies~=0 then
			local trrep=table.remove(trans_replies,1)
			if trrep~=nil then OnTransReplyDo(trrep) else toLog(log,"Nil TransReply on remove") end
		elseif on_init then
			OnInitDo()
			on_init=false
		elseif #orders~=0 then
			local order=table.remove(orders,1)
			if order~=nil then OnOrderDo(order) else toLog(log,"Nil order on remove") end
		elseif #quotes~=0 then
			local tmp=table.remove(quotes,1)
			if tmp~=nil then OnQuoteDo(tmp.class,tmp.security) else toLog(log,"Nil Quote on remove") end
		elseif #on_param~=0 then
			local trade=table.remove(on_param,1)
			if trade~=nil then OnAllTradeDo(trade) else toLog(log,"Nil trade on remove") end
		elseif tbl:IsClosed() then tbl:Show()
		else
			sleep(1)
		end
	end
	toLog(log,"Main ended")
	iup.Close()
end

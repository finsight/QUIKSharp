--vresion 0.3.0
-- transactions sync
-- bug in workbid and workask
-- moved from OnAllTrade to OnParam, no class in settings
-- different version for spot and fut
require"QL"
require"luaxml"
is_run=false
log="spreader.log"
--settings={}
--params
security="SBER"
class="QJSIM"
work_spread=5
wait_slippage=10
alert_slippage=30
alert_minprofit=5
minprofit=5
clc="56375"
account="NL0011100043"
bidEnable=true
askEnable=false
volume=1
--var
bid_order={}
bid_status=""
bid_open_price=0
bid_transaction=0
bid_bad=false
ask_bad=false
ask_open_price=0
ask_status=""
ask_order={}
ask_transaction=0
last_trade=0
transactions={}
bad_transactions={}
bid_profit=0
ask_profit=0
bid_bad_price=0
ask_bad_price=0
bid_order.price=0
ask_order.price=0
bid_open_status=""
ask_open_status=""

function OnInit()
	log=getScriptPath().."\\"..log
	log_result=getScriptPath().."\\results.log"
	toLog(log,"Initialization...")
end
function OnInitDo()
	toLog(log,"set path="..getScriptPath().."\\settings.xml")

	settings=xml.load(getScriptPath().."\\settings.xml")
	toLog(log,"Xml loaded")
	if settings==nil then
		toLog(log,"Can`t open settings file!")
		return
	end
	security=settings:find("security").value
	class=settings:find('class').value
	work_spread=tonumber(settings:find("work_spread").value)
	wait_slippage=tonumber(settings:find("wait_slippage").value)
	alert_slippage=tonumber(settings:find("alert_slippage").value)
	minprofit=tonumber(settings:find("minprofit").value)
	clc=settings:find("clc").value
	account=settings:find("account").value
	volume=tonumber(settings:find("volume").value)
	if tonumber(settings:find("bidEnable").value)==1 then bidEnable=true else bidEnable=false end
	if tonumber(settings:find("askEnable").value)==1 then askEnable=true else askEnable=false end
	toLog(log,"Settings loaded sucessfully.")
	toLog(log,settings)
	
	step=getParamEx(class,security,"SEC_PRICE_STEP").param_value
	is_run=true
	toLog(log,"Start main. step="..step)
end

function OnStop()
	toLog(log_result,"Result session: bid_profit="..bid_profit.." ask_profit="..ask_profit.." all_profit="..bid_profit+ask_profit)
	toLog(log,"Stop pressed!")
	if bidEnable and bid_order~=nil then _,_=killOrder(bid_order.ordernum) end
	if askEnable and ask_order~=nil then _,_=killOrder(ask_order.ordernum) end
	is_run=false
end

function OnQuote(pclass,psecurity)
	if not is_run or pclass~=class or psecurity~=security then return end
	local qt2=getQuoteLevel2(pclass,psecurity)
	--if new_bid==bbid and new_ask==bask then return else bbid,bask=new_bid,new_ask end
	--toLog(log,"Param changed. BBid="..bbid.." BAsk="..bask)
	if bidEnable and askEnable then
		toLog(log,"BID+ASK")
		--workboth(qt2)
		-- can be updated to make faster movement`s
		workbid(qt2)
		workask(qt2)
	elseif bidEnable then
		toLog(log,"BID only")
		workbid(qt2)
	elseif askEnable then
		toLog(log,"ASK only")
		workask(qt2)
	else
		toLog(log,"Nothing to do")
		is_run=false
	end
	--toLog(log,"OnQuote end. is_run="..tostring(is_run))
end

function workbid(quotes)
	toLog(log,"Workbid started. open_price="..bid_open_price.." price="..bid_order.price.." status="..bid_status)
	--toLog(log,quotes)
	local sbid=tonumber(quotes.bid[tonumber(quotes.bid_count)-1].price)
	local sask=tonumber(quotes.offer[2].price)
	local bask=tonumber(quotes.offer[1].price)
	local baskvol=tonumber(quotes.offer[1].quantity)
	--toLog(log,"bidCount type="..type(quotes.bid_count).." val="..quotes.bid_count)
	local bbid=tonumber(quotes.bid[tonumber(quotes.bid_count)].price)
	local bbidvol=tonumber(quotes.bid[tonumber(quotes.bid_count)].quantity)
	if ask_order.price==bbid and bbidvol==volume then
		-- Best bid - our ask. Get next bid
		sbid=tonumber(quotes.bid[tonumber(quotes.bid_count)-2].price)
		bbid=tonumber(quotes.bid[tonumber(quotes.bid_count)-1].price)
		bbidvol=tonumber(quotes.bid[tonumber(quotes.bid_count)-1].quantity)
	end
	if ask_order.price==bask and baskvol==volume then
		-- Best ask - our ask. Get next ask
		sask=tonumber(quotes.offer[3].price)
		bask=tonumber(quotes.offer[2].price)
		baskvol=tonumber(quotes.offer[2].quantity)
	end
	local spread=math.floor((bask-bbid)/step+0.5)
	toLog(log,"sbid="..sbid.." bbid="..bbid.." bbidvol="..bbidvol.." bask="..bask.." sask="..sask.." baskvol="..baskvol.." Spread="..spread)
	if bid_status=="" and spread>=work_spread and (ask_open_price==0 or bbid+(minprofit+alert_minprofit)*step<ask_open_price+alert_slippage*step) then
		-- no bid, can send
		toLog(log,"Can send bid for open. Spread="..spread..class..security.."B"..toPrice(security,bbid+step)..volume..account..clc.."openbid")
		local id,ms=sendLimit(class,security,"B",bbid+step,volume,account,clc,"SpreaderOB")
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitopen"
			bid_transaction=id
		end
		toLog(log,ms)
	elseif bid_status=="" then
		-- no bid, can send remore
		toLog(log,"Can send bid for remote. Spread="..spread..class..security.."B"..toPrice(security,bbid-wait_slippage*step)..volume..account..clc.."openask")
		local id,ms=sendLimit(class,security,"B",bbid-wait_slippage*step,volume,account,clc,'SpreaderRB')
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitremote"
			bid_transaction=id
		end
		toLog(log,ms)
	elseif bid_status=="open" and spread<work_spread-1 and bid_order.price<=bbid then
		--have bid, tiny spread, move farther
		toLog(log,"Move bid farther. Our_price="..bid_order.price.." spread="..spread)
		local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitcancellremote"
		end
		toLog(log,ms)
	elseif bid_status=="open" and bid_order.price<bbid then
		if spread>=work_spread then
			-- move bid to be first
			toLog(log,"Move bid to be first. Our_price="..bid_order.price.." BBid="..bbid)
			local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
			if id~=nil then
				transactions[id]="bid"
				bid_status="waitcancellopen"
			end
			toLog(log,ms)
		else
			--need to move bid but if we do this, spread wolud be tiny. move farther
			toLog(log,"Move bid farther. Our_price="..bid_order.price.." spread="..spread)
			local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
			if id~=nil then
				transactions[id]="bid"
				bid_status="waitcancellremote"
			end
			toLog(log,ms)
		end
	elseif bid_status=="open" and bid_order.price>sbid+step and bbidvol==volume and bid_order.price==bbid then
		-- have bid, can move to better position
		toLog(log,"Move open bid closer to second. Our_price="..bid_order.price.." SBid="..(sbid+step).." BBidVol="..bbidvol)
		local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitcancellopen"
		end
		toLog(log,ms)
	elseif bid_status=="remote" and spread>=work_spread and bid_order.price<=bbid and (ask_open_price==0 or bbid+(minprofit+alert_minprofit)*step<ask_open_price+alert_slippage*step) then
		--have remote bid, spread became good,move to be first and wait for open
		toLog(log,"Move remote bid to open position. Our_price="..bid_order.price.." spread="..spread)
		--toLog(log,"num="..bid_order.opennum.." price="..toPrice(security,bbid+step))
		local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitcancellopen"
		end
		toLog(log,ms)
	elseif bid_status=="remote" and bid_order.price<=bbid and math.floor((bbid-bid_order.price)/step+0.5)~=wait_slippage then
		--move remote to be in wait_slippage steps after bbid
		toLog(log,"Move remote bid. Our_price="..bid_order.price.." BBid="..bbid)
		local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitcancellremote"
		end
		toLog(log,ms)
	elseif bid_status=="close" and (bid_bad or bask-step>bid_open_price+minprofit*step) and bask<bid_order.price then
		-- have close on bid, found better ask
		toLog(log,"Move close bid lower. BAsk="..bask.." Bad="..tostring(bid_bad).." Our_price="..bid_order.price)
		local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitcancellclose"
		end
		toLog(log,ms)
	elseif bid_status=="close" and sask-step>bid_order.price and baskvol==volume and bask==bid_order.price then
		--can move close to better position
		toLog(log,"Move close bid to better position. Our_price="..bid_order.price.." SAsk="..sask.." BAskVol="..baskvol)
		local id,ms=killOrder(bid_order.ordernum,bid_order.seccode,bid_order.class_code)
		if id~=nil then
			transactions[id]="bid"
			bid_status="waitcancellclose"
		end
		toLog(log,ms)
	else
		--toLog(log,"Nothing to do. Bask="..bask.." Sask="..sask.." Bbid="..bbid.." sbid="..sbid.." bid_status="..bid_status)
	end
	toLog(log,"Workbid ended.")
end

function workask(quotes)
	toLog(log,"Workask started. open_price="..ask_open_price.." price="..ask_order.price.." status="..ask_status)
	--toLog(log,quotes)
	local sbid=tonumber(quotes.bid[tonumber(quotes.bid_count)-1].price)
	local sask=tonumber(quotes.offer[2].price)
	local bask=tonumber(quotes.offer[1].price)
	local baskvol=tonumber(quotes.offer[1].quantity)
	--toLog(log,"bidCount type="..type(quotes.bid_count).." val="..quotes.bid_count)
	local bbid=tonumber(quotes.bid[tonumber(quotes.bid_count)].price)
	local bbidvol=tonumber(quotes.bid[tonumber(quotes.bid_count)].quantity)
	if bid_order.price==bbid and bbidvol==volume then
		-- Best bid - our bid. Get next bid
		sbid=tonumber(quotes.bid[tonumber(quotes.bid_count)-2].price)
		bbid=tonumber(quotes.bid[tonumber(quotes.bid_count)-1].price)
		bbidvol=tonumber(quotes.bid[tonumber(quotes.bid_count)-1].quantity)
	end
	if bid_order.price==bask and baskvol==volume then
		-- Best ask - our bid. Get next ask
		sask=tonumber(quotes.offer[3].price)
		bask=tonumber(quotes.offer[2].price)
		baskvol=tonumber(quotes.offer[2].quantity)
	end
	local spread=math.floor((bask-bbid)/step+0.5)
	toLog(log,"sbid="..sbid.." bbid="..bbid.." bbidvol="..bbidvol.." bask="..bask.." sask="..sask.." baskvol="..baskvol.." Spread="..spread)
	if ask_status=="" and spread>=work_spread and (bid_open_price==0 or bask-(minprofit+alert_minprofit)*step>bid_open_price-alert_slippage*step) then
		-- no ask, can send
		toLog(log,"Can send ask for open. Spread="..spread..class..security.."S"..toPrice(security,bask-step)..volume..account..clc.."openask")
		local id,ms=sendLimit(class,security,"S",bask-step,volume,account,clc,"SpreaderOA")
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitopen"
			ask_transaction=id
		end
		toLog(log,ms)
	elseif ask_status=="" then
		-- no ask, can send remore
		toLog(log,"Can send ask for remote. Spread="..spread..class..security.."S"..toPrice(security,bask+wait_slippage*step)..volume..account..clc.."openask")
		local id,ms=sendLimit(class,security,"S",bask+wait_slippage*step,volume,account,clc,'SpreaderRA')
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitremote"
			ask_transaction=id
		end
		toLog(log,ms)
	elseif ask_status=="open" and spread<work_spread-1 and ask_order.price>=bask then
		--have ask, tiny spread, move farther
		toLog(log,"Move ask farther. Our_price="..ask_order.price.." spread="..spread)
		local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitcancellremote"
		end
		toLog(log,ms)
	elseif ask_status=="open" and ask_order.price>bask then
		if spread>=work_spread then
			-- move ask to be first
			toLog(log,"Move ask to be first. Our_price="..ask_order.price.." BAsk="..bask)
			local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
			if id~=nil then
				transactions[id]="ask"
				ask_status="waitcancellopen"
			end
			toLog(log,ms)
		else
			--need to move ask but if we do this, spread wolud be tiny. move farther
			toLog(log,"Move ask farther. Our_price="..ask_order.price.." spread="..spread)
			local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
			if id~=nil then
				transactions[id]="ask"
				ask_status="waitcancellremote"
			end
			toLog(log,ms)
		end
	elseif ask_status=="open" and ask_order.price<sask-step and baskvol==volume and ask_order.price==bask then
		-- have ask, can move to better position
		toLog(log,"Move open ask closer to second. Our_price="..ask_order.price.." SAsk="..(sask-step).." BAskVol="..baskvol)
		local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitcancellopen"
		end
		toLog(log,ms)
	elseif ask_status=="remote" and spread>=work_spread and ask_order.price>=bask and (bid_open_price==0 or bask-(minprofit+alert_minprofit)*step>bid_open_price-alert_slippage*step) then
		--have remote ask, spread became good,move to be first and wait for open
		toLog(log,"Move remote ask to open position. Our_price="..ask_order.price.." spread="..spread)
		--toLog(log,"num="..ask_order.opennum.." price="..toPrice(security,bask-step))
		local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitcancellopen"
		end
		toLog(log,ms)
	elseif ask_status=="remote" and ask_order.price>=bask and math.floor((ask_order.price-bask)/step+0.5)~=wait_slippage then
		--move remote ask to be in wait_slippage steps after best ask
		toLog(log,"Move remote ask. Our_price="..ask_order.price.." BAsk="..bask)
		local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitcancellremote"
		end
		toLog(log,ms)
	elseif ask_status=="close" and (ask_bad or bbid+step<ask_open_price-minprofit*step) and bbid>ask_order.price then
		-- have close on ask, found better bid
		toLog(log,"Move close ask lower. BBid="..bask.." Bad="..tostring(bid_bad).." Our_price="..ask_order.price)
		local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitcancellclose"
		end
		toLog(log,ms)
	elseif ask_status=="close" and sbid+step<ask_order.price and bbidvol==volume and bbid==ask_order.price then
		--can move close to better position
		toLog(log,"Move close ask to better position. Our_price="..ask_order.price.." SBid="..sbid.." BBidVol="..bbidvol)
		local id,ms=killOrder(ask_order.ordernum,ask_order.seccode,ask_order.class_code)
		if id~=nil then
			transactions[id]="ask"
			ask_status="waitcancellclose"
		end
		toLog(log,ms)
	else
		--toLog(log,"Nothing to do. Bask="..bask.." Sask="..sask.." Bbid="..bbid.." sbid="..sbid.." bid_status="..bid_status)
	end
	toLog(log,"Workask ended.")
end

function OnParam(cl,sec)
	if not is_run or sec~=security then return end
	--toLog(log,"New AllTrade price="..trade.price)
	local last=tonumber(getParamEx(cl,sec,"LAST").param_value)
	if last_trade==last then return end
	last_trade=last
	if (bid_status=="close" or bid_status=="waitcancellclose") and last<bid_open_price-alert_slippage*step then
		bid_bad=true
		toLog(log,"New AllTrade="..last.." Bid_Bad=true")
	end
	if (ask_status=="close" or ask_status=="waitcancellclose") and last>ask_open_price+alert_slippage*step then
		ask_bad=true
		toLog(log,"New AllTrade="..last.." Ask_Bad=true")
	end
end

function OnOrder(order)
	if not is_run or order.seccode~=security then return end
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
		toLog(log,"New bid order update received. Tr_id="..order.trans_id.." Num="..order.ordernum..' Cancelled='..tostring(orderflags2table(order.flags).cancelled).." Status="..bid_status)
		--toLog(log,order)
		if order.trans_id==bid_transaction then
			bid_order={}
			bid_order=order
			bid_status=string.gsub(bid_status,"wait","")
			toLog(log,"New BID status="..bid_status)
		end
		if order.balance==0 then
			toLog(log,bid_status.." order filled!. Balance="..order.balance)
			-- clear transaction
			transactions[order.trans_id]=""
			if order.trans_id~=bid_transaction and bid_transaction~=0 then
				toLog(log,"Warning! Exess transaction sended ID="..bid_transaction)
				bad_transactions[bid_transaction]=bid_status
				--watch_list.trans_bid=0
				bid_transaction=order.trans_id
				bid_order={}
				bid_order=order
				bid_status=string.gsub(bid_status,"wait","")
				toLog(log,"New BID status="..bid_status)
			end
			if (bid_status=="open" or bid_status=="remote" or bid_status=='cancellopen' or bid_status=='cancellremote') then
				local bask=getParamEx(class,security,"OFFER").param_value
				local price=0
				bid_open_status=bid_status
				bid_open_price=order.price
				bid_bad_price=bask-step
				bid_bad=false
				if bask-step>bid_open_price+minprofit*step then price=bask-step else price=bid_open_price+minprofit*step end
				toLog(log,"Send close order. open_price="..bid_open_price.." close_price="..price)
				local id,ms=sendLimit(class,security,"S",price,volume,account,clc,"SpreaderCB")
				if id~=nil then
					transactions[id]="bid"
					bid_status="waitclose"
					bid_transaction=id
				end
				toLog(log,ms)
			elseif (bid_status=='close' or bid_status=='cancellclose') then
				local profit=math.floor((order.price-bid_open_price)/step+0.5)
				local profit_bad=math.floor((bid_bad_price-bid_open_price)/step+0.5)
				bid_profit=bid_profit+profit
				toLog(log_result,"BID open_price="..bid_open_price.." close_price="..order.price.." profit="..profit.." profit_bad="..profit_bad.." open_status="..bid_open_status.." good="..tostring(profit>=minprofit))
				toLog(log,"End of BID cycle. open_price="..bid_open_price.." close_price="..order.price.." profit="..profit.." profit_bad="..profit_bad.." open_status="..bid_open_status.." good="..tostring(profit>=minprofit))
				toLog(log,"Start new cycle.")
				bid_bad=false
				bid_open_price=0
				bid_order={}
				bid_order.price=0
				bid_status=""
				OnInitDo()
			end
		end
		if orderflags2table(order.flags).cancelled then
			transactions[order.trans_id]=""
			--if watch_list.status_bid=="" then toLog(log,"Bid order cancelled") watch_list.order_bid={} end
			if bid_status=="cancellopen" then bid_status='' toLog(log,"Open bid order cancelled. Try to set new.") workbid(getQuoteLevel2(class,security)) end
			if bid_status=="cancellremote" then
				toLog(log,"Remote bid order cancelled. Try to set new.")
				local bbid=getParamEx(class,security,"BID").param_value
				tr,ms=sendLimit(class,security,"B",bbid-wait_slippage*step,volume,account,clc,'SpreaderRB')
				if tr~=nil then transactions[tr]='bid' bid_status='waitremote' bid_transaction=tr end
				toLog(log,ms)
			end
			if bid_status=="cancellclose" then
				toLog(log,"Close bid order cancelled. Try to set new.")
				local pr=0
				local bask=getParamEx(class,security,"OFFER").param_value
				bid_bad_price=getParamEx(class,security,"BID").param_value
				if (bid_bad or (bask-step>bid_open_price+minprofit*step)) then pr=bask-step else pr=bid_open_price+minprofit*step end
				local trid,ms=sendLimit(class,security,"S",pr,volume,account,clc,"SpreaderCB")
				if trid~=nil then transactions[trid]="bid" bid_status="waitclose" bid_transaction=trid end
				toLog(log,ms)
			end
		end
	elseif transactions[order.trans_id]=="ask" then
		toLog(log,"New ask order update received. Tr_id="..order.trans_id.." Num="..order.ordernum..' Cancelled='..tostring(orderflags2table(order.flags).cancelled).." Status="..ask_status)
		if order.trans_id==ask_transaction then
			ask_order={}
			ask_order=order
			ask_status=string.gsub(ask_status,"wait","")
			toLog(log,"New ASK status="..ask_status)
		end
		if order.balance==0 then
			toLog(log,ask_status.." order filled!. Balance="..order.balance)
			-- clear transaction
			transactions[order.trans_id]=""
			if order.trans_id~=ask_transaction and ask_transaction~=0 then
				toLog(log,"Warning! Exess transaction sended ID="..ask_transaction)
				bad_transactions[ask_transaction]=ask_status
				--watch_list.trans_ask=0
				ask_transaction=order.trans_id
				ask_order={}
				ask_order=order
				ask_status=string.gsub(ask_status,"wait","")
				toLog(log,"New ASK status="..ask_status)
			end
			if (ask_status=="open" or ask_status=="remote" or ask_status=='cancellopen' or ask_status=='cancellremote') then
				local bbid=getParamEx(class,security,"BID").param_value
				local price=0
				ask_open_status=ask_status
				ask_open_price=order.price
				ask_bad_price=bbid+step
				ask_bad=false
				if bbid+step<ask_open_price-minprofit*step then price=bbid+step else price=ask_open_price-minprofit*step end
				toLog(log,"Send close order. open_price="..ask_open_price.." close_price="..price)
				local id,ms=sendLimit(class,security,"B",price,volume,account,clc,"SpreaderCA")
				if id~=nil then
					transactions[id]="ask"
					ask_status="waitclose"
					ask_transaction=id
				end
				toLog(log,ms)
			elseif (ask_status=='close' or ask_status=='cancellclose') then
				local profit=math.floor((ask_open_price-order.price)/step+0.5)
				local profit_bad=math.floor((ask_open_price-ask_bad_price)/step+0.5)
				ask_profit=ask_profit+profit
				toLog(log_result,"ASK open_price="..ask_open_price.." close_price="..order.price.." profit="..profit.." profit_bad="..profit_bad.." open_status="..ask_open_status.." good="..tostring(profit>=minprofit))
				toLog(log,"End of ASK cycle. open_price="..ask_open_price.." close_price="..order.price.." profit="..profit.." profit_bad="..profit_bad.." open_status="..ask_open_status.." good="..tostring(profit>=minprofit))
				toLog(log,"Start new cycle.")
				ask_bad=false
				ask_open_price=0
				ask_order={}
				ask_order.price=0
				ask_status=""
				OnInitDo()
			end
		end
		if orderflags2table(order.flags).cancelled then
			transactions[order.trans_id]=""
			if ask_status=="cancellopen" then ask_status='' toLog(log,"Open ask order cancelled. Try to set new.") workask(getQuoteLevel2(class,security)) end
			if ask_status=="cancellremote" then
				toLog(log,"Remote ask order cancelled. Try to set new.")
				local bask=getParamEx(class,security,"OFFER").param_value
				tr,ms=sendLimit(class,security,"S",bask+wait_slippage*step,volume,account,clc,'SpreaderRA')
				if tr~=nil then transactions[tr]='ask' ask_status='waitremote' ask_transaction=tr end
				toLog(log,ms)
			end
			if ask_status=="cancellclose" then
				toLog(log,"Close ask order cancelled. Try to set new.")
				local pr=0
				local bbid=getParamEx(class,security,"BID").param_value
				ask_bad_price=getParamEx(class,security,"OFFER").param_value
				if (ask_bad or (bbid+step<ask_open_price-minprofit*step)) then pr=bbid+step else pr=ask_open_price-minprofit*step end
				local trid,ms=sendLimit(class,security,"B",pr,volume,account,clc,"SpreaderCA")
				if trid~=nil then transactions[trid]="ask" ask_status="waitclose" ask_transaction=trid end
				toLog(log,ms)
			end
		end
	else
		toLog(log,"____ some shit on OnOrder()_____")
		toLog(log,order)
		toLog(log,"________________________________")
	end
end

function main()
	OnInitDo()
	while is_run do
		sleep(50)
	end
end

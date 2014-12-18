VERSION=0.2
require'QL'
log='OptionCalculator.log'
is_run=false
futures_holding={}
on_param={}
data={}
portfolios_list={}
gui={}
last_calc_time=0
period=5
riskFreeRate=0
yearLength=365
FUTCLASSES='SPBFUT,FUTUX'
OPTCLASSES='SPBOPT,OPTUX'
delta_col_name='Delta'
gamma_col_name='Gamma(%)'
vega_col_name='Vega'
theta_col_name='Theta'
rho_col_name='Rho'
vm_col_name='Var Margin'
acc_col_name='Account'
basec_col_name='Base Contract'
-- GUI
tbl=QTable:new()

function format2f(val)
	return string.format('%.2f',val)
end
function format5f(val)
	return string.format('%.5f',val)
end
function normalDistr(z)
	local b1 =  0.31938153; 
    local b2 = -0.356563782; 
    local b3 =  1.781477937;
    local b4 = -1.821255978;
    local b5 =  1.330274429; 
    local p  =  0.2316419; 
    local c2 =  0.3989423; 

    if (z >  6.0) then return 1 end
    if (z < -6.0) then return 0 end
    local a = math.abs(z)
    local t = 1.0/(1.0+a*p)
    local b = c2*math.exp((-z)*(z/2.0))
    local n = ((((b5*t+b4)*t+b3)*t+b2)*t+b1)*t
    n = 1.0-b*n
    if ( z < 0.0 ) then n = 1.0 - n end 
    return n 
end
function normalDistrDensity(z)
	return math.exp(-0.5*z*z)/math.sqrt(2*math.pi)
end
-- different functions for greeks
function delta(opt_type,settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	if opt_type=="Call" then
		return math.exp(-1*risk_free*pdaystomate)*normalDistr(d1)
	else
		return -1*math.exp(-1*risk_free*pdaystomate)*normalDistr(-1*d1)
	end
end
--[[
function gamma(settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	return normalDistrDensity(d1)*math.exp(-1*risk_free*pdaystomate)/(settleprice*volatility*math.sqrt(pdaystomate))
end
function gammap(settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	return normalDistrDensity(d1)*math.exp(-1*risk_free*pdaystomate)/(100*volatility*math.sqrt(pdaystomate))
end]]
function gammap(settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=math.log(settleprice/strike)
	return 100*normalDistrDensity(d1)/(settleprice*volatility*math.sqrt(pdaystomate))
end
function theta(opt_type,settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	local temp=settleprice*math.exp(-1*risk_free*pdaystomate)
	local d2=d1-volatility*math.sqrt(pdaystomate)
	if opt_type=='Call' then
		return (-1*(temp*normalDistrDensity(d1)*volatility)/(2*math.sqrt(pdaystomate))+risk_free*temp*normalDistr(d1)-risk_free*strike*temp*normalDistr(d2))/yearLength
	else
		return (-1*(temp*normalDistrDensity(d1)*volatility)/(2*math.sqrt(pdaystomate))-risk_free*temp*normalDistr(-1*d1)+risk_free*strike*temp*normalDistr(-1*d2))/yearLength
	end
end
function vega(settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	return settleprice*normalDistrDensity(d1)*math.exp(-1*risk_free*pdaystomate)*math.sqrt(pdaystomate)/100
end
function rho(opt_type,settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	local d2=d1-volatility*math.sqrt(pdaystomate)
	if opt_type=='Call' then
		return pdaystomate*strike*math.exp(-1*risk_free*pdaystomate)*normalDistr(d2)/100
	else
		return -1*pdaystomate*strike*math.exp(-1*risk_free*pdaystomate)*normalDistr(-1*d2)/100
	end
end
function phi(opt_type,settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	if opt_type=='Call' then
		return -1*pdaystomate*settleprice*math.exp(-1*risk_free*pdaystomate)*normalDistr(d1)
	else
		return pdaystomate*settleprice*math.exp(-1*risk_free*pdaystomate)*normalDistr(-1*d1)
	end
end
function zeta(opt_type,settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	local d2=d1-volatility*math.sqrt(pdaystomate)
	if opt_type=='Call' then
		return normalDistr(d2)
	else
		return normalDistr(-1*d2)
	end
end
function allGreeks(opt_type,settleprice,strike,volatility,pdaystomate,risk_free)
	local d1=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	local d2=d1-volatility*math.sqrt(pdaystomate)
	local t={}
	if otp_type=="Call" then
		t.delta=math.exp(-1*risk_free*pdaystomate)*normalDistr(d1)

	else
		t.delta=-1*math.exp(-1*risk_free*pdaystomate)*normalDistr(-1*d1)
	end
end
--quik callbacks
function OnFuturesClientHolding(hold)
	if is_run and hold~=nil  then
		toLog(log,'New holding update')
		table.insert(futures_holding,hold)
	end
end
function OnStop()
	toLog(log,'Stop pressed')
	tbl:delete()
	is_run=false
end
function OnClose()
	toLog(log,'Closing terminal')
	is_run=false
end
-- our functions
function OnInitDo()
	log=getScriptPath()..'\\'..log
	tbl:SetCaption('Options Portfolios Calculator '..VERSION)
	tbl:AddColumn(acc_col_name,QTABLE_STRING_TYPE,20)
	tbl:AddColumn(basec_col_name,QTABLE_STRING_TYPE,10)
	tbl:AddColumn(delta_col_name,QTABLE_DOUBLE_TYPE,10,format2f)
	tbl:AddColumn(gamma_col_name,QTABLE_DOUBLE_TYPE,10,format5f)
	tbl:AddColumn(vega_col_name,QTABLE_DOUBLE_TYPE,10,format2f)
	tbl:AddColumn(theta_col_name,QTABLE_DOUBLE_TYPE,10,format2f)
	tbl:AddColumn(rho_col_name,QTABLE_DOUBLE_TYPE,10,format2f)
	tbl:AddColumn(vm_col_name,QTABLE_DOUBLE_TYPE,10,format2f)
	tbl:Show()
	--tbl:AddLine()
	
	local i,row
	--toLog(log,getNumberOf('futures_client_holding'))
	for i=0,getNumberOf('futures_client_holding') do
		row=getItem('futures_client_holding',i)
		if row.trdaccid~='' and row.type==0 then
			updatePortfoliosList(row)
			toLog(log,"Account "..row.trdaccid.." added to data.")
			toLog(log,row)
		end
	end
	updateGUI()
	return true
end
function updatePortfoliosList(position)
	local base=''
	local pl=portfolios_list
	local trdaccount=position.trdaccid
	local varm=position.varmargin
	local tnet=position.totalnet
	local sec=position.seccode
	local class=getSecurityInfo('',sec).class_code
	toLog(log,'Update portfolio list with position '..sec..' tnet='..tnet)
	if string.find(FUTCLASSES,class)~=nil then
		toLog(log,"Futures position")
		base=sec
	else
		toLog(log,'Option position')
		base=getParam(sec,'optionbase')
	end
	if pl[trdaccount]==nil then
		toLog(log,'First position for account '..trdaccount..'. Create new node. Base='..base)
		pl[trdaccount]={}
		pl[trdaccount][base]={}
		pl[trdaccount][base][sec]={}
		pl[trdaccount][base][sec].totalnet=tnet
		pl[trdaccount][base][sec].varmargin=varm
		pl[trdaccount][base].delta=0
		pl[trdaccount][base].gamma=0
		pl[trdaccount][base].vega=0
		pl[trdaccount][base].theta=0
		pl[trdaccount][base].rho=0
		pl[trdaccount][base].phi=0
		pl[trdaccount][base].zeta=0
		pl[trdaccount][base].vm=varm
		return true
	end
	if pl[trdaccount][base]==nil then
		toLog(log,'First position for base contract '..base..'. Create new node. Account '..trdaccount)
		pl[trdaccount][base]={}
		pl[trdaccount][base][sec]={}
		pl[trdaccount][base][sec].totalnet=tnet
		pl[trdaccount][base][sec].varmargin=varm
		pl[trdaccount][base].delta=0
		pl[trdaccount][base].gamma=0
		pl[trdaccount][base].vega=0
		pl[trdaccount][base].theta=0
		pl[trdaccount][base].rho=0
		pl[trdaccount][base].phi=0
		pl[trdaccount][base].zeta=0
		pl[trdaccount][base].vm=varm
		return true
	end
	if pl[trdaccount][base][sec]==nil then
		toLog(log,'New position for account '..trdaccount..' Base '..base..'. Add new node. Sec= '..sec)
		pl[trdaccount][base][sec]={}
		pl[trdaccount][base][sec].totalnet=tnet
		pl[trdaccount][base][sec].varmargin=varm
		pl[trdaccount][base].vm=pl[trdaccount][base].vm+varm
		return true
	end
	if pl[trdaccount][base][sec].totalnet~=tnet then
		toLog(log,'Update quantity to '..tnet..' for Acc='..trdaccount..' Base='..base..' Sec='..sec)
		pl[trdaccount][base][sec].totalnet=tnet
		return true
	end
	if pl[trdaccount][base][sec].varmargin~=varm then
		toLog(log,'Update varmargin to '..varm..' for Acc='..trdaccount..' Base='..base..' Sec='..sec)
		pl[trdaccount][base].vm=pl[trdaccount][base].vm-pl[trdaccount][base][sec].varmargin+varm
		pl[trdaccount][base][sec].varmargin=varm
		return true
	end
	--toLog(log,'Update portfolio list ended')
	return false
end
function calculateGreeks(acc,base)
	toLog(log,'calculations started')
	local pl=portfolios_list[acc][base]
	local class=''
	local opttype,volat,stryke,sprice,pdtm
	pl.delta=0
	pl.gamma=0
	pl.vega=0
	pl.theta=0
	pl.rho=0
	pl.phi=0
	pl.zeta=0
	for k,v in pairs(pl) do
		if type(v)=='table' and v.totalnet~=0 then
			class=getSecurityInfo('',k).class_code
			if string.find(FUTCLASSES,class)~=nil then
				toLog(log,'Futures position ')
				pl.delta=pl.delta+v.totalnet
			else
				opttype=getParam(k,'optiontype')
				volat=getParam(k,'volatility')/100
				strike=getParam(k,'strike')
				sprice=getParam(base,'last')
				pdtm=getParam(k,'DAYS_TO_MAT_DATE')/yearLength
				--toLog(log,k.." Position Base="..base.." type="..opttype..' BasePrice='..sprice..' Volat='..volat..' Strike='..strike..' pdtm='..pdtm)
				pl.delta=pl.delta+v.totalnet*delta(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				pl.gamma=pl.gamma+v.totalnet*gammap(sprice,strike,volat,pdtm,riskFreeRate)
				pl.vega=pl.vega+v.totalnet*vega(sprice,strike,volat,pdtm,riskFreeRate)
				pl.theta=pl.theta+v.totalnet*theta(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				pl.rho=pl.rho+v.totalnet*rho(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				--pl.phi=pl.phi+v.totalnet*phi(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				--pl.zeta=pl.zeta+v.totalnet*zeta(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				toLog(log,'sec='..k..' delta='..pl.delta)
			end
		end
	end
	toLog(log,pl)
	toLog(log,'calculations ended')
end
function createGUIelement(acc,base)
	toLog(log,'Create GUI element '..acc..' '..base)
	local line=tbl:AddLine()
	tbl:SetValue(line,'Account',acc)
	tbl:SetValue(line,'Base Contract',base)
	return line
end
function updateGUI()
	--toLog(log,'Update GUI started')
	local pl=portfolios_list
	for k,v in pairs(pl) do
		for k1,v1 in pairs(v) do
			if gui[k]==nil then
				gui[k]={}
				gui[k][k1]={}
				gui[k][k1].line=createGUIelement(k,k1)
			elseif gui[k][k1]==nil then
				gui[k][k1]={}
				gui[k][k1].line=createGUIelement(k,k1)
			else
				toLog(log,'Update controls')
				local l=gui[k][k1].line
				tbl:SetValue(l,delta_col_name,v1.delta)
				tbl:SetValue(l,gamma_col_name,v1.gamma)
				tbl:SetValue(l,theta_col_name,v1.theta)
				tbl:SetValue(l,vega_col_name,v1.vega)
				tbl:SetValue(l,rho_col_name,v1.rho)
				tbl:SetValue(l,vm_col_name,v1.vm)
				--[[
				gui[k][k1].delta_lbl.title='delta='..string.format('%.2f',v1.delta)
				gui[k][k1].gamma_lbl.title='gamma='..string.format('%.6f',v1.gamma)
				gui[k][k1].theta_lbl.title='theta='..string.format('%.2f',v1.theta)
				gui[k][k1].vega_lbl.title='vega='..string.format('%.2f',v1.vega)
				gui[k][k1].rho_lbl.title='rho='..string.format('%.2f',v1.rho)
				--gui[k][k1].phi_lbl.title='phi='..string.format('%.2f',v1.phi)
				--gui[k][k1].zeta_lbl.title='zeta='..string.format('%.2f',v1.zeta)
				]]
			end
		end
	end
	--toLog(log,'Update GUI ended')
end
--
function main()
	is_run=OnInitDo()
	if is_run then
		toLog(log,'Main start')
	end
	while is_run do
		-- calculation`s with sleep. not callbacks
		if getSTime()>last_calc_time+period and portfolios_list~=nil then
			toLog(log,'Time to calculate new values')
			for k,v in pairs(portfolios_list) do
				for k1,v1 in pairs(v) do
					toLog(log,'Start to calclate greeks for '..k..' '..k1)
					calculateGreeks(k,k1)
				end
			end
			updateGUI()
			last_calc_time=getSTime()
		elseif #futures_holding~=0 then
			local res=false
			for i=0,#futures_holding do
				local t=table.remove(futures_holding,1)
				if t~=nil then local r=updatePortfoliosList(t) res=res or r else toLog(log,'nil on holding remove') end
			end
			if res then updateGUI() end
		else
			sleep(1)
		end
	end
	toLog(log,'Main end')
end
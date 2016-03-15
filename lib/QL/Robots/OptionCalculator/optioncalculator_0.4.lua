VERSION=0.4
require("QL")
require("iuplua")
require("iuplua_pplot")
log='OptionCalculator.log'
is_run=false
futures_holding={}
on_param={}
data={}
portfolios_list={}
gui={}
base_contract_info={}
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
filter_acc=''
-- GUI
tbl=QTable:new()
--plot=iup.pplot{title='Yeild to mate',MARGINBOTTOM="65", MARGINLEFT="65", AXS_XLABEL="Strike", AXS_YLABEL="Yield", LEGENDSHOW="YES", LEGENDPOS="TOPLEFT"}
--main_dlg=iup.dialog{plot,size='200x200',title='Plot'}
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
	toLog(log,opt_type..','..settleprice..','..strike..','..volatility..','..pdaystomate..','..risk_free)
	local sqrt_pdays=math.sqrt(pdaystomate)
	local exp_risk_days=math.exp(-1*risk_free*pdaystomate)
	local log_settle_strike=math.log(settleprice/strike)
	local d1=(log_settle_strike+volatility*volatility*0.5*pdaystomate)/(volatility*sqrt_pdays)
	local d2=d1-volatility*sqrt_pdays
	local normdistr_d1=normalDistr(d1)
	local normdistr_minus_d1=normalDistr(-1*d1)
	local normdistrdens_d1=normalDistrDensity(d1)
	local normdistr_d2=normalDistr(d2)
	local normdistr_minus_d2=normalDistr(-1*d2)
	local normdistrdens_d2=normalDistrDensity(d2)
	local temp=settleprice*exp_risk_days

	local delta,gamma,vega,thetha,rho=0,0,0,0,0

	if opt_type=="Call" then
		delta=exp_risk_days*normdistr_d1
		gamma=100*normalDistrDensity(log_settle_strike)/(settleprice*volatility*sqrt_pdays)
		thetha=(-1*(temp*normdistrdens_d1*volatility)/(2*sqrt_pdays)+risk_free*temp*normdistr_d1-risk_free*strike*temp*normdistr_d2)/yearLength
		vega=settleprice*normdistrdens_d1*exp_risk_days*sqrt_pdays/100
		rho=pdaystomate*strike*exp_risk_days*normdistr_d2/100
	else
		delta=-1*exp_risk_days*normdistr_minus_d1
		gamma=100*normalDistrDensity(log_settle_strike)/(settleprice*volatility*sqrt_pdays)
		thetha=(-1*(temp*normdistrdens_d1*volatility)/(2*sqrt_pdays)-risk_free*temp*normdistr_minus_d1+risk_free*strike*temp*normdistr_minus_d2)/yearLength
		vega=settleprice*normdistrdens_d1*exp_risk_days*sqrt_pdays/100
		rho=-1*pdaystomate*strike*exp_risk_days*normdistr_minus_d2/100
	end
	local d11=(math.log(settleprice/strike)+volatility*volatility*0.5*pdaystomate)/(volatility*math.sqrt(pdaystomate))
	if opt_type=="Call" then
		del1=math.exp(-1*risk_free*pdaystomate)*normalDistr(d11)
	else
		del1=-1*math.exp(-1*risk_free*pdaystomate)*normalDistr(-1*d11)
	end
	toLog(log,'type='..opt_type..' strike='..strike..'delta='..delta..' del1='..del1)
	return delta,gamma,vega,thetha,rho
end
--quik callbacks
function OnFuturesClientHolding(hold)
	if is_run and hold~=nil and (filter_acc=='' or string.find(filter_acc,hold.trdaccid)~=nil) then
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
-- yield curve
function calcYield(settleprice,strike,opt_type,amount,premium)
	toLog(log,'calcYield '..settleprice..','..strike..','..opt_type..','..amount..','..tostring(premium))
	if opt_type=='Call' then
		if settleprice<=strike then
			return -1*amount*premium
		else
			return amount*(settleprice-strike-premium)
		end
	elseif opt_type=='Put' then
		if settleprice>=strike then
			return -1*amount*premium
		else
			return amount*(strike-settleprice-premium)
		end
	elseif opt_type=='Futures' then
		return amount*(settleprice-strike)
	else
		toLog(log,'Un-supported option type '..tostring(opt_type))
		return 0
	end
end
function buildPlot(trdacc,base)
	local pl=portfolios_list[trdacc][base]
	local last=getParam(base,'last')
	local strike_step=base_contract_info[base].strike_step
	local center=math.ceil(last/strike_step)*strike_step
	plot=iup.pplot{title='Yeild to mate',MARGINBOTTOM="65", MARGINLEFT="65", AXS_XLABEL="Strike", AXS_YLABEL="Yield", LEGENDSHOW="YES", LEGENDPOS="TOPLEFT", GRID='YES', AXS_YCROSSORIGIN='NO',AXS_XCROSSORIGIN='NO'}
	gui[trdacc][base].plot_dialog=iup.dialog{plot,size='200x200',title='Yield to mate plot '..trdacc..':'..base}
	iup.PPlotBegin(plot, 0)

	--last=882.2
	local max_yield=0
	local min_yield=99999999999999
	for step=center-3*strike_step,center+3*strike_step,strike_step do
		yield=0
		for k,v in pairs(pl) do
			if type(v)=='table' and v.totalnet~=0 then
				if v.opt_type=='Futures' then strike=last else strike=v.strike end
				yield=yield+calcYield(step,strike,v.opt_type,v.totalnet,getParam(k,'theorprice'))
				--toLog(log,k..' y'..yield)
				max_yield=math.max(yield,max_yield)
				min_yield=math.min(yield,min_yield)
			end
		end
		iup.PPlotAdd(plot, step, yield)
		--toLog(log,'strike='..step..' yield='..yield)
	end
	gui[trdacc][base].plot_dataset_index=iup.PPlotEnd(plot)
	plot.ds_legend=trdacc..':'..base
	-- zero-Y line
	iup.PPlotBegin(plot,0)
	iup.PPlotAdd(plot,center-3*strike_step,0)
	iup.PPlotAdd(plot,center+3*strike_step,0)
	toLog(log,'zeroy index='..iup.PPlotEnd(plot))
	plot.ds_legend='Zero line'
	plot.ds_color='0 0 0'
	-- zero-X line
	iup.PPlotBegin(plot,0)
	iup.PPlotAdd(plot,last,max_yield)
	iup.PPlotAdd(plot,last,min_yield)
	toLog(log,'center index='..iup.PPlotEnd(plot))
	plot.ds_legend='Current price'
	plot.ds_color='0 0 0'

	if gui[trdacc][base].plot_dialog.visible=='NO' then gui[trdacc][base].plot_dialog:show() end
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
		if row.trdaccid~='' and row.type==0 and (filter_acc=='' or string.find(filter_acc,row.trdaccid)~=nil) then
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
	-- check to add info about base contract to table
	if base_contract_info[base]==nil then
		toLog(log,'Add info about base contract '..base)
		base_contract_info[base]={}
		base_contract_info[base].strike_step=0
		base_contract_info[base].min_strike=9999999999999999999999999999
		base_contract_info[base].max_strike=0
		local all_list=getClassSecurities('SPBOPT')
		all_list=all_list..getClassSecurities('OPTUX')
		local new_all_list=''
		local mat_date=getParam(base,'MAT_DATE')
		for sec in string.gmatch(all_list,'%w+%.?%w+') do
			if getParam(sec,'optionbase')==base and getParam(sec,'optiontype')=='Call'  and mat_date==getParam(sec,'MAT_DATE') then new_all_list=new_all_list..sec..',' end
		end
		local st={}
		for sec in string.gmatch(new_all_list,'%w+%.?%w+') do
			st[#st+1]=getParam(sec,'strike')
		end
		if #st~=0 then
			table.sort(st)
			base_contract_info[base].max_strike=st[#st]
			base_contract_info[base].min_strike=st[1]
			base_contract_info[base].strike_step=st[2]-st[1]
		else toLog(log,'No options forund for '..base) end
		toLog(log,"Found values "..base..' MaxStrike='..base_contract_info[base].max_strike..' MinStrike='..base_contract_info[base].max_strike..' StepStrike='..base_contract_info[base].strike_step)
	end
	if pl[trdaccount]==nil then
		toLog(log,'First position for account '..trdaccount..'. Create new node. Base='..base)
		pl[trdaccount]={}
		pl[trdaccount][base]={}
		pl[trdaccount][base][sec]={}
		pl[trdaccount][base][sec].totalnet=tnet
		pl[trdaccount][base][sec].varmargin=varm
		if string.find(FUTCLASSES,class)~=nil then
			pl[trdaccount][base][sec].opt_type='Futures'
			pl[trdaccount][base][sec].strike=0
		else
			pl[trdaccount][base][sec].opt_type=getParam(sec,'optiontype')
			pl[trdaccount][base][sec].strike=getParam(sec,'strike')
		end
		pl[trdaccount][base][sec].days_to_mate=getParam(sec,"DAYS_TO_MAT_DATE")/yearLength
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
		if string.find(FUTCLASSES,class)~=nil then
			pl[trdaccount][base][sec].opt_type='Futures'
			pl[trdaccount][base][sec].strike=0
		else
			pl[trdaccount][base][sec].opt_type=getParam(sec,'optiontype')
			pl[trdaccount][base][sec].strike=getParam(sec,'strike')
		end
		pl[trdaccount][base][sec].days_to_mate=getParam(sec,"DAYS_TO_MAT_DATE")/yearLength
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
		if string.find(FUTCLASSES,class)~=nil then
			pl[trdaccount][base][sec].opt_type='Futures'
			pl[trdaccount][base][sec].strike=0
		else
			pl[trdaccount][base][sec].opt_type=getParam(sec,'optiontype')
			pl[trdaccount][base][sec].strike=getParam(sec,'strike')
		end
		pl[trdaccount][base][sec].days_to_mate=getParam(sec,"DAYS_TO_MAT_DATE")/yearLength
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
	--local class=''
	--local opttype,volat,stryke,sprice,pdtm
	pl.delta=0
	pl.gamma=0
	pl.vega=0
	pl.theta=0
	pl.rho=0
	pl.phi=0
	pl.zeta=0
	for k,v in pairs(pl) do
		if type(v)=='table' and v.totalnet~=0 then
			if v.opt_type=='Futures' then
				toLog(log,'Futures position ')
				pl.delta=pl.delta+v.totalnet
			else
				--[[
				opttype=v.opt_type
				volat=getParam(k,'volatility')/100
				strike=v.strike
				sprice=getParam(base,'last')
				pdtm=v.days_to_mate
				]]
				--toLog(log,k.." Position Base="..base.." type="..opttype..' BasePrice='..sprice..' Volat='..volat..' Strike='..strike..' pdtm='..pdtm)
				local d,g,veg,t,r=allGreeks(v.opt_type,getParam(base,'last'),v.strike,getParam(k,'volatility')/100,v.days_to_mate,riskFreeRate)
				toLog(log,k..' d='..d..' gam='..g..' type='..v.opt_type..' strike='..v.strike)
				pl.delta=pl.delta+v.totalnet*d
				pl.gamma=pl.gamma+v.totalnet*g
				pl.vega=pl.vega+v.totalnet*veg
				pl.theta=pl.theta+v.totalnet*t
				pl.rho=pl.rho+v.totalnet*r
				--[[
				pl.delta=pl.delta+v.totalnet*delta(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				pl.gamma=pl.gamma+v.totalnet*gammap(sprice,strike,volat,pdtm,riskFreeRate)
				pl.vega=pl.vega+v.totalnet*vega(sprice,strike,volat,pdtm,riskFreeRate)
				pl.theta=pl.theta+v.totalnet*theta(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				pl.rho=pl.rho+v.totalnet*rho(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				--pl.phi=pl.phi+v.totalnet*phi(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				--pl.zeta=pl.zeta+v.totalnet*zeta(opttype,sprice,strike,volat,pdtm,riskFreeRate)
				toLog(log,'sec='..k..' delta='..pl.delta)
				]]
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
	if tbl:IsClosed() then tbl:Show() end
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
		local time=getSTime()
		if (time~=nil) and (time>last_calc_time+period and portfolios_list~=nil) then
			toLog(log,'Time to calculate new values')
			for k,v in pairs(portfolios_list) do
				for k1,v1 in pairs(v) do
					toLog(log,'Start to calclate greeks for '..k..' '..k1)
					calculateGreeks(k,k1)
					if gui[k][k1].plot_dataset_index==nil then toLog(log,'Build Yield to mate plot. ') buildPlot(k,k1) end
				end
			end
			updateGUI()
			last_calc_time=time
		elseif #futures_holding~=0 then
			local res=false
			for i=0,#futures_holding do
				local t=table.remove(futures_holding,1)
				if t~=nil then local r=updatePortfoliosList(t) res=res or r else toLog(log,'nil on holding remove') end
			end
			if res then updateGUI() end
		else
			iup.LoopStep()
			sleep(1)
		end
	end
	iup.ExitLoop()
--	main_dlg:destroy()
	iup.Close()
	toLog(log,'Main end')
end

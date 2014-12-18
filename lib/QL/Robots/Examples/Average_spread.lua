require "QL"
require "iuplua"

is_run = false
time_period = 10  -- задаём частоту обращения к таблице параметров
average_spreads = {}
last_run=0
log_file = "Average_spreads_log.txt"
ticker_list = {"RIM3","SiM3"}

--создаём объекты интерфейса. Dialog, Label

text=iup.text{value="0",expand="YES",multiline="YES",readonly="YES"}
Dialog=iup.dialog{text; title="Average Spread", size="QUATERxQUATER"}

function Dialog:close_cb()
	is_run = false
end
function OnInit(path)
  is_run = true
  log_file=getScriptPath().."\\"..log_file
end

function OnStop()
  is_run = false
end

function OutSpreadToLog()
  local file = io.open(log_file, "a+t")

 
  for _, sec in pairs(ticker_list) do
	class=getSecurityInfo("",sec).class_code
	  
	  local tablebid = getParamEx(class,  sec, "bid")
	  local bid=tablebid.param_value
	  
      local tableoffer = getParamEx(class,  sec, "offer")
	  local offer=tableoffer.param_value
	  
      local p_spread = (offer - bid) / bid * 100

      local elem = average_spreads[sec]
      if elem == nil then
        average_spreads[sec] = { Count = 1, Spread = p_spread, Avr = p_spread}
        elem = average_spreads[sec]
      else
        elem.Spread = p_spread
        elem.Avr = (elem.Avr * elem.Count + p_spread) / (elem.Count + 1)
        elem.Count = elem.Count + 1
      end

     file:write("AVR_".. sec .. "=" .. tostring(elem.Avr).. " " .. "\n")
	 text.append=sec.." average spread="..tostring(elem.Avr).."\n"
  end

  file:write("---\n")
  file:close()
end

function main()
	Dialog:show()
	while is_run do
		if getSTime()>last_run+time_period then
			text.value=''
			OutSpreadToLog()
			last_run=getSTime()
		end
		iup.LoopStep()
	end
	Dialog:destroy()
	iup.ExitLoop()
	iup.Close()
end

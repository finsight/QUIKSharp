require "QL"
log='dg.log'
file_path=''
line=0 -- тут указываем номер линии (актуально для индикаторов типа Аллигатор)
graph_tag='2grab' -- тут пишем идентификатор графика, или используем уже указанный на нужном графике

function main()
	local err=''
	log=getScriptPath().."\\"..log
	file_path=getScriptPath().."\\"..file_path
	local candles={}
	local num_result=0
	local legend=''
	if not isChartExist(graph_tag) then toLog(log,'No chart with tag '..tostring(graph_tag)) message("DataGrabber. No chart with tag "..tostring(graph_tag),3) return end
	local n=getNumCandles(graph_tag)
	candles,num_result,legend=getCandlesByIndex(graph_tag,line,0,n)
	if candles==nil or #candles==0 or candles[0]==nil or num_result==0 then toLog(log,'No candles.') message('DataGrabber. No candles.',3) return end
	file_path=file_path..legend..'_'..datetime2string(candles[0].datetime)..'_'..datetime2string(candles[num_result-1].datetime)..'.csv'
	toLog(log,'Result filename '..file_path)
	local f=io.open(file_path,'a')
	if f==nil then toLog(log,'Can`t open file') message('DatGrabber. Can`t open file',3) f:close() return end
	for _,candle in pairs(candles) do
		if candle.doesExist==1 then
			f:write(datetime2string(candle.datetime)..','..tostring(candle.open)..','..tostring(candle.high)..','..tostring(candle.low)..','..tostring(candle.close)..','..tostring(candle.volume)..'\n')
		end
	end
	f:close()
	toLog(log,'Done')
	message('DataGrabber. Graph '..graph_tag..' successfully grabed!',1)
end

----- INI-FILE -----
--------------------
VBU3 = {}
VBU3.name = "VBU3"
VBU3.Vane = "RTSI"
VBU3.enabled = false
VBU3.market = false
VBU3.brake=0.3
VBU3.trade_volume=2
VBU3.stop_steps=15
VBU3.maxloss=100
--------------------
SRU3 = {}
SRU3.name = "SRU3"
SRU3.Vane = "RTSI"
SRU3.enabled = false
SRU3.market = false
SRU3.brake=0.3
SRU3.trade_volume=1
SRU3.stop_steps=16
SRU3.maxloss=100
--------------------
LKU3 = {}
LKU3.name="LKU3"
LKU3.Vane="MICEXO&G"
LKU3.enabled = false
LKU3.market = true
LKU3.brake=0.3
LKU3.trade_volume=1
LKU3.stop_steps=19
LKU3.maxloss=180
--------------------
GZU3={}
GZU3.name="GZU3"
GZU3.Vane="MICEXO&G"
GZU3.enabled = false
GZU3.market=true
GZU3.brake=0.3
GZU3.trade_volume=1
GZU3.stop_steps=19
GZU3.maxloss=150
--------------------
RNU3={}
RNU3.name="RNU3"
RNU3.Vane="MICEXO&G"
RNU3.enabled = false
RNU3.market = true
RNU3.brake=0.3
RNU3.trade_volume=1
RNU3.stop_steps=22
RNU3.maxloss=200
--------------------
RIU3={}
RIU3.name="RIU3"
RIU3.Vane="RTSI"
RIU3.enabled = false
RIU3.market = false
RIU3.brake=0.3
RIU3.trade_volume=1
RIU3.stop_steps=19
RIU3.maxloss=400
--------------------
VTBR={}
VTBR.name="VTBR"
VTBR.Vane="VBU3"
VTBR.enabled = false
VTBR.market = true
VTBR.brake=0.3
VTBR.trade_volume=1
VTBR.stop_steps=17
VTBR.maxloss =10
--------------------
--------------------
Tickers = {VBU3,SRU3,LKU3,GZU3,RNU3,RIU3,VTBR}
toLog(log,"ini read")
toLog(log, Tickers)

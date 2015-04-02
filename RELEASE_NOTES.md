#### 0.1.0 - December 21 2014
* Initial release. Tested connection and basic Quik functions and events.
#### 0.1.1 - December 21 2014
* NuGet package (tested)
#### 0.1.3 - December 21 2014
* More than 3x  performance improvement with cjson instead of dkjson in Quik and c.30% in 
a standalone Lua. Now they are almost at par - Quik just 13% slower. Now we have 80 micros
for Quik and 70 micros for a standalone Lua (60 in profiler in release mode).
* Added ClassFunctions (Функции для обращения к спискам доступных параметров) and several simple events
* Correct Encoding to 1251
#### 0.1.4 - December 22 2014
* Implemented sendTransaction and tested its error handling (no actual trades yet)
* Imlpemented several data structures and functions
* Improved performance by c.30% in response listener by spawning new tasks for each message
#### 0.1.5 - March 19 2015
* Fixed lua51.dll proxy issue
#### 0.1.6 - April 3 2015
* Fixed lua51.dll bug
* Apache 2.0 instead of GPL
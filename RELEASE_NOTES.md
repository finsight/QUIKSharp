#### 2.0.0 - June 17 2020
* Support for QUIK 64-bit (up to version 8.5)
* Various bug fixes
* Various Lua code improvements 

#### 1.0.0 - May 16 2019
* Updates & bug fixes from all commits since the last version.
* Added netstandard2.0 target.
* The first stable version, all essential functionality is battle-tested by users over the past year.

#### 1.0.0-beta1 - July 10 2017
* Update NuGet. Minor changes and some new functions (e.g. GetTradesAccounts).

#### 1.0.0-alpha - February 10 2017
* Cumulative update with all commits over the past 15 months
* Almost all QLua API is implemented

#### 0.3.0 - November 11 2015
* Cumulative update with all commits over the past 6 months

#### 0.2.1 - May 1 2015
* MARKET_MAKER_ORDER is nullable, because it is not supported by FORTS

#### 0.2.0 - April 20 2015
* Fixed race conditions both in Lua and on C# side
#### 0.1.0 - December 21 2014
* Initial release. Tested connection and basic Quik functions and events.
#### 0.1.1 - December 21 2014
* NuGet package (tested)
#### 0.1.3 - December 21 2014
* More than 3x  performance improvement with cjson instead of dkjson in Quik and c.30% in 
a standalone Lua. Now they are almost at par - Quik just 13% slower. Now we have 80 micros
for Quik and 70 micros for a standalone Lua (60 in profiler in release mode).
* Added ClassFunctions (������� ��� ��������� � ������� ��������� ����������) and several simple events
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
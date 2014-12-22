QuikSharp
==========
QuikSharp is the Quik Lua interface ported to .NET.

QuikSharp exposes all functions and events available in QLUA as C# async functions
and events.

A simple Ping/Pong benchmark shows c.60 microseconds roundtrip time for Quik
 (MacBook Air 2012). This is almost negligible compared to Quik's native latency
 (from the terminal to a remote server).

Russian version: вопросы и комментарии оставляйте в [Issues](https://github.com/buybackoff/QuikSharp/issues).

Install & Usage
================

Use NuGet to install QuikSharp into your project. 

    PM> Install-Package QuikSharp


A folder `lua` with all required scritps will be added to your project. 
Start `QuikSharp.lua` script from Quik and normally never stop it
manually (it will be started automatically when Quik starts). See unit tests for 
usage examples in C#.


Why use .NET for Quik
=============
(and not Lua or DDE or other libs)

* Because one should be crazy to use for trading some 3rd party *closed source* code 
with unclear licensing and support (even tested and popular like StockSharp).

* Because Quik is dumb, slow and painful - while .NET is smart, 
fast and pleasure to work with.

* Because Quik is a niche legacy soft that has its market share for 
historical reasons. One should not invest more than a minimum into such software, but 
should abstract away its idiosyncrasies as much as possible. Most platforms have .NET API
and it is the choice for new development.


LICENCE
=======
QuikSharp - Quik interface in .NET

Copyright Ⓒ 2014 Victor Baybekov

This library is dual-licensed: you can redistribute it and/or modify
it under the terms of the GNU General Public License version 3 as 
published by the Free Software Foundation. For the terms of this 
license, see the GNU GPL v3 section of the LICENSE.txt file or 
<http://www.gnu.org/licenses/>.

You could use this library under the terms of the GNU General
Public License, but WITHOUT ANY WARRANTY; without even the implied 
warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
See the GNU General Public License for more details.

Alternatively, you can license this library under a proprierary 
license. Please contact the author for details.


ЛИЦЕНЗИЯ
--------
Вы можете использовать эту библиотеку под лицензией GNU GPL v3. Это значит,
что если Вы включаете библиотеку в свое приложение и распространяете 
это приложение сторонним пользователям, то вы должны опубликовать код Вашего
приложения. Если Вы используете код для личных целей, то Вы можете вносить любые
изменения и использовать библиотеку как угодно. Ответственность автора за
результаты использования библиотеки полностью исключена. Автор не дает никаких
гарантий, что в какой-то момент Вы не сольете счет, используя библиотеку, и не будете должны 
брокеру, клиентам, родственникам и бюджету РФ (даже если читаете текст лицензии).
В случае разночтений англоязычной и русскоязычной версий приоритет имеет англоязычная.

Если Вам нужна коммерческая лицензия для этой библиотеки, напишите автору
в разделе Issues этого репозитария.



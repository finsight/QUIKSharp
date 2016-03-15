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


License
----------------------

(c) Victor Baybekov 2015

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

This software is distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
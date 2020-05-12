[![Gitter](https://badges.gitter.im/QUIKSharp/community.svg)](https://gitter.im/QUIKSharp/community)

QUIK#
==========
QUIK# (QUIK Sharp) is the QUIK Lua interface ported to .NET.

QUIK# exposes all functions and events available in QLUA as C# async functions
and events.

A simple Ping/Pong benchmark shows c.60 microseconds roundtrip time for QUIK
 (MacBook Air 2012). This is almost negligible compared to QUIK's native latency
 (from the terminal to a remote server).

[**Russian version**](https://github.com/finsight/QUIKSharp/blob/master/README.RU.md)


Install & Usage
================

The best way to use QUIK# is to install the [NuGet package](https://www.nuget.org/packages/QUIKSharp):

    PM> Install-Package QUIKSharp

Version 1.0 is stable and should cover most use cases. If you need some functionality that is 
already implemented but is not included in the stable NuGet release then you could build the 
project from the master branch. That will give you the latest features and bug fixes.

If some functionality from QLUA is missing or you find a bug please submit a pull request.

A folder `lua` with all required scritps will be added to your project. 
Start `QuikSharp.lua` script from QUIK and never stop it
manually (it will be started automatically when QUIK starts). See demo applications and 
unit tests for usage examples in C#.


Why use .NET for QUIK
=============
(and not Lua or DDE or other libs)

* Because implementing trading systems with 3rd party *closed source* code with unclear 
licensing and support is risky and leads to vendor lock-in (even tested and popular like StockSharp).

* Because QUIK's Lua interface is dumb, slow and painful - while .NET is smart, 
fast and pleasure to work with.

* Because QUIK is a niche legacy soft that has its market share for 
historical reasons. One should not invest more than a minimum into such software, but 
should abstract away its idiosyncrasies as much as possible. Most platforms have .NET API
and it is the choice for new development.


Who owns QUIK#?
================

QUIK# is owned by all its [authors and contributors](https://github.com/finsight/QUIKSharp/blob/master/AUTHORS.md).
This is an open source project licensed under the [Apache 2.0 license](https://tldrlegal.com/license/apache-license-2.0-(apache-2.0)).
There are open issues in the issues tab that still need to be taken care of, feel free to pick one up and submit a patch to the project.

QUIK workstation and trademark are owned by [ARQA Technologies](https://arqatech.com/ru/products/quik/). This project is not affiliated 
with the company in any way and our use of the name is *fair use* - we just make the live of the terminal users simpler and happier!


License
----------------------

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

This software is distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
Random collection of code related to Quik and QLua. My stuff from end of 2013 experiments and some libraries from internet

* QuikSharp is Lua - .NET connector
* QuikStarter is an obsolete utility to make Quik always running and reconnecting (probably taken from Stock#. Not needed since built-in reconnect is quite reliable. It is better to make SMS/Email notification if something unexpected happens)


Why connect to .NET
=============
One of the key rationale is that Quik is dumb, slow, illogical, made by violent lazy
undergrads for users to suffer - while .NET is smart, performant and pleasure to work with. 
So Quik is like a Russian young soldier that only ansfers to commands and callbacks. 
And nothing more.

Another rationale is that Quik is a niche legacy soft that has its market share for 
historical reasons. One should not invest more than a minimum into such software, but 
should adstract away its idiosyncraticies as much as possible. Most platforms have .NET API
and it is the choice for new development.


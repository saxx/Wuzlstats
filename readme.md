# Wuzlstats

## About

Wuzlstats is an open web app for tracking wuzl (tabletop soccer/football, foosball) scores. It's suited ideally to track the score of the wuzl table in your office.

It supports multiple leagues (tenants) per installation and tries to provide as many useful/fun statistics as possible. Wuzlstats works great on your smartphone and updates in real-time across multiple devices.

There is a public instance running at http://wuzlstats.sachsenhofer.com. Feel free to create your own league and start tracking your wuzl scores there. Please take care not to screw up the scores of the other leagues ;-) (Wuzlstats is a completely open system)


## Technology

Wuzlstats is built using ASP.NET vNext (v5 beta4 for the time being). Data is saved in SQL Server using Entity Framework, the real-time (websockets) functionality is provided by SignalR.

Since I'm [abstracting on the shoulders of giants](http://www.hanselman.com/blog/WeAreAbstractingOnTheShouldersOfGiants.aspx), these are the great open source libraries that Wuzlstats is built upon:

* [ASP.NET vNext](http://www.asp.net/vnext)
* [SignalR](http://signalr.net/)
* [Entity Framework](https://msdn.microsoft.com/en-us/data/ef.aspx)
* [jQuery](https://jquery.com/)
* [Bootstrap](http://getbootstrap.com/)
* [ImageResizer](http://imageresizing.net/)
* [Gulp](http://gulpjs.com/)


## Notes for developers

Feel free to fork Wuzlstats, of course I will accept pull requests as long as I think they are useful to everybody. Please note that Wuzlstats requires .NET 4.6 and won't run on .NET Core for the time being, because [I wasn't able to find](http://stackoverflow.com/questions/30528236/image-resizing-with-net-core) any image resizing libraries for .NET Core so far.
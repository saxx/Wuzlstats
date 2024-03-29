# Wuzlstats

## About

Wuzlstats is an open web app for tracking wuzl (tabletop soccer/football, foosball) scores. It's suited ideally to track the score of the wuzl table in your office.

It supports multiple leagues (tenants) per installation and tries to provide as many useful/fun statistics as possible. Wuzlstats works great on your smartphone and updates in real-time across multiple devices.

There is a public instance running at http://wuzlstats.sachsenhofer.com. Feel free to create your own league and start tracking your wuzl scores there. Please take care not to screw up the scores of the other leagues ;-) (Wuzlstats is a completely open system)


## Technology

Wuzlstats is built using ASP.NET Core. Data is saved in SQLite using Entity Framework Core, the real-time (websockets) functionality is provided by SignalR.

Since I'm [abstracting on the shoulders of giants](http://www.hanselman.com/blog/WeAreAbstractingOnTheShouldersOfGiants.aspx), these are the great open source libraries that Wuzlstats is built upon:

* [ASP.NET Core](http://www.asp.net/vnext)
* [SignalR](http://signalr.net/)
* [Entity Framework Core](https://learn.microsoft.com/de-de/ef/core)
* [jQuery](https://jquery.com/)
* [Bootstrap](http://getbootstrap.com/)
* [SkiaSharp](https://skia.org/)
* [Font Awesome](https://fontawesome.com/)

## Notes for developers

Feel free to fork Wuzlstats, of course I will accept pull requests as long as I think they are useful to everybody.

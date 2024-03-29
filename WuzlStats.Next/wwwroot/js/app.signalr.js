(function(app, $) {

    async function start(callback) {
        if (app.apiHub.state != "Disconnected") return;
        try {
            await app.apiHub.start();
            console.log("SignalR connection started");
            if (callback) {
                callback();
            }
        } catch (err) {
            console.log(err);
            setTimeout(start, 5000);
        }
    }

    // ReSharper disable once InconsistentNaming
    var _currentLeague = null;

    app.initSignalR = function() {
        const connection = new signalR
            .HubConnectionBuilder()
            .withUrl("/apiHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        app.apiHub = connection;

        app.apiHub.onclose(async () => {
            await start();
        });

        app.apiHub.on("leagueJoined", function(league) {
            console.log('League ' + league + ' joined.');
        })

        app.apiHub.on("reloadStatistics", function() {
            console.log('relead stats, but from league hub.');
        });
    };

    app.connectSignalR = function(league, callback) {
        console.log('Connecting to SignalR ...');

        start().then(function() {
            if (_currentLeague) {
                app.apiHub.invoke("LeaveLeague", _currentLeague);
            }

            app.apiHub.invoke("JoinLeague", (league)).then(function() {
                debugger;
                console.log('League ' + league + ' joined.');

                if (callback) {
                    callback();
                }
            }).catch(err => {
                console.log("Error while joining league");
                console.log(err);
            });
        });
    };

    app.apiHub = null;
}(window.app = window.app || {}, jQuery));

(function (app, $) {

    // ReSharper disable once InconsistentNaming
    var _currentLeague = null;

    app.initSignalR = function () {
        app.apiHub = $.connection.apiHub;

        app.apiHub.client.leagueJoined = function (league) {
            console.log('League ' + league + ' joined.');
        }

        app.apiHub.client.reloadStatistics = function () {
            console.log('relead stats, but from league hub.');
        };
    };

    app.connectSignalR = function (league, callback) {
        console.log('Connecting to SignalR ...');

        $.connection.hub.start().done(function () {
            if (_currentLeague) {
                app.apiHub.server.leaveLeague(_currentLeague);
            }

            app.apiHub.server.joinLeague(league).done(function () {
                console.log('League ' + league + ' joined.');

                if (callback) {
                    callback();
                }
            });
        });
    };

    app.apiHub = null;
}(window.app = window.app || {}, jQuery));
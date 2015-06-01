(function (app, $) {

    // ReSharper disable InconsistentNaming
    var _playerUrl;
    var _playersApiEndpoint;
    // ReSharper restore InconsistentNaming

    app.config = {

        init: function (playerUrl, playersApiEndpoint) {
            _playerUrl = playerUrl;
            _playersApiEndpoint = playersApiEndpoint;
        },

        getPlayerUrl: function () {
            return _playerUrl;
        },

        getPlayersApiEndpoint: function () {
            return _playersApiEndpoint;
        }
    };
}(window.app = window.app || {}, jQuery));
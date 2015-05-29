(function (app, $) {

    // ReSharper disable once InconsistentNaming
    var _playerUrl;

    app.config = {

        init: function (playerUrl) {
            _playerUrl = playerUrl;
        },

        getPlayerUrl: function() {
            return _playerUrl;
        }
    };
}(window.app = window.app || {}, jQuery));
// ReSharper disable once UnusedParameter
(function(app, $) {

    // ReSharper disable InconsistentNaming
    var _playerUrl;
    // ReSharper restore InconsistentNaming

    app.config = {
        init: function(playerUrl) {
            _playerUrl = playerUrl;
        },

        getPlayerUrl: function() {
            return _playerUrl;
        }
    };
}(window.app = window.app || {}, jQuery));

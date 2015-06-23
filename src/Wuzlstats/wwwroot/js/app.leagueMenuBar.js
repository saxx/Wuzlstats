(function (app, $) {
    app.registerLeagueMenuBar = function () {
        var menuBar = $('#leagueMenuBar');

        $(document).keydown(function(e) {
            if (e.ctrlKey) {
                menuBar.toggle();
            }
        });

        $("#scoreBox").dblclick(function() {
            menuBar.toggle();
        });
    }
}(window.app = window.app || {}, jQuery));
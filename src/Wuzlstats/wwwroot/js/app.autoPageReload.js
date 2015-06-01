(function (app, $) {
    app.autoPageReload = function (interval) {

        if (!interval) {
            interval = 1000 * 60 * 15; // 15 mins
        }

        var time = new Date().getTime();
        $(document.body).bind("mousemove keypress", function () {
            time = new Date().getTime();
        });

        setInterval(function() {
            if (new Date().getTime() - time >= interval) {
                window.location.reload(true);
            }
        }, 1000);
    }
}(window.app = window.app || {}, jQuery));
(function (app, $) {
    app.getLoadingHtml = function () {
        return $('<div class="progress"><div class="progress-bar progress-bar-info progress-bar-striped active" role="progressbar" style="width: 100%"><span class="sr-only">Loading</span></div></div>').html();
    }
}(window.app = window.app || {}, jQuery));
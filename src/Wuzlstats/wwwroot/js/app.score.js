(function (app, $) {

    function getValueOrFocus(selector) {
        var val = $(selector).val();
        if (val === "") {
            $(selector).focus();
            return null;
        }
        return val;
    }

    app.displayScoreForTwoPlayers = function() {
        $('#twoPlayersScore').show();
        $('#fourPlayersScore').hide();

        $('#redTeamScore').val('');
        $('#blueTeamScore').val('');
        $('#redTeamOffense').val('');
        $('#redTeamDefense').val('');
        $('#blueTeamOffense').val('');
        $('#blueTeamDefense').val('');
    };

    app.displayScoreForFourPlayers = function() {
        $('#twoPlayersScore').hide();
        $('#fourPlayersScore').show();

        $('#redPlayerScore').val('');
        $('#bluePlayerScore').val('');
        $('#redPlayer').val('');
        $('#bluePlayer').val('');
    };

    app.refreshPlayersDatalist = function (onlyIfOutdated) {
        var datalist = $('#playersDatalist');

        if (onlyIfOutdated) {
            if (datalist.data('lastrefresh') && datalist.data('lastrefresh') > (new Date().getTime() - 30000)) {
                return;
            }
        }

        $.ajax({
            type: 'GET',
            url: app.config.getPlayersApiEndpoint()
            // ReSharper disable once UnusedParameter
        }).fail(function (jqXhr, textStatus, errorThrown) {
            alert('Loading players failed: ' + textStatus);
        }).done(function (result) {
            datalist.empty();

            $.each(result, function(index, val) {
                datalist.append($("<option></option>").html(val));
            });

            datalist.data('lastrefresh', new Date().getTime());
        });
    };

    app.initScore = function (endpointUrl) {
        $('#twoPlayersButton').change(function() {
            app.displayScoreForTwoPlayers();
        });
        $('#fourPlayersButton').change(function() {
            app.displayScoreForFourPlayers();
        });

        $('.player').focus(function() {
            app.refreshPlayersDatalist(true);
        });

        var submitButton = $('#submitScore');
        submitButton.click(function() {
            var viewModel = null;

            if ($('#twoPlayersScore').is(':visible')) {
                var redPlayerScore = getValueOrFocus('#redPlayerScore');
                var bluePlayerScore = getValueOrFocus('#bluePlayerScore');
                var redPlayer = getValueOrFocus('#redPlayer');
                var bluePlayer = getValueOrFocus('#bluePlayer');

                if (redPlayerScore && bluePlayerScore && redPlayer && bluePlayer) {
                    viewModel = {
                        redPlayerScore: redPlayerScore,
                        bluePlayerScore: bluePlayerScore,
                        redPlayer: redPlayer,
                        bluePlayer: bluePlayer
                    };
                }
            } else {
                var redTeamScore = getValueOrFocus('#redTeamScore');
                var blueTeamScore = getValueOrFocus('#blueTeamScore');
                var redTeamOffense = getValueOrFocus('#redTeamOffense');
                var blueTeamOffense = getValueOrFocus('#blueTeamOffense');
                var redTeamDefense = getValueOrFocus('#redTeamDefense');
                var blueTeamDefense = getValueOrFocus('#blueTeamDefense');

                if (redTeamScore && blueTeamScore && redTeamOffense && blueTeamOffense && redTeamDefense && blueTeamDefense) {
                    viewModel = {
                        redTeamScore: redTeamScore,
                        blueTeamScore: blueTeamScore,
                        redTeamOffense: redTeamOffense,
                        blueTeamOffense: blueTeamOffense,
                        redTeamDefense: redTeamDefense,
                        blueTeamDefense: blueTeamDefense
                    };
                }
            }


            if (viewModel) {

                var progressBar = $('<div />').html(app.getLoadingHtml());
                submitButton.after(progressBar);
                submitButton.hide();

                $.ajax({
                    type: 'POST',
                    url: endpointUrl,
                    data: viewModel
                // ReSharper disable once UnusedParameter
                }).fail(function(jqXhr, textStatus, errorThrown) {
                    alert('Submit score failed: ' + textStatus);
                }).done(function() {
                    $('.player').val('');
                    $('.score').val('');

                    app.refreshPlayerRankings();
                    app.refreshTeamRankings();
                }).always(function () {
                    submitButton.show();
                    progressBar.remove();
                    app.refreshPlayersDatalist(false);
                });
            }
        });
    };
}(window.app = window.app || {}, jQuery));
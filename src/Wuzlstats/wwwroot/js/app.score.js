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

    app.initScore = function (endpointUrl) {
        $('#twoPlayersButton').change(function() {
            app.displayScoreForTwoPlayers();
        });
        $('#fourPlayersButton').change(function() {
            app.displayScoreForFourPlayers();
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

                submitButton.attr('disabled', true);

                $.ajax({
                    type: 'POST',
                    url: endpointUrl,
                    data: viewModel
                // ReSharper disable once UnusedParameter
                }).fail(function(jqXhr, textStatus, errorThrown) {
                    alert('Submit score failed: ' + textStatus);
                }).done(function() {
                    $('#redPlayerScore').val('');
                    $('#bluePlayerScore').val('');
                    $('#redPlayer').val('');
                    $('#bluePlayer').val('');
                    $('#redTeamScore').val('');
                    $('#blueTeamScore').val('');
                    $('#redTeamOffense').val('');
                    $('#blueTeamOffense').val('');
                    $('#redTeamDefense').val('');
                    $('#blueTeamDefense').val('');

                    app.refreshPlayerRankings();
                    app.refreshTeamRankings();
                }).always(function () {
                    submitButton.attr('disabled', false);
                });
            }
        });
    };
}(window.app = window.app || {}, jQuery));
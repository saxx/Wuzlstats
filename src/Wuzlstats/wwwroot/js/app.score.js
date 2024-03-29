(function(app, $) {
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

    function initPlayersDatalist() {
        var datalist = $('#playersDatalist');

        app.apiHub.on("reloadPlayers", function(result) {
            console.log('Refreshing players datalist ...');
            debugger;
            datalist.empty();

            $.each(result.players, function(index, val) {
                datalist.append($("<option></option>").html(val));
            });
        });
    }

    app.initScore = function(league) {
        initPlayersDatalist(league);

        app.apiHub.on("scorePosted", function() {
            console.log('Score posted.');
        });

        $('#twoPlayersButton').change(function() {
            app.displayScoreForTwoPlayers();
        });
        $('#fourPlayersButton').change(function() {
            app.displayScoreForFourPlayers();
        });

        var submitButton = $('#submitScore');
        submitButton.click(function() {
            var viewModel = null;

            localStorage.removeItem('lastRedPlayer');
            localStorage.removeItem('lastBluePlayer');
            localStorage.removeItem('lastRedOffensePlayer');
            localStorage.removeItem('lastRedDefensePlayer');
            localStorage.removeItem('lastBlueOffensePlayer');
            localStorage.removeItem('lastBlueDefensePlayer');

            if ($('#twoPlayersScore').is(':visible')) {
                var redPlayerScore = getValueOrFocus('#redPlayerScore');
                var bluePlayerScore = getValueOrFocus('#bluePlayerScore');
                var redPlayer = getValueOrFocus('#redPlayer');
                var bluePlayer = getValueOrFocus('#bluePlayer');

                if (redPlayerScore && bluePlayerScore && redPlayer && bluePlayer) {
                    if (redPlayer === bluePlayer) {
                        alert('Very funny. Same players not allowed.');
                        return;
                    }

                    viewModel = {
                        redPlayerScore: redPlayerScore,
                        bluePlayerScore: bluePlayerScore,
                        redPlayer: redPlayer,
                        bluePlayer: bluePlayer
                    };

                    localStorage.setItem('lastRedPlayer', redPlayer);
                    localStorage.setItem('lastBluePlayer', bluePlayer);
                }
            } else {
                var redTeamScore = getValueOrFocus('#redTeamScore');
                var blueTeamScore = getValueOrFocus('#blueTeamScore');
                var redTeamOffense = getValueOrFocus('#redTeamOffense');
                var blueTeamOffense = getValueOrFocus('#blueTeamOffense');
                var redTeamDefense = getValueOrFocus('#redTeamDefense');
                var blueTeamDefense = getValueOrFocus('#blueTeamDefense');

                if (redTeamScore && blueTeamScore && redTeamOffense && blueTeamOffense && redTeamDefense && blueTeamDefense) {

                    if (redTeamOffense === redTeamDefense || redTeamOffense === blueTeamOffense || redTeamOffense === blueTeamDefense ||
                        redTeamDefense === blueTeamOffense || redTeamDefense === blueTeamDefense ||
                        blueTeamOffense === blueTeamDefense) {
                        alert('Very funny. Same players not allowed.');
                        return;
                    }

                    viewModel = {
                        redTeamScore: redTeamScore,
                        blueTeamScore: blueTeamScore,
                        redTeamOffense: redTeamOffense,
                        blueTeamOffense: blueTeamOffense,
                        redTeamDefense: redTeamDefense,
                        blueTeamDefense: blueTeamDefense
                    };

                    localStorage.setItem('lastRedOffensePlayer', redTeamOffense);
                    localStorage.setItem('lastRedDefensePlayer', redTeamDefense);
                    localStorage.setItem('lastBlueOffensePlayer', blueTeamOffense);
                    localStorage.setItem('lastBlueDefensePlayer', blueTeamDefense);
                }
            }

            if (viewModel) {
                submitButton.attr('disabled', true).html('<span class="glyphicon glyphicon-hourglass"></span> Submitting ...');

                app.apiHub.invoke("postScore", league, viewModel)
                    .then(function() {
                        $('.player').val('');
                        $('.score').val('');
                        submitButton.attr('disabled', true).html('<span class="glyphicon glyphicon-ok-circle"></span> Saved!');

                        setTimeout(function() {
                            submitButton.attr('disabled', false).html('Submit');
                        }, 5000);
                    }).catch(err => {
                        console.error(err.toString());
                        submitButton.attr('disabled', false).html('Submit');
                        alert('Post score failed.\n\n' + err);
                    });
            }
        });
    };
}(window.app = window.app || {}, jQuery));

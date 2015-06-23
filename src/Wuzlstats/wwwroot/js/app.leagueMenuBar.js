(function (app, $) {
    app.registerLeagueMenuBar = function () {
        var menuBar = $('#leagueMenuBar');

        $(document).keydown(function (e) {
            if (e.ctrlKey) {
                menuBar.toggle();
            }
        });

        $("#scoreBox").dblclick(function () {
            menuBar.toggle();
        });

        menuBar.find('#replayButton').click(function () {
            console.log('Restoring last players ...');

            $('#redPlayer').val(localStorage.getItem('lastRedPlayer'));
            $('#bluePlayer').val(localStorage.getItem('lastBluePlayer'));

            $('#redTeamOffense').val(localStorage.getItem('lastRedOffensePlayer'));
            $('#redTeamDefense').val(localStorage.getItem('lastRedDefensePlayer'));
            $('#blueTeamOffense').val(localStorage.getItem('lastBlueOffensePlayer'));
            $('#blueTeamDefense').val(localStorage.getItem('lastBlueDefensePlayer'));

            return false;
        });

        menuBar.find('#switchSidesButton').click(function () {
            console.log('Switching sides ...');

            var tmp = $('#redPlayer').val();
            $('#redPlayer').val($('#bluePlayer').val());
            $('#bluePlayer').val(tmp);

            tmp = $('#redTeamOffense').val();
            $('#redTeamOffense').val($('#blueTeamOffense').val());
            $('#blueTeamOffense').val(tmp);
            tmp = $('#redTeamDefense').val();
            $('#redTeamDefense').val($('#blueTeamDefense').val());
            $('#blueTeamDefense').val(tmp);

            return false;
        });

        menuBar.find('#switchPositionsButton').click(function () {
            console.log('Switching positions ...');

            var tmp = $('#redTeamOffense').val();
            $('#redTeamOffense').val($('#redTeamDefense').val());
            $('#redTeamDefense').val(tmp);
            tmp = $('#blueTeamOffense').val();
            $('#blueTeamOffense').val($('#blueTeamDefense').val());
            $('#blueTeamDefense').val(tmp);

            return false;
        });
    };
}(window.app = window.app || {}, jQuery));
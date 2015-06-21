(function (app, $) {
    function buildRow(label, value) {
        var row = $('<tr />');
        row.append($('<th />').html(label));
        row.append($('<td />').addClass('text-right').html(value));
        return row;
    }

    app.renderLeagueStatistics = function (element, statistics) {
        element = $(element)
            .html('');

        var table = $('<table />').addClass('table');
        table.append(buildRow('# games:', statistics.games + ' (last ' + statistics.daysForStatistics + ' days)'));
        table.append(buildRow('# players:', '<span class="team-red">' + statistics.redPlayers + '</span> : <span class="team-blue">' + statistics.bluePlayers + '</span>'));
        table.append(buildRow('# goals:', '<span class="team-red">' + statistics.redGoals + '</span> : <span class="team-blue">' + statistics.blueGoals + '</span>'));
        table.append(buildRow('# wins:', '<span class="team-red">' + statistics.redWins + '</span> : <span class="team-blue">' + statistics.blueWins + '</span>'));
        table.append(buildRow('Ø goal difference:', statistics.goalDifference.toFixed(2)));
        table.append(buildRow('Most active player:', statistics.mostActivePlayer));

        element.append(table);
    };
}(window.app = window.app || {}, jQuery));
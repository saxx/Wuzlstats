(function (app, $) {
    function buildSimpleRow(label, value) {
        var row = $('<tr />').addClass('hover:bg-gray-50 transition-colors');
        row.append($('<td />').addClass('px-4 py-3 font-medium text-gray-900').html(label));
        row.append($('<td />').addClass('px-4 py-3 text-right text-gray-700').html(value));
        return row;
    }

    function buildComparisonRow(label, redValue, blueValue) {
        var row = $('<tr />').addClass('hover:bg-gray-50 transition-colors');
        row.append($('<td />').addClass('px-4 py-3 font-medium text-gray-900').html(label));

        // Create cell with visual comparison bars
        var cell = $('<td />').addClass('px-4 py-3');

        // Container for bars and values
        var container = $('<div />').addClass('flex items-center gap-2');

        // Calculate percentages for bar widths
        var total = redValue + blueValue;
        var redPercent = total > 0 ? (redValue / total * 100) : 50;
        var bluePercent = total > 0 ? (blueValue / total * 100) : 50;

        // Red value and bar
        var redSection = $('<div />').addClass('flex items-center gap-2 flex-1 justify-end');
        redSection.append($('<span />').addClass('font-semibold text-gray-900').html(redValue));
        var redBarContainer = $('<div />').addClass('w-24 h-2 bg-gray-200 rounded-full overflow-hidden');
        var redBar = $('<div />').addClass('h-full bg-red-400 rounded-full').css('width', redPercent + '%');
        redBarContainer.append(redBar);
        redSection.append(redBarContainer);

        // Separator
        var separator = $('<span />').addClass('text-gray-400 font-bold').html(':');

        // Blue bar and value
        var blueSection = $('<div />').addClass('flex items-center gap-2 flex-1');
        var blueBarContainer = $('<div />').addClass('w-24 h-2 bg-gray-200 rounded-full overflow-hidden');
        var blueBar = $('<div />').addClass('h-full bg-blue-400 rounded-full').css('width', bluePercent + '%');
        blueBarContainer.append(blueBar);
        blueSection.append(blueBarContainer);
        blueSection.append($('<span />').addClass('font-semibold text-gray-900').html(blueValue));

        container.append(redSection);
        container.append(separator);
        container.append(blueSection);

        cell.append(container);
        row.append(cell);

        return row;
    }

    app.renderLeagueStatistics = function (element, statistics) {
        element = $(element).html('');

        var tableContainer = $('<div />').addClass('relative overflow-x-auto');
        var table = $('<table />').addClass('w-full text-sm');
        var tbody = $('<tbody />').addClass('divide-y divide-gray-200');

        tbody.append(buildSimpleRow('# games', statistics.games + ' (last ' + statistics.daysForStatistics + ' days)'));
        tbody.append(buildComparisonRow('# players', statistics.redPlayers, statistics.bluePlayers));
        tbody.append(buildComparisonRow('# goals', statistics.redGoals, statistics.blueGoals));
        tbody.append(buildComparisonRow('# wins', statistics.redWins, statistics.blueWins));
        tbody.append(buildSimpleRow('Ø goal difference', statistics.goalDifference.toFixed(2)));
        tbody.append(buildSimpleRow('Most active player', statistics.mostActivePlayer));

        table.append(tbody);
        tableContainer.append(table);
        element.append(tableContainer);
    };
}(window.app = window.app || {}, jQuery));
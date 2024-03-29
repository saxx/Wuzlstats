(function(app, $) {
    app.renderTeamRanking = function(element, teams) {
        element = $(element)
            .addClass('ranking')
            .addClass('team-ranking')
            .html('');

        $.each(teams, function(index, team) {
            var block = $('<li />');

            block.append(
                $('<a href="' + app.config.getPlayerUrl().replace('[id]', team.player1.id) + '"></a>').append(
                    $('<img />')
                        .attr('src', 'data:image/png;base64,' + team.player1.image)
                        .addClass('ranking-image')
                )
            );
            block.append(
                $('<a href="' + app.config.getPlayerUrl().replace('[id]', team.player2.id) + '"></a>').append(
                    $('<img />')
                        .attr('src', 'data:image/png;base64,' + team.player2.image)
                        .addClass('ranking-image')
                )
            );

            block.append(
                $('<div />')
                    .html(team.player1.name)
                    .addClass('ranking-name')
            );
            block.append(
                $('<div />')
                    .html(team.player2.name)
                    .addClass('ranking-name')
            );

            var scoreBlock = $('<div />')
                .addClass('ranking-score');
            scoreBlock.append(
                $('<span />')
                    .html(team.wins)
                    .append('<span class="fa-solid fa-crown" />')
                    .addClass('ranking-wins')
            );
            scoreBlock.append(
                $('<span />')
                    .html(team.losses)
                    .append('<span class="fa-solid fa-poop" />')
                    .addClass('ranking-losses')
            );
            block.append(scoreBlock);

            element.append(block);
        });
    };
}(window.app = window.app || {}, jQuery));

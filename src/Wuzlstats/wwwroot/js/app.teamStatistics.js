(function(app, $) {
    app.renderTeamRanking = function(element, teams) {
        element = $(element).html('');

        $.each(teams, function(index, team) {
            var block = $('<li />').addClass('ranking-item');

            // Avatars (side by side)
            var avatarsContainer = $('<div />').addClass('team-ranking-avatars');
            avatarsContainer.append(
                $('<a href="' + app.config.getPlayerUrl().replace('[id]', team.player1.id) + '"></a>').append(
                    $('<img />')
                        .attr('src', 'data:image/png;base64,' + team.player1.image)
                        .attr('alt', team.player1.name)
                        .addClass('ranking-avatar')
                )
            );
            avatarsContainer.append(
                $('<a href="' + app.config.getPlayerUrl().replace('[id]', team.player2.id) + '"></a>').append(
                    $('<img />')
                        .attr('src', 'data:image/png;base64,' + team.player2.image)
                        .attr('alt', team.player2.name)
                        .addClass('ranking-avatar')
                )
            );
            block.append(avatarsContainer);

            // Names (stacked)
            var namesContainer = $('<div />').addClass('team-ranking-names');
            namesContainer.append(
                $('<div />')
                    .html(team.player1.name)
                    .addClass('ranking-name')
            );
            namesContainer.append(
                $('<div />')
                    .html(team.player2.name)
                    .addClass('ranking-name')
            );
            block.append(namesContainer);

            // Score (wins/losses)
            var scoreBlock = $('<div />').addClass('ranking-score');
            scoreBlock.append(
                $('<span />')
                    .addClass('ranking-wins')
                    .html(team.wins + ' ')
                    .append('<i class="fa-solid fa-crown" />')
            );
            scoreBlock.append(
                $('<span />')
                    .addClass('ranking-losses')
                    .html(team.losses + ' ')
                    .append('<i class="fa-solid fa-poop" />')
            );
            block.append(scoreBlock);

            element.append(block);
        });
    };
}(window.app = window.app || {}, jQuery));

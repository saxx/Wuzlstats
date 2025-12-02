(function(app, $) {

    app.renderPlayerRanking = function(element, players) {
        element = $(element).html('');

        $.each(players, function(index, player) {
            var block = $('<li />').addClass('ranking-item');

            // Avatar
            block.append(
                $('<a href="' + app.config.getPlayerUrl().replace('[id]', player.id) + '"></a>').append(
                    $('<img />')
                        .attr('src', 'data:image/png;base64,' + player.image)
                        .attr('alt', player.name)
                        .addClass('ranking-avatar')
                )
            );

            // Name
            block.append(
                $('<div />')
                    .html(player.name)
                    .addClass('ranking-name')
            );

            // Score (wins/losses)
            var scoreBlock = $('<div />').addClass('ranking-score');
            scoreBlock.append(
                $('<span />')
                    .addClass('ranking-wins')
                    .html(player.wins + ' ')
                    .append('<i class="fa-solid fa-crown" />')
            );
            scoreBlock.append(
                $('<span />')
                    .addClass('ranking-losses')
                    .html(player.losses + ' ')
                    .append('<i class="fa-solid fa-poop" />')
            );
            block.append(scoreBlock);

            element.append(block);
        });
    };

}(window.app = window.app || {}, jQuery));

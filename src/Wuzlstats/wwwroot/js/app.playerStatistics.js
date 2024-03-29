(function(app, $) {

    app.renderPlayerRanking = function(element, players) {
        element = $(element)
            .addClass('ranking')
            .addClass('player-ranking')
            .html('');

        $.each(players, function(index, player) {
            var block = $('<li />');

            block.append(
                $('<a href="' + app.config.getPlayerUrl().replace('[id]', player.id) + '"></a>').append(
                    $('<img />')
                        .attr('src', 'data:image/png;base64,' + player.image)
                        .addClass('ranking-image')
                )
            );

            block.append(
                $('<div />')
                    .html(player.name)
                    .addClass('ranking-name')
            );

            var scoreBlock = $('<div />')
                .addClass('ranking-score');
            scoreBlock.append(
                $('<span />')
                    .html(player.wins)
                    .append('<span class="fa-solid fa-crown" />')
                    .addClass('ranking-wins')
            );
            scoreBlock.append(
                $('<span />')
                    .html(player.losses)
                    .append('<span class="fa-solid fa-poop" />')
                    .addClass('ranking-losses')
            );
            block.append(scoreBlock);

            element.append(block);
        });
    };

}(window.app = window.app || {}, jQuery));

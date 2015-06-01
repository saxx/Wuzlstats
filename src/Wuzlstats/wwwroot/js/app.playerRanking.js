(function (app, $) {

    // ReSharper disable once InconsistentNaming
    var _dataKey = 'playerrankingendpointurl';

    app.initPlayerRanking = function (element, endpointUrl, playerUrl) {
        $(element).data(_dataKey, endpointUrl);
        $(element).addClass('player-ranking');
    }

    app.refreshPlayerRankings = function () {
        $('.player-ranking').each(function (index, container) {
            container = $(container)
                .addClass('ranking')
                .addClass('player-ranking')
                .html('<li>' + app.getLoadingHtml() + '</li>');

            var endpointUrl = container.data(_dataKey);

            $.getJSON(endpointUrl)
            .fail(function (jqXhr, textStatus, errorThrown) {
                alert('Player ranking failed to load: ' + errorThrown + ' - ' + textStatus);
            }).done(function (result) {
                container.html('');

                $.each(result.players, function (index, player) {
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
                        .append('<span class="glyphicon glyphicon-thumbs-up" />')
                        .addClass('ranking-wins')
                    );
                    scoreBlock.append(
                        $('<span />')
                        .html(player.losses)
                        .append('<span class="glyphicon glyphicon-thumbs-down" />')
                        .addClass('ranking-losses')
                    );
                    block.append(scoreBlock);

                    container.append(block);
                });
            });
        });
    };

}(window.app = window.app || {}, jQuery));
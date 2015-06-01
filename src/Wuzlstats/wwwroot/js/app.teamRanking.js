(function (app, $) {

    // ReSharper disable once InconsistentNaming
    var _dataKey = 'teamrankingendpointurl';

    app.initTeamRanking = function (element, endpointUrl) {
        $(element).data(_dataKey, endpointUrl);
        $(element).addClass('team-ranking');
    }

    app.refreshTeamRankings = function () {
        $('.team-ranking').each(function (index, container) {
            container = $(container)
                .addClass('ranking')
                .addClass('team-ranking')
                .html('<li>' + app.getLoadingHtml() + '</li>');

            var endpointUrl = container.data(_dataKey);

            $.getJSON(endpointUrl)
            .fail(function (jqXhr, textStatus, errorThrown) {
                alert('Team ranking failed to load: ' + errorThrown + ' - ' + textStatus);
            }).done(function (result) {
                container.html('');

                

                $.each(result.teams, function (index, team) {
                    var block = $('<li />');

                    block.append(
                        $('<a href="' + app.config.getPlayerUrl().replace('[id]', team.id1) + '"></a>').append(
                            $('<img />')
                            .attr('src', 'data:image/png;base64,' + team.image1)
                            .addClass('ranking-image')
                        )
                    );
                    block.append(
                        $('<a href="' + app.config.getPlayerUrl().replace('[id]', team.id2) + '"></a>').append(
                            $('<img />')
                            .attr('src', 'data:image/png;base64,' + team.image2)
                            .addClass('ranking-image')
                        )
                    );

                    block.append(
                        $('<div />')
                        .html(team.name1)
                        .addClass('ranking-name')
                    );
                    block.append(
                        $('<div />')
                        .html(team.name2)
                        .addClass('ranking-name')
                    );

                    var scoreBlock = $('<div />')
                        .addClass('ranking-score');
                    scoreBlock.append(
                        $('<span />')
                        .html(team.wins)
                        .append('<span class="glyphicon glyphicon-thumbs-up" />')
                        .addClass('ranking-wins')
                    );
                    scoreBlock.append(
                        $('<span />')
                        .html(team.losses)
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
!function(e,a){e.autoPageReload=function(e){e||(e=9e5);var n=(new Date).getTime();a(document.body).bind("mousemove keypress",function(){n=(new Date).getTime()}),setInterval(function(){(new Date).getTime()-n>=e&&window.location.reload(!0)},1e3)}}(window.app=window.app||{},jQuery),function(e,a){var n;e.config={init:function(e){n=e},getPlayerUrl:function(){return n}}}(window.app=window.app||{},jQuery),function(e,a){function n(e,n){var l=a("<tr />");return l.append(a("<th />").html(e)),l.append(a("<td />").addClass("text-right").html(n)),l}e.renderLeagueStatistics=function(e,l){e=a(e).html("");var r=a("<table />").addClass("table");r.append(n("# games:",l.games+" (last "+l.daysForStatistics+" days)")),r.append(n("# players:",'<span class="team-red">'+l.redPlayers+'</span> : <span class="team-blue">'+l.bluePlayers+"</span>")),r.append(n("# goals:",'<span class="team-red">'+l.redGoals+'</span> : <span class="team-blue">'+l.blueGoals+"</span>")),r.append(n("# wins:",'<span class="team-red">'+l.redWins+'</span> : <span class="team-blue">'+l.blueWins+"</span>")),r.append(n("Ø goal difference:",l.goalDifference.toFixed(2))),r.append(n("Most active player:",l.mostActivePlayer)),e.append(r)}}(window.app=window.app||{},jQuery),function(e,a){e.renderPlayerRanking=function(n,l){n=a(n).addClass("ranking").addClass("player-ranking").html(""),a.each(l,function(l,r){var s=a("<li />");s.append(a('<a href="'+e.config.getPlayerUrl().replace("[id]",r.id)+'"></a>').append(a("<img />").attr("src","data:image/png;base64,"+r.image).addClass("ranking-image"))),s.append(a("<div />").html(r.name).addClass("ranking-name"));var o=a("<div />").addClass("ranking-score");o.append(a("<span />").html(r.wins).append('<span class="glyphicon glyphicon-thumbs-up" />').addClass("ranking-wins")),o.append(a("<span />").html(r.losses).append('<span class="glyphicon glyphicon-thumbs-down" />').addClass("ranking-losses")),s.append(o),n.append(s)})}}(window.app=window.app||{},jQuery),function(e,a){function n(e){var n=a(e).val();return""===n?(a(e).focus(),null):n}function l(){var n=a("#playersDatalist");e.apiHub.client.reloadPlayers=function(e){console.log("Refreshing players datalist ..."),n.empty(),a.each(e.players,function(e,l){n.append(a("<option></option>").html(l))})}}e.displayScoreForTwoPlayers=function(){a("#twoPlayersScore").show(),a("#fourPlayersScore").hide(),a("#redTeamScore").val(""),a("#blueTeamScore").val(""),a("#redTeamOffense").val(""),a("#redTeamDefense").val(""),a("#blueTeamOffense").val(""),a("#blueTeamDefense").val("")},e.displayScoreForFourPlayers=function(){a("#twoPlayersScore").hide(),a("#fourPlayersScore").show(),a("#redPlayerScore").val(""),a("#bluePlayerScore").val(""),a("#redPlayer").val(""),a("#bluePlayer").val("")},e.initScore=function(r){l(r),e.apiHub.client.scorePosted=function(){console.log("Score posted.")},a("#twoPlayersButton").change(function(){e.displayScoreForTwoPlayers()}),a("#fourPlayersButton").change(function(){e.displayScoreForFourPlayers()});var s=a("#submitScore");s.click(function(){var l=null;if(a("#twoPlayersScore").is(":visible")){var o=n("#redPlayerScore"),i=n("#bluePlayerScore"),p=n("#redPlayer"),d=n("#bluePlayer");if(o&&i&&p&&d){if(p===d)return void alert("Very funny. Same players not allowed.");l={redPlayerScore:o,bluePlayerScore:i,redPlayer:p,bluePlayer:d}}}else{var t=n("#redTeamScore"),c=n("#blueTeamScore"),u=n("#redTeamOffense"),f=n("#blueTeamOffense"),g=n("#redTeamDefense"),y=n("#blueTeamDefense");if(t&&c&&u&&f&&g&&y){if(u===g||u===f||u===y||g===f||g===y||f===y)return void alert("Very funny. Same players not allowed.");l={redTeamScore:t,blueTeamScore:c,redTeamOffense:u,blueTeamOffense:f,redTeamDefense:g,blueTeamDefense:y}}}l&&(s.hide(),e.apiHub.server.postScore(r,l).done(function(){a(".player").val(""),a(".score").val("")}).always(function(){s.show()}).fail(function(e){alert("Post score failed.\n\n"+e)}))})}}(window.app=window.app||{},jQuery),function(e,a){var n=null;e.initSignalR=function(){e.apiHub=a.connection.apiHub,e.apiHub.client.leagueJoined=function(e){console.log("League "+e+" joined.")},e.apiHub.client.reloadStatistics=function(){console.log("relead stats, but from league hub.")}},e.connectSignalR=function(l,r){console.log("Connecting to SignalR ..."),a.connection.hub.start().done(function(){n&&e.apiHub.server.leaveLeague(n),e.apiHub.server.joinLeague(l).done(function(){console.log("League "+l+" joined."),r&&r()})})},e.apiHub=null}(window.app=window.app||{},jQuery),function(e,a){e.renderTeamRanking=function(n,l){n=a(n).addClass("ranking").addClass("team-ranking").html(""),a.each(l,function(l,r){var s=a("<li />");s.append(a('<a href="'+e.config.getPlayerUrl().replace("[id]",r.player1.id)+'"></a>').append(a("<img />").attr("src","data:image/png;base64,"+r.player1.image).addClass("ranking-image"))),s.append(a('<a href="'+e.config.getPlayerUrl().replace("[id]",r.player2.id)+'"></a>').append(a("<img />").attr("src","data:image/png;base64,"+r.player2.image).addClass("ranking-image"))),s.append(a("<div />").html(r.player1.name).addClass("ranking-name")),s.append(a("<div />").html(r.player2.name).addClass("ranking-name"));var o=a("<div />").addClass("ranking-score");o.append(a("<span />").html(r.wins).append('<span class="glyphicon glyphicon-thumbs-up" />').addClass("ranking-wins")),o.append(a("<span />").html(r.losses).append('<span class="glyphicon glyphicon-thumbs-down" />').addClass("ranking-losses")),s.append(o),n.append(s)})}}(window.app=window.app||{},jQuery);
//# sourceMappingURL=app.js.map
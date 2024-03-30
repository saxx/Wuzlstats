using Microsoft.AspNetCore.Mvc;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task PostScore(string league, PostScoreViewModel viewModel, [FromServices] LeagueStatisticsViewModel leagueViewModel)
        {
            await viewModel.Save(await CheckAndLoadLeague(league), _db);
            await NotifyGroupToReloadPlayers(league);
            await NotifyGroupToReloadStatistics(league, leagueViewModel);
            await NotifyCallerToReloadStatistics(league, leagueViewModel);
        }
    }
}

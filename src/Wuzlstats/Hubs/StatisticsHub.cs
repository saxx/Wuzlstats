using Microsoft.AspNetCore.SignalR;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task NotifyCallerToReloadStatistics(string league, LeagueStatisticsViewModel viewModel)
        {
            await viewModel.Fill(await CheckAndLoadLeague(league));
            await Clients.Caller.SendAsync("reloadStatistics", viewModel);
        }


        public async Task NotifyGroupToReloadStatistics(string league, LeagueStatisticsViewModel viewModel)
        {
            await viewModel.Fill(await CheckAndLoadLeague(league));
            await Clients.Group("league").SendAsync("reloadStatistics", viewModel);
        }
    }
}

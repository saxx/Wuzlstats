using System.Threading.Tasks;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task NotifyCallerToReloadStatistics(string league)
        {
            var viewModel = await new LeagueStatisticsViewModel(_db, _settings).Fill(await CheckAndLoadLeague(league));
            await Clients.Caller.reloadStatistics(viewModel);
        }


        public async Task NotifyGroupToReloadStatistics(string league)
        {
            var viewModel = await new LeagueStatisticsViewModel(_db, _settings).Fill(await CheckAndLoadLeague(league));
            Clients.Group(league).reloadStatistics(viewModel);
        }
    }
}
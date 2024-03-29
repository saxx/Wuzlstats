using System.Threading.Tasks;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task PostScore(string league, PostScoreViewModel viewModel)
        {
            await viewModel.Save(await CheckAndLoadLeague(league), _db);
            await NotifyGroupToReloadPlayers(league);
            // await NotifyGroupToReloadStatistics(league, viewModel);
            // await NotifyCallerToReloadStatistics(league, viewModel);
        }
    }
}

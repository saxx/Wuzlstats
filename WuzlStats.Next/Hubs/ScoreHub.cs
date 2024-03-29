using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task PostScore(string league, PostScoreViewModel viewModel)
        {
            Console.WriteLine("Attemting to post score... ");
            await viewModel.Save(await CheckAndLoadLeague(league), _db);
            await NotifyGroupToReloadPlayers(league);
            await NotifyGroupToReloadStatistics(league);
            await NotifyCallerToReloadStatistics(league);
        }
    }
}

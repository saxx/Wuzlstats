using System.Threading.Tasks;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task NotifyCallerToReloadPlayers(string league)
        {
            Clients.Caller.reloadPlayers(await new ReloadPlayersViewModel(_db).Fill(league));
        }


        public async Task NotifyGroupToReloadPlayers(string league)
        {
            Clients.Group(league).reloadPlayers(await new ReloadPlayersViewModel(_db).Fill(league));
        }
    }
}
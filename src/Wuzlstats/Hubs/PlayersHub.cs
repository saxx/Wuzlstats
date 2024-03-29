using Microsoft.AspNetCore.SignalR;
using Wuzlstats.ViewModels.Hubs;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub
    {
        public async Task NotifyCallerToReloadPlayers(string league)
        {
            await Clients.Caller.SendAsync("reloadPlayers", await new ReloadPlayersViewModel(_db).Fill(league));
        }

        public async Task NotifyGroupToReloadPlayers(string league)
        {
            await Clients.Group(league).SendAsync("reloadPlayers", await new ReloadPlayersViewModel(_db).Fill(league));
        }
    }
}

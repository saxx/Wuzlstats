using Wuzlstats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

namespace Wuzlstats.Hubs
{
    public partial class ApiHub : Hub
    {
        private readonly Db _db;
        private readonly AppSettings _settings;


        public ApiHub(Db db, AppSettings settings)
        {
            _settings = settings;
            _db = db;
        }

        public async Task JoinLeague(string league)
        {
            Console.WriteLine($"Joining {league}");
            await Groups.AddToGroupAsync(Context.ConnectionId, league);
            await NotifyCallerToReloadPlayers(league);
            //await NotifyCallerToReloadStatistics(league);
        }


        public Task LeaveLeague(string league)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, league);
        }


        private async Task<League> CheckAndLoadLeague(string league)
        {
            if (string.IsNullOrEmpty(league))
            {
                throw new Exception("Invalid league, must not be empty.");
            }
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.ToLower() == league.ToLower());
            if (leagueEntity == null)
            {
                throw new Exception("Invalid league.");
            }

            return leagueEntity;
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Wuzlstats.ExtensionMethods;
using Wuzlstats.Models;

namespace Wuzlstats.Hubs
{
    [HubName("apiHub")]
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
            await Groups.Add(Context.ConnectionId, league);
            await NotifyCallerToReloadPlayers(league);
            await NotifyCallerToReloadStatistics(league);
        }


        public Task LeaveLeague(string league)
        {
            return Groups.Remove(Context.ConnectionId, league);
        }


        private async Task<League> CheckAndLoadLeague(string league)
        {
            if (league.IsNoE())
            {
                throw new Exception("Invalid league, must not be empty.");
            }
            var leagueEntity = await _db.Leagues.FirstOrDefaultAsync(x => x.Name.Equals(league, StringComparison.CurrentCultureIgnoreCase));
            if (leagueEntity == null)
            {
                throw new Exception("Invalid league.");
            }

            return leagueEntity;
        }
    }
}

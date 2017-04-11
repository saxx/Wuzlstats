using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Hubs;
using Wuzlstats.Extensions;
using Wuzlstats.Models;

namespace Wuzlstats.Hubs
{
    [HubName("apiHub")]
    public partial class ApiHub : Hub
    {
        private readonly Db _db;
        private readonly IServiceProvider _services;


        public ApiHub(Db db, IServiceProvider services)
        {
            _services = services;
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
            if (league.IsNullOrEmpty())
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

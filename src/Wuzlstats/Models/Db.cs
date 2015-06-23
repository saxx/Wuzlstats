using Microsoft.Data.Entity;

namespace Wuzlstats.Models
{
    public class Db : DbContext
    {
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForSqlServer().UseIdentity();

            modelBuilder.Entity<Game>().Reference(x => x.League).InverseCollection(x => x.Games).ForeignKey(x => x.LeagueId);
            modelBuilder.Entity<Player>().Reference(x => x.League).InverseCollection(x => x.Players).ForeignKey(x => x.LeagueId);
            modelBuilder.Entity<PlayerPosition>().Reference(x => x.Player).InverseCollection(x => x.Positions).ForeignKey(x => x.PlayerId);
            modelBuilder.Entity<PlayerPosition>().Reference(x => x.Game).InverseCollection(x => x.Positions).ForeignKey(x => x.GameId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
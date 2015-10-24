using Microsoft.Data.Entity;

namespace Wuzlstats.Models
{
    public sealed class Db : DbContext
    {
        public Db()
        {
            Database.Migrate();
        }

        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasOne(x => x.League).WithMany(x => x.Games).ForeignKey(x => x.LeagueId);
            modelBuilder.Entity<Player>().HasOne(x => x.League).WithMany(x => x.Players).ForeignKey(x => x.LeagueId);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Player).WithMany(x => x.Positions).ForeignKey(x => x.PlayerId);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Game).WithMany(x => x.Positions).ForeignKey(x => x.GameId);

            base.OnModelCreating(modelBuilder);
        }
    }
}

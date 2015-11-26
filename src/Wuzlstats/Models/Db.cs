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
            modelBuilder.Entity<Game>().HasOne(x => x.League).WithMany(x => x.Games).HasForeignKey(x => x.LeagueId);
            modelBuilder.Entity<Game>().Ignore(x => x.BlueWins);
            modelBuilder.Entity<Game>().Ignore(x => x.RedWins);
            modelBuilder.Entity<Player>().HasOne(x => x.League).WithMany(x => x.Players).HasForeignKey(x => x.LeagueId);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Player).WithMany(x => x.Positions).HasForeignKey(x => x.PlayerId);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Game).WithMany(x => x.Positions).HasForeignKey(x => x.GameId);
            modelBuilder.Entity<PlayerPosition>().Ignore(x => x.IsBluePosition);
            modelBuilder.Entity<PlayerPosition>().Ignore(x => x.IsRedPosition);

            base.OnModelCreating(modelBuilder);
        }
    }
}

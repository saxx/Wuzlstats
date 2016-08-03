using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Wuzlstats.Models
{
    public sealed class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options)
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
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Player).WithMany(x => x.Positions).HasForeignKey(x => x.PlayerId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Game).WithMany(x => x.Positions).HasForeignKey(x => x.GameId).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<PlayerPosition>().Ignore(x => x.IsBluePosition);
            modelBuilder.Entity<PlayerPosition>().Ignore(x => x.IsRedPosition);

            // keep table names as it was in RC1
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.DisplayName();
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

using Microsoft.EntityFrameworkCore;

namespace Wuzlstats.Models
{
    public class Db : DbContext
    {
        public DbSet<League> Leagues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<PlayerPosition> PlayerPositions { get; set; }

        public Db(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasOne(x => x.League).WithMany(x => x.Games);
            modelBuilder.Entity<Player>().HasOne(x => x.League).WithMany(x => x.Players);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Player).WithMany(x => x.Positions);
            modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Game).WithMany(x => x.Positions);

            base.OnModelCreating(modelBuilder);
        }
    }
}

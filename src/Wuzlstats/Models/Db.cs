using Microsoft.EntityFrameworkCore;

namespace Wuzlstats.Models;

public sealed class Db : DbContext
{
    public Db(DbContextOptions options) : base(options)
    {
    }

    public DbSet<League> Leagues { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<PlayerPosition> PlayerPositions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>().HasOne(x => x.League).WithMany(x => x.Games);
        modelBuilder.Entity<Game>().Ignore(x => x.BlueWins);
        modelBuilder.Entity<Game>().Ignore(x => x.RedWins);
        modelBuilder.Entity<Player>().HasOne(x => x.League).WithMany(x => x.Players);
        modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Player).WithMany(x => x.Positions).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PlayerPosition>().HasOne(x => x.Game).WithMany(x => x.Positions).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<PlayerPosition>().Ignore(x => x.IsBluePosition);
        modelBuilder.Entity<PlayerPosition>().Ignore(x => x.IsRedPosition);

        base.OnModelCreating(modelBuilder);
    }
}

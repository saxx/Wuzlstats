using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using Wuzlstats.Models;

namespace Wuzlstats.Migrations
{
    [DbContext(typeof(Db))]
    partial class DbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-beta8-15964")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Wuzlstats.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlueScore");

                    b.Property<DateTime>("Date");

                    b.Property<int>("LeagueId");

                    b.Property<int>("RedScore");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Wuzlstats.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("TimeoutConfiguration");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Image");

                    b.Property<int>("LeagueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("Position");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("Wuzlstats.Models.Game", b =>
                {
                    b.HasOne("Wuzlstats.Models.League")
                        .WithMany()
                        .HasForeignKey("LeagueId");
                });

            modelBuilder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.HasOne("Wuzlstats.Models.League")
                        .WithMany()
                        .HasForeignKey("LeagueId");
                });

            modelBuilder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.HasOne("Wuzlstats.Models.Game")
                        .WithMany()
                        .HasForeignKey("GameId");

                    b.HasOne("Wuzlstats.Models.Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");
                });
        }
    }
}

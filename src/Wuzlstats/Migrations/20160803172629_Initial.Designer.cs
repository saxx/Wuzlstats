using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Wuzlstats.Models;

namespace Wuzlstats.Migrations
{
    [DbContext(typeof(Db))]
    [Migration("20160803172629_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
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

                    b.HasIndex("LeagueId");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("Wuzlstats.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("TimeoutConfiguration");

                    b.HasKey("Id");

                    b.ToTable("League");
                });

            modelBuilder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Image");

                    b.Property<int>("LeagueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("Position");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerPosition");
                });

            modelBuilder.Entity("Wuzlstats.Models.Game", b =>
                {
                    b.HasOne("Wuzlstats.Models.League", "League")
                        .WithMany("Games")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.HasOne("Wuzlstats.Models.League", "League")
                        .WithMany("Players")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.HasOne("Wuzlstats.Models.Game", "Game")
                        .WithMany("Positions")
                        .HasForeignKey("GameId");

                    b.HasOne("Wuzlstats.Models.Player", "Player")
                        .WithMany("Positions")
                        .HasForeignKey("PlayerId");
                });
        }
    }
}

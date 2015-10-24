using System;
using Microsoft.Data.Entity;
using Wuzlstats.Models;
using Microsoft.Data.Entity.Infrastructure;

namespace Wuzlstats.Migrations
{
    [DbContext(typeof(Db))]
    partial class DbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder builder)
        {
            builder
                .Annotation("ProductVersion", "7.0.0-beta6-13815")
                .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn");

            builder.Entity("Wuzlstats.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BlueScore");

                    b.Property<DateTime>("Date");

                    b.Property<int>("LeagueId");

                    b.Property<int>("RedScore");

                    b.HasKey("Id");
                });

            builder.Entity("Wuzlstats.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("TimeoutConfiguration");

                    b.HasKey("Id");
                });

            builder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Image");

                    b.Property<int>("LeagueId");

                    b.Property<string>("Name");

                    b.HasKey("Id");
                });

            builder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("Position");

                    b.HasKey("Id");
                });

            builder.Entity("Wuzlstats.Models.Game", b =>
                {
                    b.HasOne("Wuzlstats.Models.League")
                        .WithMany()
                        .ForeignKey("LeagueId");
                });

            builder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.HasOne("Wuzlstats.Models.League")
                        .WithMany()
                        .ForeignKey("LeagueId");
                });

            builder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.HasOne("Wuzlstats.Models.Game")
                        .WithMany()
                        .ForeignKey("GameId");

                    b.HasOne("Wuzlstats.Models.Player")
                        .WithMany()
                        .ForeignKey("PlayerId");
                });
        }
    }
}

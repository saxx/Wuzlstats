using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations.Infrastructure;
using Wuzlstats.Models;

namespace WuzlstatsMigrations
{
    [ContextType(typeof(Db))]
    partial class DbModelSnapshot : ModelSnapshot
    {
        public override void BuildModel(ModelBuilder builder)
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

                    b.Key("Id");
                });

            builder.Entity("Wuzlstats.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("TimeoutConfiguration");

                    b.Key("Id");
                });

            builder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Image");

                    b.Property<int>("LeagueId");

                    b.Property<string>("Name");

                    b.Key("Id");
                });

            builder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<int>("PlayerId");

                    b.Property<int>("Position");

                    b.Key("Id");
                });

            builder.Entity("Wuzlstats.Models.Game", b =>
                {
                    b.Reference("Wuzlstats.Models.League")
                        .InverseCollection()
                        .ForeignKey("LeagueId");
                });

            builder.Entity("Wuzlstats.Models.Player", b =>
                {
                    b.Reference("Wuzlstats.Models.League")
                        .InverseCollection()
                        .ForeignKey("LeagueId");
                });

            builder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                {
                    b.Reference("Wuzlstats.Models.Game")
                        .InverseCollection()
                        .ForeignKey("GameId");

                    b.Reference("Wuzlstats.Models.Player")
                        .InverseCollection()
                        .ForeignKey("PlayerId");
                });
        }
    }
}

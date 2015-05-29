using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Metadata.Builders;
using Microsoft.Data.Entity.Relational.Migrations.Infrastructure;
using Wuzlstats.Models;

namespace Wuzlstats.Migrations
{
    [ContextType(typeof(Db))]
    partial class DbModelSnapshot : ModelSnapshot
    {
        public override IModel Model
        {
            get
            {
                var builder = new BasicModelBuilder()
                    .Annotation("SqlServer:ValueGeneration", "Identity");
                
                builder.Entity("Wuzlstats.Models.Game", b =>
                    {
                        b.Property<int>("BlueScore")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<DateTime>("Date")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 2)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<int>("LeagueId")
                            .Annotation("OriginalValueIndex", 3);
                        b.Property<int>("RedScore")
                            .Annotation("OriginalValueIndex", 4);
                        b.Key("Id");
                    });
                
                builder.Entity("Wuzlstats.Models.League", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 1);
                        b.Key("Id");
                    });
                
                builder.Entity("Wuzlstats.Models.Player", b =>
                    {
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 0)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<byte[]>("Image")
                            .Annotation("OriginalValueIndex", 1);
                        b.Property<int>("LeagueId")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<string>("Name")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("Id");
                    });
                
                builder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                    {
                        b.Property<int>("GameId")
                            .Annotation("OriginalValueIndex", 0);
                        b.Property<int>("Id")
                            .GenerateValueOnAdd()
                            .Annotation("OriginalValueIndex", 1)
                            .Annotation("SqlServer:ValueGeneration", "Default");
                        b.Property<int>("PlayerId")
                            .Annotation("OriginalValueIndex", 2);
                        b.Property<int>("Position")
                            .Annotation("OriginalValueIndex", 3);
                        b.Key("Id");
                    });
                
                builder.Entity("Wuzlstats.Models.Game", b =>
                    {
                        b.ForeignKey("Wuzlstats.Models.League", "LeagueId");
                    });
                
                builder.Entity("Wuzlstats.Models.Player", b =>
                    {
                        b.ForeignKey("Wuzlstats.Models.League", "LeagueId");
                    });
                
                builder.Entity("Wuzlstats.Models.PlayerPosition", b =>
                    {
                        b.ForeignKey("Wuzlstats.Models.Game", "GameId");
                        b.ForeignKey("Wuzlstats.Models.Player", "PlayerId");
                    });
                
                return builder.Model;
            }
        }
    }
}

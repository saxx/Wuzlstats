using Microsoft.Data.Entity.Migrations;
using System;

namespace Wuzlstats.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "League",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeoutConfiguration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_League", x => x.Id);
                });
            migration.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn"),
                    BlueScore = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    RedScore = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_League_LeagueId",
                        columns: x => x.LeagueId,
                        principalTable: "League",
                        principalColumns: new[] { "Id" });
        });
            migration.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn"),
                    Image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_League_LeagueId",
                        columns: x => x.LeagueId,
                        principalTable: "League",
                        principalColumns: new[] { "Id" });
                });
            migration.CreateTable(
                name: "PlayerPosition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", "IdentityColumn"),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerPosition_Game_GameId",
                        columns: x => x.GameId,
                        principalTable: "Game",
                        principalColumns: new[] { "Id" });
                    table.ForeignKey(
                        name: "FK_PlayerPosition_Player_PlayerId",
                        columns: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumns: new[] { "Id" });
                });
        }

        protected override void Down(MigrationBuilder migration)
        {
            migration.DropTable("PlayerPosition");
            migration.DropTable("Game");
            migration.DropTable("Player");
            migration.DropTable("League");
        }
    }
}

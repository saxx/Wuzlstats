using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;

namespace Wuzlstats.Migrations
{
    public partial class Initial : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.CreateTable(
                name: "League",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Name = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_League", x => x.Id); });
            migration.CreateTable(
                name: "Game",
                columns: table => new
                {
                    BlueScore = table.Column(type: "int", nullable: false),
                    Date = table.Column(type: "datetime2", nullable: false),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    LeagueId = table.Column(type: "int", nullable: false),
                    RedScore = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_League_LeagueId",
                        columns: x => x.LeagueId,
                        referencedTable: "League",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    Image = table.Column(type: "varbinary(max)", nullable: true),
                    LeagueId = table.Column(type: "int", nullable: false),
                    Name = table.Column(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_League_LeagueId",
                        columns: x => x.LeagueId,
                        referencedTable: "League",
                        referencedColumn: "Id");
                });
            migration.CreateTable(
                name: "PlayerPosition",
                columns: table => new
                {
                    GameId = table.Column(type: "int", nullable: false),
                    Id = table.Column(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGeneration", "Identity"),
                    PlayerId = table.Column(type: "int", nullable: false),
                    Position = table.Column(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerPosition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerPosition_Game_GameId",
                        columns: x => x.GameId,
                        referencedTable: "Game",
                        referencedColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PlayerPosition_Player_PlayerId",
                        columns: x => x.PlayerId,
                        referencedTable: "Player",
                        referencedColumn: "Id");
                });
        }


        public override void Down(MigrationBuilder migration)
        {
            migration.DropTable("Game");
            migration.DropTable("League");
            migration.DropTable("Player");
            migration.DropTable("PlayerPosition");
        }
    }
}
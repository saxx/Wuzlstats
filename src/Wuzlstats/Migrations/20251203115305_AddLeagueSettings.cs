using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wuzlstats.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BannerImageUrl",
                table: "Leagues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysForStatistics",
                table: "Leagues",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Leagues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxScore",
                table: "Leagues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 10);

            migrationBuilder.AddColumn<int>(
                name: "MinimumGamesForRanking",
                table: "Leagues",
                type: "INTEGER",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<string>(
                name: "PasswordHash",
                table: "Leagues",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowPlayerRankings",
                table: "Leagues",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "ShowTeamRankings",
                table: "Leagues",
                type: "INTEGER",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamBlueColor",
                table: "Leagues",
                type: "TEXT",
                nullable: false,
                defaultValue: "#ddeeff");

            migrationBuilder.AddColumn<string>(
                name: "TeamRedColor",
                table: "Leagues",
                type: "TEXT",
                nullable: false,
                defaultValue: "#ffddee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerImageUrl",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "DaysForStatistics",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "MaxScore",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "MinimumGamesForRanking",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "PasswordHash",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "ShowPlayerRankings",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "ShowTeamRankings",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "TeamBlueColor",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "TeamRedColor",
                table: "Leagues");
        }
    }
}

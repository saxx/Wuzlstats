using System.Collections.Generic;
using Microsoft.Data.Entity.Migrations;
using Microsoft.Data.Entity.Migrations.Builders;
using Microsoft.Data.Entity.Migrations.Operations;

namespace Wuzlstats.Migrations
{
    public partial class Timeouts : Migration
    {
        public override void Up(MigrationBuilder migration)
        {
            migration.AddColumn("TimeoutConfiguration", "League", type: "nvarchar(max)", nullable: false);
        }

        public override void Down(MigrationBuilder migration)
        {
            migration.DropColumn("TimeoutConfiguration", "League");
        }
    }
}

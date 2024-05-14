using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class edituser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommunityId",
                table: "users",
                newName: "precommunity_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "precommunity_id",
                table: "users",
                newName: "CommunityId");
        }
    }
}

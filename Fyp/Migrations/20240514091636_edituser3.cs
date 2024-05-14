using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class edituser3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_pre_communities_PreCommunityId",
                table: "users");

            migrationBuilder.DropColumn(
                name: "precommunity_id",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "PreCommunityId",
                table: "users",
                newName: "PrecommunityId");

            migrationBuilder.RenameIndex(
                name: "IX_users_PreCommunityId",
                table: "users",
                newName: "IX_users_PrecommunityId");

            migrationBuilder.AddForeignKey(
                name: "FK_users_pre_communities_PrecommunityId",
                table: "users",
                column: "PrecommunityId",
                principalTable: "pre_communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_users_pre_communities_PrecommunityId",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "PrecommunityId",
                table: "users",
                newName: "PreCommunityId");

            migrationBuilder.RenameIndex(
                name: "IX_users_PrecommunityId",
                table: "users",
                newName: "IX_users_PreCommunityId");

            migrationBuilder.AddColumn<int>(
                name: "precommunity_id",
                table: "users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_users_pre_communities_PreCommunityId",
                table: "users",
                column: "PreCommunityId",
                principalTable: "pre_communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

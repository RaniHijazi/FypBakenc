using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class editcommenttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "comments");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "comments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "comments");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "comments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

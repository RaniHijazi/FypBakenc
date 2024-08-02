using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class PointsMigration6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPostPointsAwarded",
                table: "posts");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPostPointsAwarded",
                table: "users",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPostPointsAwarded",
                table: "users");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastPostPointsAwarded",
                table: "posts",
                type: "datetime2",
                nullable: true);
        }
    }
}

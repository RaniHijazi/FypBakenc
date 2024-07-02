using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class Chatroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "chat_rooms",
                newName: "ProfilePath");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfCreate",
                table: "chat_rooms",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateOfCreate",
                table: "chat_rooms");

            migrationBuilder.RenameColumn(
                name: "ProfilePath",
                table: "chat_rooms",
                newName: "Description");
        }
    }
}

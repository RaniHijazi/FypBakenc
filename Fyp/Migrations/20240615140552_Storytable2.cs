using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class Storytable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_Story_StoryId",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_users_UserId",
                table: "Story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Story",
                table: "Story");

            migrationBuilder.RenameTable(
                name: "Story",
                newName: "stories");

            migrationBuilder.RenameIndex(
                name: "IX_Story_UserId",
                table: "stories",
                newName: "IX_stories_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stories",
                table: "stories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_stories_StoryId",
                table: "messages",
                column: "StoryId",
                principalTable: "stories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_stories_users_UserId",
                table: "stories",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_messages_stories_StoryId",
                table: "messages");

            migrationBuilder.DropForeignKey(
                name: "FK_stories_users_UserId",
                table: "stories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stories",
                table: "stories");

            migrationBuilder.RenameTable(
                name: "stories",
                newName: "Story");

            migrationBuilder.RenameIndex(
                name: "IX_stories_UserId",
                table: "Story",
                newName: "IX_Story_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Story",
                table: "Story",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_messages_Story_StoryId",
                table: "messages",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Story_users_UserId",
                table: "Story",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

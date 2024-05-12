using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_users_UsersId",
                table: "documents");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "documents",
                newName: "UserId1");

            migrationBuilder.RenameIndex(
                name: "IX_documents_UsersId",
                table: "documents",
                newName: "IX_documents_UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_users_UserId1",
                table: "documents",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_users_UserId1",
                table: "documents");

            migrationBuilder.RenameColumn(
                name: "UserId1",
                table: "documents",
                newName: "UsersId");

            migrationBuilder.RenameIndex(
                name: "IX_documents_UserId1",
                table: "documents",
                newName: "IX_documents_UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_users_UsersId",
                table: "documents",
                column: "UsersId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class edit2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_users_UserId1",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "IX_documents_UserId1",
                table: "documents");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "documents");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "documents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_documents_UserId",
                table: "documents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_users_UserId",
                table: "documents",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_documents_users_UserId",
                table: "documents");

            migrationBuilder.DropIndex(
                name: "IX_documents_UserId",
                table: "documents");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "documents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "documents",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_documents_UserId1",
                table: "documents",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_documents_users_UserId1",
                table: "documents",
                column: "UserId1",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

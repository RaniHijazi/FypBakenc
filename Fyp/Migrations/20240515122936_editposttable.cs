using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
    public partial class editposttable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_pe_sub_communities_pre_communities_PreCommunityID",
                table: "pe_sub_communities");

            migrationBuilder.DropForeignKey(
                name: "FK_posts_pe_sub_communities_PreSubCommunityId",
                table: "posts");

            migrationBuilder.DropForeignKey(
                name: "FK_user_sub_communities_pe_sub_communities_SubCommunityId",
                table: "user_sub_communities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pe_sub_communities",
                table: "pe_sub_communities");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "posts");

            migrationBuilder.RenameTable(
                name: "pe_sub_communities",
                newName: "pre_sub_communities");

            migrationBuilder.RenameIndex(
                name: "IX_pe_sub_communities_PreCommunityID",
                table: "pre_sub_communities",
                newName: "IX_pre_sub_communities_PreCommunityID");

            migrationBuilder.AddColumn<int>(
                name: "CommentsCount",
                table: "posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LikesCount",
                table: "posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShareCount",
                table: "posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "posts",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_pre_sub_communities",
                table: "pre_sub_communities",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_posts_pre_sub_communities_PreSubCommunityId",
                table: "posts",
                column: "PreSubCommunityId",
                principalTable: "pre_sub_communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_pre_sub_communities_pre_communities_PreCommunityID",
                table: "pre_sub_communities",
                column: "PreCommunityID",
                principalTable: "pre_communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_user_sub_communities_pre_sub_communities_SubCommunityId",
                table: "user_sub_communities",
                column: "SubCommunityId",
                principalTable: "pre_sub_communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_posts_pre_sub_communities_PreSubCommunityId",
                table: "posts");

            migrationBuilder.DropForeignKey(
                name: "FK_pre_sub_communities_pre_communities_PreCommunityID",
                table: "pre_sub_communities");

            migrationBuilder.DropForeignKey(
                name: "FK_user_sub_communities_pre_sub_communities_SubCommunityId",
                table: "user_sub_communities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pre_sub_communities",
                table: "pre_sub_communities");

            migrationBuilder.DropColumn(
                name: "CommentsCount",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "LikesCount",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "ShareCount",
                table: "posts");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "posts");

            migrationBuilder.RenameTable(
                name: "pre_sub_communities",
                newName: "pe_sub_communities");

            migrationBuilder.RenameIndex(
                name: "IX_pre_sub_communities_PreCommunityID",
                table: "pe_sub_communities",
                newName: "IX_pe_sub_communities_PreCommunityID");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pe_sub_communities",
                table: "pe_sub_communities",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_pe_sub_communities_pre_communities_PreCommunityID",
                table: "pe_sub_communities",
                column: "PreCommunityID",
                principalTable: "pre_communities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_posts_pe_sub_communities_PreSubCommunityId",
                table: "posts",
                column: "PreSubCommunityId",
                principalTable: "pe_sub_communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_user_sub_communities_pe_sub_communities_SubCommunityId",
                table: "user_sub_communities",
                column: "SubCommunityId",
                principalTable: "pe_sub_communities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

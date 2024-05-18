using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fyp.Migrations
{
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
    public partial class InitialCreate : Migration
========
    public partial class init : Migration
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chat_rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chat_rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "communities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_communities", x => x.Id);
                });

            migrationBuilder.CreateTable(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                name: "sub_communities",
========
                name: "pre_sub_communities",
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunityID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                    table.PrimaryKey("PK_sub_communities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_sub_communities_communities_CommunityID",
                        column: x => x.CommunityID,
                        principalTable: "communities",
========
                    table.PrimaryKey("PK_pre_sub_communities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_pre_sub_communities_pre_communities_PreCommunityID",
                        column: x => x.PreCommunityID,
                        principalTable: "pre_communities",
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    School = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VerificationCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                    CommunityId = table.Column<int>(type: "int", nullable: false)
========
                    PrecommunityId = table.Column<int>(type: "int", nullable: false)
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                        name: "FK_users_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
========
                        name: "FK_users_pre_communities_PrecommunityId",
                        column: x => x.PrecommunityId,
                        principalTable: "pre_communities",
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "documents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UploadDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_documents_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    MessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_messages_chat_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "chat_rooms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_messages_users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messages_users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_messages_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    CommentsCount = table.Column<int>(type: "int", nullable: false),
                    ShareCount = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                    CommunityId = table.Column<int>(type: "int", nullable: true),
                    SubCommunityId = table.Column<int>(type: "int", nullable: true)
========
                    PreCommunityId = table.Column<int>(type: "int", nullable: true),
                    PreSubCommunityId = table.Column<int>(type: "int", nullable: true)
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_posts", x => x.Id);
                    table.ForeignKey(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                        name: "FK_posts_communities_CommunityId",
                        column: x => x.CommunityId,
                        principalTable: "communities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_posts_sub_communities_SubCommunityId",
                        column: x => x.SubCommunityId,
                        principalTable: "sub_communities",
                        principalColumn: "ID",
========
                        name: "FK_posts_pre_communities_PreCommunityId",
                        column: x => x.PreCommunityId,
                        principalTable: "pre_communities",
                        principalColumn: "Id",
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_posts_pre_sub_communities_PreSubCommunityId",
                        column: x => x.PreSubCommunityId,
                        principalTable: "pre_sub_communities",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_posts_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_chat_rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_chat_rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_user_chat_rooms_chat_rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "chat_rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_chat_rooms_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_sub_communities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SubCommunityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_sub_communities", x => x.Id);
                    table.ForeignKey(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                        name: "FK_user_sub_communities_sub_communities_SubCommunityId",
                        column: x => x.SubCommunityId,
                        principalTable: "sub_communities",
========
                        name: "FK_user_sub_communities_pre_sub_communities_SubCommunityId",
                        column: x => x.SubCommunityId,
                        principalTable: "pre_sub_communities",
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_sub_communities_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LikesCount = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_comments_posts_PostId",
                        column: x => x.PostId,
                        principalTable: "posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_comments_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_likes_comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_likes_posts_PostId",
                        column: x => x.PostId,
                        principalTable: "posts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_likes_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_comments_PostId",
                table: "comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_UserId",
                table: "comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_documents_UserId",
                table: "documents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_CommentId",
                table: "likes",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_PostId",
                table: "likes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_UserId",
                table: "likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_RecipientId",
                table: "messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_RoomId",
                table: "messages",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_SenderId",
                table: "messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_messages_UserId",
                table: "messages",
                column: "UserId");

            migrationBuilder.CreateIndex(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                name: "IX_posts_CommunityId",
                table: "posts",
                column: "CommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_posts_SubCommunityId",
========
                name: "IX_posts_PreCommunityId",
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
                table: "posts",
                column: "SubCommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_posts_UserId",
                table: "posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                name: "IX_sub_communities_CommunityID",
                table: "sub_communities",
                column: "CommunityID");
========
                name: "IX_pre_sub_communities_PreCommunityID",
                table: "pre_sub_communities",
                column: "PreCommunityID");
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs

            migrationBuilder.CreateIndex(
                name: "IX_user_chat_rooms_RoomId",
                table: "user_chat_rooms",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_user_chat_rooms_UserId",
                table: "user_chat_rooms",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_user_sub_communities_SubCommunityId",
                table: "user_sub_communities",
                column: "SubCommunityId");

            migrationBuilder.CreateIndex(
                name: "IX_user_sub_communities_UserId",
                table: "user_sub_communities",
                column: "UserId");

            migrationBuilder.CreateIndex(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                name: "IX_users_CommunityId",
                table: "users",
                column: "CommunityId");
========
                name: "IX_users_PrecommunityId",
                table: "users",
                column: "PrecommunityId");
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "documents");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "user_chat_rooms");

            migrationBuilder.DropTable(
                name: "user_sub_communities");

            migrationBuilder.DropTable(
                name: "comments");

            migrationBuilder.DropTable(
                name: "chat_rooms");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
<<<<<<<< Updated upstream:Fyp/Migrations/20240517160217_InitialCreate.cs
                name: "sub_communities");
========
                name: "pre_sub_communities");
>>>>>>>> Stashed changes:Fyp/Migrations/20240517130910_init.cs

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "communities");
        }
    }
}

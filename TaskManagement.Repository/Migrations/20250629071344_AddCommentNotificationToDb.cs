using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentNotificationToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommentNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentNotifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentNotifications_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentNotifications_TaskComments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "TaskComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskUsers_UserId",
                table: "TaskUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentNotifications_CommentId",
                table: "CommentNotifications",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentNotifications_NotificationId",
                table: "CommentNotifications",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUsers_AspNetUsers_UserId",
                table: "TaskUsers",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskUsers_AspNetUsers_UserId",
                table: "TaskUsers");

            migrationBuilder.DropTable(
                name: "CommentNotifications");

            migrationBuilder.DropIndex(
                name: "IX_TaskUsers_UserId",
                table: "TaskUsers");
        }
    }
}

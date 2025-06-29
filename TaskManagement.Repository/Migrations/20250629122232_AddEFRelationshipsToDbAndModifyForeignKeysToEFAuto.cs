using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddEFRelationshipsToDbAndModifyForeignKeysToEFAuto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_AssigneeId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_TaskOwnerId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropTable(
                name: "CommentNotifications");

            migrationBuilder.DropTable(
                name: "TaskUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Id",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationUserId",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppTaskAssignment",
                columns: table => new
                {
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTaskAssignment", x => new { x.TaskId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AppTaskAssignment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppTaskAssignment_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseNotificationTaskComment",
                columns: table => new
                {
                    NotificationsId = table.Column<int>(type: "int", nullable: false),
                    TaskCommentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseNotificationTaskComment", x => new { x.NotificationsId, x.TaskCommentsId });
                    table.ForeignKey(
                        name: "FK_BaseNotificationTaskComment_Notifications_NotificationsId",
                        column: x => x.NotificationsId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BaseNotificationTaskComment_TaskComments_TaskCommentsId",
                        column: x => x.TaskCommentsId,
                        principalTable: "TaskComments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_UserId",
                table: "TaskComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentRequests_RequestNotificationId",
                table: "TaskAssignmentRequests",
                column: "RequestNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentRequests_TaskId",
                table: "TaskAssignmentRequests",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTaskAssignment_UserId",
                table: "AppTaskAssignment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseNotificationTaskComment_TaskCommentsId",
                table: "BaseNotificationTaskComment",
                column: "TaskCommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                table: "Notifications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_AssigneeId",
                table: "TaskAssignmentRequests",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_TaskOwnerId",
                table: "TaskAssignmentRequests",
                column: "TaskOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_Notifications_RequestNotificationId",
                table: "TaskAssignmentRequests",
                column: "RequestNotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_Tasks_TaskId",
                table: "TaskAssignmentRequests",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_AspNetUsers_UserId",
                table: "TaskComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_AspNetUsers_OwnerId",
                table: "Tasks",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_ApplicationUserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_AssigneeId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_TaskOwnerId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_Notifications_RequestNotificationId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_Tasks_TaskId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_AspNetUsers_UserId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_AspNetUsers_OwnerId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "AppTaskAssignment");

            migrationBuilder.DropTable(
                name: "BaseNotificationTaskComment");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_OwnerId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_UserId",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignmentRequests_RequestNotificationId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignmentRequests_TaskId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_ApplicationUserId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Notifications");

            migrationBuilder.CreateTable(
                name: "CommentNotifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommentId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    NotificationId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "TaskUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskUsers_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Id",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CommentNotifications_CommentId",
                table: "CommentNotifications",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentNotifications_NotificationId",
                table: "CommentNotifications",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskUsers_TaskId",
                table: "TaskUsers",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskUsers_UserId",
                table: "TaskUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_AssigneeId",
                table: "TaskAssignmentRequests",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_TaskOwnerId",
                table: "TaskAssignmentRequests",
                column: "TaskOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

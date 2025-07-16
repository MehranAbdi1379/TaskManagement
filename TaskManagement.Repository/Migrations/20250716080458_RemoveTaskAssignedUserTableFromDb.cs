using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTaskAssignedUserTableFromDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_TaskOwnerId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropTable(
                name: "AppTaskAssignment");

            migrationBuilder.DropIndex(
                name: "IX_TaskAssignmentRequests_TaskOwnerId",
                table: "TaskAssignmentRequests");

            migrationBuilder.DropColumn(
                name: "TaskOwnerId",
                table: "TaskAssignmentRequests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskOwnerId",
                table: "TaskAssignmentRequests",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "IX_TaskAssignmentRequests_TaskOwnerId",
                table: "TaskAssignmentRequests",
                column: "TaskOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTaskAssignment_UserId",
                table: "AppTaskAssignment",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAssignmentRequests_AspNetUsers_TaskOwnerId",
                table: "TaskAssignmentRequests",
                column: "TaskOwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

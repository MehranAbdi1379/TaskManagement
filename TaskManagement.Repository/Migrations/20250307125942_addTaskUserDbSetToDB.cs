using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagement.Repository.Migrations
{
    /// <inheritdoc />
    public partial class addTaskUserDbSetToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTaskUser_Tasks_TaskId",
                table: "AppTaskUser");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComment_Tasks_TaskId",
                table: "TaskComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskComment",
                table: "TaskComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppTaskUser",
                table: "AppTaskUser");

            migrationBuilder.RenameTable(
                name: "TaskComment",
                newName: "TaskComments");

            migrationBuilder.RenameTable(
                name: "AppTaskUser",
                newName: "TaskUsers");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComment_TaskId",
                table: "TaskComments",
                newName: "IX_TaskComments_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_AppTaskUser_TaskId",
                table: "TaskUsers",
                newName: "IX_TaskUsers_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskComments",
                table: "TaskComments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskUsers",
                table: "TaskUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskUsers_Tasks_TaskId",
                table: "TaskUsers",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Tasks_TaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskUsers_Tasks_TaskId",
                table: "TaskUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskUsers",
                table: "TaskUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskComments",
                table: "TaskComments");

            migrationBuilder.RenameTable(
                name: "TaskUsers",
                newName: "AppTaskUser");

            migrationBuilder.RenameTable(
                name: "TaskComments",
                newName: "TaskComment");

            migrationBuilder.RenameIndex(
                name: "IX_TaskUsers_TaskId",
                table: "AppTaskUser",
                newName: "IX_AppTaskUser_TaskId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskComments_TaskId",
                table: "TaskComment",
                newName: "IX_TaskComment_TaskId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppTaskUser",
                table: "AppTaskUser",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskComment",
                table: "TaskComment",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTaskUser_Tasks_TaskId",
                table: "AppTaskUser",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComment_Tasks_TaskId",
                table: "TaskComment",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

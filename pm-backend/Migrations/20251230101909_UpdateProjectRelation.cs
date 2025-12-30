using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pm_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProjectRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "TaskStateHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskStateHistories_ProjectTaskId",
                table: "TaskStateHistories",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStateHistories_ProjectTasks_ProjectTaskId",
                table: "TaskStateHistories",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStateHistories_ProjectTasks_ProjectTaskId",
                table: "TaskStateHistories");

            migrationBuilder.DropIndex(
                name: "IX_TaskStateHistories_ProjectTaskId",
                table: "TaskStateHistories");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "TaskStateHistories");
        }
    }
}

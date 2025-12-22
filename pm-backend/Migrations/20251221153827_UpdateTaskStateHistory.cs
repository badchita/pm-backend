using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pm_backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskStateHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskStateHistories_ProjectTasks_TaskId",
                table: "TaskStateHistories");

            migrationBuilder.DropIndex(
                name: "IX_TaskStateHistories_TaskId",
                table: "TaskStateHistories");

            migrationBuilder.AlterColumn<int>(
                name: "PreviousState",
                table: "TaskStateHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PreviousState",
                table: "TaskStateHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskStateHistories_TaskId",
                table: "TaskStateHistories",
                column: "TaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskStateHistories_ProjectTasks_TaskId",
                table: "TaskStateHistories",
                column: "TaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

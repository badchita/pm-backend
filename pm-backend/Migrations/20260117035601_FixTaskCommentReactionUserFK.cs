using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pm_backend.Migrations
{
    /// <inheritdoc />
    public partial class FixTaskCommentReactionUserFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskCommentReactions_Users_UserId1",
                table: "TaskCommentReactions");

            migrationBuilder.DropIndex(
                name: "IX_TaskCommentReactions_UserId1",
                table: "TaskCommentReactions");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TaskCommentReactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TaskCommentReactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaskCommentReactions_UserId1",
                table: "TaskCommentReactions",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskCommentReactions_Users_UserId1",
                table: "TaskCommentReactions",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}

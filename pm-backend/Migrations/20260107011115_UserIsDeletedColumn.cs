using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pm_backend.Migrations
{
    /// <inheritdoc />
    public partial class UserIsDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IsDeleted",
                table: "Users",
                type: "char(1)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Users");
        }
    }
}

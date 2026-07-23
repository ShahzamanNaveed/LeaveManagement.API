using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveMangement.API.Migrations
{
    /// <inheritdoc />
    public partial class IsManagerToEmployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "Employees",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "Employees");
        }
    }
}

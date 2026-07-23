using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveMangement.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeaveBalanceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalDays",
                table: "LeaveBalances");

            migrationBuilder.RenameColumn(
                name: "RemainingDays",
                table: "LeaveBalances",
                newName: "TotalBalance");

            migrationBuilder.AddColumn<double>(
                name: "ConsumedBalance",
                table: "LeaveBalances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "RemainingBalance",
                table: "LeaveBalances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumedBalance",
                table: "LeaveBalances");

            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "LeaveBalances");

            migrationBuilder.RenameColumn(
                name: "TotalBalance",
                table: "LeaveBalances",
                newName: "RemainingDays");

            migrationBuilder.AddColumn<int>(
                name: "TotalDays",
                table: "LeaveBalances",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

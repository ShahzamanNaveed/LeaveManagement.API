using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LeaveMangement.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFiscalYearManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            // =====================================================
            // 1. Add new FiscalYearId column
            // =====================================================

            migrationBuilder.AddColumn<int>(
                name: "FiscalYearId",
                table: "LeaveBalances",
                type: "int",
                nullable: false,
                defaultValue: 0);



            // =====================================================
            // 2. Create FiscalYears table
            // =====================================================

            migrationBuilder.CreateTable(
                name: "FiscalYears",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation(
                            "SqlServer:Identity",
                            "1, 1"),

                    Name = table.Column<string>(
                        type: "nvarchar(450)",
                        nullable: false),

                    StartDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    EndDate = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    IsActive = table.Column<bool>(
                        type: "bit",
                        nullable: false),

                    CreatedAt = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_FiscalYears",
                        x => x.Id);
                });



            // =====================================================
            // 3. Insert Active Fiscal Year
            // =====================================================

            migrationBuilder.InsertData(
                table: "FiscalYears",
                columns: new[]
                {
                    "Name",
                    "StartDate",
                    "EndDate",
                    "IsActive",
                    "CreatedAt"
                },
                values: new object[]
                {
                    "FY-2026",
                    new DateTime(2026, 1, 1),
                    new DateTime(2026, 12, 31),
                    true,
                    DateTime.UtcNow
                });



            // =====================================================
            // 4. Create FiscalYearSettings table
            // =====================================================

            migrationBuilder.CreateTable(
                name: "FiscalYearSettings",
                columns: table => new
                {
                    Id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation(
                            "SqlServer:Identity",
                            "1, 1"),

                    StartMonth = table.Column<int>(
                        type: "int",
                        nullable: false),

                    StartDay = table.Column<int>(
                        type: "int",
                        nullable: false),

                    CreatedAt = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_FiscalYearSettings",
                        x => x.Id);
                });



            // =====================================================
            // 5. Assign existing LeaveBalances to FY-2026
            // =====================================================

            migrationBuilder.Sql(
            @"
                UPDATE LeaveBalances
                SET FiscalYearId = 1;
            ");



            // =====================================================
            // 6. Create Index
            // =====================================================

            migrationBuilder.CreateIndex(
                name: "IX_LeaveBalances_FiscalYearId",
                table: "LeaveBalances",
                column: "FiscalYearId");



            migrationBuilder.CreateIndex(
                name: "IX_FiscalYears_Name",
                table: "FiscalYears",
                column: "Name",
                unique: true);



            migrationBuilder.CreateIndex(
                name: "IX_FiscalYearSettings_Id",
                table: "FiscalYearSettings",
                column: "Id",
                unique: true);



            // =====================================================
            // 7. Add Foreign Key
            // =====================================================

            migrationBuilder.AddForeignKey(
                name: "FK_LeaveBalances_FiscalYears_FiscalYearId",
                table: "LeaveBalances",
                column: "FiscalYearId",
                principalTable: "FiscalYears",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);



            // =====================================================
            // 8. Remove old Year column
            // =====================================================

            migrationBuilder.DropColumn(
                name: "Year",
                table: "LeaveBalances");

        }



        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "LeaveBalances",
                type: "int",
                nullable: false,
                defaultValue: 0);



            migrationBuilder.DropForeignKey(
                name: "FK_LeaveBalances_FiscalYears_FiscalYearId",
                table: "LeaveBalances");



            migrationBuilder.DropIndex(
                name: "IX_LeaveBalances_FiscalYearId",
                table: "LeaveBalances");



            migrationBuilder.DropTable(
                name: "FiscalYearSettings");



            migrationBuilder.DropTable(
                name: "FiscalYears");



            migrationBuilder.DropColumn(
                name: "FiscalYearId",
                table: "LeaveBalances");

        }
    }
}
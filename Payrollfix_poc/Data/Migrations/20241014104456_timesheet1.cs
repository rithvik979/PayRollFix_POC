using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class timesheet1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Employee_EmployeeId1",
                table: "TimeSheets");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheets_EmployeeId1",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                table: "TimeSheets");

            migrationBuilder.AlterColumn<int>(
                name: "EmployeeId",
                table: "TimeSheets",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_EmployeeId",
                table: "TimeSheets",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Employee_EmployeeId",
                table: "TimeSheets",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Employee_EmployeeId",
                table: "TimeSheets");

            migrationBuilder.DropIndex(
                name: "IX_TimeSheets_EmployeeId",
                table: "TimeSheets");

            migrationBuilder.AlterColumn<string>(
                name: "EmployeeId",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeId1",
                table: "TimeSheets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TimeSheets_EmployeeId1",
                table: "TimeSheets",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Employee_EmployeeId1",
                table: "TimeSheets",
                column: "EmployeeId1",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

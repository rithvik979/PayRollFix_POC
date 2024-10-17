using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class timesheet4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSheets_Employee_EmployeeId",
                table: "TimeSheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TimeSheets",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "EveningHours",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "MorningHours",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "PastLunchHours",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "PreLunchHours",
                table: "TimeSheets");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "TimeSheets");

            migrationBuilder.RenameTable(
                name: "TimeSheets",
                newName: "Timesheets");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Timesheets",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "TimeSheetId",
                table: "Timesheets",
                newName: "TimesheetId");

            migrationBuilder.RenameIndex(
                name: "IX_TimeSheets_EmployeeId",
                table: "Timesheets",
                newName: "IX_Timesheets_EmployeeId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Timesheets",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Timesheets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Timesheets",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "HoursWorked",
                table: "Timesheets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timesheets",
                table: "Timesheets",
                column: "TimesheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timesheets_Employee_EmployeeId",
                table: "Timesheets",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timesheets_Employee_EmployeeId",
                table: "Timesheets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Timesheets",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Timesheets");

            migrationBuilder.DropColumn(
                name: "HoursWorked",
                table: "Timesheets");

            migrationBuilder.RenameTable(
                name: "Timesheets",
                newName: "TimeSheets");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "TimeSheets",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "TimesheetId",
                table: "TimeSheets",
                newName: "TimeSheetId");

            migrationBuilder.RenameIndex(
                name: "IX_Timesheets_EmployeeId",
                table: "TimeSheets",
                newName: "IX_TimeSheets_EmployeeId");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "date",
                table: "TimeSheets",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "EveningHours",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MorningHours",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PastLunchHours",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PreLunchHours",
                table: "TimeSheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "TotalHours",
                table: "TimeSheets",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_TimeSheets",
                table: "TimeSheets",
                column: "TimeSheetId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSheets_Employee_EmployeeId",
                table: "TimeSheets",
                column: "EmployeeId",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

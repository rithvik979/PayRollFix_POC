using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Employee_Employee_EmployeeId2",
            //    table: "Employee");

            //migrationBuilder.DropIndex(
            //    name: "IX_Employee_EmployeeId2",
            //    table: "Employee");

            //migrationBuilder.DropColumn(
            //    name: "EmployeeId2",
            //    table: "Employee");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Employee");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employee_EmployeeId1",
            //    table: "Employee",
            //    column: "EmployeeId1");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Employee_Employee_EmployeeId1",
            //    table: "Employee",
            //    column: "EmployeeId1",
            //    principalTable: "Employee",
            //    principalColumn: "EmployeeId",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Employee_Employee_EmployeeId1",
            //    table: "Employee");

            //migrationBuilder.DropIndex(
            //    name: "IX_Employee_EmployeeId1",
            //    table: "Employee");

            //migrationBuilder.AddColumn<int>(
            //    name: "EmployeeId2",
            //    table: "Employee",
            //    type: "int",
            //    nullable: false,
            //    defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Employee",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Employee_EmployeeId2",
            //    table: "Employee",
            //    column: "EmployeeId2");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Employee_Employee_EmployeeId2",
            //    table: "Employee",
            //    column: "EmployeeId2",
            //    principalTable: "Employee",
            //    principalColumn: "EmployeeId",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}

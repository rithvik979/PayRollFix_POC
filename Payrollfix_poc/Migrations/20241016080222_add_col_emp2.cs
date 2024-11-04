using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class add_col_emp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employee_Employee_EmployeeId1",
                table: "Employee");

            migrationBuilder.DropIndex(
                name: "IX_Employee_EmployeeId1",
                table: "Employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Employee_EmployeeId1",
                table: "Employee",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Employee_Employee_EmployeeId1",
                table: "Employee",
                column: "EmployeeId1",
                principalTable: "Employee",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

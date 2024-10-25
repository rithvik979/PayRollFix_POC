using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class employee3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
            name: "EmployeeId1",
            table: "Employee");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

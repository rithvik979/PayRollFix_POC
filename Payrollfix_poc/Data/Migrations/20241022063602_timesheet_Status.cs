using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class timesheet_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Timesheets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Timesheets");
        }
    }
}

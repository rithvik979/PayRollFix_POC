using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Payrollfix_poc.Migrations
{
    /// <inheritdoc />
    public partial class myteam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "Employee");
        }
    }
}

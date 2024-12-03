using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Data
{
    public class PayRollFix_pocContext : DbContext
    {
        public PayRollFix_pocContext(DbContextOptions<PayRollFix_pocContext> options) : base(options) { }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<Attandence> Attendance { get; set; }
        public DbSet<LoginActivity> LoginActivities { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
        public DbSet<Timesheet> Timesheets { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Salary> Salary { get; set; }
        public DbSet<EmployeeImage> EmployeeImage { get; set; }
    }
}

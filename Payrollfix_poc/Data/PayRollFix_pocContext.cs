using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Data
{
    public class PayRollFix_pocContext:DbContext
    {
        public PayRollFix_pocContext(DbContextOptions<PayRollFix_pocContext> options) : base(options) { }

        public DbSet<Department> Department { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Attandence> Attendance { get; set; }
        public DbSet<LoginActivity> LoginActivities { get; set; }
        public DbSet<LoginViewModel> LoginViewModel { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<LeaveBalance> LeaveBalances { get; set; }
	}
}

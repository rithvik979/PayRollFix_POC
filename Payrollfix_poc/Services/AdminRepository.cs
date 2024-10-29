using Payrollfix_poc.Data;
using Payrollfix_poc.Models;
using System.Threading.Tasks;

namespace Payrollfix_poc.Services
{
    public class AdminRepository : IAdminRepository
    {
        public readonly PayRollFix_pocContext _context;
        public AdminRepository(PayRollFix_pocContext context)
        {
            _context = context;
        }

        public async Task SaveEmployee(Employee employee)
        {
            _context.Employee.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task SaveSalary(Salary salary)
        {
            _context.Salary.Add(salary);
            await _context.SaveChangesAsync();
        }

        public async Task SaveLeaveBalance(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Add(leaveBalance);
            await _context.SaveChangesAsync();
        }

        public async Task SaveLeave(Leave leaves)
        {
            _context.Leaves.Add(leaves);
            await _context.SaveChangesAsync();
        }

        public async Task SaveEmployeeImage(EmployeeImage employeeImage)
        {
            _context.EmployeeImage.Add(employeeImage);
            await _context.SaveChangesAsync();
        }

        public async Task SaveExpenses(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
        }

        public async Task SaveLoginActivites(LoginActivity activity)
        {
            _context.LoginActivities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAttandance(Attandence attandence)
        {
            _context.Attendance.Add(attandence);
            await _context.SaveChangesAsync();
        }
        public async Task SaveTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Add(timesheet);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLoginactivity(LoginActivity activity)
        {
            _context.LoginActivities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAttandance(Attandence attandence)
        {
            _context.Attendance.Update(attandence);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeaveBalance(LeaveBalance leaveBalance)
        {
            _context.LeaveBalances.Update(leaveBalance);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLeave(Leave leave)
        {
            _context.Leaves.Update(leave);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTimesheet(Timesheet timesheet)
        {
            _context.Update(timesheet);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
        }
    }
}

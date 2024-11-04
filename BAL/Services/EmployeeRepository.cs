using Payrollfix_poc.Models;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Data;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.ViewModels;
using System.Globalization;
using System.Linq.Expressions;

namespace Payrollfix_poc.Services
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly PayRollFix_pocContext _context;
        public EmployeeRepository(PayRollFix_pocContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeDetails(int? id)
        {
            return await _context.Employee
                                 .Include(e => e.Leaves)
                                 .Include(e => e.Expenses)
                                 .Include(e => e.Timesheets)
                                 .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee> GetEmployeeById(int? id, LoginViewModel? login, ForgotPasswordViewModel forgotPassword)
        {
            if (login == null)
            {
                return await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == id);
            }
            if (forgotPassword == null)
            {
                return await _context.Employee.FirstOrDefaultAsync(e => e.EmployeeId == login.Id && e.Password == login.Password);
            }
            return await _context.Employee.FirstOrDefaultAsync(e => e.Email == forgotPassword.Email);
        }

		public async Task<List<T>> GetEntitiesByCondition<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			return await _context.Set<T>().Where(predicate).ToListAsync();
		}
		
        public async Task<List<Employee>> GetEmployeeList()
        {
            var re = await _context.Employee.ToListAsync();
            return re;
        }

        public async Task<Attandence> GetTodayAttandance(int? id, DateOnly? date)
        {
            return await _context.Attendance.FirstOrDefaultAsync(a => a.EmployeeId == id && a.Date == date);
        }

        public async Task<List<Timesheet>> GetTimesheetList(int? id)
        {
            return await _context.Timesheets
                                 .Where(t => t.EmployeeId == id)
                                 .OrderByDescending(t => t.Date)
                                 .ToListAsync();
        }

        public async Task<List<Expense>> GetExpenseList()
        {
            return await _context.Expenses.ToListAsync();
        }

        public async Task<List<ExpenseGroupViewModel>> GroupedExpenses(int? id, List<Expense> expenses)
        {
            return expenses
                .Where(e => e.EmployeeId == id)
                .GroupBy(e => new { Month = e.Date.Month, Year = e.Date.Year })
                .Select(monthGroup => new ExpenseGroupViewModel
                {
                    Month = monthGroup.Key.Month,
                    Year = monthGroup.Key.Year,
                    WeekGroups = monthGroup
                        .GroupBy(e => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                        .Select(weekGroup => new WeekGroupViewModel
                        {
                            WeekNumber = weekGroup.Key,
                            Expenses = weekGroup.ToList()
                        })
                        .ToList()
                })
                .ToList();
        }

        public async Task<EmployeeImage> GetEmployeeImage(int? id)
        {
            return await _context.EmployeeImage.FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<LoginActivity> GetLoginActivity(int? activityid, int? employeeid)
        {
            return await _context.LoginActivities
                                 .FirstOrDefaultAsync(a => a.ActivityId == activityid.Value && a.EmployeeId == employeeid.Value);
        }

        public async Task<LeaveBalance> GetLeaveBalance(int? id)
        {
            return await _context.LeaveBalances
                                 .FirstOrDefaultAsync(lb => lb.EmployeeId == id);
        }

        public async Task<List<Leave>> GetLeaves(int? id)
        {
            return await _context.Leaves
                                 .Where(l => l.EmployeeId == id)
                                 .Include(l => l.Employee)
                                 .ToListAsync();
        }

        public async Task<Leave> GetLeaveById(int? leaveid)
        {
            return await _context.Leaves.FirstOrDefaultAsync(l => l.LeaveId == leaveid);
        }

        public async Task<Timesheet> GetTimesheetById(int? id)
        {
            return await _context.Timesheets.FindAsync(id);
        }
    }
}

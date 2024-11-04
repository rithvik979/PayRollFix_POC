using System;
using System.Linq.Expressions;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;

namespace Payrollfix_poc.IRepository
{
    public interface IEmployeeRepository
    {
        Task<List<T>> GetEntitiesByCondition<T>(Expression<Func<T, bool>> predicate) where T : class;
		Task<Employee> GetEmployeeDetails(int? id);
        Task<Employee> GetEmployeeById(int? id, LoginViewModel? login, ForgotPasswordViewModel forgotPassword);
        Task<List<Employee>> GetEmployeeList();
        Task<Attandence> GetTodayAttandance(int? id, DateOnly? date);
        Task<List<Timesheet>> GetTimesheetList(int? id);
        Task<List<Expense>> GetExpenseList();
        Task<List<ExpenseGroupViewModel>> GroupedExpenses(int? id, List<Expense> expenses);
        Task<EmployeeImage> GetEmployeeImage(int? id);
        Task<LoginActivity> GetLoginActivity(int? activityid, int? employeeid);
        Task<LeaveBalance> GetLeaveBalance(int? id);
        Task<List<Leave>> GetLeaves(int? id);
        Task<Leave> GetLeaveById(int? leaveid);
        Task<Timesheet> GetTimesheetById(int? id);
    }
}

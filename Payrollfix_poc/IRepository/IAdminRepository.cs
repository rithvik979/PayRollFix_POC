using Payrollfix_poc.Models;

public interface IAdminRepository
{
    Task SaveEmployee(Employee employee);
    Task SaveSalary(Salary salary);
    Task SaveLeaveBalance(LeaveBalance leaveBalance);
    Task SaveLeave(Leave leave);
    Task SaveEmployeeImage(EmployeeImage employeeImage);
    Task SaveExpenses(Expense expense);
    Task SaveLoginActivites(LoginActivity activity); 
    Task SaveAttandance(Attandence attandence);
    Task UpdateLoginactivity(LoginActivity activity); 
    Task UpdateAttandance(Attandence attandence);     
    Task UpdateLeaveBalance(LeaveBalance leaveBalance);
    Task UpdateLeave(Leave leave);
}

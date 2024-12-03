using Payrollfix_poc.Models;

public interface IAdminRepository
{
    Task Save<T>(T entity)where T : class;
    Task Update<T>(T entity) where T : class;
    Task DeleteTimesheet(Timesheet timesheet);
}


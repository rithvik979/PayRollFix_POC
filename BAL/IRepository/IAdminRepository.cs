using Payrollfix_poc.Models;

public interface IAdminRepository
{
    Task SaveInDb<T>(T entity)where T : class;
    Task UpdateInDb<T>(T entity) where T : class;
    Task DeleteTimesheet(Timesheet timesheet);
}


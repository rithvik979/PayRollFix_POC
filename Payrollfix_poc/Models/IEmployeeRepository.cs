namespace Payrollfix_poc.Models
{
    public interface IEmployeeRepository
    {
        Employee GetById(int id);
    }
}

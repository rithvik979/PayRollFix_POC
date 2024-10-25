using Payrollfix_poc.Models;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Data;
using Microsoft.EntityFrameworkCore;

namespace Payrollfix_poc.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public readonly PayRollFix_pocContext _context;
        public EmployeeRepository(PayRollFix_pocContext context)
        {
            _context = context;
        }
        public Employee GetEmployeeDetails(int id)
        {
            return _context.Employee
                            .Include(e => e.Department) // Include LoginActivities if applicable
                            .Include(e => e.Position)
                            .Include(e => e.Leaves)
                            .FirstOrDefault(e => e.EmployeeId == id);
        }
        public Employee GetEmployeeById(int id)
        {
            return _context.Employee.FirstOrDefault(e => e.EmployeeId == id);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;

namespace Payrollfix_poc.Controllers
{
    public class AdminController : Controller
    {
        public readonly PayRollFix_pocContext _context;
        public AdminController(PayRollFix_pocContext context)
        {
            _context = context;
        }
      
        public async Task<IActionResult> Index()
        {
            int employeeId = 4;
            var employee = await _context.Employee
                .Include(e => e.Department) // Include LoginActivities if applicable
                .Include(e => e.Position)
                .Include(e => e.Leaves)
                .Include(e => e.Expenses)
                .Include(e => e.Timesheets)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
            return View(employee);
        }

        public IActionResult TimesheetList(int id)
        {
            var timesheets = _context.Timesheets
                .Where(t => t.EmployeeId == id)
                .OrderByDescending(t => t.Date) // Order by most recent first
                .ToList();

            return View(timesheets);
        }
    }
}

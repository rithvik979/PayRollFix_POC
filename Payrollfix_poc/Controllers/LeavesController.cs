using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Controllers
{
    public class LeaveController : Controller
    {
        private readonly PayRollFix_pocContext _context;

        public LeaveController(PayRollFix_pocContext context)
        {
            _context = context;
        }

        // Action method to retrieve leaves for a specific employee
        public async Task<IActionResult> LeaveDetails()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");

            // Get leave details for the specific employee
            var leaves = await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .Include(l => l.Employee) // Include Employee details
                .ToListAsync();

            // Get leave balance for the specific employee
            var leaveBalance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId);

            // Combine both into a view model
            var model = new LeaveViewModel
            {
                Leaves = leaves,
                LeaveBalance = leaveBalance
            };

            return View(model);
        }

    }
}

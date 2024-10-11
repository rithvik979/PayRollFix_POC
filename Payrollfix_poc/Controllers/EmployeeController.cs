using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Controllers
{
    public class EmployeeController : Controller
    {
        public readonly PayRollFix_pocContext _context;
        public IEmployeeRepository _repository { get; set; }
        public EmployeeController(PayRollFix_pocContext context,IEmployeeRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        public async Task<IActionResult> Dashboard()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");
			var employee = await _context.Employee
                //.Include(e => e.LoginActivities) // Include LoginActivities if applicable
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

            if (employee == null)
            {
                return NotFound(); // Handle not found
            }

            return View(employee); // Pass the single employee data to the view
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Details(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                // Return the empty view if no search query is entered
                return View();
            }

            // Search for the employee by first or last name
            var employee = _context.Employee
                                   .FirstOrDefault(e => e.FirstName.Contains(name) || e.LastName.Contains(name));

            if (employee == null)
            {
                ViewBag.ErrorMessage = "No employee found with the given name.";
                return View();
            }

            // Pass the employee object to the view
            return View(employee);
        }
        public IActionResult LoginDetails()
		{
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");
			// Fetch the list of login activities for a specific employee from the database
			string sql = @"SELECT 
            ActivityId, 
            LoginTime, 
            LogoutTime, 
            EmployeeId FROM LoginActivities WHERE EmployeeId = @employeeId";

			// Execute the raw SQL query
			var LoginActivity = _context.LoginActivities
										  .FromSqlRaw(sql, new SqlParameter("@EmployeeId", employeeId))
										  .ToList();   // Retrieve the list
            var activities = LoginActivity.ToList();
			Console.WriteLine("hi");
			if (activities == null || !activities.Any())
			{
				// Handle the case where no activities are found for the employee
				return NotFound("No login activities found for the specified employee.");
			}

			return View(activities);  // Passing the login activities to the view
		}
        public async Task<IActionResult> AM()
        {
            Int32 id = 8;
            var attendance = await _context.Attendance
                .Where(a => a.EmployeeId == id)
                .ToListAsync();
			return View(attendance);
		}
	}
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;

namespace Payrollfix_poc.Controllers
{
	public class EmployeeController : Controller
	{
		public readonly PayRollFix_pocContext _context;

		public EmployeeController(PayRollFix_pocContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Dashboard()
		{
			var employeeId = HttpContext.Session.GetInt32("EmployeeId"); // prior verification
			var employee = await _context.Employee
				.Include(e => e.Department) // Include LoginActivities if applicable
				.Include(e => e.Position)
				.Include(e => e.Leaves)
				.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);

			if (employee == null)
			{
				return NotFound(); // Handle not found
			}

			ViewData["ActiveDashboard"] = "active";
			return View(employee); // Pass the single employee data to the view
		}
		[HttpGet]
		public IActionResult ResetPassword()
		{
			return View();
		}

		[HttpPost]
		public IActionResult ResetPassword(ResetPasswordViewModel model)
		{
			var EmpId = HttpContext.Session.GetInt32("EmployeeId");
			if (ModelState.IsValid)
			{
				// Get the logged-in employee (or use the employee ID)
				var employee = _context.Employee.FirstOrDefault(e => e.EmployeeId == EmpId);

				if (employee == null)
				{
					ModelState.AddModelError("", "User not found.");
					return View(model);
				}

				// Verify the old password

				if (model.OldPassword != employee.Password)
				{
					ModelState.AddModelError("OldPassword", "The old password is incorrect.");
					return View(model);
				}

				// Check if new password is different from the old password
				if (model.NewPassword == model.OldPassword)
				{
					ModelState.AddModelError("NewPassword", "New password cannot be the same as the old password.");
					return View(model);
				}

				employee.Password = model.NewPassword;
				_context.SaveChanges();

				// Optionally redirect or show confirmation
				ViewBag.Message = "Your password has been successfully updated.";
				return View("ResetConfirmation");
			}

			return View(model);
		}

		public IActionResult Organization(string name)
		{
			ViewData["ActiveEmployee"] = "active";

			// Get all employees if no search query is provided
			var employees = _context.Employee
				.Include(e => e.Department)
				.Include(e => e.Position)
				.ToList();

			// If a search query is provided, filter the employees
			if (!string.IsNullOrEmpty(name))
			{
				employees = employees
							.Where(e => e.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
										e.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
							.ToList();

				if (employees.Count == 0)
				{
					ViewBag.ErrorMessage = "No employee found with the given name.";
				}
			}

			// If there's only one employee found in the search, show detailed view for that employee
			if (employees.Count == 1)
			{
				return View("EmployeeDetails", employees.First());
			}

			// Pass the list of employees to the view
			return View(employees);
		}

		public async Task<IActionResult> OrganizationChart()
		{
			// Fetch all employees from the database
			var employees = await _context.Employee.ToListAsync();

			// Pass the employees to the view
			return View(employees);
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

			ViewData["ActiveLoginDetails"] = "active";
			return View(activities);  // Passing the login activities to the view
		}

		public async Task<IActionResult> AM()
		{
			var id = HttpContext.Session.GetInt32("EmployeeId");
			var attendance = await _context.Attendance
				.Where(a => a.EmployeeId == id)
				.ToListAsync();

			ViewData["ActiveAttendance"] = "active";
			return View(attendance);
		}

		public IActionResult MyTeam()
		{
			var CurrentManagerId = HttpContext.Session.GetInt32("EmployeeId");

			// Get the list of employees who report to the current manager
			var employees = _context.Employee
							.Where(e => e.ManagerId == CurrentManagerId)
							.ToList();

			ViewData["ActiveMyTeam"] = "active";
			return View(employees);
		}

		public IActionResult NotManager()
		{
			return View();
		}
	}
}

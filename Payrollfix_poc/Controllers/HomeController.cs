using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;
using System.Diagnostics;

namespace Payrollfix_poc.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly PayRollFix_pocContext _context;
		public HomeController(PayRollFix_pocContext context)
		{
			_context = context;
		}

		public IActionResult ContactUs()
		{
			return View();
		}

		public IActionResult ForgotPassword()
		{
			return View();
		}

		public IActionResult Signup()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public IActionResult AboutUs()
		{
			return View();
		}

		public IActionResult Subscription()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> AdminLogin(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _context.Admin
										.FirstOrDefaultAsync(e => e.AdminId == model.Id && e.Password == model.Password);
				if (user != null)
				{
					return RedirectToAction("Index", "Admin");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Invalid login attempt.");
				}
			}
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EmployeeLogin(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Fetch employee using EmployeeId and Password
				var user = await _context.Employee
										 .FirstOrDefaultAsync(e => e.EmployeeId == model.Id && e.Password == model.Password);

				if (user != null)
				{
					// Create a new login activity
					var loginActivity = new LoginActivity
					{
						EmployeeId = user.EmployeeId, // Foreign key linking to Employee
						LoginTime = DateTime.Now
					};

					// Add login activity to the context
					_context.LoginActivities.Add(loginActivity);
					await _context.SaveChangesAsync();

					// Set session values
					HttpContext.Session.SetInt32("EmployeeId", user.EmployeeId);  // Store EmployeeId in session
					HttpContext.Session.SetInt32("ActivityId", loginActivity.ActivityId);  // Store ActivityId in session

					// Handle Attendance
					var today = DateOnly.FromDateTime(DateTime.Now);

					// Check if an attendance record already exists for today
					var attendance = await _context.Attendance
												   .FirstOrDefaultAsync(a => a.EmployeeId == user.EmployeeId && a.Date == today);
					if (attendance == null)
					{
						// Create a new attendance record for today
						attendance = new Attandence
						{
							EmployeeId = user.EmployeeId,
							Date = today,
							CheckInTime = DateTime.Now,
							Status = "Present"
						};

						_context.Attendance.Add(attendance);
						Console.WriteLine("executed");
					}
					else
					{
						// Update existing attendance record's CheckInTime if not already set
						if (attendance.CheckInTime == null)
						{
							attendance.CheckInTime = DateTime.Now;
							attendance.Status = "Present";
						}
						Console.WriteLine("Not Executed");
						// Optionally, handle multiple logins per day
					}

					// Save changes to Attendance
					await _context.SaveChangesAsync();
					Console.WriteLine("sucessfull");
					return RedirectToAction("Dashboard", "Employee");
				}
				// Invalid login attempt
				ModelState.AddModelError(string.Empty, "Invalid login attempt.");
			}
			return View(model);
		}

		public async Task<IActionResult> Logout()
		{
			// Retrieve the current login activity from session
			var activityId = HttpContext.Session.GetInt32("ActivityId");
			var employeeId = HttpContext.Session.GetInt32("EmployeeId");

			if (activityId.HasValue && employeeId.HasValue)
			{
				// Fetch the login activity using ActivityId
				var loginActivity = await _context.LoginActivities
												 .FirstOrDefaultAsync(a => a.ActivityId == activityId.Value && a.EmployeeId == employeeId.Value);

				if (loginActivity != null && loginActivity.LogoutTime == null)
				{
					// Store logout time
					loginActivity.LogoutTime = DateTime.Now;
					_context.LoginActivities.Update(loginActivity);
					await _context.SaveChangesAsync();
				}

				// Handle Attendance
				var today = DateOnly.FromDateTime(DateTime.Now);

				// Fetch today's attendance record
				var attendance = await _context.Attendance
											   .FirstOrDefaultAsync(a => a.EmployeeId == employeeId.Value && a.Date == today);

				if (attendance != null)
				{
					// Update CheckOutTime and calculate WorkHours only if CheckOutTime is not already set
					attendance.CheckOutTime = DateTime.Now;

					// Calculate WorkHours
					if (attendance.CheckInTime != null)
					{
						attendance.WorkHours = (float)(attendance.CheckOutTime - attendance.CheckInTime)?.TotalHours;
					}
					else
					{
						// If CheckInTime is somehow null, handle accordingly
						attendance.WorkHours = 0;
					}

					// Update Status based on WorkHours
					if (attendance.WorkHours >= 8)
					{
						attendance.Status = "Present";
					}
					else if (attendance.WorkHours > 0)
					{
						attendance.Status = "Partial";
					}
					else
					{
						attendance.Status = "Absent";
					}

					_context.Attendance.Update(attendance);
					await _context.SaveChangesAsync();
				}
			}

			// Clear session
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Home");
		}
	}
}
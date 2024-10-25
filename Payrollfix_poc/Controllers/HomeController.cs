using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Filters;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;
using System.Diagnostics;


namespace Payrollfix_poc.Controllers
{
	[CustomAuthorize]
    public class HomeController : Controller
	{
		public readonly IEmployeeRepository _employeeRepository;
		public readonly IAdminRepository _adminRepository;
		public readonly IServicesRepository _servicesRepository;
		public HomeController(IEmployeeRepository repository,IAdminRepository admin,IServicesRepository services)
		{
			_employeeRepository = repository;
			_adminRepository = admin;
			_servicesRepository = services;
		}

		public IActionResult ContactUs()
		{
			return View();
		}

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
				// Check if the email exists in the Employee table
				var employee = await _employeeRepository.GetEmployeeById(null, null, forgotPassword: model);

                if (employee != null)
                {
					// Generate a new random password
					string newPassword = _servicesRepository.GenerateRandomPassword();

					employee.Password = newPassword;
					await _adminRepository.SaveEmployee(employee);

                    // Send email with the new password
                    _servicesRepository.SendResetPasswordEmail(employee.Email, newPassword);

                    ViewBag.Message = "An email with the new password has been sent to your email address.";
                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    ModelState.AddModelError("", "The email address is not registered.");
                }
            }
            return View(model);
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
		public async Task<IActionResult> EmployeeLogin(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				// Fetch employee using EmployeeId and Password
				var user = await _employeeRepository.GetEmployeeById(null, model,null);

				if (user != null)
				{
					// Create a new login activity
					var loginActivity = new LoginActivity
					{
						EmployeeId = user.EmployeeId, // Foreign key linking to Employee
						LoginTime = DateTime.Now
					};

					// Save login activity to the context
					await _adminRepository.SaveLoginActivites(loginActivity);

					// Set session values
					HttpContext.Session.SetInt32("EmployeeId", user.EmployeeId);  // Store EmployeeId in session
					HttpContext.Session.SetInt32("ActivityId", loginActivity.ActivityId);  // Store ActivityId in session
					HttpContext.Session.SetString("Role", user.Position);
					// Handle Attendance
					var today = DateOnly.FromDateTime(DateTime.Now);

					// Check if an attendance record already exists for today
					var attendance = await _employeeRepository.GetTodayAttandance(user.EmployeeId, today);

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

						await _adminRepository.SaveAttandance(attendance);
					}
					else
					{
						// Update existing attendance record's CheckInTime if not already set
						if (attendance.CheckInTime == null)
						{
							attendance.CheckInTime = DateTime.Now;
							attendance.Status = "Present";
						}
						// Optionally, handle multiple logins per day
					}

					return RedirectToAction("Dashboard", "Employee");
				}
				// Invalid login attempt
				return RedirectToAction("Login");
			}
			return RedirectToAction("Login", "Home");
		}

		public async Task<IActionResult> EmployeeLogout()
		{
			// Retrieve the current login activity from session
			var activityId = HttpContext.Session.GetInt32("ActivityId");
			var employeeId = HttpContext.Session.GetInt32("EmployeeId");

			if (activityId.HasValue && employeeId.HasValue)
			{
				// Fetch the login activity using ActivityId
				var loginActivity = await _employeeRepository.GetLoginActivity(activityId, employeeId);

				if (loginActivity != null && loginActivity.LogoutTime == null)
				{
					// Store logout time
					loginActivity.LogoutTime = DateTime.Now;
					await _adminRepository.UpdateLoginactivity(loginActivity);
				}

				// Handle Attendance
				var today = DateOnly.FromDateTime(DateTime.Now);

				// Fetch today's attendance record
				var attendance =  await _employeeRepository.GetTodayAttandance(employeeId, today);

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

					await _adminRepository.UpdateAttandance(attendance);
				}
			}

			// Clear session
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Home");
		}

		public async Task<IActionResult> GetHeaderData()
		{
			var employeeId = HttpContext.Session.GetInt32("EmployeeId");
			var employeeImage = await _employeeRepository.GetEmployeeImage(employeeId);
			if (employeeImage != null)
			{
				return File(employeeImage.Image, employeeImage.ContentType);
			}
			return null;
		}

		public async Task<IActionResult> GetImageById(int employeeId)
		{
			var employeeImage = await _employeeRepository.GetEmployeeImage(employeeId);
			if (employeeImage != null)
			{
				return File(employeeImage.Image, employeeImage.ContentType);
			}
			return null;
		}
		public IActionResult Unauthorized()
		{
			return View();
		}
	}
}
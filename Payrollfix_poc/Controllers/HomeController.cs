using MailBee.SmtpMail;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;

namespace Payrollfix_poc.Controllers
{
    public class HomeController : Controller
	{
		private readonly PayRollFix_pocContext _context;
		public HomeController(PayRollFix_pocContext context)
		{
			_context = context;
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
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email exists in the Employee table
                var employee = _context.Employee
                                       .FirstOrDefault(e => e.Email == model.Email);

                if (employee != null)
                {
                    // Generate a new random password
                    string newPassword = GenerateRandomPassword();

					employee.Password = newPassword;
                    _context.SaveChanges();

                    // Send email with the new password
                    SendResetPasswordEmail(employee.Email, newPassword);

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

        private string GenerateRandomPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@!#$%^&*()";
            StringBuilder newPassword = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 6; i++)  // Generate an 6-character password
            {
                newPassword.Append(validChars[random.Next(validChars.Length)]);
            }

            return newPassword.ToString();
        }

        private void SendResetPasswordEmail(string email, string newPassword)
        {
            try
            {
                MailMessage mail = new MailMessage();

				mail.From = new MailAddress("g.rithvikreddy909@gmail.com");
                mail.To.Add(email);
				mail.Subject = "Password Reset Request";
				mail.Body = $"Your new password is: {newPassword}";

                //MailBee.SmtpMail.Smtp.QuickSend("jdoe@domain.com", email , sub, "Message Body");

                SmtpClient smtpServer = new SmtpClient();

				smtpServer.Host = "smtp.gmail.com";
				smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential("g.rithvikreddy909@gmail.com", "yxai scky pzcx aech");
				smtpServer.EnableSsl = true;

				smtpServer.Send(mail);
			}
            catch (Exception ex)
            {
                // Handle exception (log it or display an error message)
                Console.WriteLine($"Email sending failed: {ex.Message}");
            }
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
					return RedirectToAction("Login", "Home");
				}
			}
			return RedirectToAction("Login", "Home");
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

					// Save changes to Attendance
					await _context.SaveChangesAsync();
					Console.WriteLine("sucessfull");
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

		public IActionResult GetHeaderData()
		{
			var employeeId = HttpContext.Session.GetInt32("EmployeeId");
			var employeeImage = _context.EmployeeImage.FirstOrDefault(e => e.EmployeeId == employeeId);
			return File(employeeImage.Image, employeeImage.ContentType);
		}

		public async Task<IActionResult> GetImageById(int employeeId)
		{
			var employeeImage = await _context.EmployeeImage.FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
			return File(employeeImage.Image, employeeImage.ContentType);
		}
	}
}
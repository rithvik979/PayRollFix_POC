using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;


namespace Payrollfix_poc.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IAdminRepository _adminRepository;
        public readonly IServicesRepository _servicesRepository;
        public readonly IConfiguration _configuration;
        public HomeController(IEmployeeRepository repository, IAdminRepository admin, IServicesRepository services, IConfiguration configuration)
        {
            _employeeRepository = repository;
            _adminRepository = admin;
            _servicesRepository = services;
            _configuration = configuration;
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
                    await _adminRepository.SaveInDb(employee);

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
                var user = await _employeeRepository.GetEmployeeById(null, model, null);

                if (user != null)
                {
                    // Create a new login activity
                    var loginActivity = new LoginActivity
                    {
                        EmployeeId = user.EmployeeId, // Foreign key linking to Employee
                        LoginTime = DateTime.Now
                    };

                    // Save login activity to the context
                    await _adminRepository.SaveInDb(loginActivity);

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

                        await _adminRepository.SaveInDb(attendance);
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
                    var token = GenerateJwtToken(user);

                    // Store the JWT token in a cookie (optional, we can also store in session)
                    Response.Cookies.Append("JwtToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(1) // Set token expiration
                    });

                    // Return the token in the response body
                    //return Ok(new
                    //{
                    //    Token = token,
                    //    Message = "Login successful"
                    //});
                    return RedirectToAction("Dashboard", "Employee");
                }
                // Invalid login attempt
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login", "Home");
        }

        private string GenerateJwtToken(Employee user)
        {
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var claims = new[]
            {
                 new Claim(ClaimTypes.Role, user.Position)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
                    await _adminRepository.UpdateInDb(loginActivity);
                }

                // Handle Attendance
                var today = DateOnly.FromDateTime(DateTime.Now);

                // Fetch today's attendance record
                var attendance = await _employeeRepository.GetTodayAttandance(employeeId, today);

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

                    await _adminRepository.UpdateInDb(attendance);
                }
            }

            // Clear session
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
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
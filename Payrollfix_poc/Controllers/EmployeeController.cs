using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Filters;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.ViewModels;

namespace Payrollfix_poc.Controllers
{
    [CustomAuthorize]
    public class EmployeeController : Controller
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IAdminRepository _adminRepository;

        public EmployeeController(IEmployeeRepository repository,IAdminRepository admin)
        {
            _employeeRepository = repository;
            _adminRepository = admin;
        }
        public async Task<IActionResult> Dashboard()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId"); // prior verification

            var employee = await _employeeRepository.GetEmployeeDetails(employeeId);

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
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            int? EmpId = HttpContext.Session.GetInt32("EmployeeId");
            if (ModelState.IsValid)
            {
                // Get the logged-in employee (or use the employee ID)
                if (EmpId != 0)
                {
                    var employee = await _employeeRepository.GetEmployeeById(EmpId,null,null);

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
                    await _adminRepository.SaveEmployee(employee);

                    // Optionally redirect or show confirmation
                    ViewBag.Message = "Your password has been successfully updated.";
                    return View("ResetConfirmation");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Organization(string name)
        {
			var id = HttpContext.Session.GetInt32("EmployeeId");
			ViewBag.Position = (await _employeeRepository.GetEmployeeById(id, null, null)).Position;
			ViewData["ActiveEmployee"] = "active";
			// Get all employees if no search query is provided
			var employees = await _employeeRepository.GetEmployeeList();

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
			var id = HttpContext.Session.GetInt32("EmployeeId");
			var employees = await _employeeRepository.GetEmployeeList();
			ViewBag.Position = (await _employeeRepository.GetEmployeeById(id, null, null)).Position;
			ViewData["ActiveEmployee"] = "active";
			// Pass the employees to the view
			return View(employees);
        }

        public async Task<IActionResult> LoginDetails()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");

            var activities = await _employeeRepository.GetLoginById(employeeId);

            if (activities == null || !activities.Any())
            {
                // Handle the case where no activities are found for the employee
                return NotFound("No login activities found for the specified employee.");
            }

			ViewBag.Position = (await _employeeRepository.GetEmployeeById(employeeId, null, null)).Position;
			ViewData["ActiveLoginDetails"] = "active";
            return View(activities);  // Passing the login activities to the view
        }

        public async Task<IActionResult> AM()
        {
            var id = HttpContext.Session.GetInt32("EmployeeId");

            var attendance = await _employeeRepository.GetAttandanceById(id);
            ViewBag.Position = (await _employeeRepository.GetEmployeeById(id, null, null)).Position;
            ViewData["ActiveAttendance"] = "active";

            return View(attendance);
        }

        public async Task<IActionResult> MyTeam()
        {
            var CurrentManagerId = HttpContext.Session.GetInt32("EmployeeId");

            // Get the list of employees who report to the current manager
            var employees = await _employeeRepository.GetManagerById(CurrentManagerId);

			ViewBag.Position = (await _employeeRepository.GetEmployeeById(CurrentManagerId, null, null)).Position;
			ViewData["ActiveMyTeam"] = "active";
            return View(employees);
        }

        public IActionResult NotManager()
        {
            return View();
        }
    }
}

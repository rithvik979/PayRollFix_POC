using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Filters;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Controllers
{
	/// <summary>
	/// This is Controller Contains 6 action Methods:
	/// <para><see cref="Index(string)"/>- this method returns all the data of the employee to Index view </para>
	/// <para><see cref="CreateEmployee()"/>this method will create a employee using the basic details in the database</para>
	/// <para><see cref="CreateSalaryLeave(int)"/>this method is excecuted following the CreateEmployee in this </para>
	/// <para><see cref="EditEmployee(Employee)"/></para>
	/// <para><see cref="UploadImage(int, IFormFile)"/></para>
	/// <para><see cref="TimeSheet(int)"/></para>
	/// </summary>

	[CustomAuthorize(Role ="Admin")]
    public class AdminController : Controller
	{
		public readonly IEmployeeRepository _employeeRepository;
		public readonly IAdminRepository _adminRepository;
		public AdminController(IEmployeeRepository repository,IAdminRepository admin)
		{
			_employeeRepository = repository;
			_adminRepository = admin;
		}
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			int id = 43;
			var employee = await _employeeRepository.GetEmployeeDetails(id);
			return View(employee);
		}

		[HttpPost]
		public async Task<IActionResult> Index(string name)
		{
			var employees = await _employeeRepository.GetEmployeeList();

			// If a search query is provided, filter the employees
			if (!string.IsNullOrEmpty(name))
			{
				var emp = employees
							.Where(e => e.FirstName.Contains(name, StringComparison.OrdinalIgnoreCase) ||
										e.LastName.Contains(name, StringComparison.OrdinalIgnoreCase))
							.FirstOrDefault();
				if (emp != null)
				{
					int id = emp.EmployeeId;
					var employee = _employeeRepository.GetEmployeeDetails(id);

					return View(employee);
				}
			}
			return View();
		}
		[HttpGet]
		public IActionResult CreateEmployee()
		{
			return View();
		}

		// POST: AddEmployee
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateEmployee(Employee employee)
		{
			if (ModelState.IsValid)
			{
				await _adminRepository.SaveInDb(employee);

				int empid = employee.EmployeeId;
				// Redirect to salary and leave form
				return RedirectToAction("CreateSalaryLeave", new { employeeId = empid });
			}
			return View(employee);
		}

		[HttpGet]
		public IActionResult CreateSalaryLeave(int employeeId)
		{
			var Salary = new Salary { EmployeeId = employeeId };
			return View(Salary);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSalaryLeave(Salary model)
		{
			if (ModelState.IsValid)
			{
				var employee = await _employeeRepository.GetEmployeeById(model.EmployeeId,null,null);
				if (employee == null)
				{
					ModelState.AddModelError("", "Employee does not exist.");
					return View();
				}

				// Create and insert salary record
				var salary = new Salary
				{
					EmployeeId = model.EmployeeId // EmployeeId from form or view model
				};

				await _adminRepository.SaveInDb(salary);

				var Leavebalance = new LeaveBalance
				{
					EmployeeId = model.EmployeeId,
					MaxDays = 2,
					UsedDays = 0
				};

				await _adminRepository.SaveInDb(Leavebalance);

				int empid=model.EmployeeId;
				return RedirectToAction("UploadImage", new { employeeId = empid });
			}

			return View(model);
		}

		public IActionResult UploadImage(int employeeId)
		{
			EmployeeImage image = new EmployeeImage { EmployeeId = employeeId };
			return View(image);
		}

		// POST: Upload Image
		[HttpPost]
		public async Task<IActionResult> UploadImage(int employeeId, IFormFile imageFile)
		{
			if (imageFile != null && imageFile.Length > 0)
			{
				using (var memoryStream = new MemoryStream())
				{
					await imageFile.CopyToAsync(memoryStream);
					var employeeImage = new EmployeeImage
					{
						EmployeeId = employeeId,
						Image = memoryStream.ToArray(),
						ImageName = imageFile.FileName,
						ContentType = imageFile.ContentType
					};

					await _adminRepository.SaveInDb(employeeImage);
				}

				return RedirectToAction("Index", new { id = employeeId });
			}

			return View();
		}

		[HttpGet]
		public async Task<IActionResult> EditEmployee(int id)
		{
			var employee = await _employeeRepository.GetEmployeeById(id,null,null);

			if (employee == null)
			{
				return NotFound();
			}

			return View(employee);  // Return the employee to the edit view
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> EditEmployee(Employee updatedEmployee)
		{
			if (ModelState.IsValid)
			{
				var employee = await _employeeRepository.GetEmployeeById(updatedEmployee.EmployeeId,null,null);

				if (employee != null)
				{
					// Update only the specific fields
					employee.FirstName = updatedEmployee.FirstName;
					employee.LastName = updatedEmployee.LastName;
					employee.Email = updatedEmployee.Email;
					employee.Phone_no = updatedEmployee.Phone_no;
					employee.Address = updatedEmployee.Address;
					employee.DOB = updatedEmployee.DOB;
					employee.Gender = updatedEmployee.Gender;
					employee.JoinDate = updatedEmployee.JoinDate;

					// Save the changes to the database
					await _adminRepository.SaveInDb(employee);
					return RedirectToAction("Index", new { id = employee.EmployeeId });  // Redirect to the details view after saving
				}
			}

			return View(updatedEmployee);
		}

		public async Task<IActionResult> TimeSheet(int id)
		{
			var timesheets = await _employeeRepository.GetTimesheetList(id);

			return View(timesheets);
		}

    }
}

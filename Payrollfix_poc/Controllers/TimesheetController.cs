using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Filters;
using Payrollfix_poc.Models;
using Payrollfix_poc.IRepository;

namespace Payrollfix_poc.Controllers
{
	[CustomAuthorize]
	[AsyncCustomResultFilter]
	public class TimesheetController : Controller
	{
		private readonly IEmployeeRepository _employeeRepository;
		private readonly IAdminRepository _adminRepository;

		public TimesheetController(IEmployeeRepository repository,IAdminRepository adminRepository)
		{
			_employeeRepository=repository;
			_adminRepository=adminRepository;
		}
		// Action to display all timesheets
		public async Task<IActionResult> TimesheetList()
		{
			var id = HttpContext.Session.GetInt32("EmployeeId");
			var timesheets = await _employeeRepository.GetTimesheetList(id);
            ViewData["ActiveTimesheet"] = "active";
            return View(timesheets);
		}
		// GET: Timesheet/Create
		public IActionResult Create()
		{
			Timesheet timesheet = new Timesheet(); 
			int id=(int)HttpContext.Session.GetInt32("EmployeeId");
			timesheet.EmployeeId = id;
			return View(timesheet);
		}

		// POST: Timesheet/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Timesheet timesheet)
		{
			if (ModelState.IsValid)
			{
				await _adminRepository.SaveTimesheet(timesheet);
				return RedirectToAction("TimesheetList");
			}
			return View(timesheet);
		}

		// GET: Timesheet/Edit/5
		public async Task<IActionResult> Edit(int id)
		{
			Timesheet timesheet = new Timesheet();
			int Empid = (int)HttpContext.Session.GetInt32("EmployeeId");
			//timesheet.EmployeeId = id;
			timesheet = await _employeeRepository.GetTimesheetById(id);
			if (timesheet == null)
			{
				return NotFound();
			}
			return View(timesheet);
		}

		// POST: Timesheet/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Timesheet timesheet)
		{
			if (id != timesheet.TimesheetId)
			{
				return NotFound();
			}

			timesheet.EmployeeId = (int)HttpContext.Session.GetInt32("EmployeeId");
			if (ModelState.IsValid)
			{
				await _adminRepository.UpdateTimesheet(timesheet);
				return RedirectToAction("TimesheetList");
			}
			return View(timesheet);
		}

		// GET: Timesheet/Delete/5
		public async Task<IActionResult> Delete(int id)
		{
			var timesheet = await _employeeRepository.GetTimesheetById(id);
			if (timesheet == null)
			{
				return NotFound();
			}
			return View(timesheet);
		}

		// POST: Timesheet/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var timesheet = await _employeeRepository.GetTimesheetById(id);
			await _adminRepository.DeleteTimesheet(timesheet);
			return RedirectToAction("TimesheetList");
		}

		public async Task<IActionResult> Permission(int employeeId)
		{
			var employee = await _employeeRepository.GetEmployeeDetails(employeeId);

			if (employee == null)
			{
				return NotFound();
			}

			var pendingTimesheet = employee.Timesheets
				.Where(t => t.Status == "Pending")
				.ToList();

			return View(pendingTimesheet);
		}
		[HttpPost]
		public async Task<IActionResult> ApproveTimesheet(int sheetId)
		{
			var timesheet = await _employeeRepository.GetTimesheetById(sheetId);
			if (timesheet == null)
			{
				return NotFound();
			}

			// Approve the leave and save changes
			timesheet.Status = "Approved";
			await _adminRepository.UpdateTimesheet(timesheet);

			return RedirectToAction("Permission", new { employeeId = timesheet.EmployeeId });
		}
		// Action to reject leave
		[HttpPost]
		public async Task<IActionResult> RejectTimesheet(int sheetId)
		{
			var timesheet = await _employeeRepository.GetTimesheetById(sheetId);
			if (timesheet == null)
			{
				return NotFound();
			}

			// Reject the leave and save changes
			timesheet.Status = "Rejected";
			await _adminRepository.UpdateTimesheet(timesheet);

			return RedirectToAction("Permission", new { employeeId = timesheet.EmployeeId });
		}
	}
}

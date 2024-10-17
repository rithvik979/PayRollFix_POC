using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Controllers
{
	public class TimesheetController : Controller
	{
		public readonly PayRollFix_pocContext _context;

		public TimesheetController(PayRollFix_pocContext context)
		{
			_context=context;
		}
		// Action to display all timesheets
		public IActionResult TimesheetList()
		{
			var id = HttpContext.Session.GetInt32("EmployeeId");
			var timesheets = _context.Timesheets
				.Where(t=>t.EmployeeId==id)
				.OrderByDescending(t => t.Date) // Order by most recent first
				.ToList();
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
		public IActionResult Create(Timesheet timesheet)
		{
			if (ModelState.IsValid)
			{
				_context.Timesheets.Add(timesheet);
				_context.SaveChanges();
				return RedirectToAction("TimesheetList");
			}
			return View(timesheet);
		}

		// GET: Timesheet/Edit/5
		public IActionResult Edit(int id)
		{
			Timesheet timesheet = new Timesheet();
			int Empid = (int)HttpContext.Session.GetInt32("EmployeeId");
			timesheet.EmployeeId = id;
			timesheet = _context.Timesheets.Find(id);
			if (timesheet == null)
			{
				return NotFound();
			}
			return View(timesheet);
		}

		// POST: Timesheet/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(int id, Timesheet timesheet)
		{
			if (id != timesheet.TimesheetId)
			{
				return NotFound();
			}

			timesheet.EmployeeId = (int)HttpContext.Session.GetInt32("EmployeeId");
			if (ModelState.IsValid)
			{
				_context.Update(timesheet);
				_context.SaveChanges();
				return RedirectToAction("TimesheetList");
			}
			return View(timesheet);
		}

		// GET: Timesheet/Delete/5
		public IActionResult Delete(int id)
		{
			var timesheet = _context.Timesheets.Find(id);
			if (timesheet == null)
			{
				return NotFound();
			}
			return View(timesheet);
		}

		// POST: Timesheet/Delete/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeleteConfirmed(int id)
		{
			var timesheet = _context.Timesheets.Find(id);
			_context.Timesheets.Remove(timesheet);
			_context.SaveChanges();
			return RedirectToAction("TimesheetList");
		}
	}
}

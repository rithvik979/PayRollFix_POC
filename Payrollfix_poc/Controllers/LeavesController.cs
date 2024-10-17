﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Payrollfix_poc.Data;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Controllers
{
    public class LeaveController : Controller
    {
        private readonly PayRollFix_pocContext _context;

        public LeaveController(PayRollFix_pocContext context)
        {
            _context = context;
        }

        // Action method to retrieve leaves for a specific employee
        public async Task<IActionResult> LeaveDetails()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");

            // Get leave details for the specific employee
            var leaves = await _context.Leaves
                .Where(l => l.EmployeeId == employeeId)
                .Include(l => l.Employee) // Include Employee details
                .ToListAsync();

            // Get leave balance for the specific employee
            var leaveBalance = await _context.LeaveBalances
                .FirstOrDefaultAsync(lb => lb.EmployeeId == employeeId);

            // Combine both into a view model
            var model = new LeaveViewModel
            {
                Leaves = leaves,
                LeaveBalance = leaveBalance
            };

			ViewData["ActiveLeaves"] = "active";
			return View(model);
        }

		[HttpPost]
		public IActionResult ApplyLeave(Leave leave)
		{
			if (ModelState.IsValid)
			{
				leave.EmployeeId = (int)HttpContext.Session.GetInt32("EmployeeId");
				// Get the employee's leave balance
				var leaveBalance = _context.LeaveBalances.FirstOrDefault(lb => lb.EmployeeId == leave.EmployeeId);

				// Calculate the total leave days
				int totalLeaveDays = (leave.EndDate - leave.StartDate).Days + 1;

				// Check if the employee has enough remaining days
				if (totalLeaveDays > (leaveBalance.MaxDays - leaveBalance.UsedDays))
				{
					ViewBag.Message="You don't have enough leave days.";
					return View("_ApplyLeave");
				}

				// Update the used days in leave balance
				leaveBalance.UsedDays += totalLeaveDays;

				// Add the new leave to the database
				_context.Leaves.Add(leave);
				_context.SaveChanges();

				// Redirect back to the leave overview page
				return RedirectToAction("LeaveDetails");
			}

			// Return the same view with validation errors
			return View("_ApplyLeave", leave);
		}


	}
}

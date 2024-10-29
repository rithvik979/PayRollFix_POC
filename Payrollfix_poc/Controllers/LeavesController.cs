using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Data;
using Payrollfix_poc.Filters;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;

namespace Payrollfix_poc.Controllers
{
    [CustomAuthorize]
    public class LeaveController : Controller
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IAdminRepository _adminRepository;

        public LeaveController(PayRollFix_pocContext context,IEmployeeRepository repository,IAdminRepository admin)
        {
            _employeeRepository = repository;
            _adminRepository = admin;
        }

        // Action method to retrieve leaves for a specific employee
        public async Task<IActionResult> LeaveDetails()
        {
            var employeeId = HttpContext.Session.GetInt32("EmployeeId");

            // Get leave details for the specific employee
            var leaves = await _employeeRepository.GetLeaves(employeeId);

            // Get leave balance for the specific employee
            var leaveBalance = await _employeeRepository.GetLeaveBalance(employeeId);

            // Combine both into a view model
            var model = new LeaveViewModel
            {
                Leaves = leaves,
                LeaveBalance = leaveBalance
            };
			ViewBag.Position = (await _employeeRepository.GetEmployeeById(employeeId, null, null)).Position;
			ViewData["ActiveLeaves"] = "active";
			return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> ApplyLeave(Leave leave)
		{
			if (ModelState.IsValid)
			{
				leave.EmployeeId = (int)HttpContext.Session.GetInt32("EmployeeId");
				// Get the employee's leave balance
				var leaveBalance = await _employeeRepository.GetLeaveBalance(leave.EmployeeId);

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
				await _adminRepository.SaveLeave(leave);
                await _adminRepository.UpdateLeaveBalance(leaveBalance);

				// Redirect back to the leave overview page
				return RedirectToAction("LeaveDetails");
			}

			// Return the same view with validation errors
			return View("_ApplyLeave", leave);
		}

        public async Task<IActionResult> Permission(int employeeId)
        {
			var id = HttpContext.Session.GetInt32("EmployeeId");
			var employee = await _employeeRepository.GetEmployeeDetails(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var pendingLeaves = employee.Leaves
                .Where(l => l.Status == "Pending")
                .ToList();

			ViewBag.Position = (await _employeeRepository.GetEmployeeById(id, null, null)).Position;
			return View(pendingLeaves);
        }
        // Action to approve leave
        [HttpPost]
        public async Task<IActionResult> ApproveLeave(int leaveId)
        {
            var leave = await _employeeRepository.GetLeaveById(leaveId);
            if (leave == null)
            {
                return NotFound();
            }

            // Approve the leave and save changes
            leave.Status = "Approved";
            await _adminRepository.UpdateLeave(leave);

            return RedirectToAction("Permission", new {employeeId=leave.EmployeeId});
        }
        // Action to reject leave
        [HttpPost]
        public async Task<IActionResult> RejectLeave(int leaveId)
        {
            var leave = await _employeeRepository.GetLeaveById(leaveId);
            if (leave == null)
            {
                return NotFound();
            }

            // Reject the leave and save changes
            leave.Status = "Rejected";
            await _adminRepository.UpdateLeave(leave);

            return RedirectToAction("Permission", new { employeeId = leave.EmployeeId });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Models;
using Payrollfix_poc.IRepository;
using Payrollfix_poc.Filters;

namespace Payrollfix_poc.Controllers
{
    [CustomAuthorize]
	[AsyncCustomResultFilter]
	public class ExpenseController : Controller
    {
        public readonly IEmployeeRepository _employeeRepository;
        public readonly IAdminRepository _adminRepository;
        public ExpenseController(IEmployeeRepository repository, IAdminRepository admin)
        {
            _employeeRepository = repository;
            _adminRepository = admin;
        }

        // GET: Show all expenses grouped by month and week
        public async Task<IActionResult> Index()
        {
            var expenses = await _employeeRepository.GetExpenseList();
            var id = HttpContext.Session.GetInt32("EmployeeId");
            var groupedExpenses = await _employeeRepository.GroupedExpenses(id, expenses);
			ViewData["ActiveExpenses"] = "active";
            return View(groupedExpenses);
        }

        // GET: Add Expense
        public IActionResult AddExpense()
        {
            return PartialView("_AddExpense");
        }

        // POST: Save Expense
        [HttpPost]
        public async Task<IActionResult> AddExpense(Expense model)
        {
            if (ModelState.IsValid)
            {
                var employeeId = HttpContext.Session.GetInt32("EmployeeId");
                if (employeeId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                model.EmployeeId = employeeId.Value;  // Assign employee ID
                await _adminRepository.Save(model);

                return RedirectToAction("Index");
            }

            return PartialView("_AddExpense", model);
        }

    }
}

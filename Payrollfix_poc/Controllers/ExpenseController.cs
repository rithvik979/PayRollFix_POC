using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Models;
using Payrollfix_poc.ViewModels;
using Payrollfix_poc.Data;  
using System.Linq;
using System.Globalization;
using Microsoft.EntityFrameworkCore;

namespace Payrollfix_poc.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly PayRollFix_pocContext _context;

        public ExpenseController(PayRollFix_pocContext context)
        {
            _context = context;
        }

        // GET: Show all expenses grouped by month and week
        public IActionResult Index()
        {
            var expenses = _context.Expenses.ToList();
            var id = HttpContext.Session.GetInt32("EmployeeId");
            var groupedExpenses = expenses
                .Where(e => e.EmployeeId == id)
                .GroupBy(e => new { Month = e.Date.Month, Year = e.Date.Year })
                .Select(monthGroup => new ExpenseGroupViewModel
                {
                    Month = monthGroup.Key.Month,
                    Year = monthGroup.Key.Year,
                    WeekGroups = monthGroup
                        .GroupBy(e => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(e.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                        .Select(weekGroup => new WeekGroupViewModel
                        {
                            WeekNumber = weekGroup.Key,
                            Expenses = weekGroup.ToList()
                        })
                        .ToList()
                })
                .ToList();
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
        public IActionResult AddExpense(Expense model)
        {
            if (ModelState.IsValid)
            {
                var employeeId = HttpContext.Session.GetInt32("EmployeeId");
                if (employeeId == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                model.EmployeeId = employeeId.Value;  // Assign employee ID
                _context.Expenses.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return PartialView("_AddExpense", model);
        }

    }
}

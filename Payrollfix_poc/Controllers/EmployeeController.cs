using Microsoft.AspNetCore.Mvc;
using Payrollfix_poc.Models;

namespace Payrollfix_poc.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Details()
        {
            var employee = new Employee()
            {
                FirstName = "Rithvik",
                LastName = "Reddy",
                EmployeeId = 1004,
                Email="Grithvik@gmail.com",
                DOB = new DateOnly(1995, 06, 04),
                Phone_no = 123456789,
                Address = "Tarnaka, Hyderabad",
                Gender = "Male",
                JoinDate = new DateOnly(2020,08,20)
            };
            return View(employee);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Attandence()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payrollfix_poc.IRepository;

namespace Payrollfix_poc.Filters
{
    public class AsyncCustomResultFilter : IAsyncResultFilter
    {
        public readonly IEmployeeRepository _employeeRepository;
        public AsyncCustomResultFilter(IEmployeeRepository repository)
        {
            _employeeRepository = repository;
        }
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var id = context.HttpContext.Session.GetInt32("EmployeeId");
            if (context.Controller is Controller controller)
            {
                // Set ViewData before the result (e.g., view) is rendered
                controller.ViewBag.Position = (await _employeeRepository.GetEmployeeById(id, null, null)).Position;
            }
            
        }
    }
}


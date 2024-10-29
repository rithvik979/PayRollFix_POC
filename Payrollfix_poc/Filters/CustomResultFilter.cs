using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payrollfix_poc.IRepository;
using System.Threading.Tasks;

public class AsyncCustomResultFilterAttribute : TypeFilterAttribute
{
	public AsyncCustomResultFilterAttribute() : base(typeof(AsyncCustomResultFilterImpl))
	{
	}

	private class AsyncCustomResultFilterImpl : IAsyncResultFilter
	{
		private readonly IEmployeeRepository _employeeRepository;

		public AsyncCustomResultFilterImpl(IEmployeeRepository employeeRepository)
		{
			_employeeRepository = employeeRepository;
		}

		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			var id = context.HttpContext.Session.GetInt32("EmployeeId");
			if (context.Controller is Controller controller && id.HasValue)
			{
				var employee = await _employeeRepository.GetEmployeeById(id, null, null);
				controller.ViewBag.Position = employee?.Position;
			}
			await next();
		}
	}
}

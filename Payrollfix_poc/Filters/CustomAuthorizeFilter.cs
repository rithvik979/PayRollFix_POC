using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Payrollfix_poc.Filters
{
    public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public string Role { get; set; } 

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var employeeId = context.HttpContext.Session.GetInt32("EmployeeId");
            var userRole = context.HttpContext.Session.GetString("Role");

            // Skip the filter for the login page to avoid loops
            if (context.ActionDescriptor.DisplayName.Contains("Login"))
            {
                return;
            }

            // Check if the user is authenticated
            if (employeeId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
                return;
            }

            // Check if the user has the required role
            if (!string.IsNullOrEmpty(Role) && !Role.Equals(userRole, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new RedirectToActionResult("Unauthorized", "Home", null);
            }
        }
    }
}

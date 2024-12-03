using Microsoft.AspNetCore.SignalR;

namespace Payrollfix_poc.SignalR_Hub
{
    public class SessionUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            // Access the HttpContext to get session data
            var httpContext = connection.GetHttpContext();

            // Retrieve the user ID from session; ensure it's stored as a string
            return httpContext.Session.GetString("EmployeeId");
        }
    }
}

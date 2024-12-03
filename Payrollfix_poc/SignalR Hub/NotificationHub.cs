using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;


namespace Payrollfix_poc.SignalR_Hub
{
	public class NotificationHub : Hub
	{
        // Method to send a notification to a specific user with updated data (e.g., leave status)
        public async Task NotifyLeaveStatusUpdate(string leaveId, string newStatus)
        {
            // Sends notification to the user about leave status update
            await Clients.Caller.SendAsync("ReceiveLeaveStatusUpdate", leaveId, newStatus);
        }

        // You can also broadcast to all users if necessary
        public async Task BroadcastLeaveStatus(string leaveId, string newStatus)
		{
			await Clients.All.SendAsync("UpdateLeaveStatus", leaveId, newStatus);
		}
	}
}

using Payrollfix_poc.Models;

namespace Payrollfix_poc.ViewModels
{
    public class LeaveViewModel
    {
        public List<Leave> Leaves { get; set; }
        public LeaveBalance LeaveBalance { get; set; }
    }
}

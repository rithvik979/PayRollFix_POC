using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payrollfix_poc.Models
{
    public class LeaveBalance
    {
        [Key]
        public int BalanceId {  get; set; }
        public int UsedDays { get; set; }
        public int MaxDays { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId {  get; set; }
        public Employee Employee { get; set; }
    }
}

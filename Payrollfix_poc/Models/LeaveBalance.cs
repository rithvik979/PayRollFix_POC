using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.Models
{
    public class LeaveBalance
    {
        [Key]
        public int BalanceId {  get; set; }
        public int? UsedDays { get; set; }
        public int MaxDays { get; set; } = 2;
        [Required]
        public int EmployeeId {  get; set; }
        [ValidateNever]
        public Employee Employee { get; set; }
    }
}

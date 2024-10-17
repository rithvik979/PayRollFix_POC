using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace Payrollfix_poc.Models
{
    public class Leave
    {
        [Key]
        public int LeaveId { get; set; }
        [Required]
        public string LeaveType { get; set; }
        [Required]
        public DateTime StartDate { get; set;}
        [Required]
        public DateTime EndDate { get; set;}
        [Required]
        public string Status { get; set; } = "Pending";
        [Required]
        public string Reason { get; set; }
        [Required]
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
        [ValidateNever]
        public Employee Employee { get; set; }
    }
}

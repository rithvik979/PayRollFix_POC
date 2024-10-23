using System;
using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.Models
{
    public class Timesheet
    {
        [Key]
        public int TimesheetId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }   // Date of the timesheet entry

        [Required]
        [Range(0, 24, ErrorMessage = "Hours must be between 0 and 24.")]
        public decimal HoursWorked { get; set; }  // Number of hours worked that day

        [StringLength(500)]
        public string Description { get; set; }  // Description of work done

        public DateTime CreatedAt { get; set; } = DateTime.Now;  // Auto-fill current timestamp

        public string Status { get; set; } = "Pending";

        [Required]
        public int EmployeeId { get; set; }
    }
}

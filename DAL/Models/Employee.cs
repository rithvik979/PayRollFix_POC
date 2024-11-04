using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Payrollfix_poc.Models
{
    public class Employee
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Invalid EmailId")]
        public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[Required]
        [Phone]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
		public string Phone_no { get; set; }
        [Required]
        public string Address { get; set; }
        [DataType(DataType.Date)]
        public DateOnly DOB { get; set; }
        [Required]
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        [Required]
        public DateOnly JoinDate { get; set; }
        [AllowNull]
        public string? Department { get; set; }
        [AllowNull]
        public string? Position { get; set; }
		[AllowNull]
        public int? ManagerId { get; set; }
        public bool IsManager { get; set; }
        [ValidateNever]
        public ICollection<LoginActivity> LoginActivities { get; set; }
        [ValidateNever]
		public ICollection<Attandence> Attandences { get; set; }  // Navigation property for Attendance
        [ValidateNever]
        public ICollection<Leave> Leaves { get; set; }
        [ValidateNever]
        public ICollection<LeaveBalance> LeaveBalances { get; set; }
        [ValidateNever]
        public ICollection<Timesheet> Timesheets { get; set; }
        [ValidateNever]
        public ICollection<Expense> Expenses { get; set; }
        [ValidateNever]
        public ICollection<Salary> Salarys { get; set; }
    }
}

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
        [Required]
        public Department Department { get; set; }
        [AllowNull]
        public Position? Position { get; set; }
        [Column("Manager")]
        [AllowNull]
        public Employee? employee { get; set; }

		public ICollection<LoginActivity> LoginActivities { get; set; }
		public ICollection<Attandence> Attandences { get; set; }  // Navigation property for Attendance
        public ICollection<Leave> Leaves { get; set; }
        public ICollection<LeaveBalance> LeaveBalances { get; set; }


	}
}

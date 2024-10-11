using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payrollfix_poc.Models
{
	public class Attandence
	{
		[Key]
		public int AttendanceId { get; set; }

		[Required]
		public DateOnly Date { get; set; }

		public DateTime? CheckInTime { get; set; }  // Made nullable to handle cases where check-in hasn't occurred
		public DateTime? CheckOutTime { get; set; } // Made nullable to handle cases where check-out hasn't occurred

		public float? WorkHours { get; set; }

		[Required]
		public string Status { get; set; } // Present, Absent, Not In Yet, On Leave

		// Foreign Key
		[ForeignKey("Employee")]
		public int EmployeeId { get; set; }

		// Navigation Property
		public Employee Employee { get; set; }
	}
}

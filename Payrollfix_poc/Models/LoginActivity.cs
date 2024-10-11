using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payrollfix_poc.Models
{
	public class LoginActivity
	{
		[Key]
		public int ActivityId { get; set; } // Primary Key
		public DateTime LoginTime { get; set; }
		public DateTime? LogoutTime { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
		public Employee Employee { get; set; } // Navigation property for foreign key
	}

}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payrollfix_poc.Models
{
    public class Employee
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public int Phone_no { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Date)]
        public DateOnly DOB { get; set; }
        public string Gender { get; set; }
        [DataType(DataType.Date)]
        public DateOnly JoinDate { get; set; }
        public Department Department { get; set; }
        public Position Position { get; set; }
        [Column("Manager")]
        public Employee employee { get; set; }
    }
}

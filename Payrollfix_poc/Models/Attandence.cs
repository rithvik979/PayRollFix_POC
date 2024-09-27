using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.Models
{
    public class Attandence
    {
        [Key]
        public int AttendanceId { get; set; }
        public DateOnly Date { get; set; }
        public DateOnly CheckInTime { get; set; }
        public DateOnly CheckOutTime { get; set; }
        public float WorkHours { get; set; }
        public string Status { get; set; } // Present, Absent, Not In Yet, On Leave
        public Employee Employee { get; set; }
    }
}

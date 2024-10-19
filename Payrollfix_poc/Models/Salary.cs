using Microsoft.CodeAnalysis.FlowAnalysis;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Payrollfix_poc.Models
{
    public class Salary
    {
        [Key]
        public int SalaryId {  get; set; }
        public int BasicSalary {  get; set; }
        public int NetSalary { get; set; }
        public int TotalDeductions {  get; set; }
        public int MonthySalary { get; set; }
        [ForeignKey("EmployeeId")]
        [Required]
        public int EmployeeId {  get; set; }
    }
}

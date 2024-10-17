using System;
using System.ComponentModel.DataAnnotations;

namespace Payrollfix_poc.Models
{
	public class Expense
	{
		[Key]
		public int ExpenseId { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[Required]
		[StringLength(100)]
		public string ExpenseType { get; set; }

		[Required]
		[StringLength(100)]
		public string Category { get; set; }

		[Required]
		[Range(0, double.MaxValue, ErrorMessage = "Amount must be a positive number.")]
		public decimal Amount { get; set; }

		[StringLength(500)]
		public string Description { get; set; }
		[Required]
		public string Status { get; set; } = "Pending";
		[Required]
		public int EmployeeId { get; set; }
	}
}


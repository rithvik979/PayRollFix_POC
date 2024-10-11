namespace Payrollfix_poc.Models
{
	public class TimeSheetViewModel
	{
		public int EmployeeId { get; set; }
		public string EmployeeName { get; set; }
		public List<LoginActivity> Activities { get; set; }
	}
}

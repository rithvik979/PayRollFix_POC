namespace Payrollfix_poc.Models
{
    public class ExpenseGroupViewModel
    {
        public int Month { get; set; }  // Month number
        public int Year { get; set; }   // Year number
        public IEnumerable<WeekGroupViewModel> WeekGroups { get; set; }  // Grouped by weeks
    }

    public class WeekGroupViewModel
    {
        public int WeekNumber { get; set; }  // Week number in the month
        public IEnumerable<Expense> Expenses { get; set; }  // Expenses in that week
    }
}

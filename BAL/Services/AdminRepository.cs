using Payrollfix_poc.Data;
using Payrollfix_poc.Models;
using System.Threading.Tasks;

namespace Payrollfix_poc.Services
{
    public class AdminRepository : IAdminRepository
    {
        public readonly PayRollFix_pocContext _context;
        public AdminRepository(PayRollFix_pocContext context)
        {
            _context = context;
        }

        public async Task Save<T>(T entity) where T : class
		{
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

		public async Task Update<T>(T entity) where T : class
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTimesheet(Timesheet timesheet)
        {
            _context.Timesheets.Remove(timesheet);
            await _context.SaveChangesAsync();
        }
    }
}

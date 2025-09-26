using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class PayrollRepo:IPayrollRepo
    {
        private readonly VertexHRMSDbContext _context;

        public PayrollRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payroll>> GetAllAsync()
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .Include(p => p.Deductions)
                    .ThenInclude(d => d.Deduction)
                .ToListAsync();
        }

        public async Task<Payroll> GetByIdAsync(int id)
        {
            return await _context.Payrolls
                .Include(p => p.Employee)
                .Include(p => p.Deductions)
                    .ThenInclude(d => d.Deduction)
                .FirstOrDefaultAsync(p => p.PayrollId == id);
        }

        public async Task AddAsync(Payroll payroll)
        {
            _context.Payrolls.Add(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Payroll payroll)
        {
            _context.Payrolls.Update(payroll);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var payroll = await _context.Payrolls.FindAsync(id);
            if (payroll != null)
            {
                _context.Payrolls.Remove(payroll);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class PayrollDeductionRepo:IPayrollDeductionRepo
    {
        private readonly VertexHRMSDbContext _context;

        public PayrollDeductionRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayrollDeduction>> GetAllAsync()
        {
            return await _context.PayrollDeductions
                .Include(d => d.Deduction)
                .Include(d => d.Payroll)
                .ToListAsync();
        }

        public async Task<PayrollDeduction> GetByIdAsync(int id)
        {
            return await _context.PayrollDeductions
                .Include(d => d.Deduction)
                .Include(d => d.Payroll)
                .FirstOrDefaultAsync(d => d.PayrollDeductionId == id);
        }

        public async Task AddAsync(PayrollDeduction deduction)
        {
            _context.PayrollDeductions.Add(deduction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PayrollDeduction deduction)
        {
            _context.PayrollDeductions.Update(deduction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ded = await _context.PayrollDeductions.FindAsync(id);
            if (ded != null)
            {
                _context.PayrollDeductions.Remove(ded);
                await _context.SaveChangesAsync();
            }
        }
    }
}

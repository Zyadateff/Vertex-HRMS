namespace VertexHRMS.DAL.Repo.Implementation
{
    public class PayrollRunRepo:IPayrollRunRepo
    {
        private readonly VertexHRMSDbContext _context;

        public PayrollRunRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PayrollRun>> GetAllAsync()
        {
            return await _context.PayrollRuns
                .Include(r => r.Payrolls)
                    .ThenInclude(p => p.Employee)
                .ToListAsync();
        }

        public async Task<PayrollRun> GetByIdAsync(int id)
        {
            return await _context.PayrollRuns
                .Include(r => r.Payrolls)
                    .ThenInclude(p => p.Deductions)
                        .ThenInclude(d => d.Deduction)
                .FirstOrDefaultAsync(r => r.PayrollRunId == id);
        }

        public async Task AddAsync(PayrollRun payrollRun)
        {
            _context.PayrollRuns.Add(payrollRun);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PayrollRun payrollRun)
        {
            _context.PayrollRuns.Update(payrollRun);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var run = await _context.PayrollRuns.FindAsync(id);
            if (run != null)
            {
                _context.PayrollRuns.Remove(run);
                await _context.SaveChangesAsync();
            }
        }
    }
}

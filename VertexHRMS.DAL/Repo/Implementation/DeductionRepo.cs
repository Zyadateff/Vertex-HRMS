namespace VertexHRMS.DAL.Repo.Implementation
{
    public class DeductionRepo:IDeductionRepo
    {
        private readonly VertexHRMSDbContext _context;

        public DeductionRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<Deduction?> GetByIdAsync(int id)
        {
            return await _context.Deductions
                .Include(d => d.PayrollDeductions) 
                .FirstOrDefaultAsync(d => d.DeductionId == id);
        }

        public async Task<List<Deduction>> GetAllAsync()
        {
            return await _context.Deductions
                .Include(d => d.PayrollDeductions)
                .ToListAsync();
        }

        public async Task AddAsync(Deduction deduction)
        {
            await _context.Deductions.AddAsync(deduction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Deduction deduction)
        {
            _context.Deductions.Update(deduction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var deduction = await GetByIdAsync(id);
            if (deduction != null)
            {
                _context.Deductions.Remove(deduction);
                await _context.SaveChangesAsync();
            }
        }
    }
}

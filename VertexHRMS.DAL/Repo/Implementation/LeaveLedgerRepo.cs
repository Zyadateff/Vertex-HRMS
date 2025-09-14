

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveLedgerRepo : ILeaveLedgerRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeaveLedgerRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeaveLedger ledgerEntry)
        {
            await _context.LeaveLedgers.AddAsync(ledgerEntry);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int ledgerId)
        {
            var ledger = await _context.LeaveLedgers
            .FirstOrDefaultAsync(l => l.LeaveLedgerId == ledgerId);

            if (ledger != null)
            {
                _context.LeaveLedgers.Remove(ledger);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeaveLedger>> GetByEmployeeAsync(int employeeId, int year)
        {
            return await _context.LeaveLedgers
            .Include(l => l.LeaveType)
            .Where(l => l.EmployeeId == employeeId
                     && l.EffectiveDate.Year == year)
            .ToListAsync();
        }

        public async Task<LeaveLedger> GetByIdAsync(int ledgerId)
        {
            return await _context.LeaveLedgers
            .Include(l => l.Employee)
            .Include(l => l.LeaveType)
            .FirstOrDefaultAsync(l => l.LeaveLedgerId == ledgerId);
        }

        public async Task<IEnumerable<LeaveLedger>> GetByLeaveTypeAsync(int employeeId, int leaveTypeId, int year)
        {
            return await _context.LeaveLedgers
            .Where(l => l.EmployeeId == employeeId
                     && l.LeaveTypeId == leaveTypeId
                     && l.EffectiveDate.Year == year)
            .ToListAsync();
        }
    }
}

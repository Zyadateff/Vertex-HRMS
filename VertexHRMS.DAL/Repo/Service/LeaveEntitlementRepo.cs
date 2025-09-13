

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveEntitlementRepo : ILeaveEntitlementRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeaveEntitlementRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeaveEntitlement entitlement)
        {
            await _context.LeaveEntitlements.AddAsync(entitlement);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int entitlementId)
        {
            var entitlement = await _context.LeaveEntitlements
            .FirstOrDefaultAsync(e => e.LeaveEntitlementId == entitlementId);

            if (entitlement != null)
            {
                _context.LeaveEntitlements.Remove(entitlement);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<LeaveEntitlement?> GetByEmployeeAndTypeAsync(int employeeId, int leaveTypeId, int year)
        {
            return await _context.LeaveEntitlements
                .Include(e => e.Employee)
                .Include(e => e.LeaveType)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId
                                       && e.LeaveTypeId == leaveTypeId
                                       && e.Year == year);
        }

        public async Task<IEnumerable<LeaveEntitlement>> GetByEmployeeAsync(int employeeId, int year)
        {
            return await _context.LeaveEntitlements
           .Include(e => e.LeaveType)
           .Where(e => e.EmployeeId == employeeId && e.Year == year)
           .ToListAsync();
        }

        public async Task<LeaveEntitlement> GetByIdAsync(int entitlementId)
        {
            var result= await _context.LeaveEntitlements
            .Include(e => e.Employee)
            .Include(e => e.LeaveType)
            .FirstOrDefaultAsync(e => e.LeaveEntitlementId == entitlementId);
            return result;
        }

        public async Task<LeaveEntitlement> GetEntitlementAsync(int employeeId, int leaveTypeId, int year)
        {
            var result= await _context.LeaveEntitlements
            .Include(e => e.Employee)
            .Include(e => e.LeaveType)
            .FirstOrDefaultAsync(e => e.EmployeeId == employeeId
                                   && e.LeaveTypeId == leaveTypeId
                                   && e.Year == year);
            return result;
        }

        public async Task UpdateAsync(LeaveEntitlement entitlement)
        {
            _context.LeaveEntitlements.Update(entitlement);
            await _context.SaveChangesAsync();
        }
    }
}

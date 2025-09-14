

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveTypeRepo : ILeaveTypeRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeaveTypeRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeaveType leaveType)
        {
            await _context.LeaveTypes.AddAsync(leaveType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int typeId)
        {
            var leaveType = await _context.LeaveTypes
            .FirstOrDefaultAsync(t => t.LeaveTypeId == typeId);

            if (leaveType != null)
            {
                _context.LeaveTypes.Remove(leaveType);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeaveType>> GetAllAsync()
        {
            return await _context.LeaveTypes
            .Include(t => t.Policies)
            .ToListAsync();
        }

        public async Task<LeaveType> GetByIdAsync(int typeId)
        {
            return await _context.LeaveTypes
            .Include(t => t.Policies)
            .FirstOrDefaultAsync(t => t.LeaveTypeId == typeId);
        }

        public async Task UpdateAsync(LeaveType leaveType)
        {
            _context.LeaveTypes.Update(leaveType);
            await _context.SaveChangesAsync();
        }
    }
}

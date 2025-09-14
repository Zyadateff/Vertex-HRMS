

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveRequestDayRepo : ILeaveRequestDayRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeaveRequestDayRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeaveRequestDay requestDay)
        {
            await _context.LeaveRequestDays.AddAsync(requestDay);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int dayId)
        {
            var day = await _context.LeaveRequestDays
            .FirstOrDefaultAsync(d => d.LeaveRequestDayId == dayId);

            if (day != null)
            {
                _context.LeaveRequestDays.Remove(day);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeaveRequestDay>> GetByDateAsync(DateTime date)
        {
            return await _context.LeaveRequestDays
            .Include(d => d.LeaveRequest)
            .Where(d => d.TheDate.Date == date.Date)
            .ToListAsync();
        }

        public async Task<LeaveRequestDay> GetByIdAsync(int dayId)
        {
            return await _context.LeaveRequestDays
            .Include(d => d.LeaveRequest)
            .FirstOrDefaultAsync(d => d.LeaveRequestDayId == dayId);
        }

        public async Task<IEnumerable<LeaveRequestDay>> GetByRequestIdAsync(int requestId)
        {
            return await _context.LeaveRequestDays
            .Where(d => d.LeaveRequestId == requestId)
            .Include(d => d.LeaveRequest)
            .ToListAsync();
        }

        public async Task UpdateAsync(LeaveRequestDay requestDay)
        {
            _context.LeaveRequestDays.Update(requestDay);
            await _context.SaveChangesAsync();
        }
    }
}

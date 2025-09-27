using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class LeaveRequestEmailRepo:ILeaveRequestEmailRepo
    {
        private readonly VertexHRMSDbContext _context;

        public LeaveRequestEmailRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<LeaveRequestEmail> GetByIdAsync(int id)
        {
            return await _context.LeaveRequestEmails
                                 .Include(x => x.LeaveRequest)
                                 .FirstOrDefaultAsync(x => x.LeaveRequestEmailId == id);
        }

        public async Task<IEnumerable<LeaveRequestEmail>> GetAllAsync()
        {
            return await _context.LeaveRequestEmails
                                 .Include(x => x.LeaveRequest)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequestEmail>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.LeaveRequestEmails
                                 .Include(x => x.LeaveRequest)
                                 .Where(x => x.EmployeeId == employeeId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequestEmail>> GetByRequestIdAsync(int requestId)
        {
            return await _context.LeaveRequestEmails
                                 .Include(x => x.LeaveRequest)
                                 .Where(x => x.LeaveRequestId == requestId)
                                 .ToListAsync();
        }

        public async Task AddAsync(LeaveRequestEmail entity)
        {
            await _context.LeaveRequestEmails.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(LeaveRequestEmail entity)
        {
            _context.LeaveRequestEmails.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.LeaveRequestEmails.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

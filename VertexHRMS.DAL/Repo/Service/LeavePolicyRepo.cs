using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeavePolicyRepo : ILeavePolicyRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeavePolicyRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeavePolicy policy)
        {
            await _context.LeavePolicies.AddAsync(policy);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int policyId)
        {
            var policy = await _context.LeavePolicies
            .FirstOrDefaultAsync(p => p.LeavePolicyId == policyId);

            if (policy != null)
            {
                _context.LeavePolicies.Remove(policy);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeavePolicy>> GetAllAsync()
        {
            return await _context.LeavePolicies
            .Include(p => p.LeaveType)
            .ToListAsync();
        }

        public async Task<LeavePolicy> GetByIdAsync(int policyId)
        {
            return await _context.LeavePolicies
            .Include(p => p.LeaveType)
            .FirstOrDefaultAsync(p => p.LeavePolicyId == policyId);
        }

        public async Task<LeavePolicy> GetByLeaveTypeAsync(int leaveTypeId)
        {
            return await _context.LeavePolicies
            .Include(p => p.LeaveType)
            .FirstOrDefaultAsync(p => p.LeaveTypeId == leaveTypeId);
        }

        public async Task UpdateAsync(LeavePolicy policy)
        {
            _context.LeavePolicies.Update(policy);
            await _context.SaveChangesAsync();
        }
    }
}

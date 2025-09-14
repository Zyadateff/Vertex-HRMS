

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveApprovalRepo : ILeaveApprovalRepo
    {
        readonly private VertexHRMSDbContext _context;
        public LeaveApprovalRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeaveApproval approval)
        {
            await _context.LeaveApprovals.AddAsync(approval);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int approvalId)
        {
            var approval = await _context.LeaveApprovals
            .FirstOrDefaultAsync(a => a.LeaveApprovalId == approvalId);
            if (approval != null)
            {
                _context.LeaveApprovals.Remove(approval);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeaveApproval>> GetByApproverAsync(string approverUserId)
        {
            return await _context.LeaveApprovals
            .Where(a => a.ApproverUserId == approverUserId)
            .ToListAsync();
        }

        public async Task<LeaveApproval> GetByIdAsync(int approvalId)
        {
            var approval= await _context.LeaveApprovals
           .Include(a => a.LeaveRequest)
           .Include(a => a.ApproverEmployee)
           .Include(a => a.ApproverUser)
           .FirstOrDefaultAsync(a => a.LeaveApprovalId == approvalId);
            return approval;
        }

        public async Task<IEnumerable<LeaveApproval>> GetByRequestAsync(int requestId)
        {
            return await _context.LeaveApprovals
            .Where(a => a.LeaveRequestId == requestId)
            .ToListAsync();
        }

        public async Task UpdateAsync(LeaveApproval approval)
        {
            _context.LeaveApprovals.Update(approval);
            await _context.SaveChangesAsync();
        }
    }
}

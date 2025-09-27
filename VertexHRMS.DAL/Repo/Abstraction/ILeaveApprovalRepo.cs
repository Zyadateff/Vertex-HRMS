
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveApprovalRepo
    {
        Task<LeaveApproval> GetByIdAsync(int approvalId);
        Task<IEnumerable<LeaveApproval>> GetByRequestAsync(int requestId);
        Task<IEnumerable<LeaveApproval>> GetByApproverAsync(string approverUserId);
        Task AddAsync(LeaveApproval approval);
        Task UpdateAsync(LeaveApproval approval);
        Task DeleteAsync(int approvalId);
    }
}

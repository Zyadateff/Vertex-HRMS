
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeavePolicyRepo
    {
        Task<LeavePolicy> GetByIdAsync(int policyId);
        Task<LeavePolicy> GetByLeaveTypeAsync(int leaveTypeId);
        Task<IEnumerable<LeavePolicy>> GetAllAsync();
        Task AddAsync(LeavePolicy policy);
        Task UpdateAsync(LeavePolicy policy);
        Task DeleteAsync(int policyId);
    }
}

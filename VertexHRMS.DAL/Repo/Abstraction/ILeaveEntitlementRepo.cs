
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveEntitlementRepo
    {
        Task<LeaveEntitlement> GetByIdAsync(int entitlementId);
        Task<LeaveEntitlement> GetEntitlementAsync(int employeeId, int leaveTypeId, int year);
        Task<IEnumerable<LeaveEntitlement>> GetByEmployeeAsync(int employeeId, int year);
        Task<LeaveEntitlement?> GetByEmployeeAndTypeAsync(int employeeId, int leaveTypeId, int year);
        Task AddAsync(LeaveEntitlement entitlement);
        Task UpdateAsync(LeaveEntitlement entitlement);
        Task DeleteAsync(int entitlementId);
    }
}


using VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveEntitlementService
    {
        Task<GetEntitlementVM> GetEntitlementAsync(int employeeId, int leaveTypeId, int year);
        Task<IEnumerable<GetAllForEmployeeVM>> GetAllForEmployeeAsync(int employeeId, int year);
        Task AssignEntitlementAsync(int employeeId, int leaveTypeId, decimal entitledDays, int year);
        Task UpdateEntitlementAsync(LeaveEntitlement entitlement);
        Task RemoveEntitlementAsync(int entitlementId);
    }
}

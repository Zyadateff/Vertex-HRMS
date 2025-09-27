
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveRequestRepo
    {
        Task<LeaveRequest> GetByIdAsync(int requestId);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync(int managerId);
        Task AddAsync(LeaveRequest request);
        Task UpdateAsync(LeaveRequest request);
        Task DeleteAsync(int requestId);
        Task<bool> HasTakenLeaveTypeThisYearAsync(int employeeId, int leaveTypeId);
        Task<int> CountByEmployeeAndMonthAsync(int employeeId, DateTime date);
        Task<int> CountApprovedOrPendingByDayAsync(int departmentId, DateTime date);
    }
}

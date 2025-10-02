using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveRequestRepo
    {
        Task<LeaveRequest> AddAsync(LeaveRequest request); // return created entity
        Task UpdateAsync(LeaveRequest request);
        Task DeleteAsync(int requestId);
        Task<LeaveRequest> GetByIdAsync(int requestId);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync(int managerId);
        Task<int> CountByEmployeeAndMonthAsync(int employeeId, DateTime date);
        Task<int> CountApprovedOrPendingByDayAsync(int departmentId, DateTime date);
        Task<bool> HasTakenLeaveTypeThisYearAsync(int employeeId, int leaveTypeId);
        Task<LeaveRequest> GetLatestByEmployeeAsync(int employeeId); // newly added helper
    }
}

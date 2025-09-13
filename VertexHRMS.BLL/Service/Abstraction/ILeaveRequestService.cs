
namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequest> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<LeaveRequest>> GetRequestsByEmployeeAsync(int employeeId);
        Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync(int managerId);
        Task SubmitLeaveRequestAsync(LeaveRequest request, string? documentPath = null);
        Task SubmitCasualLeaveAsync(LeaveRequest request);
        Task SubmitSickLeaveAsync(LeaveRequest request, string documentPath);
        Task SubmitAnnualLeaveAsync(LeaveRequest request);
        Task SubmitMaternityLeaveAsync(LeaveRequest request, string documentPath);
        Task SubmitPaternityLeaveAsync(LeaveRequest request, string documentPath);
        Task SubmitUnpaidLeaveAsync(LeaveRequest request);
        Task SubmitSummerLeaveAsync(LeaveRequest request);

        Task ApproveRequestAsync(int requestId, int approverId);
        Task RejectRequestAsync(int requestId, int approverId, string reason);
        Task CancelRequestAsync(int requestId, int employeeId);
    }
}

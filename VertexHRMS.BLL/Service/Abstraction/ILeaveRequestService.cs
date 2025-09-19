
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveRequestService
    {
        Task<GetRequestsByIdVM> GetRequestByIdAsync(int requestId);
        Task<IEnumerable<GetByEmployeeVM>> GetRequestsByEmployeeAsync(int employeeId);
        Task<IEnumerable<GetPendingRequestsVM>> GetPendingRequestsAsync(int managerId);
        Task SubmitLeaveRequestAsync(AddVM  request, string? documentPath = null);
        Task SubmitCasualLeaveAsync(    AddVM request);
        Task SubmitSickLeaveAsync(  AddVM        request, string documentPath);
        Task SubmitAnnualLeaveAsync(AddVM request);
        Task SubmitMaternityLeaveAsync(AddVM     request, string documentPath);
        Task SubmitPaternityLeaveAsync(AddVM request, string documentPath);
        Task SubmitUnpaidLeaveAsync(AddVM request);
        Task SubmitSummerLeaveAsync(AddVM request);

        Task ApproveRequestAsync(int requestId, int approverId);
        Task RejectRequestAsync(int requestId, int approverId, string reason);
        Task CancelRequestAsync(int requestId, int employeeId);
    }
}


namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveApprovalService
    {
        Task ApproveAsync(int leaveRequestId, int approverId);
        Task RejectAsync(int leaveRequestId, int approverId, string reason);
    }
}



using AutoMapper;
using Azure.Core;
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveApprovalService
    {
        private readonly ILeaveApprovalRepo _leaveApprovalRepo;
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly ILeaveTypeRepo _leaveTypeRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IMapper _mapper;
        private readonly ApplicationUserRepo _applicationuser;

        public LeaveApprovalService(
            ILeaveApprovalRepo leaveApprovalRepo,
            ILeaveRequestRepo leaveRequestRepo, IMapper mapper, ILeaveTypeRepo leaveTypeRepo, IEmployeeRepo employeeRepo, ApplicationUserRepo applicationUserRepo)
        {
            _leaveApprovalRepo = leaveApprovalRepo;
            _leaveRequestRepo = leaveRequestRepo;
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _applicationuser = applicationUserRepo;
            _leaveTypeRepo = leaveTypeRepo;
        }

        public async Task ApproveAsync(int requestId, int approverId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null)
                throw new InvalidOperationException("Leave request not found.");
            await _leaveRequestRepo.UpdateAsync(request);
            var approval = new LeaveApproval(requestId,approverId,"Approved",DateTime.UtcNow);
            await _leaveApprovalRepo.AddAsync(approval);
        }
        public async Task RejectAsync(int requestId, int approverId, string reason)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null)
                throw new InvalidOperationException("Leave request not found.");
            request.UpdateStatus("Rejected");
            await _leaveRequestRepo.UpdateAsync(request);
            var approval = new LeaveApproval(requestId, approverId, "Rejected: "+reason, DateTime.UtcNow);
            await _leaveApprovalRepo.AddAsync(approval);
        }
    }
}

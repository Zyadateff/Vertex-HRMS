
using AutoMapper;
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly ILeaveApprovalRepo _leaveApprovalRepo;
        private readonly ILeaveEntitlementRepo _leaveEntitlementRepo;
        private readonly ILeaveLedgerRepo _leaveLedgerRepo;
        private readonly ILeaveTypeRepo _leaveTypeRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IMapper _mapper;
        private readonly ApplicationUserRepo _applicationuser;

        public LeaveRequestService( ILeaveRequestRepo leaveRequestRepo,ILeaveApprovalRepo leaveApprovalRepo,ILeaveEntitlementRepo leaveEntitlementRepo,ILeaveLedgerRepo leaveLedgerRepo, IEmployeeRepo employeeRepo, IMapper mapper, ApplicationUserRepo applicationuser)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveApprovalRepo = leaveApprovalRepo;
            _leaveEntitlementRepo = leaveEntitlementRepo;
            _leaveLedgerRepo = leaveLedgerRepo;
            _employeeRepo = employeeRepo;
            _mapper = mapper;
            _applicationuser = applicationuser;
        }
        public async Task<GetRequestsByIdVM> GetRequestByIdAsync(int requestId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            var result = _mapper.Map<GetRequestsByIdVM>(request);
            return result;
        }

        public async Task<IEnumerable<GetByEmployeeVM>> GetRequestsByEmployeeAsync(int employeeId)
        {
            var requests = await _leaveRequestRepo.GetByEmployeeAsync(employeeId);
            var result = _mapper.Map<List<GetByEmployeeVM>>(requests);
            return result   ;
        }

        public async Task<IEnumerable<GetPendingRequestsVM>> GetPendingRequestsAsync(int managerId)
        {
            var requests = await _leaveRequestRepo.GetPendingRequestsAsync(managerId);
            var result = _mapper.Map<List<GetPendingRequestsVM>>(requests);
            return result;
        }
        public async Task SubmitLeaveRequestAsync(AddVM request, string? documentPath = null)
        {
            var leaveType = await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID);
            if (leaveType == null)
                throw new InvalidOperationException("Invalid leave type.");
            var code = leaveType.Name;
            if (code == "CASUAL")
            {
                await SubmitCasualLeaveAsync(request);
            }
            else if (code == "SICK")
            {
                await SubmitSickLeaveAsync(request, documentPath);
            }
            else if (code == "ANNUAL")
            {
                await SubmitAnnualLeaveAsync(request);
            }
            else if (code == "MATERNITY")
            {
                await SubmitMaternityLeaveAsync(request, documentPath!);
            }
            else if (code == "PATERNITY")
            {
                await SubmitPaternityLeaveAsync(request, documentPath!);
            }
            else if (code == "UNPAID")
            {
                await SubmitUnpaidLeaveAsync(request);
            }
            else if (code == "SUMMER")
            {
                await SubmitSummerLeaveAsync(request);
            }
            else
            {
                throw new NotSupportedException($"Leave type {code} not supported.");
            }
        }

        public async Task SubmitCasualLeaveAsync(AddVM request)
        {
            var taken = await _leaveRequestRepo.HasTakenLeaveTypeThisYearAsync(request.EmployeeId, request.LeaveTypeID);
            if (taken)
                throw new InvalidOperationException("Casual leave already taken this year.");
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                result.GetDurationInDays(),
                DateTime.Now
            ));
        }

        public async Task SubmitSickLeaveAsync(AddVM request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath))
                throw new InvalidOperationException("Medical document required.");

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                result.GetDurationInDays(),
                DateTime.Now
            ));
        }
        public async Task SubmitAnnualLeaveAsync(AddVM request)
        {
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < result.GetDurationInDays())
                throw new InvalidOperationException("Not enough leave balance.");
            var monthlyCount = await _leaveRequestRepo.CountByEmployeeAndMonthAsync(request.EmployeeId, request.StartDateTime);
            if (monthlyCount >= 2)
                throw new InvalidOperationException("Exceeded monthly leave limit.");
            var departmentId = await _employeeRepo.GetDepartmentIdByEmployeeIdAsync(request.EmployeeId);
            var totalEmployees = await _employeeRepo.CountByDepartmentAsync(departmentId);
            var leavesThatDay = await _leaveRequestRepo.CountApprovedOrPendingByDayAsync(departmentId, request.StartDateTime);
            decimal availableRatio = (decimal)(totalEmployees - leavesThatDay) / totalEmployees;
            if (availableRatio < 0.75m)
                throw new InvalidOperationException("Department staffing would fall below 75%.");

            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Pending", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);
        }

        public async Task SubmitMaternityLeaveAsync(AddVM request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath))
                throw new InvalidOperationException("Maternity document required.");

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                result.GetDurationInDays(),
                DateTime.Now
            ));
        }

        public async Task SubmitPaternityLeaveAsync(AddVM request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath))
                throw new InvalidOperationException("Paternity document required.");

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                result.GetDurationInDays(),
                DateTime.Now
            ));
        }

        public async Task SubmitUnpaidLeaveAsync(AddVM request)
        {
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Not Paid",
                result.GetDurationInDays(),
                DateTime.Now
            ));
        }

        public async Task SubmitSummerLeaveAsync(AddVM request)
        {
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            var taken = await _leaveRequestRepo.HasTakenLeaveTypeThisYearAsync(request.EmployeeId, request.LeaveTypeID);
            if (taken)
                throw new InvalidOperationException("Summer leave already taken this year.");
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < result.GetDurationInDays())
                throw new InvalidOperationException("Not enough leave balance.");
            var departmentId = await _employeeRepo.GetDepartmentIdByEmployeeIdAsync(request.EmployeeId);
            var totalEmployees = await _employeeRepo.CountByDepartmentAsync(departmentId);
            var leavesThatDay = await _leaveRequestRepo.CountApprovedOrPendingByDayAsync(departmentId, request.StartDateTime);
            decimal availableRatio = (decimal)(totalEmployees - leavesThatDay) / totalEmployees;
            if (availableRatio < 0.75m)
                throw new InvalidOperationException("Department staffing would fall below 75%.");
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Pending", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);
        }
        public async Task ApproveRequestAsync(int requestId, int approverId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);

            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement != null)
            {
                entitlement.UpdateUsed(entitlement.Used + request.GetDurationInDays());
                await _leaveEntitlementRepo.UpdateAsync(entitlement);
            }

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
           (
               request.EmployeeId,
               await _employeeRepo.GetByIdAsync(request.EmployeeId),
               request.LeaveTypeID,
               await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
               "Is Paid",
               result.GetDurationInDays(),
               DateTime.Now
           ));
        }
        public async Task RejectRequestAsync(int requestId, int approverId, string reason)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", await _applicationuser.GetByEmployeeIdAsync(request.EmployeeId));
            await _leaveRequestRepo.AddAsync(result);
        }
        public async Task CancelRequestAsync(int requestId, int employeeId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null || request.EmployeeId != employeeId)
                throw new InvalidOperationException("Not allowed.");
            request.UpdateStatus("Rejected");
            await _leaveRequestRepo.UpdateAsync(request);
        }
    }
}

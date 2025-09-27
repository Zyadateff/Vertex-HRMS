
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

        public LeaveRequestService( ILeaveRequestRepo leaveRequestRepo,ILeaveApprovalRepo leaveApprovalRepo,ILeaveEntitlementRepo leaveEntitlementRepo,ILeaveLedgerRepo leaveLedgerRepo, IEmployeeRepo employeeRepo, IMapper mapper)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveApprovalRepo = leaveApprovalRepo;
            _leaveEntitlementRepo = leaveEntitlementRepo;
            _leaveLedgerRepo = leaveLedgerRepo;
            _employeeRepo = employeeRepo;
            _mapper = mapper;
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
                await ApproveRequestAsync(request.EmployeeId);
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
                await ApproveRequestAsync(request.EmployeeId);
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
            {
                var temp1 = request;
                var result1 = _mapper.Map<LeaveRequest>(temp1);
                result1.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected","You have taken this leeave this year");
                await _leaveRequestRepo.AddAsync(result1);
            }
            else
            {

                var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", "");
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
        }

        public async Task SubmitSickLeaveAsync(AddVM request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath)){
                var temp1 = request;
                var result1 = _mapper.Map<LeaveRequest>(temp1);
                result1.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "There is no document");
                await _leaveRequestRepo.AddAsync(result1);
            }
            else
            {
                var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved" , "");
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
        }
        public async Task SubmitAnnualLeaveAsync(AddVM request)
        {
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < result.GetDurationInDays())
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected" , "Your annual leave entitlement has ended");
                await _leaveRequestRepo.AddAsync(result);
            }
            var monthlyCount = await _leaveRequestRepo.CountByEmployeeAndMonthAsync(request.EmployeeId, request.StartDateTime);
            if (monthlyCount >= 2)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Your monthly leave entitlement has ended");
                await _leaveRequestRepo.AddAsync(result);
            }
            var departmentId = await _employeeRepo.GetDepartmentIdByEmployeeIdAsync(request.EmployeeId);
            var totalEmployees = await _employeeRepo.CountByDepartmentAsync(departmentId);
            var leavesThatDay = await _leaveRequestRepo.CountApprovedOrPendingByDayAsync(departmentId, request.StartDateTime);
            decimal availableRatio = (decimal)(totalEmployees - leavesThatDay) / totalEmployees;
            if (availableRatio < 0.75m)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Rejected from the manager" );
                await _leaveRequestRepo.AddAsync(result);
            }
            else
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Pending" , "");
                await _leaveRequestRepo.AddAsync(result);
            }
        }

        public async Task SubmitMaternityLeaveAsync(AddVM request, string documentPath)
        {

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            if (string.IsNullOrEmpty(documentPath))
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "There is no document");
                await _leaveRequestRepo.AddAsync(result);
            }
            else
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", "" );
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
        }

        public async Task SubmitPaternityLeaveAsync(AddVM request, string documentPath)
        {
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            if (string.IsNullOrEmpty(documentPath))
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected" , "There is no document");
                await _leaveRequestRepo.AddAsync(result);
            }
            else
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved" , "");
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
        }

        public async Task SubmitUnpaidLeaveAsync(AddVM request)
        {
            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved" , "");
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
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "You have taken this leeave this year");
                await _leaveRequestRepo.AddAsync(result);
            }
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < result.GetDurationInDays())
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Your annual leave entitlement has ended");
                await _leaveRequestRepo.AddAsync(result);
            }
            var departmentId = await _employeeRepo.GetDepartmentIdByEmployeeIdAsync(request.EmployeeId);
            var totalEmployees = await _employeeRepo.CountByDepartmentAsync(departmentId);
            var leavesThatDay = await _leaveRequestRepo.CountApprovedOrPendingByDayAsync(departmentId, request.StartDateTime);
            decimal availableRatio = (decimal)(totalEmployees - leavesThatDay) / totalEmployees;
            if (availableRatio < 0.75m)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Rejected from the manager");
                await _leaveRequestRepo.AddAsync(result);
            }
            else
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Pending", "" );
                await _leaveRequestRepo.AddAsync(result);
            }
        }
        public async Task ApproveRequestAsync(int requestId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            var temp = request;
            var result = _mapper.Map<LeaveRequest>(temp);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", "" );
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
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "" );
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

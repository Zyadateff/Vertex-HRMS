
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Repo.Abstraction;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.BLL.Service.Implementation
{
    internal class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly ILeaveApprovalRepo _leaveApprovalRepo;
        private readonly ILeaveEntitlementRepo _leaveEntitlementRepo;
        private readonly ILeaveLedgerRepo _leaveLedgerRepo;
        private readonly ILeaveTypeRepo _leaveTypeRepo;
        private readonly IEmployeeRepo _employeeRepo;

        public LeaveRequestService( ILeaveRequestRepo leaveRequestRepo,ILeaveApprovalRepo leaveApprovalRepo,ILeaveEntitlementRepo leaveEntitlementRepo,ILeaveLedgerRepo leaveLedgerRepo, IEmployeeRepo employeeRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveApprovalRepo = leaveApprovalRepo;
            _leaveEntitlementRepo = leaveEntitlementRepo;
            _leaveLedgerRepo = leaveLedgerRepo;
            _employeeRepo = employeeRepo;
        }

        // -------------------- GET --------------------
        public async Task<LeaveRequest> GetRequestByIdAsync(int requestId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            return request;
        }

        public async Task<IEnumerable<LeaveRequest>> GetRequestsByEmployeeAsync(int employeeId)
        {
            var requests = await _leaveRequestRepo.GetByEmployeeAsync(employeeId);
            return requests;
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync(int managerId)
        {
            var requests = await _leaveRequestRepo.GetPendingRequestsAsync(managerId);
            return requests;
        }

        // -------------------- SUBMIT TYPES --------------------
        public async Task SubmitLeaveRequestAsync(LeaveRequest request, string? documentPath = null)
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

        public async Task SubmitCasualLeaveAsync(LeaveRequest request)
        {
            var taken = await _leaveRequestRepo.HasTakenLeaveTypeThisYearAsync(request.EmployeeId, request.LeaveTypeID);
            if (taken)
                throw new InvalidOperationException("Casual leave already taken this year.");

            request.UpdateStatus("Approved");
            await _leaveRequestRepo.AddAsync(request);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                request.LeaveTypeID,
                "Is Paid",
                request.DurationHours,
                DateTime.Now
            ));
        }

        public async Task SubmitSickLeaveAsync(LeaveRequest request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath))
                throw new InvalidOperationException("Medical document required.");

            request.UpdateStatus("Approved");
            await _leaveRequestRepo.AddAsync(request);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                request.LeaveTypeID,
                "Is Paid",
                request.DurationHours,
                DateTime.Now
            ));
        }

        public async Task SubmitAnnualLeaveAsync(LeaveRequest request)
        {
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < request.GetDurationInDays())
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

            request.UpdateStatus("Pending");
            await _leaveRequestRepo.AddAsync(request);
        }

        public async Task SubmitMaternityLeaveAsync(LeaveRequest request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath))
                throw new InvalidOperationException("Maternity document required.");

            request.UpdateStatus("Approved");
            await _leaveRequestRepo.AddAsync(request);
        }

        public async Task SubmitPaternityLeaveAsync(LeaveRequest request, string documentPath)
        {
            if (string.IsNullOrEmpty(documentPath))
                throw new InvalidOperationException("Paternity document required.");

            request.UpdateStatus("Approved");
            await _leaveRequestRepo.AddAsync(request);
        }

        public async Task SubmitUnpaidLeaveAsync(LeaveRequest request)
        {
            request.UpdateStatus("Approved");
            await _leaveRequestRepo.AddAsync(request);
        }

        public async Task SubmitSummerLeaveAsync(LeaveRequest request)
        {
            var taken = await _leaveRequestRepo.HasTakenLeaveTypeThisYearAsync(request.EmployeeId, request.LeaveTypeID);
            if (taken)
                throw new InvalidOperationException("Summer leave already taken this year.");

            // Also ensure balance
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < request.GetDurationInDays())
                throw new InvalidOperationException("Not enough leave balance.");

            request.UpdateStatus("Pending");
            await _leaveRequestRepo.AddAsync(request);
        }

        // -------------------- WORKFLOW --------------------
        public async Task ApproveRequestAsync(int requestId, int approverId)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            request.UpdateStatus("Approved");
            await _leaveRequestRepo.UpdateAsync(request);

            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement != null)
            {
                entitlement.UpdateUsed(entitlement.Used + request.GetDurationInDays());
                await _leaveEntitlementRepo.UpdateAsync(entitlement);
            }

            await _leaveLedgerRepo.AddAsync(new LeaveLedger
            (
                request.EmployeeId,
                request.LeaveTypeID,
                "Debit",
                request.GetDurationInDays(),
                DateTime.Now
            ));
        }

        public async Task RejectRequestAsync(int requestId, int approverId, string reason)
        {
            var request = await _leaveRequestRepo.GetByIdAsync(requestId);
            if (request == null) throw new InvalidOperationException("Request not found.");

            request.UpdateStatus("Rejected");
            await _leaveRequestRepo.UpdateAsync(request);
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

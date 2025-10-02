using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly ILeaveEntitlementRepo _leaveEntitlementRepo;
        private readonly ILeaveLedgerRepo _leaveLedgerRepo;
        private readonly ILeaveTypeRepo _leaveTypeRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IMapper _mapper;

        public LeaveRequestService(
            ILeaveRequestRepo leaveRequestRepo,
            ILeaveEntitlementRepo leaveEntitlementRepo,
            ILeaveLedgerRepo leaveLedgerRepo,
            IEmployeeRepo employeeRepo,
            IMapper mapper,
            ILeaveTypeRepo leaveTypeRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _leaveTypeRepo = leaveTypeRepo;
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
            return result;
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

            if (code == "CASUAL") await SubmitCasualLeaveAsync(request);
            else if (code == "SICK") await SubmitSickLeaveAsync(request, documentPath);
            else if (code == "ANNUAL")
            {
                await SubmitAnnualLeaveAsync(request);
                await ApproveRequestAsync(request.LeaveRequestId);
            }
            else if (code == "MATERNITY") await SubmitMaternityLeaveAsync(request, documentPath);
            else if (code == "PATERNITY") await SubmitPaternityLeaveAsync(request, documentPath);
            else if (code == "UNPAID") await SubmitUnpaidLeaveAsync(request);
            else if (code == "SUMMER")
            {
                await SubmitSummerLeaveAsync(request);
                await ApproveRequestAsync(request.LeaveRequestId);
            }
            else throw new NotSupportedException($"Leave type {code} not supported.");
        }

        // --- Example of creation: AddAsync returns created entity
        public async Task SubmitCasualLeaveAsync(AddVM request)
        {
            var taken = await _leaveRequestRepo.HasTakenLeaveTypeThisYearAsync(request.EmployeeId, request.LeaveTypeID);
            var result = _mapper.Map<LeaveRequest>(request); // mapping ignores PK (configured in AutoMapper)
            if (taken)
            {
                result.updateMissing(
                    await _employeeRepo.GetByIdAsync(request.EmployeeId),
                    await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                    "Rejected",
                    "You have taken this leave this year");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            result.updateMissing(
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Approved",
                "N/A");

            var created = await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                created.GetDurationInDays(),
                DateTime.Now));
        }

        public async Task SubmitSickLeaveAsync(AddVM request, string documentPath)
        {
            var result = _mapper.Map<LeaveRequest>(request);
            if (string.IsNullOrEmpty(documentPath))
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId),
                                     await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                                     "Rejected", "There is no document");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId),
                                 await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                                 "Approved", "N/A");
            var created = await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                created.GetDurationInDays(),
                DateTime.Now));
        }

        public async Task SubmitAnnualLeaveAsync(AddVM request)
        {
            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            var result = _mapper.Map<LeaveRequest>(request);

            if (entitlement == null || entitlement.Entitled - entitlement.Used < result.GetDurationInDays())
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Your annual leave entitlement has ended");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            var monthlyCount = await _leaveRequestRepo.CountByEmployeeAndMonthAsync(request.EmployeeId, request.StartDateTime);
            if (monthlyCount >= 2)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Your monthly leave entitlement has ended");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            var departmentId = await _employeeRepo.GetDepartmentIdByEmployeeIdAsync(request.EmployeeId);
            var totalEmployees = await _employeeRepo.CountByDepartmentAsync(departmentId);
            var leavesThatDay = await _leaveRequestRepo.CountApprovedOrPendingByDayAsync(departmentId, request.StartDateTime);
            decimal availableRatio = (decimal)(totalEmployees - leavesThatDay) / (totalEmployees == 0 ? 1 : totalEmployees);

            if (availableRatio < 0.75m)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Rejected from the manager");
                await _leaveRequestRepo.AddAsync(result);
            }
            else
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Pending", "N/A");
                await _leaveRequestRepo.AddAsync(result);
            }
        }

        // Implement SubmitMaternity/SubmitPaternity/SubmitUnpaid/SubmitSummer similarly to above...
        public async Task SubmitMaternityLeaveAsync(AddVM request, string documentPath)
        {
            var result = _mapper.Map<LeaveRequest>(request);
            if (string.IsNullOrEmpty(documentPath))
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "There is no document");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", "N/A");
            var created = await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                created.GetDurationInDays(),
                DateTime.Now));
        }

        public async Task SubmitPaternityLeaveAsync(AddVM request, string documentPath)
        {
            var result = _mapper.Map<LeaveRequest>(request);
            if (string.IsNullOrEmpty(documentPath))
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "There is no document");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", "N/A");
            var created = await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                created.GetDurationInDays(),
                DateTime.Now));
        }

        public async Task SubmitUnpaidLeaveAsync(AddVM request)
        {
            var result = _mapper.Map<LeaveRequest>(request);
            result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Approved", "N/A");
            var created = await _leaveRequestRepo.AddAsync(result);

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Not Paid",
                created.GetDurationInDays(),
                DateTime.Now));
        }

        public async Task SubmitSummerLeaveAsync(AddVM request)
        {
            var result = _mapper.Map<LeaveRequest>(request);

            var taken = await _leaveRequestRepo.HasTakenLeaveTypeThisYearAsync(request.EmployeeId, request.LeaveTypeID);
            if (taken)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "You have taken this leave this year");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            var entitlement = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(request.EmployeeId, request.LeaveTypeID, request.StartDateTime.Year);
            if (entitlement == null || entitlement.Entitled - entitlement.Used < result.GetDurationInDays())
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Your annual leave entitlement has ended");
                await _leaveRequestRepo.AddAsync(result);
                return;
            }

            var departmentId = await _employeeRepo.GetDepartmentIdByEmployeeIdAsync(request.EmployeeId);
            var totalEmployees = await _employeeRepo.CountByDepartmentAsync(departmentId);
            var leavesThatDay = await _leaveRequestRepo.CountApprovedOrPendingByDayAsync(departmentId, request.StartDateTime);
            decimal availableRatio = (decimal)(totalEmployees - leavesThatDay) / (totalEmployees == 0 ? 1 : totalEmployees);

            if (availableRatio < 0.75m)
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Rejected", "Rejected from the manager");
                await _leaveRequestRepo.AddAsync(result);
            }
            else
            {
                result.updateMissing(await _employeeRepo.GetByIdAsync(request.EmployeeId), await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID), "Pending", "N/A");
                await _leaveRequestRepo.AddAsync(result);
            }
        }

        // ---- Approval / Rejection should update existing request, not Add a new one
        public async Task ApproveRequestAsync(int requestId)
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

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(
                request.EmployeeId,
                await _employeeRepo.GetByIdAsync(request.EmployeeId),
                request.LeaveTypeID,
                await _leaveTypeRepo.GetByIdAsync(request.LeaveTypeID),
                "Is Paid",
                request.GetDurationInDays(),
                DateTime.Now));
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

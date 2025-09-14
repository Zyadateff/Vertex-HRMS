using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveEntitlementService:ILeaveEntitlementService
    {
        private readonly ILeaveEntitlementRepo _leaveEntitlementRepo;

        public LeaveEntitlementService(ILeaveEntitlementRepo leaveEntitlementRepo)
        {
            _leaveEntitlementRepo = leaveEntitlementRepo;
        }
        public async Task<LeaveEntitlement?> GetEntitlementAsync(int employeeId, int leaveTypeId, int year)
        {
            return await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, year);
        }
        public async Task<IEnumerable<LeaveEntitlement>> GetAllForEmployeeAsync(int employeeId, int year)
        {
            return await _leaveEntitlementRepo.GetAllForEmployeeAsync(employeeId, year);
        }
        public async Task AssignEntitlementAsync(int employeeId, int leaveTypeId, decimal entitledDays, int year)
        {
            var existing = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, year);
            if (existing != null)
                throw new InvalidOperationException("Entitlement already exists for this year.");

            var entitlement = new LeaveEntitlement(employeeId, leaveTypeId, year, entitledDays, 0);
            await _leaveEntitlementRepo.AddAsync(entitlement);
        }
        public async Task UpdateEntitlementAsync(LeaveEntitlement entitlement)
        {
            await _leaveEntitlementRepo.UpdateAsync(entitlement);
        }
        public async Task RemoveEntitlementAsync(int entitlementId)
        {
            await _leaveEntitlementRepo.DeleteAsync(entitlementId);
        }
    }
}


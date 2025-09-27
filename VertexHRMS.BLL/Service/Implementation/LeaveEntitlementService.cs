

using AutoMapper;
using VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveEntitlementService:ILeaveEntitlementService
    {
        private readonly ILeaveEntitlementRepo _leaveEntitlementRepo;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ILeaveTypeRepo _leaveTyperepo;

        public LeaveEntitlementService(ILeaveEntitlementRepo leaveEntitlementRepo, IMapper mapper, IEmployeeRepo employeeRepo, ILeaveTypeRepo leaveTypeRepo)
        {
            _leaveEntitlementRepo = leaveEntitlementRepo;
            _mapper = mapper;
            _employeeRepo = employeeRepo; 
            _leaveTyperepo = leaveTypeRepo;
        }
        public async Task<GetEntitlementVM> GetEntitlementAsync(int employeeId, int leaveTypeId, int year)
        {
            var N= await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, year);
            return _mapper.Map<GetEntitlementVM>(N);
          
        }
        public async Task<IEnumerable<GetAllForEmployeeVM>> GetAllForEmployeeAsync(int employeeId, int year)
        {
            var N= await _leaveEntitlementRepo.GetAllForEmployeeAsync(employeeId, year);
            return _mapper.Map<List<GetAllForEmployeeVM>>(N);
        }
        public async Task AssignEntitlementAsync(int employeeId, int leaveTypeId, decimal entitledDays, int year)
        {
            var existing = await _leaveEntitlementRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId, year);
            if (existing != null)
                throw new InvalidOperationException("Entitlement already exists for this year.");

            var entitlement = new LeaveEntitlement(employeeId, await _employeeRepo.GetByIdAsync(employeeId), leaveTypeId, await _leaveTyperepo.GetByIdAsync(leaveTypeId), year, entitledDays,0, 0);
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


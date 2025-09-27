
using AutoMapper;
using VertexHRMS.BLL.ModelVM.LeaveLedger;
using VertexHRMS.BLL.ModelVM.LeaveLedgerVM;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveLedgerService:ILeaveLedgerService
    {
        private readonly ILeaveLedgerRepo _leaveLedgerRepo;
        private readonly IMapper _mapper;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ILeaveTypeRepo _leaveTyperepo;

        public LeaveLedgerService(ILeaveLedgerRepo leaveLedgerRepo, IMapper mapper, IEmployeeRepo employeeRepo, ILeaveTypeRepo leaveType)
        {
            _leaveLedgerRepo = leaveLedgerRepo;
            _mapper = mapper;
            _employeeRepo = employeeRepo;
            _leaveTyperepo = leaveType;
        }

        public async Task<GetByIdVM> GetByIdAsync(int ledgerId)
        {
            var n= await _leaveLedgerRepo.GetByIdAsync(ledgerId);
            return _mapper.Map<GetByIdVM>(n);
        }

        public async Task<IEnumerable<GetByEmployeeVM>> GetByEmployeeAsync(int employeeId, int year)
        {
            var n = await _leaveLedgerRepo.GetByEmployeeAsync(employeeId, year);
            return _mapper.Map<List<GetByEmployeeVM>>(n);
        }

        public async Task<IEnumerable<GetByEmployeeAndTypeVM>> GetByEmployeeAndTypeAsync(int employeeId, int leaveTypeId)
        {
            var n= await _leaveLedgerRepo.GetByEmployeeAndTypeAsync(employeeId, leaveTypeId);
            return _mapper.Map<List<GetByEmployeeAndTypeVM>>(n);
        }

        public async Task AddEntryAsync(AddVM ledger)
        {
            if (ledger == null)
                throw new ArgumentNullException(nameof(ledger));

            await _leaveLedgerRepo.AddAsync(new LeaveLedger(ledger.EmployeeId,
                await _employeeRepo.GetByIdAsync(ledger.EmployeeId),
                ledger.LeaveTypeId,
                await _leaveTyperepo.GetByIdAsync(ledger.LeaveTypeId),
                ledger.TxnType,
                ledger.Quantity,
                ledger.EffectiveDate));
        }

        public async Task UpdateEntryAsync(LeaveLedger ledger)
        {
            if (ledger == null)
                throw new ArgumentNullException(nameof(ledger));

            await _leaveLedgerRepo.UpdateAsync(ledger);
        }

        public async Task DeleteEntryAsync(int ledgerId)
        {
            await _leaveLedgerRepo.DeleteAsync(ledgerId);
        }
    }
}

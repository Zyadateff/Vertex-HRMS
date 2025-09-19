
using VertexHRMS.BLL.ModelVM.LeaveLedger;
using VertexHRMS.BLL.ModelVM.LeaveLedgerVM;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveLedgerService
    {
        Task<GetByIdVM> GetByIdAsync(int ledgerId);
        Task<IEnumerable<GetByEmployeeVM>> GetByEmployeeAsync(int employeeId, int year);
        Task<IEnumerable<GetByEmployeeAndTypeVM>> GetByEmployeeAndTypeAsync(int employeeId, int leaveTypeId);
        Task AddEntryAsync(AddVM ledger);
        Task UpdateEntryAsync(LeaveLedger ledger);
        Task DeleteEntryAsync(int ledgerId);
    }
}

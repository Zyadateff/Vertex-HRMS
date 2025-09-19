
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveLedgerRepo
    {
        Task<LeaveLedger> GetByIdAsync(int ledgerId);
        Task<IEnumerable<LeaveLedger>> GetByEmployeeAsync(int employeeId, int year);
        Task<IEnumerable<LeaveLedger>> GetByLeaveTypeAsync(int employeeId, int leaveTypeId, int year);
        Task<IEnumerable<LeaveLedger>> GetByEmployeeAndTypeAsync(int employeeId, int leaveTypeId);
        Task AddAsync(LeaveLedger ledgerEntry);
        Task UpdateAsync(LeaveLedger ledger);
        Task DeleteAsync(int ledgerId);
    }
}

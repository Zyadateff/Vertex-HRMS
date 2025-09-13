
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveLedgerRepo
    {
        Task<LeaveLedger> GetByIdAsync(int ledgerId);
        Task<IEnumerable<LeaveLedger>> GetByEmployeeAsync(int employeeId, int year);
        Task<IEnumerable<LeaveLedger>> GetByLeaveTypeAsync(int employeeId, int leaveTypeId, int year);
        Task AddAsync(LeaveLedger ledgerEntry);
        Task DeleteAsync(int ledgerId);
    }
}

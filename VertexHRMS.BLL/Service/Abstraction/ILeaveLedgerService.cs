using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveLedgerService
    {
        Task<LeaveLedger?> GetByIdAsync(int ledgerId);
        Task<IEnumerable<LeaveLedger>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<LeaveLedger>> GetByEmployeeAndTypeAsync(int employeeId, int leaveTypeId);
        Task AddEntryAsync(LeaveLedger ledger);
        Task UpdateEntryAsync(LeaveLedger ledger);
        Task DeleteEntryAsync(int ledgerId);
    }
}

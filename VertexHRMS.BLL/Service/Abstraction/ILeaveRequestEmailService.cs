using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface ILeaveRequestEmailService
    {
        Task<LeaveRequestEmail> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequestEmail>> GetAllAsync();
        Task<IEnumerable<LeaveRequestEmail>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<LeaveRequestEmail>> GetByRequestIdAsync(int requestId);
        Task AddAsync(LeaveRequestEmail email);
        Task UpdateAsync(LeaveRequestEmail email);
        Task DeleteAsync(int id);
    }
}

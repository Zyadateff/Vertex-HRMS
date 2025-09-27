using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveRequestEmailRepo
    {
        Task<LeaveRequestEmail> GetByIdAsync(int id);
        Task<IEnumerable<LeaveRequestEmail>> GetAllAsync();
        Task<IEnumerable<LeaveRequestEmail>> GetByEmployeeIdAsync(int employeeId);
        Task<IEnumerable<LeaveRequestEmail>> GetByRequestIdAsync(int requestId);
        Task AddAsync(LeaveRequestEmail entity);
        Task UpdateAsync(LeaveRequestEmail entity);
        Task DeleteAsync(int id);
    }
}

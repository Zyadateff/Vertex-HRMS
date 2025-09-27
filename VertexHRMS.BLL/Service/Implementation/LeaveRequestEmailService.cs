using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class LeaveRequestEmailService:ILeaveRequestEmailService
    {
        private readonly ILeaveRequestEmailRepo _repo;

        public LeaveRequestEmailService(ILeaveRequestEmailRepo repo)
        {
            _repo = repo;
        }

        public async Task<LeaveRequestEmail> GetByIdAsync(int id)
            => await _repo.GetByIdAsync(id);

        public async Task<IEnumerable<LeaveRequestEmail>> GetAllAsync()
            => await _repo.GetAllAsync();

        public async Task<IEnumerable<LeaveRequestEmail>> GetByEmployeeIdAsync(int employeeId)
            => await _repo.GetByEmployeeIdAsync(employeeId);

        public async Task<IEnumerable<LeaveRequestEmail>> GetByRequestIdAsync(int requestId)
            => await _repo.GetByRequestIdAsync(requestId);

        public async Task AddAsync(LeaveRequestEmail email)
            => await _repo.AddAsync(email);

        public async Task UpdateAsync(LeaveRequestEmail email)
            => await _repo.UpdateAsync(email);

        public async Task DeleteAsync(int id)
            => await _repo.DeleteAsync(id);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IEmployeeTrainingRepo
    {
        Task<IEnumerable<EmployeeTraining>> GetAllAsync();
        Task<EmployeeTraining?> GetByIdAsync(int id);
        Task AddAsync(EmployeeTraining training);
        Task UpdateAsync(EmployeeTraining training);
        Task DeleteAsync(int id);
        Task<IEnumerable<EmployeeTraining>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeTraining>> GetPendingTrainingsAsync(int employeeId);
        Task<IEnumerable<EmployeeTraining>> GetOverdueTrainingsAsync(int employeeId);
    }
}

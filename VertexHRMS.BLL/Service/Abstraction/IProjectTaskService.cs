using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IProjectTaskService
    {
        Task<IEnumerable<ProjectTask>> GetAllAsync();
        Task<ProjectTask?> GetByIdAsync(int id);
        Task AddAsync(ProjectTask task);
        Task UpdateAsync(ProjectTask task);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId);
        Task<IEnumerable<ProjectTask>> GetTasksByEmployeeAsync(int employeeId);
        Task<IEnumerable<ProjectTask>> GetActiveTasksAsync();
        Task<IEnumerable<ProjectTask>> GetOverdueTasksAsync();
    }
}

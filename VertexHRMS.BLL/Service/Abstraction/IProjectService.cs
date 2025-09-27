using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task AddAsync(Project project);
        Task UpdateAsync(Project project);
        Task DeleteAsync(int id);
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
        Task<IEnumerable<Project>> GetCompletedProjectsAsync();
    }
}


using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class ProjectService:IProjectService
    {
        private readonly IProjectRepo _projectRepository;

        public ProjectService(IProjectRepo projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
            => await _projectRepository.GetAllAsync();

        public async Task<Project?> GetByIdAsync(int id)
            => await _projectRepository.GetByIdAsync(id);

        public async Task AddAsync(Project project)
            => await _projectRepository.AddAsync(project);

        public async Task UpdateAsync(Project project)
            => await _projectRepository.UpdateAsync(project);

        public async Task DeleteAsync(int id)
            => await _projectRepository.DeleteAsync(id);

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
            => await _projectRepository.GetActiveProjectsAsync();

        public async Task<IEnumerable<Project>> GetCompletedProjectsAsync()
            => await _projectRepository.GetCompletedProjectsAsync();
    }
}

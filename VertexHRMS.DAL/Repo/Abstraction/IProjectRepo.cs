namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IProjectRepo
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

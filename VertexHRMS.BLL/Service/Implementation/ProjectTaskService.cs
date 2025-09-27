namespace VertexHRMS.BLL.Service.Implementation
{
    public class ProjectTaskService:IProjectTaskService
    {
        private readonly IProjectTaskRepo _taskRepository;

        public ProjectTaskService(IProjectTaskRepo taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<ProjectTask>> GetAllAsync()
            => await _taskRepository.GetAllAsync();

        public async Task<ProjectTask?> GetByIdAsync(int id)
            => await _taskRepository.GetByIdAsync(id);

        public async Task AddAsync(ProjectTask task)
            => await _taskRepository.AddAsync(task);

        public async Task UpdateAsync(ProjectTask task)
            => await _taskRepository.UpdateAsync(task);

        public async Task DeleteAsync(int id)
            => await _taskRepository.DeleteAsync(id);

        public async Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId)
            => await _taskRepository.GetTasksByProjectAsync(projectId);

        public async Task<IEnumerable<ProjectTask>> GetTasksByEmployeeAsync(int employeeId)
            => await _taskRepository.GetTasksByEmployeeAsync(employeeId);

        public async Task<IEnumerable<ProjectTask>> GetActiveTasksAsync()
            => await _taskRepository.GetActiveTasksAsync();

        public async Task<IEnumerable<ProjectTask>> GetOverdueTasksAsync()
            => await _taskRepository.GetOverdueTasksAsync();
    }
}

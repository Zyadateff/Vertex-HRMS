namespace VertexHRMS.DAL.Repo.Implementation
{
    public class ProjectTaskRepo:IProjectTaskRepo
    {
        private readonly VertexHRMSDbContext _context;
        public ProjectTaskRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProjectTask>> GetAllAsync()
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .OrderBy(t => t.StartDate)
                .ToListAsync();
        }

        public async Task<ProjectTask?> GetByIdAsync(int id)
        {
            return await _context.ProjectTasks
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(ProjectTask task)
        {
            await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ProjectTask task)
        {
            _context.ProjectTasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _context.ProjectTasks.FindAsync(id);
            if (task != null)
            {
                _context.ProjectTasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByProjectAsync(int projectId)
        {
            return await _context.ProjectTasks
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByEmployeeAsync(int employeeId)
        {
            return await _context.ProjectTasks
                .Where(t => t.AssignedToEmployeeId == employeeId)
                .Include(t => t.Project)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetActiveTasksAsync()
        {
            return await _context.ProjectTasks
                .Where(t => t.Status == Enum.ProjectStatus.Active)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetOverdueTasksAsync()
        {
            return await _context.ProjectTasks
                .Where(t => t.DueDate.HasValue && t.DueDate < DateTime.UtcNow && t.Status != Enum.ProjectStatus.Completed)
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .ToListAsync();
        }
    }
}

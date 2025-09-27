using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class ProjectRepo:IProjectRepo
    {
        private readonly VertexHRMSDbContext _context;
        public ProjectRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .OrderBy(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.Status == Enum.ProjectStatus.Active)
                .Include(p => p.Tasks)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetCompletedProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.Status == Enum.ProjectStatus.Completed)
                .Include(p => p.Tasks)
                .ToListAsync();
        }
    }
}

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class EmployeeTrainingRepo:IEmployeeTrainingRepo
    {
        private readonly VertexHRMSDbContext _context;
        public EmployeeTrainingRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<EmployeeTraining>> GetAllAsync()
        {
            return await _context.EmployeeTrainings
                .Include(t => t.Employee)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<EmployeeTraining?> GetByIdAsync(int id)
        {
            return await _context.EmployeeTrainings
                .Include(t => t.Employee)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(EmployeeTraining training)
        {
            await _context.EmployeeTrainings.AddAsync(training);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(EmployeeTraining training)
        {
            _context.EmployeeTrainings.Update(training);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var training = await _context.EmployeeTrainings.FindAsync(id);
            if (training != null)
            {
                _context.EmployeeTrainings.Remove(training);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<EmployeeTraining>> GetByEmployeeAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(t => t.EmployeeId == employeeId)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTraining>> GetPendingTrainingsAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(t => t.EmployeeId == employeeId && t.Status == "Pending")
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeTraining>> GetOverdueTrainingsAsync(int employeeId)
        {
            return await _context.EmployeeTrainings
                .Where(t => t.EmployeeId == employeeId &&
                            t.Status != "Completed" &&
                            t.DueDate.HasValue &&
                            t.DueDate < DateTime.UtcNow)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }
    }
}

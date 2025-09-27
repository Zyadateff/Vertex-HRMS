namespace VertexHRMS.DAL.Repo.Implementation
{

    public class ProfileRepo : IProfileRepo
    {
        private readonly VertexHRMSDbContext _context;
        public ProfileRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> GetEmployeeWithDetailsAsync(int employeeId)
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .FirstOrDefaultAsync(e => e.EmployeeId == employeeId);
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class ApplicationUserRepo : IApplicationUserRepo
    {
        private readonly VertexHRMSDbContext _context;

        public ApplicationUserRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task<ApplicationUser> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.ApplicationUsers
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Employee.EmployeeId == employeeId);
        }
    }
}

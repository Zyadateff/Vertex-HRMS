using Microsoft.EntityFrameworkCore;
using System;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Repo.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly VertexHRMSDbContext _context;

        public EmployeeRepository(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetByIdentityUserIdAsync(string userId)
        {
            return await _context.Employees.FirstOrDefaultAsync(e => e.IdentityUserId == userId);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Employees.AnyAsync(e => e.Email == email);
        }

        public async Task<string> GenerateEmployeeCodeAsync()
        {
            var lastEmployee = await _context.Employees.OrderByDescending(e => e.EmployeeId).FirstOrDefaultAsync();
            int lastCode = lastEmployee != null ? int.Parse(lastEmployee.EmployeeCode.Substring(3)) : 0;
            return $"EMP{lastCode + 1:D4}";
        }

        public async Task AddAsync(Employee employee)
        {
            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<Employee>> GetAllActiveAsync()
        {
            return await _context.Employees
                .Where(e => e.Status == "Active")
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .Include(e => e.Manager)
                .Include(e => e.DirectReports)
                .Include(e => e.Payrolls)
                .Include(e => e.LeaveRequests)
                .ToListAsync();
        }
    }
}

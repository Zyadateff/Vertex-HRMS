using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Service
{
    public class EmployeeRepo:IEmployeeRepo
    {
        private readonly VertexHRMSDbContext _context;

        public EmployeeRepo(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task<int> CountByDepartmentAsync(int departmentId)
        {
            return await _context.Employees.CountAsync(e => e.DepartmentId == departmentId);
        }

        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Employees
        .   Include(e => e.Department)
        .   Include(e => e.Position)
        .   Include(e => e.Manager)
        .   Include(e => e.DirectReports)
        .   Include(e => e.IdentityUser)
           .Include(e => e.LeaveRequests)
        .   Include(e => e.OvertimeRequests)
        .   Include(e => e.Resignations)
             .Include(e => e.AttendanceRecords)
           .Include(e => e.LeaveEntitlements)
            .Include(e => e.LeaveLedgerEntries)
        .Include(e => e.Payrolls)
        .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<int> GetDepartmentIdByEmployeeIdAsync(int employeeId)
        {
            var departmentId = await _context.Employees
                .Where(e => e.EmployeeId == employeeId)
                .Select(e => e.DepartmentId)
                .FirstOrDefaultAsync();

            return departmentId;
        }
    }
}

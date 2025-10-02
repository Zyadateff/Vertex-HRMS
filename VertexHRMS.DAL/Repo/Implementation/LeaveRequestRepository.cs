using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveRequestRepository : ILeaveRequestRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeaveRequestRepository(VertexHRMSDbContext context)
        {
            _context = context;
        }

        // Now returns the created entity (so callers can read the identity)
        public async Task<LeaveRequest> AddAsync(LeaveRequest request)
        {
            var entry = await _context.LeaveRequests.AddAsync(request);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<int> CountApprovedOrPendingByDayAsync(int departmentId, DateTime date)
        {
            return await _context.LeaveRequests
                .Include(r => r.Employee)
                .CountAsync(r => r.Employee.DepartmentId == departmentId
                              && r.StartDateTime <= date
                              && r.EndDateTime >= date
                              && (r.Status == "Approved" || r.Status == "Pending"));
        }

        public async Task<int> CountByEmployeeAndMonthAsync(int employeeId, DateTime date)
        {
            return await _context.LeaveRequests
                .CountAsync(r => r.EmployeeId == employeeId
                              && r.StartDateTime.Year == date.Year
                              && r.StartDateTime.Month == date.Month
                              && r.Status.StartsWith("Approved"));
        }

        public async Task DeleteAsync(int requestId)
        {
            var request = await _context.LeaveRequests.FindAsync(requestId);
            if (request != null)
            {
                _context.LeaveRequests.Remove(request);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeAsync(int employeeId)
        {
            var query = _context.LeaveRequests
                .Include(r => r.Employee)
                .Include(r => r.LeaveType)
                .AsQueryable();

            if (employeeId != 0)
                query = query.Where(r => r.EmployeeId == employeeId);

            return await query.ToListAsync();
        }

        public async Task<LeaveRequest> GetByIdAsync(int requestId)
        {
            var leave = await _context.LeaveRequests
                .Include(r => r.Employee)
                .Include(r => r.LeaveType)
                .FirstOrDefaultAsync(r => r.LeaveRequestId == requestId);
            return leave;
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingRequestsAsync(int managerId)
        {
            return await _context.LeaveRequests
                .Include(r => r.Employee)
                .Include(r => r.LeaveType)
                .Where(r => r.Status == "Pending" && r.Employee.ManagerId == managerId)
                .ToListAsync();
        }

        public async Task<bool> HasTakenLeaveTypeThisYearAsync(int employeeId, int leaveTypeId)
        {
            int year = DateTime.Now.Year;
            return await _context.LeaveRequests
                .AnyAsync(r => r.EmployeeId == employeeId
                            && r.LeaveTypeID == leaveTypeId
                            && r.StartDateTime.Year == year
                            && r.Status.StartsWith("Approved"));
        }

        public async Task UpdateAsync(LeaveRequest request)
        {
            _context.LeaveRequests.Update(request);
            await _context.SaveChangesAsync();
        }

        // New helper to get the most recent leave request for a given employee
        public async Task<LeaveRequest> GetLatestByEmployeeAsync(int employeeId)
        {
            return await _context.LeaveRequests
                .Include(r => r.Employee)
                .Include(r => r.LeaveType)
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.LeaveRequestId) // or OrderByDescending(r => r.StartDateTime)
                .FirstOrDefaultAsync();
        }
    }
}

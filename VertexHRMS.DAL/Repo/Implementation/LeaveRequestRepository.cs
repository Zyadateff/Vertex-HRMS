

namespace VertexHRMS.DAL.Repo.Service
{
    public class LeaveRequestRepository : ILeaveRequestRepo
    {
        private readonly VertexHRMSDbContext _context;
        public LeaveRequestRepository(VertexHRMSDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(LeaveRequest request)
        {
            await _context.LeaveRequests.AddAsync(request);
            await _context.SaveChangesAsync();
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
            return await _context.LeaveRequests
            .Where(r => r.EmployeeId == employeeId)
            .ToListAsync();
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
    }
}

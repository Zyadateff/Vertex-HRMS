namespace VertexHRMS.DAL.Repo.Implementation
{
    public class AttendanceRecordsRepo : IAttendanceRecordsRepo
    {
        private readonly VertexHRMSDbContext db;
        public AttendanceRecordsRepo(VertexHRMSDbContext db)
        {
            this.db = db;
        }

        public async Task<AttendanceRecord> AddAttendanceRecordAsync(AttendanceRecord attendanceRecord)
        {
            try
            {
                var result = await db.AttendanceRecords.AddAsync(attendanceRecord);
                await db.SaveChangesAsync();
                return result.Entity;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<AttendanceRecord>?> GetAttendanceRecordByNameAsync(string firstname, string? lastname = null)
        {
            var result = db.AttendanceRecords.AsNoTracking()
                .Include(a => a.Employee).AsQueryable();
            if(string.IsNullOrEmpty(lastname))
            {
                result = result.Where(a => a.Employee.FirstName == firstname);
            }
            else
            {
                result = result.Where(a => a.Employee.FirstName == firstname && a.Employee.LastName == lastname);
            }
            return await result.ToListAsync();
        }

        public async Task<List<AttendanceRecord>> GetAttendanceRecordsFilteredAsync(string? departmentName = null, string? positionName = null, DateTime? date = null, string? Status = null)
        {
            var result = db.AttendanceRecords.AsNoTracking().
                Include(a => a.Employee).
                ThenInclude(e => e.Department).
                Include(a => a.Employee).
                ThenInclude(e => e.Position).
                AsQueryable();
            if (!string.IsNullOrEmpty(departmentName)) 
            {
                result = result.Where(a => a.Employee.Department.DepartmentName == departmentName);
            }
            if (!string.IsNullOrEmpty(positionName))
            {
                result = result.Where(a => a.Employee.Position.PositionName == positionName);
            }
            if (date.HasValue)
            {
                var dt = date.Value.Date;
                result = result.Where(a => EF.Functions.DateDiffDay(dt, a.CheckIn) == 0);
            }
            if (!string.IsNullOrEmpty(Status))
            {
                result = result.Where(a => a.Status == Status);
            }
            return await result.ToListAsync();
        }
        public async Task<AttendanceRecord?> CheckoutAsync(int employeeId)
        {
            var record = await db.AttendanceRecords
                .Where(a => a.EmployeeId == employeeId && a.CheckOut == null)
                .OrderByDescending(a => a.CheckIn)
                .FirstOrDefaultAsync();

            if (record == null)
                return null;

            record.CheckOutNow();
            await db.SaveChangesAsync();
            return record;
        }

    }
}

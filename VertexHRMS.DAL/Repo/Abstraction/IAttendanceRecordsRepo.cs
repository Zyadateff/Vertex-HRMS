namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IAttendanceRecordsRepo
    {
        Task<List<AttendanceRecord>> GetAttendanceRecordsFilteredAsync(string? departmentName = null, string? positionName = null, DateTime? date = null, string? Status = null);
        Task<List<AttendanceRecord>?> GetAttendanceRecordByNameAsync(string firstname, string? lastname = null);
        Task<AttendanceRecord> AddAttendanceRecordAsync(AttendanceRecord attendanceRecord);
        Task<AttendanceRecord?> CheckoutAsync(int employeeId);
    }
}

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IAttendanceRecordsService
    {
        Task<List<AttendanceRecordsVM>> GetAttendanceRecordsFilteredAsync(string? departmentName = null, string? positionName = null, DateTime? date = null, string? status = null);
        Task<List<AttendanceRecordsVM>?> GetAttendanceRecordByNameAsync(string firstname, string? lastname = null);
        Task<AttendanceRecordsVM?> CheckoutAsync(int employeeId);
        Task AddAttendanceRecordAsync(AttendanceRecordsVM attendanceRecord);

    }
}

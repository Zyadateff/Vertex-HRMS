using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.ModelVM.AttendanceRecords
{
    public class AttendanceRecordsVM
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? DepartmentName { get; set; }
        public string? Position { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public decimal? WorkHours { get; set; }
        public string Status { get; set; }
    }
}

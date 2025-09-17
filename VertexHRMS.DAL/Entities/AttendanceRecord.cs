
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{

    public class AttendanceRecord
    {
        public AttendanceRecord()
        {
            
        }
        public AttendanceRecord(int employeeId, DateTime attendanceDate, DateTime checkIn, DateTime checkOut, decimal workHours, string status)
        {
            EmployeeId = employeeId;
            AttendanceDate = attendanceDate;
            CheckIn = checkIn;
            CheckOut = checkOut;
            WorkHours = workHours;
            Status = status;
        }
        public int AttendanceRecordId { get; private set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime AttendanceDate { get; private set; }
        public DateTime CheckIn { get; private set; }
        public DateTime CheckOut { get; private set; }
        public decimal WorkHours { get; private set; }
        public string Status { get; private set; }
    }
}

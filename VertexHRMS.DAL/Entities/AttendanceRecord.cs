
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{
    public class AttendanceRecord
    {
        public int AttendanceId { get; private set; }
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

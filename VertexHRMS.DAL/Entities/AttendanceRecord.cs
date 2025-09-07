
namespace VertexHRMS.DAL.Entities
{
    public class AttendanceRecord
    {
        public int AttendanceID { get; private set; }
        public int EmployeeID { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime AttendanceDate { get; private set; }
        public DateTime CheckIn { get; private set; }
        public DateTime CheckOut { get; private set; }
        public decimal WorkHours { get; private set; }
        public string Status { get; private set; }
    }
}

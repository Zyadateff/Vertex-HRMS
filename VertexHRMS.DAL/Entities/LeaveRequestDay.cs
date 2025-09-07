
namespace VertexHRMS.DAL.Entities
{
    public class LeaveRequestDay
    {
        public int LeaveRequestDayID { get; private set; }
        public int LeaveRequestID { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public DateTime TheDate { get; private set; }
        public decimal ChargeableHours { get; private set; }
        public bool IsHoliday { get; private set; }
    }
}

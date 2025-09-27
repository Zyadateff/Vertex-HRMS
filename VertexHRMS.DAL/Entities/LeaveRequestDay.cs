
namespace VertexHRMS.DAL.Entities
{
    public class LeaveRequestDay
    {
        public LeaveRequestDay()
        {

        }
        public LeaveRequestDay(int leaveRequestId, DateTime theDate, decimal chargeableHours, bool isHoliday)
        {
            LeaveRequestId = leaveRequestId;
            TheDate = theDate;
            ChargeableHours = chargeableHours;
            IsHoliday = isHoliday;
        }
        public int LeaveRequestDayId { get; private set; }
        public int LeaveRequestId { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public DateTime TheDate { get; private set; }
        public decimal ChargeableHours { get; private set; }
        public bool IsHoliday { get; private set; }
    }
}

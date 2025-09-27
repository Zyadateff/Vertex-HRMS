
namespace VertexHRMS.DAL.Entities
{
    public class OvertimeRequest
    {
        public OvertimeRequest()
        {

        }
        public OvertimeRequest(int employeeId, DateTime overtimeDate, DateTime startTime, DateTime endTime, decimal hours, string status, string requestedByUserId)
        {
            EmployeeId = employeeId;
            OvertimeDate = overtimeDate;
            StartTime = startTime;
            EndTime = endTime;
            Hours = hours;
            Status = status;
            RequestedByUserId = requestedByUserId;
        }
        public int OvertimeRequestId { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime OvertimeDate { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public decimal Hours { get; private set; }
        public string Status { get; private set; }
        public string RequestedByUserId { get; private set; }
        public ApplicationUser RequestedByUser { get; private set; }
    }
}

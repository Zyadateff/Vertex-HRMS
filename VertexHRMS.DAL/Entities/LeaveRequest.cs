
namespace VertexHRMS.DAL.Entities
{
    public class LeaveRequest
    {
        public int LeaveRequestID { get; private set; }
        public int EmployeeID { get; private set; }
        public Employee Employee { get; private set; }
        public int LeaveTypeID { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public DateTime StartDateTime { get; private set; }
        public DateTime EndDateTime { get; private set; }
        public decimal DurationHours { get; private set; }
        public string Status { get; private set; }
        public string RequestedByUserId { get; private set; }
        public ApplicationUser RequestedByUser { get; private set; }
        public ICollection<LeaveRequestDay> Days { get; private set; } = new List<LeaveRequestDay>();
        public ICollection<LeaveApproval> Approvals { get; private set; } = new List<LeaveApproval>();
    }
}

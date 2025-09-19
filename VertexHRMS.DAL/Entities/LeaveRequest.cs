
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.DAL.Entities
{
    public class LeaveRequest
    {
        public LeaveRequest()
        {

        }

        public LeaveRequest(int employeeId, int leaveTypeID, DateTime startDateTime, DateTime endDateTime, string requestedByUserId, string status = "Pending")
        {
            EmployeeId = employeeId;
            LeaveTypeID = leaveTypeID;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            RequestedByUserId = requestedByUserId;
            Status = status;
            DurationHours = (decimal)(endDateTime - startDateTime).TotalDays * 8;

            Days = new List<LeaveRequestDay>();
            Approvals = new List<LeaveApproval>();
        }

        public void UpdateStatus(string status)
        {
            Status = status;
        }
        public void CalculateDurationHours()
        {
            var n = (decimal)(EndDateTime - StartDateTime).TotalDays;
            DurationHours = n * 8;
        }
        public void updateMissing(Employee _employee, LeaveType leaveType, string status, ApplicationUser user)
        {
            Employee = _employee;
            LeaveType=leaveType;
            CalculateDurationHours();
            UpdateStatus(status);
            RequestedByUserId = EmployeeId.ToString();
            RequestedByUser = user;
        }

        public decimal GetDurationInDays()
        {
            return DurationHours / 8;
        }
        public int LeaveRequestId { get; private set; }
        public int EmployeeId { get; private set; }
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

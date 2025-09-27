
using VertexHRMS.DAL.Repo.Implementation;
using VertexHRMS.DAL.Repo.Service;

namespace VertexHRMS.DAL.Entities
{
    public class LeaveRequest
    {
        public LeaveRequest()
        {

        }

        public LeaveRequest(int employeeId, int leaveTypeID, DateTime startDateTime, DateTime endDateTime, string status = "Pending", string rejectionReason = null)
        {
            EmployeeId = employeeId;
            LeaveTypeID = leaveTypeID;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            Status = status;
            DurationHours = (decimal)(endDateTime - startDateTime).TotalDays * 8;

            Days = new List<LeaveRequestDay>();
            Approvals = new List<LeaveApproval>();
            RejectionReason = rejectionReason;
        }

        public LeaveRequest(int leaveRequestId, int employeeId, Employee employee, int leaveTypeID, LeaveType leaveType, DateTime startDateTime, DateTime endDateTime, decimal durationHours, string status, string rejectionReason)
        {
            LeaveRequestId = leaveRequestId;
            EmployeeId = employeeId;
            Employee = employee;
            LeaveTypeID = leaveTypeID;
            LeaveType = leaveType;
            StartDateTime = startDateTime;
            EndDateTime = endDateTime;
            DurationHours = durationHours;
            Status = status;
            RejectionReason = rejectionReason;
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
        public void updateMissing(Employee _employee, LeaveType leaveType, string status, string rejectionReason)
        {
            Employee = _employee;
            LeaveType=leaveType;
            CalculateDurationHours();
            UpdateStatus(status);
            RejectionReason=rejectionReason;
        }

        public decimal GetDurationInDays()
        {
            return DurationHours / 8;
        }
        public string getReason()
        {
            return RejectionReason;
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
        public string RejectionReason { get; private set; }
        public ICollection<LeaveRequestDay> Days { get; private set; } = new List<LeaveRequestDay>();
        public ICollection<LeaveApproval> Approvals { get; private set; } = new List<LeaveApproval>();
    }
}

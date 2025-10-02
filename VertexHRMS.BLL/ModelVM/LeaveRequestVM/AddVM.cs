    namespace VertexHRMS.BLL.ModelVM.LeaveRequestVM
    {
        public class AddVM
        {
            public AddVM(int employeeId, int leaveTypeID, DateTime startDateTime, DateTime endDateTime)
            {
                EmployeeId = employeeId;
                LeaveTypeID = leaveTypeID;
                StartDateTime = startDateTime;
                EndDateTime = endDateTime;
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

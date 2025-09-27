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

        public int EmployeeId { get;  set; }
        public int LeaveTypeID { get;  set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}

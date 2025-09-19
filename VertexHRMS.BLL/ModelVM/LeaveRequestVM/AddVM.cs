namespace VertexHRMS.BLL.ModelVM.LeaveRequestVM
{
    public class AddVM
    {
        public int EmployeeId { get;  set; }
        public int LeaveTypeID { get;  set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string RequestedByUserId { get; set; }
    }
}


namespace VertexHRMS.DAL.Entities
{
    public class LeaveApproval
    {
        public int LeaveApprovalID { get; private set; }
        public int LeaveRequestID { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public int Level { get; private set; }
        public int? ApproverEmployeeID { get; private set; }
        public Employee ApproverEmployee { get; private set; }
        public string ApproverUserId { get; private set; }
        public ApplicationUser ApproverUser { get; private set; }
        public string Action { get; private set; }
        public DateTime ActionAt { get; private set; }
    }
}

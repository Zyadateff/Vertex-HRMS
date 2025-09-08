
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{
    public class LeaveApproval
    {
        public LeaveApproval()
        {

        }
        public int LeaveApprovalId { get; private set; }
        public int LeaveRequestId { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public int Level { get; private set; }
        public int? ApproverEmployeeId { get; private set; }
        [ForeignKey("ApproverEmployeeId")]
        public Employee ApproverEmployee { get; private set; }
        public string ApproverUserId { get; private set; }
        [ForeignKey("ApproverUserId")]
        public ApplicationUser ApproverUser { get; private set; }
        public string Action { get; private set; }
        public DateTime ActionAt { get; private set; }
    }
}

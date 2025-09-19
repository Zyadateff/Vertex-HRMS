using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveApprovalVM
{
    public class GetByApproverVM
    {
        public int LeaveApprovalId { get;  set; }
        public int LeaveRequestId { get;  set; }
        public LeaveRequest LeaveRequest { get;  set; }
        public int Level { get; set; }
        public int? ApproverEmployeeId { get; set; }
        [ForeignKey("ApproverEmployeeId")]
        public Employee ApproverEmployee { get; set; }
        public ApplicationUser ApproverUser { get; set; }
        public string Action { get; set; }
        public DateTime ActionAt { get; set; }
    }
}

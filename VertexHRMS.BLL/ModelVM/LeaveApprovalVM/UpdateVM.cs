using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveApprovalVM
{
    public class UpdateVM
    {
        public int LeaveApprovalId { get;  set; }
        public int LeaveRequestId { get;  set; }
        public int Level { get; set; }
        public int? ApproverEmployeeId { get; set; }
        [ForeignKey("ApproverEmployeeId")]
        public string ApproverUserId { get; set; }
        [ForeignKey("ApproverUserId")]
        public string Action { get; set; }
        public DateTime ActionAt { get;  set; }
    }
}

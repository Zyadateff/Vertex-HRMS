using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveApprovalVM
{
    public class AddVM
    {
        public int LeaveRequestId { get; private set; }
        public int Level { get; private set; }
        public int? ApproverEmployeeId { get; private set; }
        [ForeignKey("ApproverEmployeeId")]
        public string ApproverUserId { get; private set; }
        [ForeignKey("ApproverUserId")]
        public string Action { get; private set; }
        public DateTime ActionAt { get; private set; }
    }
}

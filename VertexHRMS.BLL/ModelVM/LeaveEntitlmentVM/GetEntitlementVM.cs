using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM
{
    public class GetEntitlementVM
    {
        public int LeaveEntitlementId { get;  set; }
        public Employee Employee { get; set; }
        public LeaveType LeaveType { get;  set; }
        public decimal Entitled { get;  set; }
        public decimal CarriedIn { get;  set; }
        public decimal Used { get;  set; }
    }
}

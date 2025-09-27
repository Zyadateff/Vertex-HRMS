using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM
{
    public class GetAllForEmployeeVM
    {
        public int LeaveEntitlementId { get; set; }
        public Employee Employee { get; set; }
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public decimal Entitled { get; set; }
        public decimal CarriedIn { get; set; }
        public decimal Used { get; set; }
        public int Year { get; private set; }
    }
}

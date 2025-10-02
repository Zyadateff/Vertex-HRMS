using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM
{
    public class GetAllForEmployeeVM
    {
        public int LeaveEntitlementId { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public int LeaveTypeId { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public int Year { get; private set; }
        public decimal Entitled { get; private set; }
        public decimal CarriedIn { get; private set; }
        public decimal Used { get; private set; }
    }
}

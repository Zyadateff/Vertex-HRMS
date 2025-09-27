using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveLedgerVM
{
    public class GetByEmployeeVM
    {
        public int LeaveLedgerId { get;  set; }
        public Employee Employee { get;  set; }
        public int LeaveTypeId { get;  set; }
        public LeaveType LeaveType { get;  set; }
        public string TxnType { get;  set; }
        public decimal Quantity { get;  set; }
        public DateTime EffectiveDate { get;  set; }
    }
}

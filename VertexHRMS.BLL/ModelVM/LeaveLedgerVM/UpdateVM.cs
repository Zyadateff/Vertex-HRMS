using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveLedgerVM
{
    public class UpdateVM
    {
        public int LeaveLedgerId { get;  set; }
        public int EmployeeId { get;  set; }
        public int LeaveTypeId { get; set; }
        public string TxnType { get;  set; }
        public decimal Quantity { get;  set; }
        public DateTime EffectiveDate { get;  set; }
    }
}

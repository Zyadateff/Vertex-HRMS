using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveLedgerVM
{
    public class AddVM
    {
        public int EmployeeId { get; private set; }
        public int LeaveTypeId { get; private set; }
        public string TxnType { get; private set; }
        public decimal Quantity { get; private set; }
        public DateTime EffectiveDate { get; private set; }
    }
}

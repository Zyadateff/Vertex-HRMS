using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM
{
    public class AddVM
    {
        public int EmployeeId { get;  set; }
        public int LeaveTypeId { get; set; }
        public int Year { get; set; }
        public decimal Entitled { get;  set; }
        public decimal CarriedIn { get;  set; }
        public decimal Used { get;  set; }
    }
}

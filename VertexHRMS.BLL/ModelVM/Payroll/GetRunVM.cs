using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.Payroll
{
    public class GetRunVM
    {

        public int PayrollRunId { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public DateTime RunDate { get; private set; }
        public ICollection<GetPayrollVM> Payrolls { get; private set; } = new List<GetPayrollVM>();


    }
}

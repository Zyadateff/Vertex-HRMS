using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.ModelVM.Payroll
{
    public class GetPayrollVM
    {
        public int PayrollId { get; private set; }
        public int PayrollRunId { get; private set; }
        public PayrollRun PayrollRun { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public decimal BaseSalary { get; private set; }
        public decimal GrossEarnings { get; private set; }
        public decimal NetSalary { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public ICollection<PayrollDeduction> Deductions { get; private set; } = new List<PayrollDeduction>();

    }
}

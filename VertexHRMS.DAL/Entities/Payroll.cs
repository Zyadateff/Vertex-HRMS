
namespace VertexHRMS.DAL.Entities
{
    public class Payroll
    {
        public Payroll()
        {

        }
        public Payroll(int payrollRunId, int employeeId, decimal baseSalary, decimal grossEarnings, decimal netSalary, DateTime paymentDate)
        {
            PayrollRunId = payrollRunId;
            EmployeeId = employeeId;
            BaseSalary = baseSalary;
            GrossEarnings = grossEarnings;
            NetSalary = netSalary;
            PaymentDate = paymentDate;
        }
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

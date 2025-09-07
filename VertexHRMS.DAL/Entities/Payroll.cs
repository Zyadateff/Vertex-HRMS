
namespace VertexHRMS.DAL.Entities
{
    public class Payroll
    {
        public int PayrollID { get; private set; }
        public int PayrollRunID { get; private set; }
        public PayrollRun PayrollRun { get; private set; }
        public int EmployeeID { get; private set; }
        public Employee Employee { get; private set; }
        public decimal BaseSalary { get; private set; }
        public decimal GrossEarnings { get; private set; }
        public decimal NetSalary { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public ICollection<PayrollDeduction> Deductions { get; private set; } = new List<PayrollDeduction>();
    }
}

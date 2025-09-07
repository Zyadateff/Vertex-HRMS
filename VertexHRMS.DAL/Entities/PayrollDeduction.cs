
namespace VertexHRMS.DAL.Entities
{
    public class PayrollDeduction
    {
        public int PayrollDeductionID { get; private set; }
        public int PayrollID { get; private set; }
        public Payroll Payroll { get; private set; }
        public int DeductionID { get; private set; }
        public Deduction Deduction { get; private set; }
        public decimal Amount { get; private set; }
    }
}


namespace VertexHRMS.DAL.Entities
{
    public class PayrollDeduction
    {

        public PayrollDeduction(int payrollId, int deductionId, decimal amount)
        {
            PayrollId = payrollId;
            DeductionId = deductionId;
            Amount = amount;
        }
        public int PayrollDeductionId { get; private set; }
        public int PayrollId { get; private set; }
        public Payroll Payroll { get; private set; }
        public int DeductionId { get; private set; }
        public Deduction Deduction { get; private set; }
        public decimal Amount { get; private set; }
    }
}

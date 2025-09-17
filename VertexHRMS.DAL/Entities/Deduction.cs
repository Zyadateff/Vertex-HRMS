
namespace VertexHRMS.DAL.Entities
{
    public class Deduction
    {
        public Deduction()
        {

        }
        public Deduction(string name, bool isPercentage, decimal amountOrPercent)
        {
            Name = name;
            IsPercentage = isPercentage;
            AmountOrPercent = amountOrPercent;
        }
        public int DeductionId { get; private set; }
        public string Name { get; private set; }
        public bool IsPercentage { get; private set; }
        public decimal AmountOrPercent { get; private set; }
        public ICollection<PayrollDeduction> PayrollDeductions { get; private set; } = new List<PayrollDeduction>();
    }
}

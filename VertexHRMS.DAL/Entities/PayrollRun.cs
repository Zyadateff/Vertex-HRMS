
namespace VertexHRMS.DAL.Entities
{
    public class PayrollRun
    {
        public PayrollRun()
        {

        }
        public int PayrollRunId { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public DateTime RunDate { get; private set; }
        public string RunByUserId { get; private set; }
        public ApplicationUser RunByUser { get; private set; }
        public ICollection<Payroll> Payrolls { get; private set; } = new List<Payroll>();
    }
}

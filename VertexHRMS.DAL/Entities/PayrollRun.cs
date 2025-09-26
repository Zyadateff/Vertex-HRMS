
namespace VertexHRMS.DAL.Entities
{
    public class PayrollRun
    {
        public PayrollRun()
        {

        }
        public PayrollRun(DateTime periodStart, DateTime periodEnd, DateTime runDate)
        {
            PeriodStart = periodStart;
            PeriodEnd = periodEnd;
            RunDate = runDate;
        }

        public int PayrollRunId { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }
        public DateTime RunDate { get; private set; }
        public ICollection<Payroll> Payrolls { get; private set; } = new List<Payroll>();
    }
}

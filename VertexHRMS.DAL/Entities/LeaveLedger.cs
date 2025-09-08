
namespace VertexHRMS.DAL.Entities
{
    public class LeaveLedger
    {
        public LeaveLedger()
        {

        }
        public int LedgerId { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public int LeaveTypeId { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string TxnType { get; private set; }
        public decimal Quantity { get; private set; }
        public DateTime EffectiveDate { get; private set; }
    }
}


namespace VertexHRMS.DAL.Entities
{
    public class LeaveLedger
    {
        public int LedgerID { get; private set; }
        public int EmployeeID { get; private set; }
        public Employee Employee { get; private set; }
        public int LeaveTypeID { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string TxnType { get; private set; }
        public decimal Quantity { get; private set; }
        public DateTime EffectiveDate { get; private set; }
    }
}

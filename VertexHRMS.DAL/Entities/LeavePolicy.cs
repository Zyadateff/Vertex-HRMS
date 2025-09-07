
namespace VertexHRMS.DAL.Entities
{
    public class LeavePolicy
    {
        public int LeavePolicyID { get; private set; }
        public int LeaveTypeID { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string AccrualMethod { get; private set; }
        public decimal EntitlementPerYear { get; private set; }
    }
}

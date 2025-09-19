
namespace VertexHRMS.DAL.Entities
{
    public class LeavePolicy
    {
        public LeavePolicy()
        {

        }
        public LeavePolicy(int leaveTypeId, string accrualMethod, decimal entitlementPerYear)
        {
            LeaveTypeId = leaveTypeId;
            AccrualMethod = accrualMethod;
            EntitlementPerYear = entitlementPerYear;
        }
        public int LeavePolicyId { get; private set; }
        public int LeaveTypeId { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public string AccrualMethod { get; private set; }
        public decimal EntitlementPerYear { get; private set; }
    }
}

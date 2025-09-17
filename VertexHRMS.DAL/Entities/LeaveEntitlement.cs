
namespace VertexHRMS.DAL.Entities
{
    public class LeaveEntitlement
    {
        public LeaveEntitlement()
        {

        }
        public LeaveEntitlement(int employeeId, int leaveTypeId, int year, decimal entitled, decimal carriedIn, decimal used)
        {
            EmployeeId = employeeId;
            LeaveTypeId = leaveTypeId;
            Year = year;
            Entitled = entitled;
            CarriedIn = carriedIn;
            Used = used;
        }
        public int LeaveEntitlementId { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public int LeaveTypeId { get; private set; }
        public LeaveType LeaveType { get; private set; }
        public int Year { get; private set; }
        public decimal Entitled { get; private set; }
        public decimal CarriedIn { get; private set; }
        public decimal Used { get; private set; }
    }
}

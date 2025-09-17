
namespace VertexHRMS.DAL.Entities
{
    public class LeaveType
    {
        public LeaveType()
        {

        }
        public LeaveType(string name, bool isPaid, string unit)
        {
            Name = name;
            IsPaid = isPaid;
            Unit = unit;
        }
        public int LeaveTypeId { get; private set; }
        public string Name { get; private set; }
        public bool IsPaid { get; private set; }
        public string Unit { get; private set; }
        public ICollection<LeavePolicy> Policies { get; private set; } = new List<LeavePolicy>();
    }
}

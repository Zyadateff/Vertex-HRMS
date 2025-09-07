
namespace VertexHRMS.DAL.Entities
{
    public class Resignation
    {
        public int ResignationID { get; private set; }
        public int EmployeeID { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime NoticeDate { get; private set; }
        public DateTime LastWorkingDate { get; private set; }
        public string Status { get; private set; }
        public string RequestedByUserId { get; private set; }
        public ApplicationUser RequestedByUser { get; private set; }
        public ExitClearance ExitClearance { get; private set; }
    }
}

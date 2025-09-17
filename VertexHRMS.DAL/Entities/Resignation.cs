
namespace VertexHRMS.DAL.Entities
{
    public class Resignation
    {
        public Resignation()
        {

        }
        public Resignation(int employeeId, DateTime noticeDate, DateTime lastWorkingDate, string status, string requestedByUserId)
        {
            EmployeeId = employeeId;
            NoticeDate = noticeDate;
            LastWorkingDate = lastWorkingDate;
            Status = status;
            RequestedByUserId = requestedByUserId;
        }
        public int ResignationId { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime NoticeDate { get; private set; }
        public DateTime LastWorkingDate { get; private set; }
        public string Status { get; private set; }
        public string RequestedByUserId { get; private set; }
        public ApplicationUser RequestedByUser { get; private set; }
        public ExitClearance ExitClearance { get; private set; }
    }
}


namespace VertexHRMS.DAL.Entities
{
    public class JobOpening
    {
        public JobOpening()
        {

        }
        public int JobOpeningId { get; private set; }
        public int PositionId { get; private set; }
        public Position Position { get; private set; }
        public int DepartmentId { get; private set; }
        public Department Department { get; private set; }
        public string JobTitle { get; private set; }
        public DateTime PostedDate { get; private set; }
        public DateTime ClosingDate { get; private set; }
        public string Status { get; private set; }
        public ICollection<Applicant> Applicants { get; private set; } = new List<Applicant>();
        public ICollection<Interview> Interviews { get; private set; } = new List<Interview>();
        public ICollection<Offer> Offers { get; private set; } = new List<Offer>();
    }
}


namespace VertexHRMS.DAL.Entities
{
    public class Applicant
    {
        public int ApplicantID { get; private set; }
        public int JobOpeningID { get; private set; }
        public JobOpening JobOpening { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string ResumePath { get; private set; }
        public DateTime AppliedDate { get; private set; }
        public string Status { get; private set; }
        public string IdentityUserId { get; private set; }
        public ApplicationUser IdentityUser { get; private set; }
        public ICollection<Interview> Interviews { get; private set; } = new List<Interview>();
        public ICollection<Offer> Offers { get; private set; } = new List<Offer>();
        public Onboarding Onboarding { get; private set; }
    }
}

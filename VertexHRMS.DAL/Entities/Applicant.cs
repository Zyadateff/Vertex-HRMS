
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{
    [Table("Applicants")]
    public class Applicant
    {
        public Applicant()
        {

        }
        public Applicant(int jobOpeningId, string firstName, string lastName, string email, string resumePath, DateTime appliedDate, string status, string identityUserId)
        {
            JobOpeningId = jobOpeningId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ResumePath = resumePath;
            AppliedDate = appliedDate;
            Status = status;
            IdentityUserId = identityUserId;
        }
        public int ApplicantId { get; private set; }
        public int JobOpeningId { get; private set; }
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

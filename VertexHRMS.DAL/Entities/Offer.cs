
namespace VertexHRMS.DAL.Entities
{
    public class Offer
    {
        public Offer()
        {

        }
        public int OfferId { get; private set; }
        public int ApplicantId { get; private set; }
        public Applicant Applicant { get; private set; }
        public int JobOpeningId { get; private set; }
        public JobOpening JobOpening { get; private set; }
        public DateTime OfferDate { get; private set; }
        public decimal OfferedSalary { get; private set; }
        public DateTime JoiningDate { get; private set; }
        public string Status { get; private set; }
        public string IssuedByUserId { get; private set; }
        public ApplicationUser IssuedByUser { get; private set; }
    }
}

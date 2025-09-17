
namespace VertexHRMS.DAL.Entities
{
    public class Offer
    {
        public Offer()
        {

        }
        public Offer(int applicantId, int jobOpeningId, DateTime offerDate, decimal offeredSalary, DateTime joiningDate, string status, string issuedByUserId)
        {
            ApplicantId = applicantId;
            JobOpeningId = jobOpeningId;
            OfferDate = offerDate;
            OfferedSalary = offeredSalary;
            JoiningDate = joiningDate;
            Status = status;
            IssuedByUserId = issuedByUserId;
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


namespace VertexHRMS.DAL.Entities
{
    public class Offer
    {
        public int OfferID { get; private set; }
        public int ApplicantID { get; private set; }
        public Applicant Applicant { get; private set; }
        public int JobOpeningID { get; private set; }
        public JobOpening JobOpening { get; private set; }
        public DateTime OfferDate { get; private set; }
        public decimal OfferedSalary { get; private set; }
        public DateTime JoiningDate { get; private set; }
        public string Status { get; private set; }
        public string IssuedByUserId { get; private set; }
        public ApplicationUser IssuedByUser { get; private set; }
    }
}

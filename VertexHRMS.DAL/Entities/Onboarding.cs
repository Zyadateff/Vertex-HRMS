
namespace VertexHRMS.DAL.Entities
{
    public class Onboarding
    {
        public int OnboardingID { get; private set; }
        public int ApplicantID { get; private set; }
        public Applicant applicant { get; private set; }
        public int EmployeeID { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime StartDate { get; private set; }
        public bool OrientationCompleted { get; private set; }
        public string ResponsibleUserId { get; private set; }
        public ApplicationUser ResponsibleUser { get; private set; }
    }
}

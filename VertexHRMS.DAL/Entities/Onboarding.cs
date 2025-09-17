
namespace VertexHRMS.DAL.Entities
{
    public class Onboarding
    {
        public Onboarding()
        {

        }
        public Onboarding(int applicantId, int employeeId, DateTime startDate, bool orientationCompleted, string responsibleUserId)
        {
            ApplicantId = applicantId;
            EmployeeId = employeeId;
            StartDate = startDate;
            OrientationCompleted = orientationCompleted;
            ResponsibleUserId = responsibleUserId;
        }
        public int OnboardingId { get; private set; }
        public int ApplicantId { get; private set; }
        public Applicant applicant { get; private set; }
        public int EmployeeId { get; private set; }
        public Employee Employee { get; private set; }
        public DateTime StartDate { get; private set; }
        public bool OrientationCompleted { get; private set; }
        public string ResponsibleUserId { get; private set; }
        public ApplicationUser ResponsibleUser { get; private set; }
    }
}

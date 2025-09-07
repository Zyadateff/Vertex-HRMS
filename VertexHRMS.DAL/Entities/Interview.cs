using System;
using System.Collections.Generic;

namespace VertexHRMS.DAL.Entities
{
    public class Interview
    {
        public int InterviewID { get; private set; }
        public int ApplicantID { get; private set; }
        public Applicant Applicant { get; private set; }
        public int JobOpeningID { get; private set; }
        public JobOpening JobOpening { get; private set; }
        public DateTime InterviewDate { get; private set; }
        public int? InterviewerID { get; private set; }
        public Employee Interviewer { get; private set; }
        public string Feedback { get; private set; }
        public string InterviewerUserId { get; private set; }
        public ApplicationUser InterviewerUser { get; private set; }
    }
}

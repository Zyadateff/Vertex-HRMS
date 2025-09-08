using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VertexHRMS.DAL.Entities
{
    public class Interview
    {
        public Interview()
        {

        }
        public int InterviewId { get; private set; }
        public int ApplicantId { get; private set; }
        public Applicant Applicant { get; private set; }
        public int JobOpeningId { get; private set; }
        public JobOpening JobOpening { get; private set; }
        public DateTime InterviewDate { get; private set; }
        public int? InterviewerId { get; private set; }
        [ForeignKey("InterviewerId")]
        public Employee Interviewer { get; private set; }
        public string Feedback { get; private set; }
        public string InterviewerUserId { get; private set; }

        [ForeignKey("InterviewerUserId")]
        public ApplicationUser InterviewerUser { get; private set; }
    }
}

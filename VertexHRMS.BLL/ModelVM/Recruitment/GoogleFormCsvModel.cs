using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.Recruitment
{
    using CsvHelper.Configuration.Attributes;

    public class GoogleFormCsvModel
    {
        [Name("SubmittedAt")]
        public string SubmittedAt { get; set; } = "";

        [Name("FullName")]
        public string FullName { get; set; } = "";

        [Name("Email")]
        public string Email { get; set; } = "";

        [Name("Phone")]
        public string Phone { get; set; } = "";

        [Name("LinkedIn Profile URL")]
        public string LinkedInProfileURL { get; set; } = "";

        [Name("ResumeUrl")]
        public string ResumeUrl { get; set; } = "";

        [Name("YearsOfExperience")]
        public string YearsOfExperience { get; set; } = "";

        [Name("Which of the following best describes your current employment status?")]
        public string EmploymentStatus { get; set; } = "";

        [Name("How did you hear about this job opening?")]
        public string ReferralSource { get; set; } = "";

        [Name("Please provide a cover letter (optional)")]
        public string CoverLetter { get; set; } = "";

        [Name("SkillsCsv")]
        public string SkillsCsv { get; set; } = "";
    }


}

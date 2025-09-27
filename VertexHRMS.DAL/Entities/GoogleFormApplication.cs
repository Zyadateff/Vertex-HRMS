using System;

namespace VertexHRMS.DAL.Entities.Recruitment
{
    public class GoogleFormApplication
    {
        public int Id { get; set; }

        // A unique row id from Google Form / sheet
        public string ExternalRowId { get; set; } = default!;

        // Some parts of the code use FullName, others use First/Last — have both
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; } = default!;
        public string? Phone { get; set; }

        public string? ResumeUrl { get; set; }

        // Optional: a free-text comma list captured from the form
        public string? SkillsCsv { get; set; }

        // Optional: the form’s years of experience
        public int YearsOfExperience { get; set; }

        // Link to an existing opening if your form captured it
        public int? JobOpeningId { get; set; }

        public DateTime SubmittedAt { get; set; }

        // Keep the raw row for audit/debug
        public string RawJson { get; set; } = default!;

        // Light state used by the ingest pipeline
        // "New" / "Imported" / "Invalid" / "Duplicate"
        public string ImportStatus { get; set; } = "New";

        // Simple flag used by pipeline for de-dupe/import
        public bool Imported { get; set; } = false;
    }
}

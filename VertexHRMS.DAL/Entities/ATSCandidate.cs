using VertexHRMS.DAL.Enum;

namespace VertexHRMS.DAL.Entities
{
    public class ATSCandidate
    {
        public int Id { get; set; }

        // link to job opening
        public int JobOpeningId { get; set; }

        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }

        // file path to stored resume
        public string? ResumePath { get; set; }

        // 🔑 use only one hash property for deduplication
        public string ResumeHash { get; set; } = default!;

        // skills + experience
        public string SkillsCsv { get; set; } = "";
        public int YearsOfExperience { get; set; }

        // status tracking
        public CandidateStatus Status { get; set; } = CandidateStatus.Applied;

        // audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // optional: link back to source app (Google Form, external form, etc.)
        public int? SourceApplicationId { get; set; }
        // New interview fields
        public DateTime? InterviewDate { get; set; }
        public string? Interviewer { get; set; }
        public string? InterviewNotes { get; set; }
    }
}

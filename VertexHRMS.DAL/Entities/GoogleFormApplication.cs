namespace VertexHRMS.DAL.Entities
{
    public class GoogleFormApplication
    {
        public int Id { get; set; }

        public string ExternalRowId { get; set; } = default!;

        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string Email { get; set; } = default!;
        public string? Phone { get; set; }

        public string? ResumeUrl { get; set; }

        public string? SkillsCsv { get; set; }

        public int YearsOfExperience { get; set; }

        public int? JobOpeningId { get; set; }

        public DateTime SubmittedAt { get; set; }

        public string RawJson { get; set; } = default!;

        public string ImportStatus { get; set; } = "New";

        public bool Imported { get; set; } = false;
    }
}

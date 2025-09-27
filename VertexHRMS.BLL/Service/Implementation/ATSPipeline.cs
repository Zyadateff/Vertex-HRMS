using Microsoft.EntityFrameworkCore;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities.Recruitment;

namespace VertexHRMS.BLL.Services.Implementation
{
    public class ATSPipeline : IATSPipeline
    {
        private readonly VertexHRMSDbContext _db;
        private readonly IFileStore _files;
        private readonly IResumeParser _parser;

        public ATSPipeline(VertexHRMSDbContext db, IFileStore files, IResumeParser parser)
        { _db = db; _files = files; _parser = parser; }

        public async Task ProcessNewAsync(CancellationToken ct = default)
        {
            var items = await _db.GoogleFormApplications.Where(x => !x.Imported).ToListAsync(ct);

            foreach (var row in items)
            {
                var resumePath = string.IsNullOrWhiteSpace(row.ResumeUrl) ? null
                    : await _files.FetchResumeAsync(row.ResumeUrl, ct);

                var hash = resumePath is null ? null : await _files.Sha256Async(resumePath, ct);

                // duplicate filter: email or resume hash
                var duplicate = await _db.ATSCandidates.AnyAsync(
                    c => c.Email == row.Email || (hash != null && c.ResumeHash == hash), ct);

                if (duplicate) { row.Imported = true; continue; }

                var parsed = resumePath is null ? default : await _parser.ParseAsync(resumePath, ct);

                _db.ATSCandidates.Add(new ATSCandidate
                {
                    FirstName = row.FirstName ?? parsed.name?.Split(' ').FirstOrDefault() ?? "N/A",
                    LastName = row.LastName ?? parsed.name?.Split(' ').Skip(1).FirstOrDefault() ?? "",
                    Email = row.Email ?? parsed.email ?? "",
                    SkillsCsv = string.IsNullOrWhiteSpace(row.SkillsCsv) ? parsed.skillsCsv : row.SkillsCsv,
                    YearsOfExperience = row.YearsOfExperience != 0
    ? row.YearsOfExperience
    : parsed.years,

                    ResumePath = resumePath,
                    ResumeHash = hash,
                    Status = CandidateStatus.Applied,
                    CreatedAt = DateTime.UtcNow
                });

                row.Imported = true;
            }

            await _db.SaveChangesAsync(ct);
        }
    }
}

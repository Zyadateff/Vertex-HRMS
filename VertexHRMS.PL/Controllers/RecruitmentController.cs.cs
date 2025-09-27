using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Database;
using VertexHRMS.DAL.Entities.Recruitment;
using CsvHelper;
using System.Globalization;
using Newtonsoft.Json;
using VertexHRMS.BLL.ModelVM.Recruitment;



namespace VertexHRMS.PL.Controllers
{
    public class RecruitmentController : Controller
    {
        private readonly VertexHRMSDbContext _db;
        private readonly IFormIngestService _ingest;
        private readonly IATSPipeline _pipeline;

        public RecruitmentController(
            VertexHRMSDbContext db,
            IFormIngestService ingest,
            IATSPipeline pipeline)
        {
            _db = db;
            _ingest = ingest;
            _pipeline = pipeline;
        }

        // GET: /Recruitment
        // GET: /Recruitment
        public async Task<IActionResult> Index(string? q, CancellationToken ct)
        {
            

            var query = _db.ATSCandidates.AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(c =>
                    c.FirstName.Contains(q) ||
                    c.LastName.Contains(q) ||
                    c.Email.Contains(q) ||
                    c.SkillsCsv.Contains(q));
            }

            var candidates = await query
                .OrderBy(c => c.CreatedAt)
                .ToListAsync(ct);

            return View(candidates);
        }

        // Existing Import (ATS pipeline)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(CancellationToken ct)
        {
            await _ingest.ImportNewAsync(ct);
            return RedirectToAction(nameof(Index));
        }

        // Existing Process (ATS pipeline)
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Process(CancellationToken ct)
        {
            await _pipeline.ProcessNewAsync(ct);
            return RedirectToAction(nameof(Index));
        }

        // Update candidate status
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, CandidateStatus newStatus, string? notes, CancellationToken ct)
        {
            var candidate = await _db.ATSCandidates.FindAsync(new object[] { id }, ct);
            if (candidate == null) return NotFound();

            candidate.Status = newStatus;

            if (!string.IsNullOrWhiteSpace(notes))
            {
                _db.CandidateReviews.Add(new CandidateReview
                {
                    ATSCandidateId = id,
                    ReviewerUserId = User.Identity?.Name ?? "system",
                    NewStatus = newStatus,
                    Notes = notes,
                    ReviewedAt = DateTime.UtcNow,
                    ChangedAt = DateTime.UtcNow
                });
            }

            await _db.SaveChangesAsync(ct);
            return RedirectToAction(nameof(Index));
        }

        // NEW: Import Google Form CSV
        [HttpPost]
        public async Task<IActionResult> ImportGoogleFormCsv(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var path = Path.Combine(Path.GetTempPath(), file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Call method to read CSV and save to DB
                await ImportCsvToGoogleFormApplications(path);
            }

            return RedirectToAction(nameof(Index));
        }

        // CSV import helper
        private async Task ImportCsvToGoogleFormApplications(string filePath)
        {
            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<GoogleFormCsvModel>().ToList();


            foreach (var r in records)
            {
                var names = r.FullName.Trim().Split(' ', 2);
                var first = names.Length > 0 ? names[0] : "";
                var last = names.Length > 1 ? names[1] : "";

                var app = new GoogleFormApplication
                {
                    ExternalRowId = Guid.NewGuid().ToString(),
                    FullName = r.FullName,
                    FirstName = first,
                    LastName = last,
                    Email = r.Email,
                    Phone = r.Phone,
                    ResumeUrl = r.ResumeUrl,
                    SkillsCsv = r.SkillsCsv,
                    YearsOfExperience = int.TryParse(r.YearsOfExperience.Replace(" months", ""), out var y) ? y : 0,
                    SubmittedAt = DateTime.Parse(r.SubmittedAt),
                    RawJson = JsonConvert.SerializeObject(r),
                    ImportStatus = "New",
                    Imported = false
                };

                _db.GoogleFormApplications.Add(app);
            }

            await _db.SaveChangesAsync();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportGoogleFormsToATSAction(CancellationToken ct)
        {
            // 1️⃣ Get all new/unimported Google Form rows
            var newForms = await _db.GoogleFormApplications
                .Where(g => !g.Imported)
                .ToListAsync(ct);

            int importedCount = 0;

            foreach (var form in newForms)
            {
                // 2️⃣ Skip if candidate with same email already exists
                bool exists = await _db.ATSCandidates.AnyAsync(c => c.Email == form.Email, ct);
                if (exists) continue;

                // 3️⃣ Split full name into first & last
                var names = form.FullName?.Trim().Split(' ', 2) ?? new string[] { "", "" };
                var first = names.Length > 0 ? names[0] : "";
                var last = names.Length > 1 ? names[1] : "";

                // 4️⃣ Create ATSCandidate
                var candidate = new ATSCandidate
                {
                    FirstName = first,
                    LastName = last,
                    Email = form.Email,
                    Phone = form.Phone,
                    SkillsCsv = form.SkillsCsv ?? "",
                    YearsOfExperience = form.YearsOfExperience,
                    ResumePath = form.ResumeUrl,
                    ResumeHash = Guid.NewGuid().ToString(), // simple dedupe
                    Status = CandidateStatus.Applied,
                    CreatedAt = DateTime.UtcNow,
                    SourceApplicationId = form.Id
                };

                _db.ATSCandidates.Add(candidate);

                // 5️⃣ Mark form as imported
                form.Imported = true;
                form.ImportStatus = "Imported";

                importedCount++;
            }

            await _db.SaveChangesAsync(ct);

            TempData["Message"] = $"{importedCount} Google Form rows imported to ATS.";
            return RedirectToAction(nameof(Index));
        }
        private void SimpleATSRule(ATSCandidate candidate)
        {
            bool shortlisted = false;

            // Rule 1: Experience >= 6 months → Shortlisted
            if (candidate.YearsOfExperience >= 6)
            {
                candidate.Status = CandidateStatus.Shortlisted;
                shortlisted = true;
            }

            // Rule 2: Has Python in skills → Shortlisted
            else if (!string.IsNullOrWhiteSpace(candidate.SkillsCsv)
                     && candidate.SkillsCsv.Contains("Python", StringComparison.OrdinalIgnoreCase))
            {
                candidate.Status = CandidateStatus.Shortlisted;
                shortlisted = true;
            }

            // Rule 3: Add more rules here if needed
            // Example: if (candidate.SkillsCsv.Contains("C#", StringComparison.OrdinalIgnoreCase)) { ... }

            // If none of the rules matched → Rejected
            if (!shortlisted)
            {
                candidate.Status = CandidateStatus.Rejected;
            }
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> RunATS(CancellationToken ct)
        {
            // جلب كل المرشحين اللي حالتهم Applied
            var candidates = await _db.ATSCandidates
                .Where(c => c.Status == CandidateStatus.Applied)
                .ToListAsync(ct);

            foreach (var c in candidates)
            {
                SimpleATSRule(c);
            }

            await _db.SaveChangesAsync(ct);

            TempData["Message"] = $"{candidates.Count} candidates processed.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignInterview(int id, string interviewer, DateTime interviewDate, string? notes, CancellationToken ct)
        {
            var candidate = await _db.ATSCandidates.FindAsync(new object[] { id }, ct);
            if (candidate == null) return NotFound();

            candidate.InterviewDate = interviewDate;
            candidate.Interviewer = interviewer;
            candidate.InterviewNotes = notes;
            candidate.Status = CandidateStatus.InterviewScheduled;


            await _db.SaveChangesAsync(ct);

            TempData["Message"] = $"Interview scheduled for {candidate.FirstName} {candidate.LastName}.";
            return RedirectToAction(nameof(Index));
        }


    }

}

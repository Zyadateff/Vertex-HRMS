using Microsoft.EntityFrameworkCore;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Database;

namespace VertexHRMS.BLL.Services.Implementation
{
    /// <summary>
    /// Demo: reads directly from GoogleFormApplications where Imported = false.
    /// Replace with a true reader that queries your external Google Form responses table.
    /// </summary>
    public class DemoExternalFormDb : IExternalFormDb
    {
        private readonly VertexHRMSDbContext _db;
        public DemoExternalFormDb(VertexHRMSDbContext db) => _db = db;

        public Task<List<GoogleFormApplication>> GetNewRowsAsync(CancellationToken ct = default)
            => _db.GoogleFormApplications
                  .Where(g => !g.Imported)
                  .OrderBy(g => g.SubmittedAt)
                  .ToListAsync(ct);
    }
}

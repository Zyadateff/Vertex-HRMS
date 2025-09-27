using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Database;

namespace VertexHRMS.BLL.Services.Implementation
{
    public class FormIngestService : IFormIngestService
    {
        private readonly IExternalFormDb _external;
        private readonly VertexHRMSDbContext _db;

        public FormIngestService(IExternalFormDb external, VertexHRMSDbContext db)
        { _external = external; _db = db; }

        public async Task ImportNewAsync(CancellationToken ct = default)
        {
            // If you have an external responses table, insert missing rows here.
            // Demo: simply confirms rows exist and leaves the Import flag to ATS step.
            var _ = await _external.GetNewRowsAsync(ct);
            // no-op in demo.
        }
    }
}

namespace VertexHRMS.BLL.Services.Abstraction
{
    public record ResumeParseResult(string name, string email, string skillsCsv, int years);

    public interface IResumeParser
    {
        Task<ResumeParseResult> ParseAsync(string filePath, CancellationToken ct = default);
    }
}

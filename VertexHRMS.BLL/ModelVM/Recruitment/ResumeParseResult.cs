namespace VertexHRMS.BLL.ModelVM
{
    // Minimal shape the pipeline uses
    public sealed record ResumeParseResult(
        string name,
        string email,
        string skillsCsv,
        int years
    );
}

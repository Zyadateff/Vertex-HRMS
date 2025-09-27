namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IATSPipeline
    {
        /// <summary>Validates, dedupes, and moves candidates into ATSCandidates.</summary>
        Task ProcessNewAsync(CancellationToken ct = default);
    }
}

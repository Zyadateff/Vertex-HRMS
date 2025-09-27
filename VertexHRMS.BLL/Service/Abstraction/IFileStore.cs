namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IFileStore
    {
        Task<string> FetchResumeAsync(string urlOrPath, CancellationToken ct = default);
        Task<string> Sha256Async(string path, CancellationToken ct = default);
    }
}

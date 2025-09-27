using System.Security.Cryptography;
using VertexHRMS.BLL.Services.Abstraction;

namespace VertexHRMS.BLL.Services.Implementation
{
    public class LocalFileStore : IFileStore
    {
        private readonly string _root;
        public LocalFileStore(string webRoot) => _root = Path.Combine(webRoot, "resumes");

        public async Task<string> FetchResumeAsync(string urlOrPath, CancellationToken ct = default)
        {
            Directory.CreateDirectory(_root);
            var file = Path.Combine(_root, $"resume_{Guid.NewGuid():N}.txt");
            // Demo: create a placeholder resume file. Replace with HTTP download / file copy.
            await File.WriteAllTextAsync(file, "Demo resume", ct);
            return file;
        }

        public async Task<string> Sha256Async(string path, CancellationToken ct = default)
        {
            await using var fs = File.OpenRead(path);
            var hash = await SHA256.Create().ComputeHashAsync(fs, ct);
            return Convert.ToHexString(hash);
        }
    }
}

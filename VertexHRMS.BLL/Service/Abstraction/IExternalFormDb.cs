
namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IExternalFormDb
    {
        Task<List<GoogleFormApplication>> GetNewRowsAsync(CancellationToken ct = default);
    }
}

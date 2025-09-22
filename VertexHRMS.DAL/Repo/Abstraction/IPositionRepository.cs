using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Repositories.Abstraction
{
    public interface IPositionRepository
    {
        Task<IEnumerable<Position>> GetAllActiveAsync();
    }
}

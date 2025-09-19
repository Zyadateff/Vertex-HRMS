using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Repositories.Abstraction
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllActiveAsync();
    }
}

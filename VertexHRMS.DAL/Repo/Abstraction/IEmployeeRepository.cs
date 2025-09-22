using VertexHRMS.DAL.Entities;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IEmployeeRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task<string> GenerateEmployeeCodeAsync();
        Task AddAsync(Employee employee);
        Task<Employee> GetByIdentityUserIdAsync(string userId);
        Task<IEnumerable<Employee>> GetAllActiveAsync();

    }
}

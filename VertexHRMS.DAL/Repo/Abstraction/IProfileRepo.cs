
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IProfileRepo
    {
        Task<Employee?> GetEmployeeWithDetailsAsync(int employeeId);
        Task UpdateEmployeeAsync(Employee employee);
    }
}

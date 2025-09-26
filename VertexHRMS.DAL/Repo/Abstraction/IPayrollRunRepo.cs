
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IPayrollRunRepo
    {
        Task<IEnumerable<PayrollRun>> GetAllAsync();
        Task<PayrollRun> GetByIdAsync(int id);
        Task AddAsync(PayrollRun payrollRun);
        Task UpdateAsync(PayrollRun payrollRun);
        Task DeleteAsync(int id);
    }
}

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IPayrollService
    {
        Task<GetPayrollVM> GetPayrollByIdAsync(int id);
        Task<IEnumerable<GetPayrollVM>> GetPayrollsByRunIdAsync(int runId);
    }
}

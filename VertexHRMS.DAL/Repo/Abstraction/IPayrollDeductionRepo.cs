namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IPayrollDeductionRepo
    {
        Task<IEnumerable<PayrollDeduction>> GetAllAsync();
        Task<PayrollDeduction> GetByIdAsync(int id);
        Task AddAsync(PayrollDeduction deduction);
        Task UpdateAsync(PayrollDeduction deduction);
        Task DeleteAsync(int id);
    }
}

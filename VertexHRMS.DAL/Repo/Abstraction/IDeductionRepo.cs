namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IDeductionRepo
    {
        Task<Deduction?> GetByIdAsync(int id);
        Task<List<Deduction>> GetAllAsync();
        Task AddAsync(Deduction deduction);
        Task UpdateAsync(Deduction deduction);
        Task DeleteAsync(int id);
    }
}

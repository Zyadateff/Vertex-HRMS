using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

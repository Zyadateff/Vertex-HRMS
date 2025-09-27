using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

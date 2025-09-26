using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IPayrollRepo
    {
        Task<IEnumerable<Payroll>> GetAllAsync();
        Task<Payroll> GetByIdAsync(int id);
        Task AddAsync(Payroll payroll);
        Task UpdateAsync(Payroll payroll);
        Task DeleteAsync(int id);
    }
}

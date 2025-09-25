using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IRevenueService
    {
        Task<IEnumerable<Revenue>> GetAllAsync();
        Task<Revenue?> GetByIdAsync(int id);
        Task AddAsync(Revenue revenue);
        Task UpdateAsync(Revenue revenue);
        Task DeleteAsync(int id);
        Task<Dictionary<int, IEnumerable<Revenue>>> GetQuarterlyAsync(int year);
        Task<IEnumerable<Revenue>> GetQuarterAsync(int year, int quarter);
    }
}

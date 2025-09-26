using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.BLL.ModelVM.Payroll;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IPayrollRunService
    {
        Task<IEnumerable<GetRunVM>> GetAllRunsAsync();
        Task<GetRunVM> GetRunByIdAsync(int id);
        Task<GetRunVM> CreateRunAsync(DateTime start, DateTime end, string runByUserId);
        Task ApproveRunAsync(int id);
        Task RejectRunAsync(int id);
    }
}

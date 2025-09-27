using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.BLL.ModelVM.Payroll;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IPayrollService
    {
        Task<GetPayrollVM> GetPayrollByIdAsync(int id);
        Task<IEnumerable<GetPayrollVM>> GetPayrollsByRunIdAsync(int runId);
    }
}

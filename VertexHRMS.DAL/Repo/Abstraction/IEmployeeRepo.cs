using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IEmployeeRepo
    {
        Task<int> CountByDepartmentAsync(int departmentId);
        Task<int> GetDepartmentIdByEmployeeIdAsync(int employeeId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IApplicationUserRepo
    {
        Task<ApplicationUser> GetByEmployeeIdAsync(int employeeId);
    }
}

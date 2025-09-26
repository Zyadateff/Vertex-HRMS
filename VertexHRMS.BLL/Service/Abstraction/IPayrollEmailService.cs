using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IPayrollEmailService
    {
        Task SendPayrollEmailAsync(Employee employee, Payroll payroll);

        Task SendPayrollRunEmailsAsync(PayrollRun run);

    }
}

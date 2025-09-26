using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.BLL.ModelVM.Payroll;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class PayrollRunService:IPayrollRunService
    {
        private readonly IPayrollRunRepo _runRepo;
        private readonly IPayrollRepo _payrollRepo;
        private readonly IPayrollDeductionRepo _payrolldeductionRepo;
        private readonly IEmployeeRepository _employeeRepo; // لازم يكون عندك Employee Repo
        private readonly IAttendanceRecordsRepo _attendanceRepo;
        private readonly IDeductionRepo _deductionRepo;
        private readonly IProjectTaskRepo _projectTaskRepo;
        private readonly IMapper _mapper;

        public PayrollRunService(
            IPayrollRunRepo runRepo,
            IPayrollRepo payrollRepo,
            IPayrollDeductionRepo payrooldeductionRepo,
            IEmployeeRepository employeeRepo,
            IAttendanceRecordsRepo attendanceRepo,
            IDeductionRepo deductionRepo,
            IProjectTaskRepo projectTaskRepo,
            IMapper mapper
            )
        {
            _runRepo = runRepo;
            _payrollRepo = payrollRepo;
            _payrolldeductionRepo = payrooldeductionRepo;
            _employeeRepo = employeeRepo;
            _attendanceRepo = attendanceRepo;
            _deductionRepo = deductionRepo;
            _projectTaskRepo = projectTaskRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetRunVM>> GetAllRunsAsync()
        {
            var x= await _runRepo.GetAllAsync();
            return _mapper.Map<List<GetRunVM>>(x);
        }

        public async Task<GetRunVM> GetRunByIdAsync(int id)
        {
            var x= await _runRepo.GetByIdAsync(id);
            return _mapper.Map<GetRunVM>(x);
        }

        public async Task<GetRunVM> CreateRunAsync(DateTime start, DateTime end, string runByUserId)
        {
            var employees = await _employeeRepo.GetAllAsync();

            var run = new PayrollRun(start, end, DateTime.Now, runByUserId);
            int c = 0;
            foreach (var emp in employees)
            {
                c++;
                decimal baseSalary = (decimal)emp.Salary;
                decimal gross = baseSalary; // تقدر تزود Allowances هنا
                decimal deductions = 0;     // هنا المفروض تجيب خصومات الموظف
                var records = await _attendanceRepo.GetByEmployeeIdAsync(emp.EmployeeId);
                foreach(var x in records)
                {
                    if (x.WorkHours > 8)
                    {
                        gross += (decimal)(((decimal)emp.Salary) / 180 * (x.WorkHours - 8));
                    }
                    if (x.WorkHours < 8)
                    {
                        deductions += (decimal)(((decimal)emp.Salary) / 180 * (8- x.WorkHours));
                    }
                }
                if ((decimal)emp.Salary > 40000)
                {
                    var ded = new PayrollDeduction( c, 1, (decimal)emp.Salary / 10);
                    _payrolldeductionRepo.AddAsync(ded);
                    deductions += (decimal)emp.Salary / 10;

                }
                var ded2 = new PayrollDeduction(c, 2, (decimal)emp.Salary / 10);
                _payrolldeductionRepo.AddAsync(ded2);
                deductions += (decimal)emp.Salary / 10;
                var Tasks = _projectTaskRepo.GetTasksByEmployeeAsync(emp.EmployeeId);
                foreach (var task in await Tasks)
                {
                    if(task.EstimatedHours < task.SpentHours)
                    {
                        gross += (decimal)emp.Salary / 5;
                    }

                }
                decimal net = gross - deductions;

                var payroll = new Payroll(run.PayrollRunId, emp.EmployeeId, baseSalary, gross, net, DateTime.Now);
                run.Payrolls.Add(payroll);
            }

            await _runRepo.AddAsync(run);
            return _mapper.Map<GetRunVM>(run);
        }

        public async Task ApproveRunAsync(int id)
        {
            var run = await _runRepo.GetByIdAsync(id);
            if (run == null) throw new Exception("Payroll Run not found");

            // هنا ممكن تزود Property اسمها Status في PayrollRun 
            // run.Status = "Approved";

            await _runRepo.UpdateAsync(run);
        }

        public async Task RejectRunAsync(int id)
        {
            var run = await _runRepo.GetByIdAsync(id);
            if (run == null) throw new Exception("Payroll Run not found");

            // run.Status = "Rejected";

            await _runRepo.UpdateAsync(run);
        }
    }
}

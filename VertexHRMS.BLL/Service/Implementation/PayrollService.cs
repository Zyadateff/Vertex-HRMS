namespace VertexHRMS.BLL.Service.Implementation
{
    public class PayrollService:IPayrollService
    {
        private readonly IPayrollRepo _payrollRepo;
        private readonly IPayrollDeductionRepo _deductionRepo;
        private readonly IMapper _mapper;

        public PayrollService(IPayrollRepo payrollRepo, IPayrollDeductionRepo deductionRepo, IMapper mapper)
        {
            _payrollRepo = payrollRepo;
            _deductionRepo = deductionRepo;
            _mapper = mapper;
        }

        public async Task<GetPayrollVM> GetPayrollByIdAsync(int id)
        {
            var x= await _payrollRepo.GetByIdAsync(id);
            return _mapper.Map<GetPayrollVM>(x);
        }

        public async Task<IEnumerable<GetPayrollVM>> GetPayrollsByRunIdAsync(int runId)
        {
            var payrolls = await _payrollRepo.GetAllAsync();
            var x= _mapper.Map<List<GetPayrollVM>>(payrolls);
            return x.Where(p => p.PayrollRunId == runId);
        }
    }
}

namespace VertexHRMS.BLL.Service.Implementation
{
    public class RevenueService:IRevenueService
    {
        private readonly IRevenueRepo _revenueRepository;

        public RevenueService(IRevenueRepo revenueRepository)
        {
            _revenueRepository = revenueRepository;
        }
        public async Task<IEnumerable<Revenue>> GetAllAsync()
            => await _revenueRepository.GetAllAsync();

        public async Task<Revenue?> GetByIdAsync(int id)
            => await _revenueRepository.GetByIdAsync(id);

        public async Task AddAsync(Revenue revenue)
            => await _revenueRepository.AddAsync(revenue);

        public async Task UpdateAsync(Revenue revenue)
            => await _revenueRepository.UpdateAsync(revenue);

        public async Task DeleteAsync(int id)
            => await _revenueRepository.DeleteAsync(id);

        public async Task<Dictionary<int, IEnumerable<Revenue>>> GetQuarterlyAsync(int year)
            => await _revenueRepository.GetQuarterlyAsync(year);

        public async Task<IEnumerable<Revenue>> GetQuarterAsync(int year, int quarter)
            => await _revenueRepository.GetQuarterAsync(year, quarter);
    }
}


namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveRequestDayRepo
    {
        Task<LeaveRequestDay> GetByIdAsync(int dayId);
        Task<IEnumerable<LeaveRequestDay>> GetByRequestIdAsync(int requestId);
        Task<IEnumerable<LeaveRequestDay>> GetByDateAsync(DateTime date);
        Task AddAsync(LeaveRequestDay requestDay);
        Task UpdateAsync(LeaveRequestDay requestDay);
        Task DeleteAsync(int dayId);
    }
}

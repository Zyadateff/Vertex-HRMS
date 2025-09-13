
namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface ILeaveTypeRepo
    {
        Task<LeaveType> GetByIdAsync(int typeId);
        Task<IEnumerable<LeaveType>> GetAllAsync();
        Task AddAsync(LeaveType leaveType);
        Task UpdateAsync(LeaveType leaveType);
        Task DeleteAsync(int typeId);
    }
}

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IEmployeeTrainingService
    {
        Task<IEnumerable<EmployeeTraining>> GetAllAsync();
        Task<EmployeeTraining?> GetByIdAsync(int id);
        Task AddAsync(EmployeeTraining training);
        Task UpdateAsync(EmployeeTraining training);
        Task DeleteAsync(int id);
        Task<IEnumerable<EmployeeTraining>> GetByEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeTraining>> GetPendingTrainingsAsync(int employeeId);
        Task<IEnumerable<EmployeeTraining>> GetOverdueTrainingsAsync(int employeeId);
    }
}

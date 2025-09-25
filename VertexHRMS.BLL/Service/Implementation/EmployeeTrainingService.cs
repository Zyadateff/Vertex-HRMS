using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class EmployeeTrainingService:IEmployeeTrainingService
    {
            private readonly IEmployeeTrainingRepo _trainingRepository;

            public EmployeeTrainingService(IEmployeeTrainingRepo trainingRepository)
            {
                _trainingRepository = trainingRepository;
            }

            public async Task<IEnumerable<EmployeeTraining>> GetAllAsync()
                => await _trainingRepository.GetAllAsync();

            public async Task<EmployeeTraining?> GetByIdAsync(int id)
                => await _trainingRepository.GetByIdAsync(id);

            public async Task AddAsync(EmployeeTraining training)
                => await _trainingRepository.AddAsync(training);

            public async Task UpdateAsync(EmployeeTraining training)
                => await _trainingRepository.UpdateAsync(training);

            public async Task DeleteAsync(int id)
                => await _trainingRepository.DeleteAsync(id);

            public async Task<IEnumerable<EmployeeTraining>> GetByEmployeeAsync(int employeeId)
                => await _trainingRepository.GetByEmployeeAsync(employeeId);

            public async Task<IEnumerable<EmployeeTraining>> GetPendingTrainingsAsync(int employeeId)
                => await _trainingRepository.GetPendingTrainingsAsync(employeeId);

            public async Task<IEnumerable<EmployeeTraining>> GetOverdueTrainingsAsync(int employeeId)
                => await _trainingRepository.GetOverdueTrainingsAsync(employeeId);
        }
}

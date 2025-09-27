namespace VertexHRMS.BLL.Service.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepo _repo;
        private readonly IMapper _mapper;

        public ProfileService(IProfileRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ProfileVM?> GetProfileAsync(int employeeId)
        {
            var emp = await _repo.GetEmployeeWithDetailsAsync(employeeId);
            return emp == null ? null : _mapper.Map<ProfileVM>(emp);
        }

        public async Task UpdateProfileAsync(ProfileVM vm, IFormFile? profileImage)
        {
            var emp = await _repo.GetEmployeeWithDetailsAsync(vm.EmployeeId);
            if (emp == null) throw new Exception("Employee not found.");

            var oldImagePath = emp.ImagePath; 
            var oldFileName = string.IsNullOrWhiteSpace(oldImagePath) ? null : Path.GetFileName(oldImagePath);

            _mapper.Map(vm, emp);

            if (profileImage != null && profileImage.Length > 0)
            {
                var uploadedFileName = Upload.UploadFile("Files", profileImage);
                if (!string.IsNullOrWhiteSpace(uploadedFileName))
                {
                    emp.UpdateInfo(emp.FirstName, emp.LastName, emp.Phone, emp.DepartmentId, emp.PositionID, $"/Files/{uploadedFileName}", emp.ManagerId);

                    if (!string.IsNullOrWhiteSpace(oldFileName))
                    {
                        Upload.RemoveFile("Files", oldFileName);
                    }
                }
            }

            await _repo.UpdateEmployeeAsync(emp);
        }
    }
}

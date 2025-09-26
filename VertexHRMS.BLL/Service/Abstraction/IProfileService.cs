
using Microsoft.AspNetCore.Http;
using VertexHRMS.BLL.ModelVM;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IProfileService
    {
        Task<ProfileVM?> GetProfileAsync(int employeeId);
        Task UpdateProfileAsync(ProfileVM vm, IFormFile? profileImage);
    }
}

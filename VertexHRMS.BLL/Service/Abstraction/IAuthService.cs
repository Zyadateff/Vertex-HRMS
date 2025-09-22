
using VertexHRMS.BLL.ModelVM.Models;
using VertexHRMS.BLL.ModelVM.ViewModels;

using VertexHRMS.BLL.Common;

namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IAuthService
    {
        Task<ApiResponse<EmployeeViewModel>> LoginAsync(LoginModel model);
        Task<ApiResponse> LogoutAsync();
        Task<ApiResponse> ResetPasswordAsync(ResetPasswordModel model);
        Task<ApiResponse> ChangePasswordAsync(ChangePasswordModel model);

    }
}

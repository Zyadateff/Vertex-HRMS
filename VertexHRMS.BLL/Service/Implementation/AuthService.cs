using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using VertexHRMS.BLL.Common;
using VertexHRMS.BLL.ModelVM.Models;
using VertexHRMS.BLL.ModelVM.ViewModels;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repositories.Abstraction;

namespace VertexHRMS.BLL.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper _mapper;
        

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmployeeRepository employeeRepository,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeRepository = employeeRepository;
            _mapper = mapper;
           
        }

        // ------------------ LOGIN ------------------
        public async Task<ApiResponse<EmployeeViewModel>> LoginAsync(LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return ApiResponse<EmployeeViewModel>.FailureResult("Invalid email or password");

                if (!user.EmailConfirmed)
                    return ApiResponse<EmployeeViewModel>.FailureResult("Please confirm your email address before logging in");

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (!result.Succeeded)
                    return result.IsLockedOut
                        ? ApiResponse<EmployeeViewModel>.FailureResult("Account is locked due to multiple failed attempts. Please try again later.")
                        : ApiResponse<EmployeeViewModel>.FailureResult("Invalid email or password");

                // تحقق إذا المستخدم يجب عليه تغيير الباسورد
                if (user.MustChangePassword)
                    return ApiResponse<EmployeeViewModel>.FailureResult("You must change your password before logging in.");

                var employee = await _employeeRepository.GetByIdentityUserIdAsync(user.Id);
                if (employee == null)
                    return ApiResponse<EmployeeViewModel>.FailureResult("Employee record not found");

                var employeeViewModel = _mapper.Map<EmployeeViewModel>(employee);
                return ApiResponse<EmployeeViewModel>.SuccessResult(employeeViewModel, "Login successful");
            }
            catch (Exception ex)
            {
                return ApiResponse<EmployeeViewModel>.FailureResult("Login failed", new List<string> { ex.Message });
            }
        }

        // ------------------ LOGOUT ------------------
        public async Task<ApiResponse> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return ApiResponse.SuccessResult("Logged out successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.FailureResult("Logout failed", new List<string> { ex.Message });
            }
        }

        

        // ------------------ RESET PASSWORD ------------------
        public async Task<ApiResponse> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return ApiResponse.FailureResult("Invalid reset request");

                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));
                var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponse.FailureResult("Failed to reset password", errors);
                }

                user.MustChangePassword = false;
                await _userManager.UpdateAsync(user);

                return ApiResponse.SuccessResult("Password has been reset successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.FailureResult("Password reset failed", new List<string> { ex.Message });
            }
        }

        // ------------------ CHANGE PASSWORD ------------------
        public async Task<ApiResponse> ChangePasswordAsync(ChangePasswordModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Email))
                    return ApiResponse.FailureResult("Email is required");

                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                    return ApiResponse.FailureResult("User not found");

                IdentityResult result;

                if (!string.IsNullOrEmpty(model.CurrentPassword))
                {
                    // Normal password change
                    result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                }
                else
                {
                    // Forced password change (MustChangePassword = true)
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                }

                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ApiResponse.FailureResult("Failed to change password", errors);
                }

                user.MustChangePassword = false;
                await _userManager.UpdateAsync(user);

                return ApiResponse.SuccessResult("Password changed successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse.FailureResult("Password change failed", new List<string> { ex.Message });
            }
        }


    
    }
}

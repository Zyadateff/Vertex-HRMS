using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.Models;       // LoginModel, ForgotPasswordModel, ResetPasswordModel, ChangePasswordModel
using VertexHRMS.BLL.ModelVM.ViewModels;   // EmployeeViewModel
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Entities; // ApplicationUser

namespace VertexHRMS.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager; // <-- added

        public AuthController(IAuthService authService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _mapper = mapper;
            _userManager = userManager; // <-- assign
        }

        // ------------------ LOGIN ------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Dashboard");

            return View(new LoginModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.LoginAsync(model);

            if (!result.Success)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);

                ViewBag.ErrorMessage = result.Message;
                return View(model);
            }

            // ------------------ Role-based redirect ------------------
            var user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(user);

            // Optional: store user info in session
            HttpContext.Session.SetString("UserId", user.Id);
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", roles.FirstOrDefault() ?? "Employee");

            if (roles.Contains("HR"))
                return RedirectToAction("Dashboard", "HR");      // HR placeholder
            else
                return RedirectToAction("Profile", "Employee"); // Employee placeholder
        }

        // ------------------ CHANGE PASSWORD ------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            var email = TempData["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
                return RedirectToAction("Login");

            var model = new ChangePasswordModel { Email = email };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ChangePasswordAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Password changed successfully. Please login.";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            ViewBag.ErrorMessage = result.Message;
            return View(model);
        }



        // ------------------ RESET PASSWORD ------------------
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Invalid password reset link";
                return RedirectToAction("Login");
            }

            var model = new ResetPasswordModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _authService.ResetPasswordAsync(model);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            ViewBag.ErrorMessage = result.Message;
            return View(model);
        }

        // ------------------ LOGOUT ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return RedirectToAction("Login");
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using VertexHRMS.BLL.ModelVM;
using VertexHRMS.BLL.Service.Abstraction;

[Authorize] 
public class ProfileController : Controller
{
    private readonly IProfileService _profileService;
    private readonly IWebHostEnvironment _env;

    public ProfileController(IProfileService profileService, IWebHostEnvironment env)
    {
        _profileService = profileService;
        _env = env;
    }
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (User.IsInRole("HR"))
            ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
        else
            ViewData["Layout"] = "~/Views/Shared/_Layout_emp.cshtml";

        base.OnActionExecuting(context);
    }

    private int GetEmployeeId()
    {
        var employeeId = HttpContext.Session.GetInt32("EmployeeId");
        if (!employeeId.HasValue)
            throw new InvalidOperationException("EmployeeId not found in session.");

        return employeeId.Value;
    }

    [HttpGet]
    public async Task<IActionResult> Details()
    {
        var employeeId = GetEmployeeId();
        var vm = await _profileService.GetProfileAsync(employeeId);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpGet]
    public async Task<IActionResult> Edit()
    {
        var employeeId = GetEmployeeId();
        var vm = await _profileService.GetProfileAsync(employeeId);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProfileVM model, IFormFile? profileImage)
    {
        var employeeId = GetEmployeeId();

        if (model.EmployeeId != employeeId)
            return BadRequest("Unauthorized profile update.");

        if (!ModelState.IsValid)
            return View(model);

        await _profileService.UpdateProfileAsync(model, profileImage);

        TempData["Success"] = "Profile updated successfully";

        return RedirectToAction("Details", "Profile");
    }

}
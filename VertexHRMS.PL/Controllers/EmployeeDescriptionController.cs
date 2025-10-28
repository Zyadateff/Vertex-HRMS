using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class EmployeeDescriptionController : Controller
    {
        private readonly IEmployeeDescriptionService _employeeDescriptionService;
        public EmployeeDescriptionController(IEmployeeDescriptionService employeeDescriptionService)
        {
            _employeeDescriptionService = employeeDescriptionService;
        }
        [HttpGet]
        public async Task<IActionResult> EmployeeDescription(int EmployeeId)
        {
            var result = await _employeeDescriptionService.GetByEmployeeId(EmployeeId);
            if (result == null)
            {
                return NotFound();
            }
            return View("EmployeeDescription", result);
        }
    }
}

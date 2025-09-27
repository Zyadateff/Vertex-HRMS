using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.BLL.Service.Implementation;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class EmployeeCardsController : Controller
    {
        private readonly IEmployeeCardsService _employeeCardsService;
        public EmployeeCardsController(IEmployeeCardsService employeeCardsService)
        {
            _employeeCardsService =employeeCardsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int DepartmentId)
        {
            var result = await _employeeCardsService.GetByDepartmentId ( DepartmentId);
            if (result == null)
                ViewBag.Message = "No employees found in this department.";
            return View("GetAll", result); 
        }
    }
}

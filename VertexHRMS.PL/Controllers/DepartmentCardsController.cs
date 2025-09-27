using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.Department;
using VertexHRMS.BLL.Service.Abstraction;


namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class DepartmentCardsController : Controller
    {
        private readonly IDepartmentCardsService _departService;

        public DepartmentCardsController(IDepartmentCardsService departService)
        {
            _departService = departService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Getdepart()
        {
            var model = await _departService.GetAllDepartmentsAsync();
            return View(model); 
        }

      
        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentCardsVM createDepartVM)
        {
            if (ModelState.IsValid)
            {
                var result = _departService.Create(createDepartVM);
                if (!result.Item1)
                {
                    return RedirectToAction("Getdepart"); 
                }
                ViewBag.Message = result.Item2;
            }
            return View(createDepartVM);
        }
  



    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class LeaveEntitlementController : Controller
    {
        private readonly ILeaveEntitlementService _leaveEntitlementService;

        public LeaveEntitlementController(ILeaveEntitlementService leaveEntitlementService)
        {
            _leaveEntitlementService = leaveEntitlementService;
        }

        // الصفحة الرئيسية (فقط تعرض الفيو + JS)
        public IActionResult Index()
        {
            ViewBag.Year = DateTime.Now.Year;
            return View();
        }

        // API للفلترة ترجع JSON
        [HttpGet]
        public async Task<IActionResult> GetEntitlements(int employeeId, int year = 2025)
        {
            var result = await _leaveEntitlementService.GetAllForEmployeeAsync(employeeId, year);
            result = result.Where(x => x.LeaveTypeId == 1).ToList();
            return Json(result);
        }
    }

}

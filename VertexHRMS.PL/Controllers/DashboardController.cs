using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.Dashboard;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.BLL.Services.Implementation;

namespace VertexHRMS.PL.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _service;
        public DashboardController(IDashboardService dashboardService)
        {
            _service = dashboardService;
        }

        // Server-side render: Index returns view with VM
        public async Task<IActionResult> Index(string department = null, string from = null, string to = null)
        {
            DateTime? fromDt = null, toDt = null;
            if (!string.IsNullOrWhiteSpace(from) && DateTime.TryParse(from, out var f)) fromDt = f;
            if (!string.IsNullOrWhiteSpace(to) && DateTime.TryParse(to, out var t)) toDt = t;

            DashboardVM vm = await _service.GetDashboardAsync(fromDt, toDt, department);
            return View(vm);
        }

        // optional JSON endpoint (used by AJAX if you prefer)
        [HttpGet]
        public async Task<IActionResult> GetDashboard(string department = null, string from = null, string to = null)
        {
            DateTime? fromDt = null, toDt = null;
            if (!string.IsNullOrWhiteSpace(from) && DateTime.TryParse(from, out var f)) fromDt = f;
            if (!string.IsNullOrWhiteSpace(to) && DateTime.TryParse(to, out var t)) toDt = t;

            DashboardVM vm = await _service.GetDashboardAsync(fromDt, toDt, department);
            return Json(vm);
        }
    }
}

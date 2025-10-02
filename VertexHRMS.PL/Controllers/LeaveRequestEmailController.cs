using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class LeaveRequestEmailController : Controller
    {
        private readonly ILeaveRequestEmailService _service;

        public LeaveRequestEmailController(ILeaveRequestEmailService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index(int? employeeId, int? requestId, int? year)
        {
            var emails = await _service.GetAllAsync();

            if (employeeId.HasValue)
                emails = emails.Where(e => e.EmployeeId == employeeId.Value);

            if (requestId.HasValue)
                emails = emails.Where(e => e.LeaveRequestId == requestId.Value);

            if (year.HasValue)
                emails = emails.Where(e => e.ReceivedAt.Year == year.Value);

            ViewBag.EmployeeId = employeeId;
            ViewBag.RequestId = requestId;
            ViewBag.Year = year ?? DateTime.Now.Year;

            return View(emails.OrderByDescending(e => e.ReceivedAt));
        }
    }
}

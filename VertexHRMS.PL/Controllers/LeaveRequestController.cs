using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        // Index with filters
        public async Task<IActionResult> Index(int? year, string status)
        {
            var employeeId = 1; // TODO: Replace with logged-in employeeId or admin logic
            var requests = await _leaveRequestService.GetRequestsByEmployeeAsync(employeeId);

            // Apply filters
            if (year.HasValue)
            {
                requests = requests.Where(r => r.StartDateTime.Year == year.Value).ToList();
            }

            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                requests = requests.Where(r => r.Status == status).ToList();
            }

            ViewBag.Year = year ?? DateTime.Now.Year;
            ViewBag.Status = status ?? "All";

            return View(requests);
        }
    }
}

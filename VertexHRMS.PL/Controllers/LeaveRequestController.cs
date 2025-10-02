using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;

        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        // Index with filters
        public async Task<IActionResult> Index(int? year, string status, int id)
        {
            var employeeId = id; // TODO: Replace with logged-in employeeId or admin logic
            var requests = await _leaveRequestService.GetRequestsByEmployeeAsync(employeeId);

            // Apply filters
            if (year.HasValue)
            {
                requests = requests.Where(r => r.StartDateTime.Year == year.Value).ToList();
            }
            if(id != 0)
            {
                requests = requests.Where(r => r.EmployeeId == id).ToList();
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

using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.LeaveEntitlmentVM;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    public class LeaveEntitlementController : Controller
    {
        private readonly ILeaveEntitlementService _leaveEntitlementService;

        public LeaveEntitlementController(ILeaveEntitlementService leaveEntitlementService)
        {
            _leaveEntitlementService = leaveEntitlementService;
        }

        public async Task<IActionResult> Index(int? employeeId, int? leaveTypeId, int year = 2025)
        {
            ViewBag.Year = year;
            ViewBag.EmployeeId = employeeId;
            ViewBag.LeaveTypeId = leaveTypeId;

            if (employeeId.HasValue)
            {
                var list = await _leaveEntitlementService.GetAllForEmployeeAsync(employeeId.Value, year);
                if (leaveTypeId.HasValue)
                {
                    list = list.Where(x => x.LeaveTypeId == leaveTypeId.Value).ToList();
                }
                return View(list);
            }

            // لو employeeId مش متحدد هرجع List فاضية
            return View(new List<GetAllForEmployeeVM>());
        }
    }
}

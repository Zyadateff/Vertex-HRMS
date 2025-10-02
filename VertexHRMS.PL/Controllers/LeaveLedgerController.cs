using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.LeaveLedgerVM;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class LeaveLedgerController : Controller
    {
        private readonly ILeaveLedgerService _leaveLedgerService;

        public LeaveLedgerController(ILeaveLedgerService leaveLedgerService)
        {
            _leaveLedgerService = leaveLedgerService;
        }

        public async Task<IActionResult> Index(int? employeeId, int? leaveTypeId, int year = 2025)
        {
            ViewBag.Year = year;
            ViewBag.EmployeeId = employeeId;
            ViewBag.LeaveTypeId = leaveTypeId;

            if (employeeId.HasValue)
            {
                var list = await _leaveLedgerService.GetByEmployeeAsync(employeeId.Value, year);

                if (leaveTypeId.HasValue)
                {
                    list = list.Where(x => x.LeaveTypeId == leaveTypeId.Value).ToList();
                }

                return View(list);
            }

            return View(new List<GetByEmployeeVM>());
        }
    }
}

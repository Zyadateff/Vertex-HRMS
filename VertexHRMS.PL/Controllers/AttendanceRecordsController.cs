using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class AttendanceRecordsController : Controller
    {
        private readonly IAttendanceRecordsService attendanceRecordsService;
        public AttendanceRecordsController(IAttendanceRecordsService attendanceRecordsService)
        {
            this.attendanceRecordsService = attendanceRecordsService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFilterOptions()
        {
            var all = await attendanceRecordsService.GetAttendanceRecordsFilteredAsync(null, null, null, null);
            var depts = all.Select(x => x.DepartmentName).Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x).ToList();
            var positions = all.Select(x => x.Position).Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x).ToList();
            var statuses = all.Select(x => x.Status).Where(x => !string.IsNullOrEmpty(x)).Distinct().OrderBy(x => x).ToList();

            return Json(new { departments = depts, positions = positions, statuses = statuses });
        }

        [HttpGet]
        public async Task<IActionResult> GetRecords(string? departmentName, string? positionName, string? date, string? status, string? searchName)
        {

            DateTime? parsedDate = null;
            if (!string.IsNullOrEmpty(date))
            {
                if (DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d1))
                    parsedDate = d1;
                else if (DateTime.TryParse(date, out var d2))
                    parsedDate = d2;
            }

            var result = await attendanceRecordsService.GetAttendanceRecordsFilteredAsync(departmentName, positionName, parsedDate, status);

            if (!string.IsNullOrEmpty(searchName))
            {
                var s = searchName.Trim();
                result = result
                    .Where(r => !string.IsNullOrEmpty(r.EmployeeName) && r.EmployeeName.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
            }

            return Json(new { data = result });
        }
    }
}

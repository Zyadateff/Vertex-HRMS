using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    public class PayrollController : Controller
    {
        private readonly IPayrollRunService _payrollRunService;
        private readonly IPayrollService _payrollService;

        public PayrollController(IPayrollRunService payrollRunService, IPayrollService payrollService)
        {
            _payrollRunService = payrollRunService;
            _payrollService = payrollService;
        }

        // 📌 1. عرض كل الـ Payroll Runs
        public async Task<IActionResult> Index(int? year, int? month)
        {
            var runs = await _payrollRunService.GetAllRunsAsync();

            // فلترة حسب السنة
            if (year.HasValue)
                runs = runs.Where(r => r.RunDate.Year == year.Value).ToList();

            // فلترة حسب الشهر
            if (month.HasValue)
                runs = runs.Where(r => r.RunDate.Month == month.Value).ToList();

            // تمرير القيم الحالية للـ View (لـ select options)
            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;

            return View(runs);
        }

        // 📌 2. عرض تفاصيل Payroll Run معين
        public async Task<IActionResult> Details(int id)
        {
            var run = await _payrollRunService.GetRunByIdAsync(id);
            if (run == null) return NotFound();

            return View(run); // View اسمه Details.cshtml
        }

        // 📌 3. صفحة إنشاء Run جديد (GET)
        [HttpGet]
        public IActionResult Create()
        {
            return View(); // View اسمه Create.cshtml
        }

        // 📌 4. إنشاء Run جديد (POST)
        [HttpPost]
        public async Task<IActionResult> Create(DateTime periodStart, DateTime periodEnd)
        {
            if (periodStart > periodEnd)
            {
                ModelState.AddModelError("", "تاريخ البداية لازم يكون قبل تاريخ النهاية");
                return View();
            }

            // الـ UserId اللي شغّل الـ Run (ممكن تاخده من الـ Identity)
            string runByUserId = User?.Identity?.Name ?? "system";

            var run = await _payrollRunService.CreateRunAsync(periodStart, periodEnd, runByUserId);
            return RedirectToAction("Details", new { id = run.PayrollRunId });
        }

        // 📌 5. Approve Run
        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _payrollRunService.ApproveRunAsync(id);
            return RedirectToAction("Details", new { id });
        }

        // 📌 6. Reject Run
        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            await _payrollRunService.RejectRunAsync(id);
            return RedirectToAction("Details", new { id });
        }
    }
}

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

        public async Task<IActionResult> Index(int? year, int? month)
        {
            var runs = await _payrollRunService.GetAllRunsAsync();

            if (year.HasValue)
                runs = runs.Where(r => r.RunDate.Year == year.Value).ToList();

            if (month.HasValue)
                runs = runs.Where(r => r.RunDate.Month == month.Value).ToList();

            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;

            return View(runs);
        }

        public async Task<IActionResult> Details(int id)
        {
            var run = await _payrollRunService.GetRunByIdAsync(id);
            if (run == null) return NotFound();

            return View(run); 
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> Create(DateTime periodStart, DateTime periodEnd)
        {
            if (periodStart > periodEnd)
            {
                ModelState.AddModelError("", "تاريخ البداية لازم يكون قبل تاريخ النهاية");
                return View();
            }
            var run = await _payrollRunService.CreateRunAsync(periodStart, periodEnd);
            return RedirectToAction("Details", new { id = run.PayrollRunId });
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int id)
        {
            await _payrollRunService.ApproveRunAsync(id);
            return RedirectToAction("Details", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id)
        {
            await _payrollRunService.RejectRunAsync(id);
            return RedirectToAction("Details", new { id });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.PL.Controllers
{
    public class RevenueController : Controller
    {
        private readonly IRevenueService _revenueService;

        public RevenueController(IRevenueService revenueService)
        {
            _revenueService = revenueService;
        }
        public async Task<IActionResult> Index(int year = 2025, string mode = "Quarterly")
        {
            if (mode == "Quarterly")
            {
                var data = await _revenueService.GetQuarterlyAsync(year);
                ViewBag.Mode = "Quarterly";
                ViewBag.Year = year;
                return View(data);
            }
            else 
            {
                var data = await _revenueService.GetAllAsync();
                var monthly = data
                    .Where(r => r.MonthYear.Year == year)
                    .GroupBy(r => r.MonthYear.Month)
                    .ToDictionary(g => g.Key, g => g.AsEnumerable());

                ViewBag.Mode = "Monthly";
                ViewBag.Year = year;
                return View(monthly);
            }
        }
    }
}

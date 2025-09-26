using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VertexHRMS.DAL.Database;

namespace VertexHRMS.PL.Controllers
{
    public class CalendarController : Controller
    {
        private readonly VertexHRMSDbContext _context;

        public CalendarController(VertexHRMSDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetEvents()
        {
            var holidays = await _context.Holidays
                .Select(h => new
                {
                    title = "Holiday - " + h.Name,
                    start = h.HolidayDate.ToString("yyyy-MM-dd"),
                    color = "#dc3545",
                    type = "holiday"   
                })
                .ToListAsync();

            var interviews = await _context.Interviews
                .Include(i => i.Applicant) 
                .Select(i => new
                {
                    title = "Interview - " +
                            (i.Applicant != null
                                ? i.Applicant.FirstName + " " + i.Applicant.LastName
                                : "Unknown Applicant"),
                    start = i.InterviewDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    color = "#007bff", 
                    type = "interview" 
                })
                .ToListAsync();

            var events = holidays.Concat(interviews);

            return Json(events);
        }
    }
}

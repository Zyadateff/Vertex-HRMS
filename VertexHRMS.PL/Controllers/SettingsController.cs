using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR,Employee")]
    public class SettingsController : Controller
    {
        public IActionResult Settings()
        {
            return View();
        }
    }
}

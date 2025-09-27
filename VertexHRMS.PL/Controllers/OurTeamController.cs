using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VertexHRMS.PL.Controllers
{
    public class OurTeamController : Controller
    {
        [Authorize(Roles = "HR,Employee")]
        public IActionResult OurTeam()
        {
            return View();
        }
    }
}

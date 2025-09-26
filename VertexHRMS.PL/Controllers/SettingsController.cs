using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VertexHRMS.PL.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (User.IsInRole("HR"))
                ViewData["Layout"] = "~/Views/Shared/_Layout.cshtml";
            else
                ViewData["Layout"] = "~/Views/Shared/_Layout_emp.cshtml";

            base.OnActionExecuting(context);
        }
        public IActionResult Settings()
        {
            return View();
        }
    }
}

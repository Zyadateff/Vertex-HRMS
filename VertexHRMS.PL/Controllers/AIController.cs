using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.AI;
using VertexHRMS.BLL.Services.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    public class AIController : Controller
    {
        private readonly IAIService _aIService;
        public AIController(IAIService aIService)
        {
            _aIService = aIService;
        }

        [HttpGet]
        public IActionResult AI()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAnswer([FromBody] AIVM ai)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, error = "Invalid input" });
            }

            try
            {
                var result = await _aIService.GetAnswerAsync(ai.Question ?? string.Empty);

                return Json(new
                {
                    success = true,
                    response = result.Answer,
                    answer = result.Answer
                });
            }
            catch (Exception)
            {
                return Json(new { success = false, error = "Something went wrong. Please try again later." });
            }
        }
    }

}

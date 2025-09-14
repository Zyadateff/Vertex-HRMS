using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.ModelVM.AI;
using VertexHRMS.BLL.Services.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    public class AIController : Controller
    {
        private readonly IAIService aIService;
        public AIController(IAIService aIService)
        {
            this.aIService = aIService;
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
                return BadRequest("Data Not Valid");

            var answer = await aIService.GetAnswerAsync(ai.Question);

            if (answer.Item1 == true)
            {
                return Json(new { success = true, response = answer.Item3.Answer });
            }

            return Json(new { success = false, error = answer.Item2 });
        }
    }

}


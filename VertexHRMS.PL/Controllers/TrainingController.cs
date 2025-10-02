using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class TrainingController : Controller
    {
        private readonly IEmployeeTrainingService _trainingService;

        public TrainingController(IEmployeeTrainingService trainingService)
        {
            _trainingService = trainingService;
        }

        public async Task<IActionResult> Index(string filter = "All")
        {
            var trainings = await _trainingService.GetAllAsync();
            if (filter == "Pending")
                trainings = trainings.Where(t => t.Status == "Pending");
            else if (filter == "Completed")
                trainings = trainings.Where(t => t.Status == "Completed");
            else if (filter == "Overdue")
                trainings = trainings.Where(t => t.Status == "Overdue");

            ViewBag.Filter = filter;
            return View(trainings);
        }
    }
}


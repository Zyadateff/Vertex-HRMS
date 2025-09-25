using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Enum;

namespace VertexHRMS.PL.Controllers
{
    public class ProjectTaskController : Controller
    {
        private readonly IProjectTaskService _taskService;

        public ProjectTaskController(IProjectTaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<IActionResult> Index(int? projectId, int? employeeId, string? status)
        {
            var tasks = await _taskService.GetAllAsync();

            if (projectId.HasValue)
                tasks = tasks.Where(t => t.ProjectId == projectId.Value);

            if (employeeId.HasValue)
                tasks = tasks.Where(t => t.AssignedToEmployeeId == employeeId.Value);

            if (!string.IsNullOrEmpty(status))
            {
                if (Enum.TryParse<ProjectStatus>(status, out var parsedStatus))
                {
                    tasks = tasks.Where(t => t.Status == parsedStatus);
                }
            }

            // عشان نرجع القيم المختارة تفضل في الفلتر
            ViewBag.ProjectId = projectId;
            ViewBag.EmployeeId = employeeId;
            ViewBag.Status = status;

            return View(tasks);
        }
    }
}

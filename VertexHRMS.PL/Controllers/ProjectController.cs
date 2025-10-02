using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "HR")]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index(string filter = "All")
        {
            IEnumerable<Project> projects;

            if (filter == "Active")
                projects = await _projectService.GetActiveProjectsAsync();
            else if (filter == "Completed")
                projects = await _projectService.GetCompletedProjectsAsync();
            else
                projects = await _projectService.GetAllAsync();

            ViewBag.Filter = filter;
            return View(projects);
        }
    }
}

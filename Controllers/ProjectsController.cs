using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
namespace COMP2139_ICE.Controllers;



    public class ProjectsController : Controller
    {
        private static readonly List<Project> _projects = new List<Project>
        {
            new Project
            {
                Id = 1,
                Name = "Sample Project",
                Description = "This is a prewritten sample project to demonstrate the application functionality.",
                StartDate = DateTime.Now.AddDays(-10), // 10 days ago
                EndDate = DateTime.Now.AddDays(10), // 10 days from now
                Status = "In Progress"
            }
        };
        
        
        [HttpGet]
        public IActionResult Index()
        {
            return View(_projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                project.Id = _projects.Count + 1;
                _projects.Add(project);
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }

        public IActionResult Details(int id)
        {
            var project = _projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
    }


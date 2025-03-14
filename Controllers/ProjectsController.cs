using COMP2139_ICE.Data;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Controllers;



public class ProjectsController : Controller
{

    private readonly ApplicationDbContext _context;

    public ProjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Index action will retrieve a listing of projects (database)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IActionResult Index()
    {
        //retrieve all projects from the database
        var projects = _context.Projects.ToList();
        return View(projects);
    }

    [HttpGet]
    public IActionResult Create()
    {

        return View();

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Project project)
    {

        if (ModelState.IsValid)  // Save only if the model is valid
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(project);

    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        //retrieve the project with the specified id or null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        //retrieve the project with the specified id or null if not found
        var project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public IActionResult Edit(int id,
        [Bind("ProjectId, Name, Description, StartDate, EndDate, Status")]
        Project project)
    {
        //[bind] ensures that only the specified properties are updated
        if (id != project.ProjectId)
        {
            return NotFound(); //ensure the in the route matches the id in the model
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }
        return View(project);
    }

    
    private bool ProjectExists(int id)
    {
        return _context.Projects.Any(e => e.ProjectId == id);
    }
    
    [HttpGet]
    public IActionResult Delete(int id)
    {
        //retrieve the project with the specified id or null if not found
        var project = _context.Projects.FirstOrDefault(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }

        return View(project);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id, Project project)
    {
        //retrieve the project with the specified id or null if not found
        project = _context.Projects.Find(id);
        if (project == null)
        {
            return NotFound();
        }

        _context.Projects.Remove(project);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }

}


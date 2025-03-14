using COMP2139_ICE.Data;
using COMP2139_ICE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Controllers;

public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(int projectId)
    {
        var tasks = _context
            .Tasks
            .Where(t => t.ProjectId == projectId)
            .ToList();
        
        
        ViewBag.ProjectId = projectId;
        
        return View(tasks);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var task  = _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
        
    }
    
    
    [HttpGet]
    public IActionResult Create(int projectId)
    {
        var project = _context.Projects.Find(projectId);
        if (project == null)
        {
            return NotFound();
        }
        
        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = "",
        };
        
        
        return View(task);
    }
    
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title" , "Description" , "ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new {projectId = task.ProjectId});
        }
        
        return View(task);
    }
    
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var task  = _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }
        
        return View(task);
        
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("ProjectTaskId", "Title" , "Description" , "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }
        
        if (ModelState.IsValid)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
            return RedirectToAction("Index", new {projectId = task.ProjectId});
        }
        
        return View(task);
    }
    
    
    [HttpGet]
    public IActionResult Delete(int id)
    
   
    
    
}
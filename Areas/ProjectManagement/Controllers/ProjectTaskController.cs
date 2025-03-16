using COMP2139_ICE.Areas.ProjectManagement.Models;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]")]
public class ProjectTaskController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{projectId:int}")]
    public async Task<IActionResult> Index(int projectId)
    {
        var tasks = await _context
            .Tasks
            .Where(t => t.ProjectId == projectId)
            .ToListAsync();
        
        ViewBag.ProjectId = projectId;
        return View(tasks);
    }
    
    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var task = await _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpGet("Create/{projectId:int}")]
    public async Task<IActionResult> Create(int projectId)
    {
        var project = await _context.Projects.FindAsync(projectId);
        if (project == null)
        {
            return NotFound();
        }

        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = "",
            Description = "",
            Name = ""
        };
        return View(task);
    }

    [HttpPost("Create/{projectId:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int projectId, [Bind("Title", "Description", "ProjectId")] ProjectTask task)
    {
        // Set the Name property to match the Title
        task.Name = task.Title;
    
        // Ensure the ProjectId from the route is used
        if (task.ProjectId != projectId)
        {
            task.ProjectId = projectId;
        }
    
        // Clear any ModelState errors related to the Name property
        ModelState.Remove("Name");
    
        if (ModelState.IsValid)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }
    
        // If validation fails, output errors for debugging
        foreach (var state in ModelState)
        {
            if (state.Value.Errors.Count > 0)
            {
                Console.WriteLine($"Error in {state.Key}: {state.Value.Errors[0].ErrorMessage}");
            }
        }
    
        // Redisplay the form with validation errors
        return View(task);
    }
    
    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }

    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, 
        [Bind("ProjectTaskId", 
            "Title", 
            "Description", 
            "ProjectId")] 
        ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }

        // Set the Name property to match the Title
        task.Name = task.Title;
    
        // Clear any ModelState errors related to the Name property
        ModelState.Remove("Name");

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(task);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { projectId = task.ProjectId });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Tasks.AnyAsync(e => e.ProjectTaskId == task.ProjectTaskId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }
    
        // If validation fails, output errors for debugging
        foreach (var state in ModelState)
        {
            if (state.Value.Errors.Count > 0)
            {
                Console.WriteLine($"Error in {state.Key}: {state.Value.Errors[0].ErrorMessage}");
            }
        }
    
        // Retrieve the Project for the task to maintain the relationship
        task.Project = await _context.Projects.FindAsync(task.ProjectId);
    
        return View(task);
    }
    
    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context
            .Tasks
            .Include(t => t.Project)
            .FirstOrDefaultAsync(t => t.ProjectTaskId == id);
        if (task == null)
        {
            return NotFound();
        }
        return View(task);
    }
    
    [HttpPost("Delete/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task != null)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { projectId = task.ProjectId });
        }

        return NotFound();
    }
    
    [HttpGet("Search")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        /*
           Fetch all projects from the database as Queryable, this allows us to execute filters
           before executing the database query
         */
        var tasksQuery = _context.Tasks.AsQueryable();
        
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        if (projectId.HasValue)
        {
            tasksQuery = tasksQuery.Where(t => t.ProjectId == projectId);
        }
        
        if (searchPerformed)
        {
            searchString = searchString.ToLower();
            tasksQuery = tasksQuery
                .Where(p => p.Title.ToLower().Contains(searchString)|| 
                            p.Description.ToLower().Contains(searchString));
        }

        
        /*
         This is an asynchronous execution, which means this method does not block the thread while
         waiting for the database 
         */
        var tasks = await tasksQuery.ToListAsync();
        
        ViewBag.ProjectId = projectId;
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;
        
        return View("Index", tasks);
    }
}
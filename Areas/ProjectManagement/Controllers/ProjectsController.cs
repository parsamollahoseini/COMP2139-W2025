using COMP2139_ICE.Areas.ProjectManagement.Models;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[area]")]
public class ProjectController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        //Retrieve a list of projects from the database
        var projects = await _context.Projects.ToListAsync();
        return View(projects);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Project project)
    {
        if (ModelState.IsValid)
        {
            _context.Projects.Add(project);     //Project added to db (memory)
            await _context.SaveChangesAsync();             //Persists data to db
            //Only these two lines are saving data to the database, not only the first one
            return RedirectToAction("Index");
        }
        //Data persistence to the database
        return View(project);
    }

    [HttpGet("Details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        //Retrieves the project with the specified ID or returns null if not found
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();      //Code 404
        }
        return View(project);
    }

    [HttpGet("Edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        //Retrieves the project with the specified ID or returns null if not found
        var project = await _context.Projects.FindAsync(id);
        if (project == null)
        {
            return NotFound();      //Code 404
        }
        return View(project);
    }
    
    
    
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProjectId, Name, Description")] Project project)
    {
        // [Bind] Ensures only the specified properties are updated (security)
        if (id != project.ProjectId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(project); //Update the project with new values
                await _context.SaveChangesAsync(); //Commit the changes to the database
            }
            catch (DbUpdateConcurrencyException)
            {
                //Handles concurrency issues where another process (or user) modifies the project simultaneously
                if (!await ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;  //Returns the original exception back to the caller
                }
            }
            return RedirectToAction("Index");
        }
        return View(project);   //Re-display the form with validation errors
    }

    
    
    private async Task<bool> ProjectExists(int id)
    {
        return await _context.Projects.AnyAsync(e => e.ProjectId == id);
    }
    
    

    [HttpGet("Delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.ProjectId == id);
        if (project == null)
        {
            return NotFound();
        }
        return View(project);
    }

    
    
    [HttpPost("DeleteConfirmed/{ProjectId}"), ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int ProjectId)
    {
        var project = await _context.Projects.FindAsync(ProjectId);
        if (project != null)
        {
            _context.Projects.Remove(project); //Remove project from database
            await _context.SaveChangesAsync();            //Commit changes to database
            return RedirectToAction("Index");
        }
        return NotFound();
    }

    
    

    [HttpGet("Search/{searchString?}")]
    public async Task<IActionResult> Search(string searchString)
    {
        /*
           Fetch all projects from the database as Queryable, this allows us to execute filters
           before executing the database query 
         */
        var projectsQuery = _context.Projects.AsQueryable();
        
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);
        if (searchPerformed)
        {
            searchString = searchString.ToLower();
            projectsQuery = projectsQuery
                .Where(p => p.Name.ToLower().Contains(searchString)|| 
                            p.Description.ToLower().Contains(searchString));
        }

        var projects = await projectsQuery.ToListAsync();
        
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;
        
        return View("Index", projects);
    }
    
}


